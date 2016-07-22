using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace HuskyRescue.Web.HtmlHelpers
{
	public static class Common
	{
		public static MvcHtmlString Image(this HtmlHelper html, string src, string alt = "", object htmlAttributes = null)
		{
			if (src.StartsWith("~"))
				src = VirtualPathUtility.ToAbsolute(src);

			var tb = new TagBuilder("img");
			tb.MergeAttribute("src", src);
			tb.MergeAttribute("alt", alt);
			tb.MergeAttributes(new RouteValueDictionary(htmlAttributes));
			return MvcHtmlString.Create(tb.ToString(TagRenderMode.SelfClosing));
		}
	}
}