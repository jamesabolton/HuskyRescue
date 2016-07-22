using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.EventsSponsor;
using HuskyRescue.Web.Filters;
using Microsoft.AspNet.Identity;
using Serilog;
using System.IO;

namespace HuskyRescue.Web.Controllers
{
	public class AdminEventsSponsorsController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAdminEventsManagerService _adminEventsManagerService;

		private readonly IAdminEventsSponsorManagerService _adminEventsSponsorManagerService;

		private readonly IAdminEventsSponsorshipManagerService _adminEventsSponsorshipManagerService;

		private readonly IAdminOrganizationsManagerService _adminOrganizationsManagerService;

		public AdminEventsSponsorsController(ILogger iLogger, IAdminEventsManagerService iAdminEventsManagerService,
			IAdminEventsSponsorManagerService iAdminEventsSponsorManagerService,
			IAdminOrganizationsManagerService iAdminOrganizationsManagerService,
			IAdminEventsSponsorshipManagerService iAdminEventsSponsorshipManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_adminEventsManagerService = iAdminEventsManagerService;
			_adminEventsSponsorManagerService = iAdminEventsSponsorManagerService;
			_adminOrganizationsManagerService = iAdminOrganizationsManagerService;
			_adminEventsSponsorshipManagerService = iAdminEventsSponsorshipManagerService;
		}

		public async Task<ActionResult> Index()
		{
			var sponsors = await _adminEventsSponsorManagerService.GetAllSponsorsListAsync();
			return View(sponsors);
		}

		[ImportModelStateFromTempData]
		public async Task<ActionResult> Create()
		{
			var sponsor = new Create();

			var events = await _adminEventsManagerService.GetAllEventsListAsync();

			sponsor.Events.Add(new SelectListItem
			{
				Text = "-- event name --",
				Value = string.Empty,
				Selected = true
			});

			foreach (var eventItem in events)
			{
				sponsor.Events.Add(new SelectListItem
								   {
									   Text = eventItem.EventName,
									   Value = eventItem.Id.ToString()
								   });
			}

			var organizations = await _adminOrganizationsManagerService.GetFilteredOrgsListAsync();

			sponsor.Organizations.Add(new SelectListItem
									{
										Text = "-- sponsor name --",
										Value = string.Empty,
										Selected = true
									});

			foreach (var org in organizations)
			{
				sponsor.Organizations.Add(new SelectListItem
										   {
											   Text = org.Name,
											   Value = org.Id.ToString()
										   });
			}

			foreach (var org in organizations)
			{
				sponsor.Organizations.Add(new SelectListItem
				{
					Text = org.Name,
					Value = org.Id.ToString()
				});
			}

			sponsor.People.Add(new SelectListItem { Disabled = true, Text = "No People", Value = "" });

			//var sponsorships = await _adminEventsSponsorshipManagerService.GetAllSponsorshipsListAsync();

			//foreach (var s in sponsorships)
			//{
			//	sponsor.EventSponsorships.Add(new SelectListItem
			//								   {
			//									   Text = s.Name,
			//									   Value = s.Id.ToString()
			//								   });
			//}

			return View(sponsor);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Create(Create sponsorObj, FormCollection form)
		{
			const string virtualDirectory = @"\Content\Images\Controllers\Golf\";

			try
			{
				// get model state errors
				var errors = ModelState.Values.SelectMany(v => v.Errors);

				if (!errors.Any())
				{
					HttpPostedFileBase postedFile = null;
					foreach (var fileObject in Request.Files)
					{
						postedFile = Request.Files[(string)fileObject];
						//Save file content goes here
						if (postedFile == null || postedFile.ContentLength <= 0) continue;
						//var physicalDirectory = Server.MapPath(virtualDirectory);

						//var fileName = postedFile.FileName;

						//var fileInfo = new FileInfo(Path.Combine(physicalDirectory, fileName));

						//if (fileInfo.Exists)
						//{
						//	fileName = fileInfo.Name.Insert(fileInfo.Name.Length - (fileInfo.Extension.Length), "-" + DateTime.Now);
						//}

						//var path = string.Format("{0}\\{1}", fileInfo.Directory.FullName, fileName);
						//sponsorObj.LogoFilePhysicalPath = path;
						sponsorObj.LogoFileName = postedFile.FileName;
						sponsorObj.FileLogoContentType = postedFile.ContentType;

						using (var reader = new BinaryReader(postedFile.InputStream))
						{
							sponsorObj.FileLogoContent = reader.ReadBytes(postedFile.ContentLength);
						}
					}

					sponsorObj.AddedByUserId = User.Identity.GetUserId();

					var result = await _adminEventsSponsorManagerService.AddEventSponsorAsync(sponsorObj);
					if (result.Succeeded)
					{
						if (postedFile != null)
						{
							postedFile.SaveAs(sponsorObj.LogoFilePhysicalPath);
						}

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
				ModelState.AddModelError("", "Unable to process creation of new event sponsor. Try again, and if the problem persists contact the admin (James).");

				_logger.Error(ex, "Sponsor add error: {message} {@ex}", ex.Message, ex);
			}

			return RedirectToAction("Index");
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