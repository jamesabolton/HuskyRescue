using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.AccountManager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace HuskyRescue.BusinessLogic.Identity
{
	public interface IIdentityManagerService
	{
		Task<RequestResult> RegisterNewAccount(string email, string userName, string password);
		Task<SignInStatus> Login(string email, string password, bool rememberMe);
		Task<SignInStatus> ExternalSignIn(ExternalLoginInfo loginInfo, bool isPersistant);
		Task<RequestResult> ExternalSignInConfirm(ExternalLoginInfo loginInfo, string email);

		/// <summary>
		/// Remove External Login from user's profile
		/// </summary>
		/// <param name="loginProvider"></param>
		/// <param name="providerKey"></param>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<bool> RemoveExternalLogin(string loginProvider, string providerKey, string userId);

		Task<RequestResult> LinkLogin(string userId, ExternalLoginInfo externalLoginInfo);
		Task<bool> HasBeenVerified();
		Task<string> GetVerifiedUserId();
		Task<string> GetEmailConfirmationToken(string userId);
		Task<RequestResult> ConfirmEmail(string userId, string code);
		Task<bool> IsEmailConfirmedByUserId(string userId);
		Task<bool> IsEmailConfirmedByEmail(string email);
		Task<bool> AccountExistCheckByEmail(string email);
		Task<bool> AccountExistCheckByName(string name);
		Task<bool> AccountExistCheckById(string id);
		Task<bool> CheckUserExist(string userId);
		Task<string> GetPasswordResetToken(string email);
		Task<RequestResult> ResetPassword(string email, string code, string password);
		Task<bool> HasPassword(string userId);

		/// <summary>
		/// Change a user's password after validating the old password
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="oldPassword"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		Task<RequestResult> ChangeUserPassword(string userId, string oldPassword, string newPassword);

		/// <summary>
		/// set an user's password
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="newPassword"></param>
		/// <returns></returns>
		Task<RequestResult> SetUserPassword(string userId, string newPassword);

		Task<string> GeneratePasswordResetToken(string userId);
		Task<string> GetTwoFactorToken(string provider);
		Task<SignInStatus> TwoFactorLogIn(string provider, string code, bool rememberMe, bool rememberBrowser);
		Task<List<string>> GetValidTwoFactorProvidersForUser(string userId);
		Task<RequestResult> SendTwoFactorCode(string provider);

		/// <summary>
		/// Enable two factor auth for an user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<bool> EnableTwoFactorAuth(string userId);

		/// <summary>
		/// Disable two factor auth for an user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<bool> DisableTwoFactorAuth(string userId);

		/// <summary>
		/// Send sms text to a phone number to verify it before adding to a user's account
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="number"></param>
		/// <returns></returns>
		Task<string> SendPhoneNumberVerificationSms(string userId, string number);

		/// <summary>
		/// Verify the code a user provides is the one sent to their phone via sms
		/// </summary>
		/// <param name="userId"></param>
		/// <param name="number"></param>
		/// <param name="code"></param>
		/// <returns></returns>
		Task<bool> VerifyPhoneNumberSmsCode(string userId, string number, string code);

		/// <summary>
		/// remove a user's phone number from their account
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<bool> RemovePhoneNumber(string userId);

		Task SendEmail(string body, string subject, string to);

		/// <summary>
		/// Get a user's profile
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<ApplicationUser> GetUserProfileById(string userId);

		Task<ApplicationUser> GetUserProfileByEmail(string email);
		Task<ApplicationUser> GetUserProfileByName(string name);
		Task<string> GetUserIdByEmail(string email);
		Task<IndexViewModel> GetManageAccountIndexView(string userId);

		/// <summary>
		/// Get External Login information for an user
		/// </summary>
		/// <param name="userId"></param>
		/// <returns></returns>
		Task<IList<UserLoginInfo>> GetLoginsForUser(string userId);

		List<AuthenticationDescription> GetOtherLoginsFotUser(IList<UserLoginInfo> userLogins);
		Task<RequestResult> CreateRole(string name);
		Task<bool> RoleExistsByName(string name);
		Task<IdentityRole> GetRoleById(string id);
		Task<IdentityRole> GetRoleByName(string name);
		Task<RequestResult> UpdateRole(IdentityRole role);
		Task<RequestResult> DeleteRoleByName(string id);
		Task<RequestResult> DeleteRoleById(string id);

		/// <summary>
		/// Check if a user is authorized to use a resource
		/// </summary>
		/// <param name="userEmail">Email address of the user</param>
		/// <param name="resourceId">resource id</param>
		/// <returns>true or false</returns>
		Task<bool> AuthorizeByEmail(string userEmail, string resourceId);

		/// <summary>
		/// Check if a user is authorized to use a resource
		/// </summary>
		/// <param name="userId">User's ID</param>
		/// <param name="resourceId">resource id</param>
		/// <returns>true or false</returns>
		Task<bool> AuthorizeById(string userId, string resourceId);
	}
}