using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.EventsSponsorship;
using HuskyRescue.Web.Filters;
using Microsoft.AspNet.Identity;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuskyRescue.Web.Controllers
{
	public class AdminEventsSponsorshipController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminEventsManagerService _adminEventsManagerService;
		private readonly IAdminEventsSponsorshipManagerService _adminEventsSponsorshipManagerService;
		private readonly IAdminOrganizationsManagerService _adminOrganizationsManagerService;

		public AdminEventsSponsorshipController(ILogger iLogger, IAdminEventsManagerService iAdminEventsManagerService,
			IAdminEventsSponsorshipManagerService iAdminEventsSponsorshipManagerService,
			IAdminOrganizationsManagerService iAdminOrganizationsManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_adminEventsManagerService = iAdminEventsManagerService;
			_adminEventsSponsorshipManagerService = iAdminEventsSponsorshipManagerService;
			_adminOrganizationsManagerService = iAdminOrganizationsManagerService;
		}

		// GET: AdminEventsSponsorship
		public async Task<ActionResult> Index()
		{
			var eventSponsorships = await _adminEventsSponsorshipManagerService.GetAllSponsorshipsListAsync();
			return View(eventSponsorships);
		}

		[HttpPost]
		public async Task<JsonResult> ListByEvent(Guid eventId)
		{
			var eventSponsorships = await _adminEventsSponsorshipManagerService.GetEventSponsorshipsListAsync(eventId);
			return Json(eventSponsorships);
		}

		[ImportModelStateFromTempData]
		public async Task<ActionResult> Create()
		{
			var sponsorship = new Create();

			var events = await _adminEventsManagerService.GetAllEventsListAsync();

			foreach (var e in events)
			{
				sponsorship.Events.Add(new SelectListItem
										   {
											   Text = e.EventName,
											   Value = e.Id.ToString()
										   });
			}

			return View(sponsorship);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create sponsorshipObj, FormCollection form)
		{
			try
			{
				// get model state errors
				var errors = ModelState.Values.SelectMany(v => v.Errors);

				if (!errors.Any())
				{
					sponsorshipObj.AddedByUserId = User.Identity.GetUserId();

					var result = await _adminEventsSponsorshipManagerService.AddEventSponsorshipAsync(sponsorshipObj);
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
				ModelState.AddModelError("", "Unable to process creation of new event sponsorship. Try again, and if the problem persists contact the admin (James).");

				_logger.Error(ex, "Sponsorship add error: {message} {@ex}", ex.Message, ex);
			}
			return RedirectToAction("Create");
		}

		[ImportModelStateFromTempData]
		public async Task<ActionResult> Edit(string id)
		{
			if (string.IsNullOrEmpty(id)) RedirectToAction("Index");

			var eventId = new Guid(id);

			var sponsorship = _adminEventsSponsorshipManagerService.GetSponsorshipDetailForEdit(eventId);
			
			var events = await _adminEventsManagerService.GetAllEventsListAsync();

			foreach (var e in events)
			{
				sponsorship.Events.Add(new SelectListItem
										   {
											   Text = e.EventName,
											   Value = e.Id.ToString(),
											   Selected = e.Id.Equals(eventId)
										   });
			}

			return View(sponsorship);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Edit(Edit sponsorshipObj, FormCollection form)
		{
			try
			{
				// get model state errors
				var errors = ModelState.Values.SelectMany(v => v.Errors);

				if (!errors.Any())
				{
					sponsorshipObj.UpdatedByUserId = User.Identity.GetUserId();

					var result = await _adminEventsSponsorshipManagerService.UpdateEventSponsorshipAsync(sponsorshipObj);
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
				//Log the error (uncomment ex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to process update of existing event sponsorship. Try again, and if the problem persists contact the admin (James).");

				_logger.Error(ex, "Sponsorship update error: {message} {@ex}", ex.Message, ex);
			}
			return RedirectToAction("Edit");
		}
	}
}