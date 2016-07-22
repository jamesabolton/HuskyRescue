using HuskyRescue.Web.Filters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace HuskyRescue.Web.Controllers
{
	public class VolunteerController : BaseController
	{
		private readonly ILogger _logger;

		public VolunteerController(ILogger iLogger)
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
		[ImportModelStateFromTempData]
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