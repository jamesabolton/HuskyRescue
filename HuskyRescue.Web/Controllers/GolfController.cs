using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Golf.Register;
using HuskyRescue.Web.Filters;
using GoogleRecaptcha;
using Serilog;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuskyRescue.Web.Controllers
{
	public class GolfController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IBraintreePaymentService _braintreePaymentService;
		private readonly IGolfEventManagerService _golfEventManagerService;

		public GolfController(ILogger iLogger, IBraintreePaymentService iBraintreePaymentService, 
			IGolfEventManagerService iGolfEventManagerService)
			: base(iLogger)
		{
			_logger = iLogger;
			_braintreePaymentService = iBraintreePaymentService;
			_golfEventManagerService = iGolfEventManagerService;
		}

		[AllowAnonymous]
		public ActionResult Index()
		{
			return View();
		}

		[AllowAnonymous]
		[ImportModelStateFromTempData]
		public ActionResult Register()
		{
			var token = _braintreePaymentService.GetClientToken(string.Empty);
			ViewData.Add("clientToken", token);
			ViewData.Add("merchantId", AdminSystemSettings.GetSetting("BraintreeMerchantId"));
			ViewData.Add("environment", AdminSystemSettings.GetSetting("BraintreeIsProduction"));
			ViewData.Add("recapPublicKey", AdminSystemSettings.GetSetting("RecaptchaPublicKey"));
			var register = new CreatePublic();
			register.States = ViewModel.Common.Lists.GetStateList();
			register.BanquetTicketCost = Decimal.Parse(AdminSystemSettings.GetSetting("GolfBanquetTicketCost"));
			register.GolfTicketCost = Decimal.Parse(AdminSystemSettings.GetSetting("GolfTicketCost"));
			return View(register);
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Register(CreatePublic register, FormCollection formCollection)
		{
			#region reCaptcha
			// Init the recaptcha processor to start verifying...
			IRecaptcha<RecaptchaV2Result> recaptcha = new RecaptchaV2(new RecaptchaV2Data() { Secret = AdminSystemSettings.GetSetting("RecaptchaPrivateKey") });

			// Verify the captcha
			var recaptchaResult = recaptcha.Verify();

			if (!recaptchaResult.Success)
			{
				_logger.Error("reCaptcha Authentication Failed");
				ModelState.AddModelError("", "captcha authentication failed - try again");
				return RedirectToAction("Register");
			} 
			#endregion

			// payment information in an encrypted string rather than sending the payment information to the server from the client
			var nonce = formCollection["payment_method_nonce"];
			var deivceData = formCollection["device_data"];

			if (string.IsNullOrEmpty(nonce))
			{
				ModelState.AddModelError("", "incomplete payment information provided");
			}
			else
			{
				// remove comma at end of value
				// comma appears to be a result of two payment_method_nonce fields in the form, one with a value and one without.
				if (nonce.EndsWith(","))
				{
					nonce = nonce.TrimEnd(',');
				}
				if (nonce.StartsWith(","))
				{
					nonce = nonce.TrimStart(',');
				}

				try
				{
					// get model state errors
					var errors = ModelState.Values.SelectMany(v => v.Errors);

					// if paying with a credit card the fields for credit card number/cvs/month/year will be invalid because we do not send them to the server
					// so count the errors on the field validation that do not start with 'card ' (comes from the property attributes in the model class donation.cs)
					// TODO validate if this is still needed - all card validation has been removed b/c client side validation requires 'name' properties
					//      which have been removed for PCI compliance. 
					var errorCount = errors.Count(m => !m.ErrorMessage.StartsWith("card "));

					if (errorCount == 0)
					{
						register.Nonce = nonce;
						register.DeviceData = deivceData;

						//register.AddedByUserId = User.Identity.GetUserId();

						var result = await _golfEventManagerService.RegisterForTournamentPublic(register);
						if (result.Succeeded)
						{
							return RedirectToAction("RegisterThankyou");
						}
						foreach (var error in result.Errors)
						{
							ModelState.AddModelError("", error);
						}
						return RedirectToAction("Register");
					}
				}
				catch (Exception ex)
				{
					//Log the error (uncomment dex variable name and add a line here to write a log.
					ModelState.AddModelError("", "Unable to process golf registration. Try again, and if the problem persists contact us.");

					_logger.Error(ex, "Golf Registration error {0}", ex.Message);
				}
			}
			return RedirectToAction("Register");
		}

		[AllowAnonymous]
		public ActionResult RegisterThankyou()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Sponsorships()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Sponsors()
		{
			return View();
		}

		[AllowAnonymous]
		[ImportModelStateFromTempData]
		public ActionResult Sponsor()
		{
			return View();
		}

		[HttpPost]
		[AllowAnonymous]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public ActionResult Sponsor(FormCollection form)
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult SponsorThankYou()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult RaffleAuction()
		{
			return View();
		}

		[AllowAnonymous]
		public ActionResult Support()
		{
			return View();
		}
	}
}