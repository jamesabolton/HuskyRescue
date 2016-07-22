using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HuskyRescue.BusinessLogic.Identity
{
	// IdentityConfig the application sign-in manager which is used in this application.
	public class SignInManager : SignInManager<ApplicationUser, string>
	{
		public SignInManager(UserManager userManager, IAuthenticationManager authenticationManager)
			: base(userManager, authenticationManager)
		{
		}

		public override Task<ClaimsIdentity> CreateUserIdentityAsync(ApplicationUser user)
		{
			return user.GenerateUserIdentityAsync((UserManager)UserManager);
		}

		//public static SignInManager Create(IdentityFactoryOptions<SignInManager> options, IOwinContext context)
		//{
		//	return new SignInManager(context.GetUserManager<UserManager>(), context.Authentication);
		//}
	}
}
