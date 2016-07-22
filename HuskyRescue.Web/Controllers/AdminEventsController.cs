using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Events;
using HuskyRescue.Web.Filters;
using Microsoft.AspNet.Identity;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	public class AdminEventsController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminEventsManagerService _adminEventsManagerService;
		private readonly IAdminOrganizationsManagerService _adminOrganizationsManagerService;
		

		public AdminEventsController(ILogger iLogger, IAdminEventsManagerService iAdminEventsManagerService,
			IAdminOrganizationsManagerService iAdminOrganizationsManagerService): base(iLogger)
		{
			_logger = iLogger;
			_adminEventsManagerService = iAdminEventsManagerService;
			_adminOrganizationsManagerService = iAdminOrganizationsManagerService;
		}

		public async Task<ActionResult> Index()
		{
			var events = await _adminEventsManagerService.GetAllEventsListAsync();
			return View(events);
		}

		[ImportModelStateFromTempData]
		public async Task<ActionResult> Create()
		{
			var eventObj = new Create();

			var organizations = await _adminOrganizationsManagerService.GetFilteredOrgsListAsync();

			foreach (var org in organizations)
			{
				eventObj.Organizations.Add(new SelectListItem
				                           {
					                           Text = org.Name,
											   Value = org.Id.ToString()
				                           });
			}

			return View(eventObj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create eventObj, FormCollection form)
		{
			try
			{
				// get model state errors
				var errors = ModelState.Values.SelectMany(v => v.Errors);

				if (!errors.Any())
				{
					eventObj.AddedByUserId = User.Identity.GetUserId();

					var result = await _adminEventsManagerService.AddEventAsync(eventObj);
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
				ModelState.AddModelError("", "Unable to process creation of new event. Try again, and if the problem persists contact the admin (James).");

				_logger.Error(ex, "Event add error: {message} {@ex}", ex.Message, ex);
			}
			return RedirectToAction("Create");
		}

		[ImportModelStateFromTempData]
		public async Task<ActionResult> Edit(Guid id)
		{
			var eventObj = await _adminEventsManagerService.GetEventByIdAsync(id);

			var organizations = await _adminOrganizationsManagerService.GetFilteredOrgsListAsync();

			foreach (var org in organizations)
			{
				eventObj.Organizations.Add(new SelectListItem
				{
					Text = org.Name,
					Value = org.Id.ToString()
				});
			}

			return View(eventObj);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Edit(Edit eventObj, FormCollection form)
		{
			try
			{
				// get model state errors
				var errors = ModelState.Values.SelectMany(v => v.Errors);

				if (!errors.Any())
				{
					eventObj.UpdatedByUserId = User.Identity.GetUserId();

					var result = await _adminEventsManagerService.EditEventAsync(eventObj);
					if (result.Succeeded)
					{
						return RedirectToAction("Index");
					}
					foreach (var error in result.Errors)
					{
						ModelState.AddModelError("", error);
					}
					return RedirectToAction("Edit");
				}
			}
			catch (Exception ex)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to process update of event. Try again, and if the problem persists contact the admin (James).");

				_logger.Error(ex, "Event add error: {message} {@ex}", ex.Message, ex);
			}
			return RedirectToAction("Edit");
		}
	}
}