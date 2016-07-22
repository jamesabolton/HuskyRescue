using System.Net.Mail;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.AccountManager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Serilog;

namespace HuskyRescue.BusinessLogic.Identity
{
	public class IdentityManagerService : IIdentityManagerService
	{
		private readonly UserManager _userManager;
		private readonly SignInManager _signInManager;
		private readonly RoleManager _roleManager;
		private readonly IAuthenticationManager _authenticationManager;
		private readonly ILogger _logger;
		private readonly IResourceManagerService _resourceManagerService;

		public IdentityManagerService(UserManager userManager, SignInManager signInManager, RoleManager roleManager, IAuthenticationManager authenticationManager, ILogger iLogger, IResourceManagerService iResourceManagerService)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
			_authenticationManager = authenticationManager;
			_logger = iLogger;
			_resourceManagerService = iResourceManagerService;
		}

		#region Account Register
		public async Task<RequestResult> RegisterNewAccount(string email, string userName, string password)
		{
			var user = new ApplicationUser { UserName = userName, Email = email };
			var identityResult = await _userManager.CreateAsync(user, password);
			var result = new RequestResult(identityResult);

			return result;
		}
		#endregion

		#region Account Log In
		public async Task<SignInStatus> Login(string email, string password, bool rememberMe)
		{
			// Require the user to have a confirmed email before they can log in
			var user = await GetUserProfileByEmail(email);
			if (user != null)
			{
				if (!await IsEmailConfirmedByUserId(user.Id))
				{
					return SignInStatus.RequiresEmailConfirmation;
				}
			}

			return SignInStatusConversion(await _signInManager.PasswordSignInAsync(email, password, rememberMe, false));
		}

		private async Task SignInAsync(ApplicationUser user, bool isPersistent)
		{
			_authenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.TwoFactorCookie);
			_authenticationManager.SignIn(new AuthenticationProperties { IsPersistent = isPersistent }, await user.GenerateUserIdentityAsync(_userManager));
		}

		private SignInStatus SignInStatusConversion(Microsoft.AspNet.Identity.Owin.SignInStatus status)
		{
			var signInStatus = SignInStatus.Failure;
			switch (status)
			{
				case Microsoft.AspNet.Identity.Owin.SignInStatus.Failure:
					signInStatus = SignInStatus.Failure;
					break;
				case Microsoft.AspNet.Identity.Owin.SignInStatus.LockedOut:
					signInStatus = SignInStatus.LockedOut;
					break;
				case Microsoft.AspNet.Identity.Owin.SignInStatus.RequiresVerification:
					signInStatus = SignInStatus.RequiresVerification;
					break;
				case Microsoft.AspNet.Identity.Owin.SignInStatus.Success:
					signInStatus = SignInStatus.Success;
					break;
			}
			return signInStatus;
		}
		#endregion

		#region External Log In
		public async Task<SignInStatus> ExternalSignIn(ExternalLoginInfo loginInfo, bool isPersistant)
		{
			return SignInStatusConversion(await _signInManager.ExternalSignInAsync(loginInfo, isPersistant));
		}

		public async Task<RequestResult> ExternalSignInConfirm(ExternalLoginInfo loginInfo, string email)
		{
			var user = new ApplicationUser { UserName = email, Email = email };

			var identityResult = await _userManager.CreateAsync(user);
			if (identityResult.Succeeded)
			{
				identityResult = await _userManager.AddLoginAsync(user.Id, loginInfo.Login);
				if (identityResult.Succeeded)
				{
					await _signInManager.SignInAsync(user, false, false);
				}
			}

			return new RequestResult(identityResult);
		}

		/// <summary>
		/// Remove External Login from user's profile
		/// </summary>
		/// <param name="loginProvider"></param>
		/// <param name="providerKey"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<bool> RemoveExternalLogin(string loginProvider, string providerKey, string userId)
		{
			bool success = false;
			var result = await _userManager.RemoveLoginAsync(userId, new UserLoginInfo(loginProvider, providerKey));
			if (result.Succeeded)
			{
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
				success = true;
			}

			return success;
		}

		public async Task<RequestResult> LinkLogin(string userId, ExternalLoginInfo externalLoginInfo)
		{
			var identityResult = await _userManager.AddLoginAsync(userId, externalLoginInfo.Login);
			var result = new RequestResult(identityResult);
			return result;

		}
		#endregion

		#region Account Verification
		public async Task<bool> HasBeenVerified()
		{
			return await _signInManager.HasBeenVerifiedAsync();
		}

		public async Task<string> GetVerifiedUserId()
		{
			var userId = await _signInManager.GetVerifiedUserIdAsync();
			return userId;
		}

		public async Task<string> GetEmailConfirmationToken(string userId)
		{
			// For more information on how to enable account confirmation and password reset please visit http://go.microsoft.com/fwlink/?LinkID=320771
			// Send an email with this link
			var code = await _userManager.GenerateEmailConfirmationTokenAsync(userId);
			return code;
		}

		public async Task<RequestResult> ConfirmEmail(string userId, string code)
		{
			var identityResult = await _userManager.ConfirmEmailAsync(userId, code);
			var result = new RequestResult(identityResult);
			return result;
		}

		public async Task<bool> IsEmailConfirmedByUserId(string userId)
		{
			return await _userManager.IsEmailConfirmedAsync(userId);
		}

		public async Task<bool> IsEmailConfirmedByEmail(string email)
		{
			var confirmed = false;
			var user = await GetUserProfileByEmail(email);
			if (user != null)
			{
				confirmed = await _userManager.IsEmailConfirmedAsync(user.Id);
			}
			return confirmed;
		}

		public async Task<bool> AccountExistCheckByEmail(string email)
		{
			var exist = false;
			var user = await GetUserProfileByEmail(email);
			if (user != null)
			{
				exist = true;
			}
			return exist;
		}

		public async Task<bool> AccountExistCheckByName(string name)
		{
			var exist = false;
			var user = await GetUserProfileByName(name);
			if (user != null)
			{
				exist = true;
			}
			return exist;
		}

		public async Task<bool> AccountExistCheckById(string id)
		{
			var exist = false;
			var user = await GetUserProfileById(id);
			if (user != null)
			{
				exist = true;
			}
			return exist;
		}

		public async Task<bool> CheckUserExist(string userId)
		{
			var user = await GetUserProfileById(userId);
			if (user != null)
			{
				return true;
			}
			return false;
		}
		#endregion

		#region Account Password

		public async Task<string> GetPasswordResetToken(string email)
		{
			var user = await GetUserProfileByEmail(email);
			var code = string.Empty;
			if (user != null)
			{
				code = await _userManager.GeneratePasswordResetTokenAsync(user.Id);
			}
			return code;
		}
		
		public async Task<RequestResult> ResetPassword(string email, string code, string password)
		{
			var user = await GetUserProfileByEmail(email);
			var result = new RequestResult(false);
			if (user != null)
			{
				var identityResult = await _userManager.ResetPasswordAsync(user.Id, code, password);
				result = new RequestResult(identityResult);
			}
			return result;
		}

		public async Task<bool> HasPassword(string userId)
		{
			var user = await GetUserProfileById(userId);
			return user.PasswordHash != null;
		}

		/// <summary>
		/// Change a user's password after validating the old password
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="oldPassword"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		public async Task<RequestResult> ChangeUserPassword(string userId, string oldPassword, string newPassword)
		{
			var identityResult = await _userManager.ChangePasswordAsync(userId, oldPassword, newPassword);
			var result = new RequestResult(identityResult);
			if (result.Succeeded)
			{
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
			}
			return result;
		}

		/// <summary>
		/// set an user's password
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		public async Task<RequestResult> SetUserPassword(string userId, string newPassword)
		{
			var identityResult = await _userManager.AddPasswordAsync(userId, newPassword);
			var result = new RequestResult(identityResult);
			if (result.Succeeded)
			{
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
			}
			return result;
		}

		public async Task<string> GeneratePasswordResetToken(string userId)
		{
			return await _userManager.GeneratePasswordResetTokenAsync(userId);
		}
		#endregion

		#region Two Factor
		public async Task<string> GetTwoFactorToken(string provider)
		{
			var user = await GetUserProfileById(await _signInManager.GetVerifiedUserIdAsync());
			string code = string.Empty;
			if (user != null)
			{
				code = await _userManager.GenerateTwoFactorTokenAsync(user.Id, provider);
			}
			return code;
		}

		public async Task<SignInStatus> TwoFactorLogIn(string provider, string code, bool rememberMe, bool rememberBrowser)
		{
			return SignInStatusConversion(await _signInManager.TwoFactorSignInAsync(provider, code, rememberMe, rememberBrowser));
		}

		public async Task<List<string>> GetValidTwoFactorProvidersForUser(string userId)
		{
			var userFactors = new List<string>(await _userManager.GetValidTwoFactorProvidersAsync(userId));
			return userFactors;
		}

		public async Task<RequestResult> SendTwoFactorCode(string provider)
		{
			var result = await _signInManager.SendTwoFactorCodeAsync(provider);

			return new RequestResult(result);
		}

		/// <summary>
		/// Enable two factor auth for an user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<bool> EnableTwoFactorAuth(string userId)
		{
			bool success = false;
			var result = await _userManager.SetTwoFactorEnabledAsync(userId, true);
			if (result.Succeeded)
			{
				success = true;
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
			}
			return success;
		}

		/// <summary>
		/// Disable two factor auth for an user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<bool> DisableTwoFactorAuth(string userId)
		{
			bool success = false;
			var result = await _userManager.SetTwoFactorEnabledAsync(userId, false);
			if (result.Succeeded)
			{
				success = true;
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
			}
			return success;
		}
		#endregion

		#region Phone
		/// <summary>
		/// Send sms text to a phone number to verify it before adding to a user's account
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		public async Task<string> SendPhoneNumberVerificationSms(string userId, string number)
		{
			// Generate the token and send it
			var code = await _userManager.GenerateChangePhoneNumberTokenAsync(userId, number);
			if (_userManager.SmsService != null)
			{
				var message = new IdentityMessage
				{
					Destination = number,
					Body = "Your security code is: " + code
				};
				await _userManager.SmsService.SendAsync(message);
			}
			return code;
		}

		/// <summary>
		/// Verify the code a user provides is the one sent to their phone via sms
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="number"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		public async Task<bool> VerifyPhoneNumberSmsCode(string userId, string number, string code)
		{
			bool success = false;
			var result = await _userManager.ChangePhoneNumberAsync(userId, number, code);
			if (result.Succeeded)
			{
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, isPersistent: false);
				}
				success = true;
			}
			return success;
		}

		/// <summary>
		/// remove a user's phone number from their account
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<bool> RemovePhoneNumber(string userId)
		{
			var success = false;
			var result = await _userManager.SetPhoneNumberAsync(userId, null);
			if (result.Succeeded)
			{
				var user = await GetUserProfileById(userId);
				if (user != null)
				{
					await SignInAsync(user, false);
					success = true;
				}
			}
			return success;
		}
		#endregion

		#region Email
		public async Task SendEmail(string body, string subject, string to)
		{
			//var emailService = new EmailService(EmailService.EmailFrom.Contact, to, subject, string.Empty, body);
			var emailService = new EmailService(EmailService.FromContactMailAddress, new MailAddress(to), subject, body);
			await emailService.SendAsync();
		}
		#endregion

		#region Profile
		/// <summary>
		/// Get a user's profile
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<ApplicationUser> GetUserProfileById(string userId)
		{
			return await _userManager.FindByIdAsync(userId);
		}

		public async Task<ApplicationUser> GetUserProfileByEmail(string email)
		{
			return await _userManager.FindByEmailAsync(email);
		}

		public async Task<ApplicationUser> GetUserProfileByName(string name)
		{
			return await _userManager.FindByNameAsync(name);
		}

		public async Task<string> GetUserIdByEmail(string email)
		{
			var user = await GetUserProfileByEmail(email);
			return user.Id;
		}
		
		#endregion

		#region Account Management
		public async Task<IndexViewModel> GetManageAccountIndexView(string userId)
		{
			var model = new IndexViewModel
			{
				HasPassword = await HasPassword(userId),
				PhoneNumber = await _userManager.GetPhoneNumberAsync(userId),
				TwoFactor = await _userManager.GetTwoFactorEnabledAsync(userId),
				Logins = await _userManager.GetLoginsAsync(userId),
				BrowserRemembered = await _authenticationManager.TwoFactorBrowserRememberedAsync(userId)
			};
			return model;
		}

		/// <summary>
		/// Get External Login information for an user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		public async Task<IList<UserLoginInfo>> GetLoginsForUser(string userId)
		{
			return await _userManager.GetLoginsAsync(userId);
		}
		public List<AuthenticationDescription> GetOtherLoginsFotUser(IList<UserLoginInfo> userLogins)
		{
			return _authenticationManager.GetExternalAuthenticationTypes().Where(auth => userLogins.All(ul => auth.AuthenticationType != ul.LoginProvider)).ToList();
		}
		#endregion

		#region Role Management

		public async Task<RequestResult> CreateRole(string name)
		{
			var identityResult = await _roleManager.CreateAsync(new IdentityRole { Name = name });
			return new RequestResult(identityResult);
		}

		public async Task<bool> RoleExistsByName(string name)
		{
			return await _roleManager.RoleExistsAsync(name);
		}

		public async Task<IdentityRole> GetRoleById(string id)
		{
			return await _roleManager.FindByIdAsync(id);
		}

		public async Task<IdentityRole> GetRoleByName(string name)
		{
			return await _roleManager.FindByNameAsync(name);
		}

		public async Task<RequestResult> UpdateRole(IdentityRole role)
		{
			return new RequestResult(await _roleManager.UpdateAsync(role));
		}

		public async Task<RequestResult> DeleteRoleByName(string id)
		{
			var result = new RequestResult();
			var role = await GetRoleByName(id);
			if (role != null)
			{
				result = new RequestResult(await _roleManager.DeleteAsync(role));
			}
			return result;
		}

		public async Task<RequestResult> DeleteRoleById(string id)
		{
			var result = new RequestResult();
			var role = await GetRoleById(id);
			if (role != null)
			{
				result = new RequestResult(await _roleManager.DeleteAsync(role));
			}
			return result;
		}


		#endregion

		/// <summary>
		/// Check if a user is authorized to use a resource
		/// </summary>
		/// <param name="userEmail">Email address of the user</param>
		/// <param name="resourceId">resource id</param>
		/// <returns>true or false</returns>
		public async Task<bool> AuthorizeByEmail(string userEmail, string resourceId)
		{
			var user = await _userManager.FindByEmailAsync(userEmail);
			var roles = (await _userManager.GetRolesAsync(user.Id)).ToArray();

			return _resourceManagerService.Authorize(resourceId, roles);
		}

		/// <summary>
		/// Check if a user is authorized to use a resource
		/// </summary>
		/// <param name="userId">User's ID</param>
		/// <param name="resourceId">resource id</param>
		/// <returns>true or false</returns>
		public async Task<bool> AuthorizeById(string userId, string resourceId)
		{
			var roles = (await _userManager.GetRolesAsync(userId)).ToArray();

			return _resourceManagerService.Authorize(resourceId, roles);
		}
	}
}
