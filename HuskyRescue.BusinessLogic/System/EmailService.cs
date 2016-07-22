using System;
using System.IO;
using HuskyRescue.ViewModel;
using Microsoft.AspNet.Identity;
using PostmarkDotNet;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using Serilog;
using Attachment = System.Net.Mail.Attachment;

namespace HuskyRescue.BusinessLogic
{
	public class EmailService : IIdentityMessageService, IEmailService
	{
		public ILogger Logger { get; set; }

		private MailMessage _message = new MailMessage();
		public MailMessage Message
		{
			get { return _message; }
			set { _message = value; }
		}

		public string Tag { get; set; }

		private bool _trackEmailOpens = true;
		public bool TrackEmailOpens
		{
			get { return _trackEmailOpens; }
			set { _trackEmailOpens = value; }
		}

		private List<KeyValuePair<string, string>> _attachmentsList = new List<KeyValuePair<string, string>>();
		public List<KeyValuePair<string, string>> Attachments
		{
			get { return _attachmentsList; }
			set { _attachmentsList = value; }
		}

		public EmailFrom From { get; set; }

		public enum EmailFrom
		{
			Golf,
			RoughRider,
			Contact,
			Intake,
			WebAdmin,
			Board
		}

		public static readonly MailAddress FromContactMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-Contact"), "TXHR Contact");
		public static readonly MailAddress FromIntakeMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-Intake"), "TXHR Intake");
		public static readonly MailAddress FromBoardMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-Board"), "TXHR Board of Directors");
		public static readonly MailAddress FromWebAdminMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-WebAdmin"), "TXHR Website Admin");
		public static readonly MailAddress FromAdminMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-Admin"), "TXHR Admin");
		public static readonly MailAddress FromGolfMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-Golf"), "TXHR Golf Tournament Committee");
		public static readonly MailAddress FromRoughRiderMailAddress = new MailAddress(AdminSystemSettings.GetSetting("Email-RoughRiders"), "TXHR Roughriders Event Committee");

		public EmailService()
		{
			
		}

		public EmailService(string from, string to, string subject, string body, bool isBodyHtml = false, string fromName = "", string toName = "", IEnumerable<Attachment> attachments = null)
		{
			//From = from;

			var isInternalMail = string.Equals(to.Split('@')[1], "texashuskyrescue.org");

			if (isInternalMail)
			{
				// Internal example
				//	to		contact@texashuskyrescue.org
				//	from	web@texashuskyrescue.org
				//	replyto	test@gmail.com
				SetupeMailMessage(FromWebAdminMailAddress, GetInternalMailAddress(to), new MailAddress(from, fromName), subject, body, isBodyHtml, attachments);
			}
			else
			{
				// External example
				//	to		test@gmail.com
				//	from	contact@texashuskyrescue.org
				//	replyto	n/a
				SetupeMailMessage(new MailAddress(from, fromName), new MailAddress(to, toName), null, subject, body, isBodyHtml, attachments);
			}
		}

		public EmailService(MailAddress from, MailAddress to, string subject, string body, bool isBodyHtml = false, IEnumerable<Attachment> attachments = null)
		{
			//From = from;

			var isInternalMail = string.Equals(to.Address.Split('@')[1], "texashuskyrescue.org");

			if (isInternalMail)
			{
				// Internal example
				//	to		contact@texashuskyrescue.org
				//	from	web@texashuskyrescue.org
				//	replyto	test@gmail.com
				SetupeMailMessage(FromWebAdminMailAddress, to, from, subject, body, isBodyHtml, attachments);
			}
			else
			{
				// External example
				//	to		test@gmail.com
				//	from	contact@texashuskyrescue.org
				//	replyto	n/a
				SetupeMailMessage(from, to, null, subject, body, isBodyHtml, attachments);
			}
		}

		private static MailAddress GetInternalMailAddress(string address)
		{
			switch (address.ToLower())
			{
				case "board@texashuskyrescue.org":
					return FromBoardMailAddress;
				case "contact@texashuskyrescue.org":
					return FromContactMailAddress;
				case "intake@texashuskyrescue.org":
					return FromIntakeMailAddress;
				//case "roughrider@texashuskyrescue.org":
				//	return fromRoughRiderAddress;
				case "golf@texashuskyrescue.org":
					return FromGolfMailAddress;
				case "web-admin@texashuskyrescue.org":
					return FromWebAdminMailAddress;
				default:
					// bug Gmail requires the 'from' to not be the same as the 'to' or the 'reply-to' is ignored
					return FromAdminMailAddress;
			}
		}


		//public EmailService(EmailFrom from, string to, string subject, string textBody, string htmlBody, IEnumerable<Attachment> attachments)
		//{
		//	From = from;
		//	SetupeMailMessage(to, subject, textBody, htmlBody, attachments);
		//}

		//public EmailService(EmailFrom from, string to, string subject, string textBody, string htmlBody, Attachment attachment)
		//{
		//	From = from;
		//	var a = new List<Attachment> { attachment };
		//	SetupeMailMessage(to, subject, textBody, htmlBody, a);
		//}

		//public EmailService(EmailFrom from, string to, string subject, string textBody, string htmlBody)
		//{
		//	From = from;
		//	SetupeMailMessage(to, subject, textBody, htmlBody, null);
		//}

		//public EmailService(EmailFrom from, string to, string replyToEmail, string replayToName, string subject, string textBody, string htmlBody, IEnumerable<Attachment> attachments)
		//{
		//	From = from;
		//	SetupeMailMessage(to, replyToEmail, replayToName, subject, textBody, htmlBody, attachments);
		//}

		//public EmailService(EmailFrom from, string to, string replyToEmail, string replayToName, string subject, string textBody, string htmlBody, Attachment attachment)
		//{
		//	From = from;
		//	var a = new List<Attachment> { attachment };
		//	SetupeMailMessage(to, replyToEmail, replayToName, subject, textBody, htmlBody, a);
		//}

		//public EmailService(EmailFrom from, string to, string replyToEmail, string replayToName, string subject, string textBody, string htmlBody)
		//{
		//	From = from;
		//	SetupeMailMessage(to, replyToEmail, replayToName, subject, textBody, htmlBody, null);
		//}

		//private void SetupeMailMessage(string to, string subject, string textBody, string htmlBody, IEnumerable<Attachment> attachments)
		//{
		//	SetupeMailMessage(to, null, null, subject, textBody, htmlBody, attachments);
		//}

		private void SetupeMailMessage(MailAddress from, MailAddress to, MailAddress replyTo, string subject, string body, bool isBodyHtml, IEnumerable<Attachment> attachments = null)
		{
			Message.IsBodyHtml = isBodyHtml;
			Message.Body = body;
			Message.To.Add(to);
			if (replyTo != null)
			{
				Message.ReplyToList.Add(replyTo);
			}
			Message.From = from;
			Message.Subject = subject;

			if (attachments == null) return;
			foreach (var attachment in attachments)
			{
				Message.Attachments.Add(attachment);
			}
		}

		public async Task<RequestResult> SendAsync()
		{
			var result = new RequestResult();

			var client = new PostmarkClient(AdminSystemSettings.GetSetting("PostMarkKey"));

			var postmarkMessage = new PostmarkMessage();
			postmarkMessage.From = Message.From.ToString();
			postmarkMessage.To = Message.To.ToString();
			postmarkMessage.Tag = Tag;

			if (Message.ReplyToList.Count > 0)
			{
				postmarkMessage.ReplyTo = Message.ReplyToList[0].ToString();
			}

			if (Message.IsBodyHtml)
			{
				postmarkMessage.HtmlBody = Message.Body;
			}
			else
			{
				postmarkMessage.TextBody = Message.Body;
			}

			postmarkMessage.Subject = Message.Subject;

			// attachments
			foreach (var attachment in Message.Attachments)
			{
				string contentString;
				using (var memoryStream = new MemoryStream())
				{

					attachment.ContentStream.CopyTo(memoryStream);
					var streamBytes = memoryStream.ToArray();
					contentString = Convert.ToBase64String(streamBytes);

				}
				postmarkMessage.Attachments.Add(new PostmarkMessageAttachment
												{
													ContentId = attachment.ContentId,
													Name = attachment.Name,
													ContentType = attachment.ContentType.MediaType,
													Content = contentString
												});
			}

			try
			{
				var emailResult = await client.SendMessageAsync(postmarkMessage);

				if (emailResult.Status == PostmarkStatus.Success)
				{
					result.Succeeded = true;
				}
				else
				{
					result.Succeeded = false;
					var errorMessage = emailResult.ErrorCode + " -- " + emailResult.Message;
					result.Errors.Add(errorMessage);
				}
			}
			catch(Exception ex)
			{
				Logger.Error(ex, "Exception sending email {@Message}", Message);
			}

			return result;
		}

		#region IIdentityMessageService Members

		public Task SendAsync(IdentityMessage message)
		{
			//SetupeMailMessage(message.Destination, null, null, message.Subject, message.Body, string.Empty, null);
			SetupeMailMessage(FromContactMailAddress, new MailAddress(message.Destination), null, message.Subject, message.Body, false);
			var result = SendAsync();
			return result;
		}

		#endregion
	}
}
