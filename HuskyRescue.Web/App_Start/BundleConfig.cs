using System.Web.Optimization;

namespace HuskyRescue.Web
{
	public class BundleConfig
	{
		// For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
		public static void RegisterBundles(BundleCollection bundles)
		{
			bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
						"~/Content/Scripts/jQuery/jquery-{version}.js",
						"~/Content/Scripts/jQuery/jquery-migrate-{version}.js"));

			bundles.Add(new ScriptBundle("~/bundles/jqueryplugins").Include(
				//"~/Content/Scripts/jQuery/jquery.unobtrusive*",
						"~/Content/Scripts/jQuery/jquery.validate.js",
						"~/Content/Scripts/jQuery/jquery.validate.extramethods.js",
						"~/Content/Scripts/jQuery/jquery.validate.unobtrusive.js",
				//"~/Content/Scripts/jQuery/jquery.validate.txhr.js",
						"~/Content/Scripts/jQuery/jquery.mask.js",
						"~/Content/Scripts/jQuery/jquery.blockUI.js",
						"~/Content/Scripts/jQuery/jquery.plugin.js",
						"~/Content/Scripts/jQuery/jquery.countdown.js",
						"~/Content/Scripts/jQuery/php-date-formatter.js",
						"~/Content/Scripts/jQuery/jquery.datetimepicker.js",
						"~/Content/Scripts/Misc/expressive.annotations.validate.js",
						"~/Content/Scripts/Misc/expressive.annotations.validate.txhr.js"));

			// Use the development version of Modernizr to develop with and learn from. Then, when you're
			// ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
			bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
						"~/Content/Scripts/Misc/modernizr-*"));


			bundles.Add(new StyleBundle("~/Content/css").Include(
						"~/Content/Styles/iconomous.css",
						"~/Content/Styles/font-awesome.css",
						"~/Content/Styles/jquery.countdown.css",
						"~/Content/Styles/jquery.datetimepicker.css"));


			bundles.Add(new ScriptBundle("~/bundles/dropzone").Include(
					 "~/Content/Scripts/dropzone/dropzone.js"));

			bundles.Add(new StyleBundle("~/Content/css/dropzone").Include(
					 "~/Content/Styles/dropzone/basic.css",
					 "~/Content/Styles/dropzone/dropzone.css"));

			#region Foundation Bundles

			bundles.Add(Foundation.Styles());

			bundles.Add(Foundation.Scripts());

			#endregion

			#region Controller
			#region Home
			bundles.Add(new ScriptBundle("~/bundles/Home-Index").Include(
				"~/Content/Scripts/jQuery/jquery.easing.{version}.js", 
				"~/Content/Scripts/Misc/camera.js",
				"~/Content/Scripts/Controller/Home-Index.js"));
			bundles.Add(new StyleBundle("~/Content/css/Home-Index").Include(
				"~/Content/Styles/camera/camera.css"));

			bundles.Add(new ScriptBundle("~/bundles/Adoption-Index").Include(
				"~/Content/Scripts/jQuery/jquery.easing.{version}.js",
				"~/Content/Scripts/Misc/camera.js",
				"~/Content/Scripts/Controller/Adoption-Index.js"));
			bundles.Add(new StyleBundle("~/Content/css/Adoption-Index").Include(
				"~/Content/Styles/camera/camera.css"));

			bundles.Add(new ScriptBundle("~/bundles/Adoption-Apply").Include(
				"~/Content/Scripts/Controller/Adoption-Apply.js"));

			bundles.Add(new ScriptBundle("~/bundles/Golf-Register").Include(
				"~/Content/Scripts/Controller/Golf-Register.js"));

			bundles.Add(new ScriptBundle("~/bundles/RoughRiders-Register").Include(
				"~/Content/Scripts/Controller/RoughRiders-Register.js"));

			bundles.Add(new ScriptBundle("~/bundles/Donate-Index").Include(
				"~/Content/Scripts/Controller/Donate-Index.js"));
			#endregion

			#region AdminEventsSponsors
			bundles.Add(new ScriptBundle("~/bundles/AdminEventsSponsors-Create").Include(
				"~/Content/Scripts/Controller/AdminEventsSponsors-Create.js"));
			#endregion

			#endregion
		}
	}
}