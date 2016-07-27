using System.Data;
using System.Threading.Tasks;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Adoption;
using HuskyRescue.ViewModel.Common;
using HuskyRescue.Web.Filters;
using Serilog;
using System.Linq;
using System.Web.Mvc;
using System;

namespace HuskyRescue.Web.Controllers
{
    public class AdoptionController : BaseController
    {
        private readonly ILogger _logger;
        private readonly IApplicationManagerService _applicationManagerService;
        private readonly IRescueGroupsManagerService _rescueGroupsManagerService;
        private readonly IBraintreePaymentService _braintreePaymentService;

        public AdoptionController(ILogger iLogger, IApplicationManagerService appservice, IRescueGroupsManagerService iRescueGroupsManagerService, IBraintreePaymentService iBraintreePaymentService)
            : base(iLogger)
        {
            _logger = iLogger;
            _applicationManagerService = appservice;
            _rescueGroupsManagerService = iRescueGroupsManagerService;
            _braintreePaymentService = iBraintreePaymentService;
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
                          StudentTypeList = ViewModel.Common.Lists.GetStudentTypeList(),
                          States = ViewModel.Common.Lists.GetStateList()
                      };

            #region Payment
            // Values needed for submitting a payment to BrainTree
            var token = _braintreePaymentService.GetClientToken(string.Empty);
            ViewData.Add("clientToken", token);
            ViewData.Add("merchantId", AdminSystemSettings.GetSetting("BraintreeMerchantId"));
            ViewData.Add("environment", AdminSystemSettings.GetSetting("BraintreeIsProduction"));
            app.ApplicationFeeAmount = Decimal.Parse(AdminSystemSettings.GetSetting("AdoptionApplicationFee"));
            #endregion

            return View(app);
        }

        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ExportModelStateToTempData]
        public async Task<ActionResult> Apply(FormCollection formCollection, Apply app)
        {
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
                    // so count the errors on the field validation that do not start with 'card ' (comes from the property attributes in the model class Apply.cs)
                    // TODO validate if this is still needed - all card validation has been removed b/c client side validation requires 'name' properties
                    //      which have been removed for PCI compliance. 
                    var errorCount = errors.Count(m => !m.ErrorMessage.StartsWith("card "));

                    if (errorCount == 0)
                    {
                        app.Nonce = nonce;
                        app.DeviceData = deivceData;

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