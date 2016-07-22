using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Organization;
using HuskyRescue.Web.Filters;
using Microsoft.AspNet.Identity;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	public class AdminOrganizationsController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminOrganizationsManagerService _adminOrganizationsManagerService;
		
		public AdminOrganizationsController(ILogger iLogger, IAdminOrganizationsManagerService iAdminOrganizationsManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_adminOrganizationsManagerService = iAdminOrganizationsManagerService;
		}

		public async Task<ActionResult> Index()
		{
			var orgs = await _adminOrganizationsManagerService.GetAllOrgsListAsync();
			return View(orgs);
		}

		[ImportModelStateFromTempData]
		public ActionResult Create()
		{
			var org = new Create();

			return View(org);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create org, FormCollection form)
		{
			try
			{
				// get model state errors
				var errors = ModelState.Values.SelectMany(v => v.Errors);

				if (!errors.Any())
				{
					org.AddedByUserId = User.Identity.GetUserId();

					var result = await _adminOrganizationsManagerService.AddOrganizationAsync(org);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
					return RedirectToAction("Create");
				}
			}
			catch (Exception ex)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to process creation of new organization. Try again, and if the problem persists contact the admin (James).");

				_logger.Error(ex, "Organization add error: {message} {@ex}", ex.Message, ex);
			}
			return RedirectToAction("Create");
		}

		[ImportModelStateFromTempData]
		public ActionResult Edit()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public ActionResult Edit(FormCollection form)
		{
			return View();
		}
	}
}