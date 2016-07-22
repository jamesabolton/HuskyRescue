using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuskyRescue.Web.Filters;

namespace HuskyRescue.Web.Controllers
{
	public class InventoryController : Controller
	{
		// GET: Inventory
		public ActionResult Index()
		{
			return View();
		}

		#region Categories
		public ActionResult Categories()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult CategoryCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult CategoryCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult CategoryEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult CategoryEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult CategoryDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult CategoryDelete(FormCollection form)
		{
			return View();
		}
		#endregion

		#region Items
		public ActionResult Items()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ItemCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ItemCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ItemEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ItemEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ItemDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ItemDelete(FormCollection form)
		{
			return View();
		}
		#endregion
	}
}