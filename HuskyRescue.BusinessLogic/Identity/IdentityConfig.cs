using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Owin;
using Owin.Security.Providers.Yahoo;
using System;

namespace HuskyRescue.BusinessLogic.Identity
{
	public class IdentityConfig
	{
		// For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
		public IAppBuilder ConfigureAuth(IAppBuilder app)
		{

			// Configure the db context, user manager and signin manager to use a single instance per request
			app.CreatePerOwinContext(() => DependencyResolver.Current.GetService<UserManager>());

			// Enable the application to use a cookie to store information for the signed in user
			// and to use a cookie to temporarily store information about a user logging in with a third party login provider
			// Configure the sign in cookie
			var provider = new CookieAuthenticationProvider { OnException = context => { } };
			app.UseCookieAuthentication(new CookieAuthenticationOptions
			{
				AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
				LoginPath = new PathString("/Account/Login"),
				Provider = new CookieAuthenticationProvider
				{
					// Enables the application to validate the security stamp when the user logs in.
					// This is a security feature which is used when you change a password or add an external login to your account.  
					OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<UserManager, ApplicationUser>(
						validateInterval: TimeSpan.FromMinutes(30),
						regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
				}
			});
			app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			// Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
			app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

			// Enables the application to remember the second login verification factor such as phone or email.
			// Once you check this option, your second step of verification during the login process will be remembered on the device where you logged in from.
			// This is similar to the RememberMe option when you log in.
			app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

			// http://www.asp.net/web-api/overview/security/external-authentication-services#MICROSOFT
			app.UseMicrosoftAccountAuthentication(
				clientId: "000000004C12B8A3",
				clientSecret: "xX4bSHqvdYYTX9PIpoob-N7zvPt6E05g");

			//app.UseTwitterAuthentication(
			//   consumerKey: "",
			//   consumerSecret: "");

			// http://www.beabigrockstar.com/blog/introducing-the-yahoo-linkedin-oauth-security-providers-for-owin
			// https://github.com/owin-middleware/OwinOAuthProviders
			app.UseYahooAuthentication(
				consumerKey: "dj0yJmk9V29DUXNhZkg4aHV3JmQ9WVdrOVpWYzFUbWhxTldNbWNHbzlNQS0tJnM9Y29uc3VtZXJzZWNyZXQmeD05Mg--",
				consumerSecret: "d481374b1b99a98a07806e693bef931e341b93c1");

			// http://www.asp.net/mvc/tutorials/mvc-5/create-an-aspnet-mvc-5-app-with-facebook-and-google-oauth2-and-openid-sign-on
			app.UseFacebookAuthentication(
			   appId: "321434264706690",
			   appSecret: "fa8a5de929ba2be2c3603de0a58cfa10");

			// http://www.asp.net/mvc/tutorials/mvc-5/create-an-aspnet-mvc-5-app-with-facebook-and-google-oauth2-and-openid-sign-on
			app.UseGoogleAuthentication(
				clientId: "8896062100-nde5i58lt8rd9sr855he377q5ifilpbu.apps.googleusercontent.com",
				clientSecret: "LSvV0FAt2Uqb_JGbVDyFBsXr"
			);

			return app;
		}

	}
}
