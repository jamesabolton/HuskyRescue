using System.Data;
using System.Threading.Tasks;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.SystemSetting;
using HuskyRescue.Web.Filters;
using Microsoft.AspNet.Identity;
using Serilog;
using System.Web.Mvc;

namespace HuskyRescue.Web.Controllers
{
	public class AdminSystemSettingsController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminSystemSettingsManagerService _systemSettingsService;

		public AdminSystemSettingsController(ILogger logger, IAdminSystemSettingsManagerService service)
			: base(logger)
		{
			_logger = logger;
			_systemSettingsService = service;
		}

		public async Task<ActionResult> Index()
		{
			var settings = await _systemSettingsService.GetSettingListAsync();
			return View(settings);
		}

		public async Task<ActionResult> Details(string id)
		{
			var setting = await _systemSettingsService.GetSettingDetailAsync(id);
			return View(setting);
		}

		[ImportModelStateFromTempData]
		public ActionResult Create()
		{
			return View(new Create());
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create setting)
		{
			try
			{
				if (ModelState.IsValid)
				{
					setting.AddedByUserId = User.Identity.GetUserName();
					var result = await _systemSettingsService.AddSettingAsync(setting);
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
			var setting = await _systemSettingsService.GetSettingDetailAsync(id);
			var editSetting = new Edit
			                  {
				                  Name = setting.Name,
				                  Value = setting.Value,
				                  Notes = setting.Notes
			                  };
			return View(editSetting);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Edit(string id, Edit setting)
		{
			try
			{
				if (ModelState.IsValid)
				{
					setting.UpdatedByUserId = User.Identity.GetUserName();
					var result = await _systemSettingsService.UpdateSettingAsync(setting);
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

		//[ImportModelStateFromTempData]
		//public async Task<ActionResult> Delete(string id)
		//{
		//	var setting = await _systemSettingsService.GetSettingDetailAsync(id);
		//	return View(setting);
		//}

		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//[ExportModelStateToTempData]
		//public async Task<ActionResult> Delete(int id)
		//{
		//	try
		//	{
		//		var result = await _systemSettingsService.DeleteSettingAsync(id);
		//		if (result.Succeeded)
		//		{
		//			return RedirectToAction("Index");
		//		}
		//		else
		//		{
		//			return RedirectToAction("Delete");
		//		}
		//	}
		//	catch
		//	{
		//		ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
		//		return RedirectToAction("Delete");
		//	}
		//}
	}
}