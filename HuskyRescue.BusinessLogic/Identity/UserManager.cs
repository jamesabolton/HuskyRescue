using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;

namespace HuskyRescue.BusinessLogic.Identity
{
	// IdentityConfig the application user manager used in this application. 
	// UserManager is defined in ASP.NET Identity and is used by the application.
	public class UserManager : UserManager<ApplicationUser>
	{
		public UserManager(IUserStore<ApplicationUser> store)
			: base(store)
		{
			// IdentityConfig validation logic for usernames
			UserValidator = new UserValidator<ApplicationUser>(this)
			{
				AllowOnlyAlphanumericUserNames = false,
				RequireUniqueEmail = true
			};

			// IdentityConfig validation logic for passwords
			PasswordValidator = new PasswordValidator
			{
				RequiredLength = 6,
				RequireNonLetterOrDigit = true,
				RequireDigit = true,
				RequireLowercase = true,
				RequireUppercase = true,
			};

			// IdentityConfig user lockout defaults
			UserLockoutEnabledByDefault = true;
			DefaultAccountLockoutTimeSpan = TimeSpan.FromMinutes(5);
			MaxFailedAccessAttemptsBeforeLockout = 5;

			// Register two factor authentication providers. This application uses Phone and Emails as a step of receiving a code for verifying the user
			// You can write your own provider and plug it in here.
			RegisterTwoFactorProvider("Phone Code", new PhoneNumberTokenProvider<ApplicationUser>
			{
				MessageFormat = "Your security code is {0}"
			});
			RegisterTwoFactorProvider("Email Code", new EmailTokenProvider<ApplicationUser>
			{
				Subject = "Security Code",
				BodyFormat = "Your security code is {0}"
			});
			EmailService = new EmailService();
			SmsService = new SmsService();
			var dataProtectionProvider = Startup.DataProtectionProvider;
			if (dataProtectionProvider != null)
			{
				var dataProtector = dataProtectionProvider.Create("ASP.NET Identity");
				this.UserTokenProvider = new DataProtectorTokenProvider<ApplicationUser>(dataProtector);
			}
		}
	}
}
