using Microsoft.Owin.Security.DataProtection;

namespace HuskyRescue.BusinessLogic.Identity
{
	public static class Startup
	{
		public static IDataProtectionProvider DataProtectionProvider { get; set; }
	}
}
