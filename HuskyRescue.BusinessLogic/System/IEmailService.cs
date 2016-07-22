using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using Microsoft.AspNet.Identity;

namespace HuskyRescue.BusinessLogic
{
	public interface IEmailService
	{
		MailMessage Message { get; set; }
		bool TrackEmailOpens { get; set; }
		List<KeyValuePair<string, string>> Attachments { get; set; }
		EmailService.EmailFrom From { get; set; }
		Task<RequestResult> SendAsync();
		Task SendAsync(IdentityMessage message);
	}
}