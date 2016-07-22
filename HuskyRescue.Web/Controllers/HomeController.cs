using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web.Mvc;
using HuskyRescue.BusinessLogic;
using HuskyRescue.ViewModel.Home;
using HuskyRescue.Web.Filters;
using GoogleRecaptcha;
using Serilog;

namespace HuskyRescue.Web.Controllers
{
	[AllowAnonymous]
	public class HomeController : BaseController
	{
		private readonly ILogger _logger;
		private readonly IFacebookManagerService _facebookManagerService;

		public HomeController(ILogger logger, IFacebookManagerService iFacebookManagerService)
			: base(logger)
		{
			_logger = logger;
			_facebookManagerService = iFacebookManagerService;
		}

		public ActionResult Index()
		{
			var events = _facebookManagerService.GetEvents();
			return View(events);
		}

		public ActionResult About()
		{
			ViewBag.Message = "Your application description page.";

			return View();
		}

		[ImportModelStateFromTempData]
		public ActionResult Contact()
		{
			ViewData.Add("recapPublicKey", AdminSystemSettings.GetSetting("RecaptchaPublicKey"));
			var contact = new Contact { ContactReasonList = ViewModel.Common.Lists.GetContactTypeList() };
			return View(contact);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[ExportModelStateToTempData]
		public async Task<ActionResult> Contact(FormCollection form, Contact contact)
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
				return RedirectToAction("Contact");
			}
			#endregion

			if (ModelState.IsValid)
			{
				//TODO move logic to Business project
				try
				{
					foreach (var fileName in Request.Files)
					{
						var file = Request.Files[(string)fileName];
						if (file != null && file.ContentLength > 0)
						{
							contact.PostedFiles.Add(file);
						}
					}

					#region send emails

					var subject = string.Join(" ", "TXHR Website Contact From", contact.FullName);
					var bodyText = string.Join(" ", "Thank you for contacting TXHR. We will respond, if needed, within 48 hours");

					if (contact.ContactReasonId == "2" || contact.ContactReasonId == "3")
					{
						bodyText = @"
Thank you for your email, we will do our best to get back to you as soon as possible but please be patient as we are volunteers with full time jobs and families. If you have not heard from us within 7 days please email us again. Also, feel free to call us at 877-894-8759.

Disclaimer: Please be aware that we are unable to take every husky needing help and we are unable to take mix breed Huskies because our adopters are seeking pure bred Huskies. If you are an owner wishing to surrender your Husky, be aware that we give priority to shelter dogs, as we are often their last hope. If we are able to help your dog please be aware that there is a $100 owner surrender fee that goes to help cover the costs we absorb by taking your dog. 
";
					}

					string groupEmail;
					switch (contact.ContactReasonId)
					{
						case "7":
							groupEmail = AdminSystemSettings.GetSetting("Email-Admin");
							break;
						case "2":
						case "3":
							groupEmail = AdminSystemSettings.GetSetting("Email-Intake");
							break;
						default:
							groupEmail = AdminSystemSettings.GetSetting("Email-Contact");
							break;
					}

					EmailService emailService;
					if (!string.IsNullOrEmpty(contact.EmailAddress))
					{
						//emailService = new EmailService(EmailService.EmailFrom.Contact, contact.EmailAddress, null, null, subject, bodyText);
						emailService = new EmailService(EmailService.FromContactMailAddress, new MailAddress(contact.EmailAddress, contact.FullName), subject, bodyText);
						emailService.Tag = "contact";
						await emailService.SendAsync();
					}

					bodyText = string.Join(" ", "Contact from", contact.FullName, ". ");
					if (!string.IsNullOrEmpty(contact.EmailAddress))
					{
						bodyText += Environment.NewLine + contact.EmailAddress;
					}
					if (!string.IsNullOrEmpty(contact.Number))
					{
						bodyText += Environment.NewLine + contact.Number;
					}
					if (contact.IsEmailable)
					{
						bodyText += Environment.NewLine + "This person opted in to receive newsletters, promotions, and event information via email in the future";
					}
					bodyText += Environment.NewLine + Environment.NewLine + "-----------------------------------------------";
					bodyText += Environment.NewLine + contact.Message;
					bodyText += Environment.NewLine + "-----------------------------------------------";

					var attachments = new List<Attachment>();

					if (contact.PostedFiles != null && contact.PostedFiles.Count > 0)
					{
						foreach (var file in contact.PostedFiles)
						{
							attachments.Add(new Attachment(file.InputStream, file.FileName, file.ContentType));
						}
					}

					//emailService = new EmailService(EmailService.EmailFrom.Admin, groupEmail, contact.EmailAddress, contact.FullName, subject, bodyText, null, attachments);
					emailService = new EmailService(contact.EmailAddress, groupEmail, subject, bodyText, false, contact.FullName, "", attachments);
					emailService.Tag = "contact";
					await emailService.SendAsync();

					#endregion

					return RedirectToAction("ThankYou");
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("", "Unable to process message. Try again, and if the problem persists contact us by phone or social media.");

					_logger.Error(ex, "Contact error {0}", ex.Message);
				}
			}
			return RedirectToAction("Contact");
		}

		public ActionResult ThankYou()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<ActionResult> EmailSignUp(FormCollection form)
		{
			try
			{
				var email = form.Get("email");

				if (string.IsNullOrEmpty(email))
				{
					return RedirectToAction("Index");
				}

				if (!IsValidEmail(email))
				{
					return RedirectToAction("Index");
				}

				#region send emails

				const string subject = "TXHR Email Sign Up";
				var bodyText = "Thank you for signing up to receive emails from Texas Husky Rescue via our website. We will add you to our database to receive emails regarding donation drives, events, and opportunities to help. If at any time you wish to be removed from these emails you will be able to unsubscribe from them directly or you may send us an email and ask to be removed. Thank you!";

				//var emailService = new EmailService(EmailService.EmailFrom.Contact, email, subject, bodyText, null);
				var emailService = new EmailService(EmailService.FromWebAdminMailAddress, new MailAddress(email), subject, bodyText);
				await emailService.SendAsync();

				var groupEmail = AdminSystemSettings.GetSetting("Email-Contact");

				bodyText = "Please add the following email address to receive email from TXHR: " + email;

				//emailService = new EmailService(EmailService.EmailFrom.Contact, groupEmail, email, "", subject, bodyText, null);
				emailService = new EmailService(new MailAddress(email), EmailService.FromWebAdminMailAddress, subject, bodyText);
				await emailService.SendAsync();

				#endregion

				return RedirectToAction("Index");
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Unable to process message. Try again, and if the problem persists contact us by phone or social media.");

				_logger.Error(ex, "Contact error {0}", ex.Message);
			}
			return RedirectToAction("Index");
		}

		public ActionResult Sponsors()
		{
			return View();
		}

		public ActionResult Partners()
		{
			return View();
		}

		public ActionResult BoardOfDirectors()
		{
			return View();
		}

		public ActionResult Reports()
		{
			return View();
		}

		public ActionResult History()
		{
			return View();
		}

		public ActionResult Mission()
		{
			return View();
		}

		public ActionResult HuskyRehab()
		{
			return View();
		}

		public ActionResult Advocacy()
		{
			return View();
		}

		public ActionResult Volunteer()
		{
			return View();
		}

		public ActionResult Foster()
		{
			return View();
		}

		public ActionResult Sponsor()
		{
			return View();
		}

		public ActionResult Policies()
		{
			return View();
		}

		public ActionResult Stream()
		{
			return View();
		}

		private bool _invalid;
		private bool IsValidEmail(string strIn)
		{
			_invalid = false;
			if (String.IsNullOrEmpty(strIn))
				return false;

			// Use IdnMapping class to convert Unicode domain names. 
			try
			{
				strIn = Regex.Replace(strIn, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}

			if (_invalid)
				return false;

			// Return true if strIn is in valid e-mail format. 
			try
			{
				return Regex.IsMatch(strIn,
					  @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
					  @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$",
					  RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
			}
			catch (RegexMatchTimeoutException)
			{
				return false;
			}
		}

		private string DomainMapper(Match match)
		{
			// IdnMapping class with default property values.
			var idn = new IdnMapping();

			var domainName = match.Groups[2].Value;
			try
			{
				domainName = idn.GetAscii(domainName);
			}
			catch (ArgumentException)
			{
				_invalid = true;
			}
			return match.Groups[1].Value + domainName;
		}
	}
}