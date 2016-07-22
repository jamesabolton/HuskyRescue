using HuskyRescue.BusinessLogic.Identity;
using HuskyRescue.ViewModel.AccountManager;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	public class AccountManagerController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IAuthenticationManager _authenticationManager;
		private readonly IIdentityManagerService _identityManagerService;

		public AccountManagerController(
			IIdentityManagerService identityManagerService, 
			ILogger logger,
			IAuthenticationManager authenticationManager)
			: base(logger)
		{
			_logger = logger;
			_identityManagerService = identityManagerService;
			_authenticationManager = authenticationManager;
		}

		//
		// GET: /Manage/Index
		public async Task<ActionResult> Index(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
				: message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
				: message == ManageMessageId.SetTwoFactorSuccess ? "Your two-factor authentication provider has been set."
				: message == ManageMessageId.Error ? "An error has occurred."
				: message == ManageMessageId.AddPhoneSuccess ? "Your phone number was added."
				: message == ManageMessageId.RemovePhoneSuccess ? "Your phone number was removed."
				: "";

			var model = await _identityManagerService.GetManageAccountIndexView(User.Identity.GetUserId());

			return View(model);
		}

		//
		// GET: /Manage/RemoveLogin
		public async Task<ActionResult> RemoveLogin()
		{
			var userId = User.Identity.GetUserId();
			var hasPassword = await _identityManagerService.HasPassword(userId);
			var linkedAccounts = await _identityManagerService.GetLoginsForUser(userId);
			ViewBag.ShowRemoveButton = hasPassword || linkedAccounts.Count > 1;
			return View(linkedAccounts);
		}

		//
		// POST: /Manage/RemoveLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> RemoveLogin(string loginProvider, string providerKey)
		{
			bool success = await _identityManagerService.RemoveExternalLogin(loginProvider, providerKey, User.Identity.GetUserId());

			ManageMessageId message = success == true ? ManageMessageId.RemoveLoginSuccess : ManageMessageId.Error;

			return RedirectToAction("ManageLogins", new { Message = message });
		}

		//
		// GET: /Manage/AddPhoneNumber
		public ActionResult AddPhoneNumber()
		{
			return View();
		}

		//
		// POST: /Manage/AddPhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> AddPhoneNumber(AddPhoneNumberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			await _identityManagerService.SendPhoneNumberVerificationSms(User.Identity.GetUserId(), model.Number);

			return RedirectToAction("VerifyPhoneNumber", new { PhoneNumber = model.Number });
		}

		//
		// GET: /Manage/VerifyPhoneNumber
		public ActionResult VerifyPhoneNumber(string phoneNumber)
		{
			//var code = await __identityManagerService.SendPhoneNumberVerificationSms(User.Identity.GetUserId(), phoneNumber);
			// Send an SMS through the SMS provider to verify the phone number
			return phoneNumber == null ? View("Error") : View(new VerifyPhoneNumberViewModel { PhoneNumber = phoneNumber });
		}

		//
		// POST: /Manage/VerifyPhoneNumber
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> VerifyPhoneNumber(VerifyPhoneNumberViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}

			bool success = await _identityManagerService.VerifyPhoneNumberSmsCode(User.Identity.GetUserId(), model.PhoneNumber, model.Code);

			if (success)
			{
				return RedirectToAction("Index", new { Message = ManageMessageId.AddPhoneSuccess });
			}

			// If we got this far, something failed, redisplay form
			ModelState.AddModelError("", "Failed to verify phone");
			return View(model);
		}

		//
		// GET: /Manage/RemovePhoneNumber
		public async Task<ActionResult> RemovePhoneNumber()
		{
			bool success = await _identityManagerService.RemovePhoneNumber(User.Identity.GetUserId());

			if (!success)
			{
				return RedirectToAction("Index", new { Message = ManageMessageId.Error });
			}

			return RedirectToAction("Index", new { Message = ManageMessageId.RemovePhoneSuccess });
		}

		//
		// POST: /Manage/EnableTwoFactorAuthentication
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<ActionResult> EnableTwoFactorAuthentication()
		{
			await _identityManagerService.EnableTwoFactorAuth(User.Identity.GetUserId());
			return RedirectToAction("Index", "AccountManager");
		}

		//
		// POST: /Manage/DisableTwoFactorAuthentication
		[HttpPost, ValidateAntiForgeryToken]
		public async Task<ActionResult> DisableTwoFactorAuthentication()
		{
			await _identityManagerService.DisableTwoFactorAuth(User.Identity.GetUserId());
			return RedirectToAction("Index", "AccountManager");
		}

		//
		// GET: /Manage/ChangePassword
		public ActionResult ChangePassword()
		{
			return View();
		}

		//
		// POST: /Manage/ChangePassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
		{
			if (!ModelState.IsValid)
			{
				return View(model);
			}
			var result = await _identityManagerService.ChangeUserPassword(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
			}
			AddErrors(result.Errors);
			return View(model);
		}

		//
		// GET: /Manage/SetPassword
		public ActionResult SetPassword()
		{
			return View();
		}

		//
		// POST: /Manage/SetPassword
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> SetPassword(SetPasswordViewModel model)
		{
			if (ModelState.IsValid)
			{
				var result = await _identityManagerService.SetUserPassword(User.Identity.GetUserId(), model.NewPassword);
				if (result.Succeeded)
				{
					return RedirectToAction("Index", new { Message = ManageMessageId.SetPasswordSuccess });
				}
				AddErrors(result.Errors);
			}

			// If we got this far, something failed, redisplay form
			return View(model);
		}

		//
		// GET: /Manage/ManageLogins
		public async Task<ActionResult> ManageLogins(ManageMessageId? message)
		{
			ViewBag.StatusMessage =
				message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
				: message == ManageMessageId.Error ? "An error has occurred."
				: "";
			var userId = User.Identity.GetUserId();
			var userExists = await _identityManagerService.CheckUserExist(userId);

			if (!userExists)
			{
				return View("Error");
			}
			var userLogins = await _identityManagerService.GetLoginsForUser(userId);
			var otherLogins = _identityManagerService.GetOtherLoginsFotUser(userLogins);
			var hasPassword = await _identityManagerService.HasPassword(userId);
			ViewBag.ShowRemoveButton = hasPassword || userLogins.Count > 1;
			return View(new ManageLoginsViewModel
			{
				CurrentLogins = userLogins,
				OtherLogins = otherLogins
			});
		}

		//
		// POST: /Manage/LinkLogin
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult LinkLogin(string provider)
		{
			// Request a redirect to the external login provider to link a login for the current user
			return new AccountController.ChallengeResult(provider, Url.Action("LinkLoginCallback", "AccountManager"), User.Identity.GetUserId());
		}

		//
		// GET: /Manage/LinkLoginCallback
		public async Task<ActionResult> LinkLoginCallback()
		{
			var loginInfo = await _authenticationManager.GetExternalLoginInfoAsync(XsrfKey, User.Identity.GetUserId());
			if (loginInfo == null)
			{
				return RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
			}
			var result = await _identityManagerService.LinkLogin(User.Identity.GetUserId(), loginInfo);
			return result.Succeeded ? RedirectToAction("ManageLogins") : RedirectToAction("ManageLogins", new { Message = ManageMessageId.Error });
		}

		#region Helpers
		// Used for XSRF protection when adding external logins
		private const string XsrfKey = "XsrfId";

		private void AddErrors(IEnumerable<string> errors)
		{
			foreach (var error in errors)
			{
				ModelState.AddModelError("", error);
			}
		}

		public enum ManageMessageId
		{
			AddPhoneSuccess,
			ChangePasswordSuccess,
			SetTwoFactorSuccess,
			SetPasswordSuccess,
			RemoveLoginSuccess,
			RemovePhoneSuccess,
			Error
		}

		#endregion
	}
}