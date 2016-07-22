using Microsoft.AspNet.Identity;
using System.Diagnostics;
using System.Threading.Tasks;
using Twilio;

namespace HuskyRescue.BusinessLogic
{
	public class SmsService : IIdentityMessageService, HuskyRescue.BusinessLogic.ISmsService
	{
		public Task SendAsync(IdentityMessage message)
		{
			var twilioClient = new TwilioRestClient("ACb2cb2c7c184dd2bb13f8ccc26b33922b", "af56f40067961c28e5de70d0ee45bc1f");
			var result = twilioClient.SendMessage("+12147176733", message.Destination, message.Body);

			// Status is one of Queued, Sending, Sent, Failed or null if the number is not valid
			Trace.TraceInformation(result.Status);

			// Twilio doesn't currently have an async API, so return success.
			return Task.FromResult(0);
		}
	}
}
