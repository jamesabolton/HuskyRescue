using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using ExpressiveAnnotations.Attributes;
//using ExpressiveAnnotations.MvcUnobtrusiveValidatorProvider.Validators;
using ExpressiveAnnotations.MvcUnobtrusive.Validators;
using Serilog;

namespace HuskyRescue.Web
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private ILogger _logger;

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
			RouteConfig.RegisterRoutes(RouteTable.Routes);
			BundleConfig.RegisterBundles(BundleTable.Bundles);

			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(RequiredIfAttribute), typeof(RequiredIfValidator));
			DataAnnotationsModelValidatorProvider.RegisterAdapter(typeof(AssertThatAttribute), typeof(AssertThatValidator));
		}

		void Application_Error(Object sender, EventArgs e)
		{
			var exception = Server.GetLastError();
			if (exception == null)
				return;

			_logger = DependencyResolver.Current.GetService<ILogger>();

			if (exception is HttpException)
			{
				_logger.Error(exception, "Http Exception {0}", exception.Message);
			}
			else
			{
				_logger.Error(exception, "Uncaught application exception {0}", exception.Message);
			}

			// Clear the error
			//Server.ClearError();

			// Redirect to a landing page
			//Response.Redirect("home/index");
		}
	}
}
