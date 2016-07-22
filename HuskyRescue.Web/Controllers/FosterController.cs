using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuskyRescue.Web.Filters;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	public class FosterController : BaseController
	{
		private readonly ILogger _logger;

		public FosterController(ILogger iLogger)
			: base(iLogger)
		{
			_logger = iLogger;
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Apply()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult Apply(FormCollection form)
		{
			return View();
		}
	}
}