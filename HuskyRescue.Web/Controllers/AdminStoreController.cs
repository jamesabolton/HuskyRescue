using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using HuskyRescue.Web.Filters;

namespace HuskyRescue.Web.Controllers
{
	public class AdminStoreController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}
		#region Shipping Methods
		public ActionResult ShippingMethods()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ShippingMethodCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ShippingMethodCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ShippingMethodEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ShippingMethodEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ShippingMethodDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ShippingMethodDelete(FormCollection form)
		{
			return View();
		}
		#endregion

		#region Product Catagories
		public ActionResult ProductCatagories()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ProductCatagoryCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ProductCatagoryCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ProductCatagoryEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ProductCatagoryEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ProductCatagoryDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ProductCatagoryDelete(FormCollection form)
		{
			return View();
		}
		#endregion

		#region Products
		public ActionResult Products()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ProductCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ProductCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ProductEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ProductEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult ProductDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult ProductDelete(FormCollection form)
		{
			return View();
		}
		#endregion

		#region Orders
		public ActionResult Orders()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult OrderCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult OrderCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult OrderEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult OrderEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult OrderDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult OrderDelete(FormCollection form)
		{
			return View();
		}
		#endregion

		#region Carts
		public ActionResult Carts()
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult CartCreate()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult CartCreate(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult CartEdit()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult CartEdit(FormCollection form)
		{
			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult CartDelete()
		{
			return View();
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public ActionResult CartDelete(FormCollection form)
		{
			return View();
		}
		#endregion
	}
}