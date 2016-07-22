using System.Web.Optimization;

namespace HuskyRescue.Web
{
	public static class Foundation
	{
		public static Bundle Styles()
		{
			return new StyleBundle("~/Content/foundation/css").Include(
					   "~/Content/Styles/foundation/foundation.css",
					   "~/Content/Styles/foundation/foundation.mvc.css",
					   "~/Content/Styles/foundation/foundation-datepicker.css",
					   "~/Content/Styles/foundation/app.css");
		}

		public static Bundle Scripts()
		{
			return new ScriptBundle("~/bundles/foundation").Include(
					  "~/Content/Scripts/foundation/fastclick.js",
					  "~/Content/Scripts/jQuery/jquery.cookie.js",
					  "~/Content/Scripts/foundation/foundation.js",
					  "~/Content/Scripts/foundation/foundation.*",
					  "~/Content/Scripts/foundation/foundation-datepicker.js",
					  "~/Content/Scripts/foundation/app.js");
		}
	}
}