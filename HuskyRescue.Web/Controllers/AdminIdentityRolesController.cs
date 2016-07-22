using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Admin.Roles;
using HuskyRescue.Web.Filters;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	[AllowAnonymous]
	public class AdminIdentityRolesController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminRolesManagerService _adminRolesManagerService;

		public AdminIdentityRolesController(ILogger iLogger, IAdminRolesManagerService iAdminRolesManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_adminRolesManagerService = iAdminRolesManagerService;
		}

		// GET: RoleManager
		public async Task<ActionResult> Index()
		{
			var roles = await _adminRolesManagerService.GetRolesListAsync();
			return View(roles);
		}

		// GET: RoleManager/Details/5
		public async Task<ActionResult> Details(string id)
		{
			var role = await _adminRolesManagerService.GetRoleDetailAsync(id);
			if (role.Resources.Count > 0)
			{
				role.Resources.RemoveAll(r => r.Selected != true);
			}
			return View(role);
		}

		// GET: RoleManager/Create
		[ImportModelStateFromTempData]
		public ActionResult Create()
		{
			var create = new Create();
			create.Resources = _adminRolesManagerService.GetRoleResourcesListAsync(string.Empty);
			return View(create);
		}

		// POST: RoleManager/Create
		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create role)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var resources = new List<RoleResource>();
					foreach (var formFieldKey in Request.Form.AllKeys)
					{
						if (!formFieldKey.Equals("Name") && !formFieldKey.Equals("__RequestVerificationToken"))
						{
							var formFieldValue = Request.Form[formFieldKey];
							resources.Add(
								new RoleResource
						       {
								   Id = formFieldKey.Split('|')[0], 
								   Selected = formFieldValue.Contains("true,"),
								   Operations = int.Parse(formFieldKey.Split('|')[1])
						       });
						}
					}

					role.Resources = resources;
					var result = await _adminRolesManagerService.AddRoleAsync(role);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						return RedirectToAction("Create");
					}
				}
			}
			catch (DataException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("Create");
		}

		// GET: RoleManager/Edit/5
		[ImportModelStateFromTempData]
		public async Task<ActionResult> Edit(string id)
		{
			var detail = await _adminRolesManagerService.GetRoleDetailAsync(id);
			var edit = new Edit
					   {
						   Id = id,
						   Name = detail.Name,
						   Resources = detail.Resources
					   };
			return View(edit);
		}

		// POST: RoleManager/Edit/5
		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public async Task<ActionResult> Edit(Edit role)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var resources = new List<RoleResource>();
					foreach (var formFieldKey in Request.Form.AllKeys)
					{
						if (!formFieldKey.Equals("Name") && !formFieldKey.Equals("__RequestVerificationToken") && !formFieldKey.Equals("Id"))
						{
							var formFieldValue = Request.Form[formFieldKey];
							resources.Add(
								new RoleResource
								{
									Id = formFieldKey.Split('|')[0],
									Selected = formFieldValue.Contains("true,"),
									Operations = int.Parse(formFieldKey.Split('|')[1])
								});
						}
					}
					role.Resources = resources;
					var result = await _adminRolesManagerService.UpdateRoleAsync(role);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					else
					{
						return RedirectToAction("Edit");
					}
				}
			}
			catch (DataException /* dex */)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
			}
			return RedirectToAction("Edit");
		}

		// GET: RoleManager/Delete/5
		public ActionResult Delete(int id)
		{
			return View();
		}

		// POST: RoleManager/Delete/5
		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public async Task<ActionResult> Delete(int id, FormCollection collection)
		{
			try
			{
				// TODO: Add delete logic here

				return RedirectToAction("Index");
			}
			catch
			{
				return View();
			}
		}
	}
}
