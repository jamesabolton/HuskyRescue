using System;
namespace HuskyRescue.BusinessLogic
{
	interface ISmsService
	{
		System.Threading.Tasks.Task SendAsync(Microsoft.AspNet.Identity.IdentityMessage message);
	}
}
