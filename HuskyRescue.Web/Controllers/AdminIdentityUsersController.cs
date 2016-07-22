using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Admin.Users;
using HuskyRescue.Web.Filters;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	[AllowAnonymous]
	public class AdminIdentityUsersController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminUserManagerService _adminUserManagerService;

		public AdminIdentityUsersController(ILogger iLogger, IAdminUserManagerService iAdminUserManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_adminUserManagerService = iAdminUserManagerService;
		}

		public async Task<ActionResult> Index()
		{
			var users = await _adminUserManagerService.GetUsersListAsync();
			return View(users);
		}

		public async Task<ActionResult> Details(string id)
		{
			var user = await _adminUserManagerService.GetUserDetailAsync(id);
			if (user.Roles.Any())
			{
				user.Roles.RemoveAll(r => r.Selected != true);
			}
			return View(user);
		}

		[ImportModelStateFromTempData]
		public ActionResult Create()
		{
			var create = new Create();
			create.Roles = _adminUserManagerService.GetUserRolesListAsync(string.Empty);
			return View(create);
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create role)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var roles = new List<Role>();
					foreach (var formFieldKey in Request.Form.AllKeys)
					{
						var validGuid = new Guid();
						if (Guid.TryParse(formFieldKey, out validGuid))
						{
							var formFieldValue = Request.Form[formFieldKey];
							roles.Add(
								new Role
								{
									Id = formFieldKey,
									Selected = formFieldValue.Contains("true,")
								});
						}
					}

					role.Roles = roles;
					var result = await _adminUserManagerService.AddUserAsync(role);
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

		[ImportModelStateFromTempData]
		public async Task<ActionResult> Edit(string id)
		{
			var detail = await _adminUserManagerService.GetUserDetailAsync(id);
			var edit = new Edit
			{
				Id = id,
				AccessFailedCount = detail.AccessFailedCount,
				Email = detail.Email,
				EmailConfirmed = detail.EmailConfirmed,
				LockoutEnabled = detail.LockoutEnabled,
				PhoneNumber = detail.PhoneNumber,
				PhoneNumberConfirmed = detail.PhoneNumberConfirmed,
				TwoFactorEnabled = detail.TwoFactorEnabled,
				UserName = detail.UserName,
				Roles = detail.Roles
			};
			return View(edit);
		}

		[HttpPost, ValidateAntiForgeryToken, ExportModelStateToTempData]
		public async Task<ActionResult> Edit(Edit role)
		{
			try
			{
				if (ModelState.IsValid)
				{
					var roles = new List<Role>();
					foreach (var formFieldKey in Request.Form.AllKeys)
					{
						var validGuid = new Guid();
						if (Guid.TryParse(formFieldKey, out validGuid))
						{
							var formFieldValue = Request.Form[formFieldKey];
							roles.Add(
								new Role
								{
									Id = formFieldKey,
									Selected = formFieldValue.Contains("true,")
								});
						}
					}
					role.Roles = roles;
					var result = await _adminUserManagerService.UpdateUserAsync(role);
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

		public ActionResult Delete(int id)
		{
			return View();
		}

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
