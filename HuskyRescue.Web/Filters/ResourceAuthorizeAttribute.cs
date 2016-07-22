using HuskyRescue.BusinessLogic.Identity;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace HuskyRescue.Web.Filters
{
	[AttributeUsageAttribute(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class ResourceAuthorize : AuthorizeAttribute
	{
		public ILogger Logger { get; set; }
		public IResourceManagerService ResourceManagerService { get; set; }

		protected override bool AuthorizeCore(HttpContextBase httpContext)
		{
			// http://kevin-junghans.blogspot.com/2014/03/performing-authorization-in-class.html

			//Get the current claims principal
			var prinicpal = (ClaimsPrincipal)Thread.CurrentPrincipal;
			//Make sure they are authenticated
			if (!prinicpal.Identity.IsAuthenticated)
				return false;
			//Get the roles from the claims
			var roles = prinicpal.Claims.Where(c => c.Type == ClaimTypes.Role).Select(c => c.Value).ToArray();
			var controller = httpContext.Request.RequestContext.RouteData.Values["controller"].ToString().ToLower();
			var action = httpContext.Request.RequestContext.RouteData.Values["action"].ToString().ToLower();
			var requestType = httpContext.Request.RequestType.ToLower();
			//Check if they are authorized
			return ResourceManagerService.Authorize(string.Join("-", controller, action, requestType), roles);
		}

		protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
		{
			// http://www.paulallen.org/blog/aspnet-mvc-redirect-unauthorized-access-page-401-page
			if (filterContext.HttpContext.User.Identity.IsAuthenticated)
			{
				var acceptedTypes = filterContext.HttpContext.Request.AcceptTypes;
				foreach (var type in acceptedTypes)
				{
					if (type.Contains("html"))
					{
						if (filterContext.HttpContext.Request.IsAjaxRequest())
							filterContext.Result = new ViewResult { ViewName = "AccessDeniedPartial" };
						else
							filterContext.Result = new ViewResult { ViewName = "AccessDenied" };
						break;
					}
					else if (type.Contains("javascript"))
					{
						filterContext.Result = new JsonResult { Data = new { success = false, message = "Access denied." } };
						break;
					}
					else if (type.Contains("xml"))
					{
						filterContext.Result = new HttpUnauthorizedResult(); //this will redirect to login page with forms auth you could instead serialize a custom xml payload and return here.
					}
				}
			}
			else
			{
				base.HandleUnauthorizedRequest(filterContext);
			}
		}
	}
}