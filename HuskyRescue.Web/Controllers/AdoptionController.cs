using System.Data;
using System.Threading.Tasks;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Adoption;
using HuskyRescue.ViewModel.Common;
using HuskyRescue.Web.Filters;
using Serilog;
using System.Linq;
using System.Web.Mvc;

namespace HuskyRescue.Web.Controllers
{
	public class AdoptionController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IApplicationManagerService _applicationManagerService;
		private readonly IRescueGroupsManagerService _rescueGroupsManagerService;

		public AdoptionController(ILogger iLogger, IApplicationManagerService appservice, IRescueGroupsManagerService iRescueGroupsManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_applicationManagerService = appservice;
			_rescueGroupsManagerService = iRescueGroupsManagerService;
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			var huskies = _rescueGroupsManagerService.GetAdoptableHuskiesAsync();

			return View(new RescueGroupAnimals { Animals = huskies });
		}

		[AllowAnonymous]
		[ImportModelStateFromTempData]
		public ActionResult Apply()
		{
			var app = new Apply
					  {
						  AppAddressStateList = ViewModel.Common.Lists.GetStateList(),
						  GenderList = ViewModel.Common.Lists.GetSexTypeList(),
						  ResidenceOwnershipList = ViewModel.Common.Lists.GetResidenceOwnershipTypeList(),
						  ResidencePetDepositCoverageList = ViewModel.Common.Lists.GetResidenceCoverageTypeList(),
						  ResidenceTypeList = ViewModel.Common.Lists.GetResidencTypeList(),
						  StudentTypeList = ViewModel.Common.Lists.GetStudentTypeList()
					  };
			return View(app);
		}

		[AllowAnonymous]
		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Apply(Apply app)
		{
			try
			{
				var errors = ModelState.Values.SelectMany(v => v.Errors);
				if (ModelState.IsValid)
				{
					var result = await _applicationManagerService.AddAdoptionApplication(app);
					if (result.Succeeded)
					{
						return RedirectToAction("ThankYou");
					}
					else
					{
						return RedirectToAction("Apply");
					}
				}
				_logger.Information("Adoption App Model Errors {@errors} {@app}", errors, app);
			}
			catch (DataException dex)
			{
				//Log the error (uncomment dex variable name and add a line here to write a log.
				ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
				_logger.Error(dex, "Data Exception saving Adoption Application {@app}", app);
			}
			return RedirectToAction("Apply");
		}

		[AllowAnonymous]
		public ActionResult ThankYou()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Process()
		{
			return View();
		}
	}
}