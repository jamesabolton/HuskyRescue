using System.Web.Mvc;
using System.Web.Routing;
using HuskyRescue.Web.MiscHelpers;

namespace HuskyRescue.Web
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
			routes.Add(new LegacyUrlRoute());

			routes.MapRoute(
				name: "FailHusky",
				url: "FailHusky/{action}/{id}",
				defaults: new
				          {
					          controller = "Error",
					          action = "FailHusky",
					          id = UrlParameter.Optional
				          });

			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
			);

		}
	}
}
