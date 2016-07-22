using System.Web.Mvc;
using HuskyRescue.Web.Filters;

namespace HuskyRescue.Web
{
	public class FilterConfig
	{
		public static void RegisterGlobalFilters(GlobalFilterCollection filters)
		{
			filters.Add(new HandleErrorExtendedAttribute());
			filters.Add(new AuthorizeAttribute());
#if RELEASE_CONFIG
			// only included in the release build to force https on clients
			filters.Add(new RequireHttpsAttribute());
#endif
		}
	}
}
