using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	public class StoreController : BaseController
	{
		private readonly ILogger _logger;

		public StoreController(ILogger iLogger)
			: base(iLogger)
		{
			_logger = iLogger;
		}

		public ActionResult Index()
		{
			return View();
		}
	}
}