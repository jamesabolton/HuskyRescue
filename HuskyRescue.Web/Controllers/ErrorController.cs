using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	[AllowAnonymous]
	public class ErrorController : BaseController
	{
		private readonly ILogger _logger;

		public ErrorController(ILogger iLogger) : base(iLogger)
		{
			_logger = iLogger;
		}

		// GET: Error
		public ActionResult FailHusky()
		{
			Response.StatusCode = 404;
			// Setting the TrySkipIisCustomErrors = true so that IIS doesn’t try 
			// to hijack the 404 and show it’s own error page. 
			// Without this, when remote users try to navigate to an invalid URL 
			// they will see the IIS 404 error page instead of this page.
			Response.TrySkipIisCustomErrors = true;
			return View();
		}
	}
}