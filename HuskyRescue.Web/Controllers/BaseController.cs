using System.Web.Mvc;
using HuskyRescue.Web.Filters;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	[Authorize]
	[ResourceAuthorize]
	public class BaseController : Controller
	{
		private readonly ILogger _logger;

		public BaseController(ILogger iLogger)
		{
			_logger = iLogger;
		}

		protected override void OnActionExecuted(ActionExecutedContext filterContext)
		{
			_logger.Information("{requestType} - {controller} - {action}",
				filterContext.HttpContext.Request.RequestType,
				filterContext.ActionDescriptor.ControllerDescriptor.ControllerName,
				filterContext.ActionDescriptor.ActionName);

			base.OnActionExecuted(filterContext);
		}

		//protected override void HandleUnknownAction(string actionName)
		//{
		//	base.HandleUnknownAction(actionName);
		//}

		/*
		protected override void OnException(ExceptionContext filterContext)
		{
			// Let other exceptions just go unhandled
			if (filterContext.Exception is InvalidOperationException)
			{
				// Default view is "error"
				var view = "error";
				var master = string.Empty;
				var controllerName = filterContext.RouteData.Values["controller"] as String;
				var actionName = filterContext.RouteData.Values["action"] as String;
				var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
				var result = new ViewResult
				{
					ViewName = view,
					MasterName = master,
					ViewData = new ViewDataDictionary(model),
					TempData = filterContext.Controller.TempData
				};
				filterContext.Result = result;

				// Configure the response object
				filterContext.ExceptionHandled = true;
				filterContext.HttpContext.Response.Clear();
				filterContext.HttpContext.Response.StatusCode = 500;
				filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
			}
			//base.OnException(filterContext);
		}
		*/
	}
}