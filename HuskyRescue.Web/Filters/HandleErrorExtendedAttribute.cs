using System;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace HuskyRescue.Web.Filters
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = true)]
	public class HandleErrorExtendedAttribute : FilterAttribute, IExceptionFilter
	{
		private const string DefaultView = "Error";

		private readonly object _typeId = new object();

		private Type _exceptionType = typeof(Exception);
		private string _master;
		private string _view;

		private ILogger _logger;

		public Type ExceptionType
		{
			get { return _exceptionType; }
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				if (!typeof(Exception).IsAssignableFrom(value))
				{
					throw new ArgumentException(value.FullName);
				}

				_exceptionType = value;
			}
		}

		public string Master
		{
			get { return _master ?? String.Empty; }
			set { _master = value; }
		}

		public override object TypeId
		{
			get { return _typeId; }
		}

		public string View
		{
			get { return (!String.IsNullOrEmpty(_view)) ? _view : DefaultView; }
			set { _view = value; }
		}

		public void OnException(ExceptionContext filterContext)
		{
			if (filterContext == null)
			{
				throw new ArgumentNullException("filterContext");
			}
			if (filterContext.IsChildAction)
			{
				return;
			}

			// If custom errors are disabled, we need to let the normal ASP.NET exception handler
			// execute so that the user can see useful debugging information.
			if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
			{
				return;
			}

			_logger = DependencyResolver.Current.GetService<ILogger>();

			var exception = filterContext.Exception;

			_logger.Error(exception,"HandleErrorExtendedAttribute {message}", exception.Message);

			// If this is not an HTTP 500 (for example, if somebody throws an HTTP 404 from an action method), ignore it.
			if (new HttpException(null, exception).GetHttpCode() != 500)
			{
				return;
			}

			if (!ExceptionType.IsInstanceOfType(exception))
			{
				return;
			}

			var statusCode = (int)HttpStatusCode.InternalServerError;
			if (filterContext.Exception is HttpException)
			{
				statusCode = ((HttpException)(filterContext.Exception)).GetHttpCode();
			}
			else if (filterContext.Exception is UnauthorizedAccessException)
			{
				//to prevent login prompt in IIS which will appear when returning 401.
				statusCode = (int)HttpStatusCode.Forbidden;
			}

			// if the request is AJAX return JSON else view.
			if (filterContext.HttpContext.Request.Headers["X-Requested-With"] == "XMLHttpRequest")
			{
				filterContext.Result = new JsonResult
				{
					JsonRequestBehavior = JsonRequestBehavior.AllowGet,
					Data = new
					{
						error = true,
						message = filterContext.Exception.Message
					}
				};
			}
			else
			{
				// Non-AJAX request - normal response process
				filterContext.Result = CreateActionResult(filterContext, statusCode);
			}

			filterContext.ExceptionHandled = true;
			filterContext.HttpContext.Response.Clear();
			filterContext.HttpContext.Response.StatusCode = statusCode;// 500;

			// Certain versions of IIS will sometimes use their own error page when
			// they detect a server error. Setting this property indicates that we
			// want it to try to render ASP.NET MVC's error page instead.
			filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
		}

		protected virtual ActionResult CreateActionResult(ExceptionContext filterContext, int statusCode)
		{
			var ctx = new ControllerContext(filterContext.RequestContext, filterContext.Controller);
			var statusCodeName = ((HttpStatusCode)statusCode).ToString();

			var viewName = SelectFirstView(ctx,
											string.Format("~/Views/Error/{0}.cshtml", statusCodeName),
											"~/Views/Error/General.cshtml",
											statusCodeName,
											"Error");

			var controllerName = (string)filterContext.RouteData.Values["controller"];
			var actionName = (string)filterContext.RouteData.Values["action"];
			var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);
			var result = new ViewResult
			{
				ViewName = viewName,
				ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
			};
			result.ViewBag.StatusCode = statusCode;
			return result;
		}

		protected string SelectFirstView(ControllerContext ctx, params string[] viewNames)
		{
			return viewNames.First(view => ViewExists(ctx, view));
		}

		protected bool ViewExists(ControllerContext ctx, string name)
		{
			var result = ViewEngines.Engines.FindView(ctx, name, null);
			return result.View != null;
		}
	}

}