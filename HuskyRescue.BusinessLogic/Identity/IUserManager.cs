using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace HuskyRescue.BusinessLogic.Identity
{
	public interface IUserManager
	{
		void Dispose();
		Task<ClaimsIdentity> CreateIdentityAsync(ApplicationUser user, string authenticationType);
		Task<IdentityResult> CreateAsync(ApplicationUser user);
		Task<IdentityResult> UpdateAsync(ApplicationUser user);
		Task<IdentityResult> DeleteAsync(ApplicationUser user);
		Task<ApplicationUser> FindByIdAsync(string userId);
		Task<ApplicationUser> FindByNameAsync(string userName);
		Task<IdentityResult> CreateAsync(ApplicationUser user, string password);
		Task<ApplicationUser> FindAsync(string userName, string password);
		Task<bool> CheckPasswordAsync(ApplicationUser user, string password);
		Task<bool> HasPasswordAsync(string userId);
		Task<IdentityResult> AddPasswordAsync(string userId, string password);
		Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);
		Task<IdentityResult> RemovePasswordAsync(string userId);
		Task<string> GetSecurityStampAsync(string userId);
		Task<IdentityResult> UpdateSecurityStampAsync(string userId);
		Task<string> GeneratePasswordResetTokenAsync(string userId);
		Task<IdentityResult> ResetPasswordAsync(string userId, string token, string newPassword);
		Task<ApplicationUser> FindAsync(UserLoginInfo login);
		Task<IdentityResult> RemoveLoginAsync(string userId, UserLoginInfo login);
		Task<IdentityResult> AddLoginAsync(string userId, UserLoginInfo login);
		Task<IList<UserLoginInfo>> GetLoginsAsync(string userId);
		Task<IdentityResult> AddClaimAsync(string userId, Claim claim);
		Task<IdentityResult> RemoveClaimAsync(string userId, Claim claim);
		Task<IList<Claim>> GetClaimsAsync(string userId);
		Task<IdentityResult> AddToRoleAsync(string userId, string role);
		Task<IdentityResult> AddToRolesAsync(string userId, params string[] roles);
		Task<IdentityResult> RemoveFromRolesAsync(string userId, params string[] roles);
		Task<IdentityResult> RemoveFromRoleAsync(string userId, string role);
		Task<IList<string>> GetRolesAsync(string userId);
		Task<bool> IsInRoleAsync(string userId, string role);
		Task<string> GetEmailAsync(string userId);
		Task<IdentityResult> SetEmailAsync(string userId, string email);
		Task<ApplicationUser> FindByEmailAsync(string email);
		Task<string> GenerateEmailConfirmationTokenAsync(string userId);
		Task<IdentityResult> ConfirmEmailAsync(string userId, string token);
		Task<bool> IsEmailConfirmedAsync(string userId);
		Task<string> GetPhoneNumberAsync(string userId);
		Task<IdentityResult> SetPhoneNumberAsync(string userId, string phoneNumber);
		Task<IdentityResult> ChangePhoneNumberAsync(string userId, string phoneNumber, string token);
		Task<bool> IsPhoneNumberConfirmedAsync(string userId);
		Task<string> GenerateChangePhoneNumberTokenAsync(string userId, string phoneNumber);
		Task<bool> VerifyChangePhoneNumberTokenAsync(string userId, string token, string phoneNumber);
		Task<bool> VerifyUserTokenAsync(string userId, string purpose, string token);
		Task<string> GenerateUserTokenAsync(string purpose, string userId);
		void RegisterTwoFactorProvider(string twoFactorProvider, IUserTokenProvider<ApplicationUser, string> provider);
		Task<IList<string>> GetValidTwoFactorProvidersAsync(string userId);
		Task<bool> VerifyTwoFactorTokenAsync(string userId, string twoFactorProvider, string token);
		Task<string> GenerateTwoFactorTokenAsync(string userId, string twoFactorProvider);
		Task<IdentityResult> NotifyTwoFactorTokenAsync(string userId, string twoFactorProvider, string token);
		Task<bool> GetTwoFactorEnabledAsync(string userId);
		Task<IdentityResult> SetTwoFactorEnabledAsync(string userId, bool enabled);
		Task SendEmailAsync(string userId, string subject, string body);
		Task SendSmsAsync(string userId, string message);
		Task<bool> IsLockedOutAsync(string userId);
		Task<IdentityResult> SetLockoutEnabledAsync(string userId, bool enabled);
		Task<bool> GetLockoutEnabledAsync(string userId);
		Task<DateTimeOffset> GetLockoutEndDateAsync(string userId);
		Task<IdentityResult> SetLockoutEndDateAsync(string userId, DateTimeOffset lockoutEnd);
		Task<IdentityResult> AccessFailedAsync(string userId);
		Task<IdentityResult> ResetAccessFailedCountAsync(string userId);
		Task<int> GetAccessFailedCountAsync(string userId);
		IPasswordHasher PasswordHasher { get; set; }
		IIdentityValidator<ApplicationUser> UserValidator { get; set; }
		IIdentityValidator<string> PasswordValidator { get; set; }
		IClaimsIdentityFactory<ApplicationUser, string> ClaimsIdentityFactory { get; set; }
		IIdentityMessageService EmailService { get; set; }
		IIdentityMessageService SmsService { get; set; }
		IUserTokenProvider<ApplicationUser, string> UserTokenProvider { get; set; }
		bool UserLockoutEnabledByDefault { get; set; }
		int MaxFailedAccessAttemptsBeforeLockout { get; set; }
		TimeSpan DefaultAccountLockoutTimeSpan { get; set; }
		bool SupportsUserTwoFactor { get; }
		bool SupportsUserPassword { get; }
		bool SupportsUserSecurityStamp { get; }
		bool SupportsUserRole { get; }
		bool SupportsUserLogin { get; }
		bool SupportsUserEmail { get; }
		bool SupportsUserPhoneNumber { get; }
		bool SupportsUserClaim { get; }
		bool SupportsUserLockout { get; }
		bool SupportsQueryableUsers { get; }
		IQueryable<ApplicationUser> Users { get; }
		IDictionary<string, IUserTokenProvider<ApplicationUser, string>> TwoFactorProviders { get; }
	}
}