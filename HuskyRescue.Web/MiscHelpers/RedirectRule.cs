namespace HuskyRescue.Web.MiscHelpers
{
	/// <summary>
	/// Class that defines parameters for redirecting a user to a new URI based on the contents of the current URI.
	/// see here for more information http://www.bloggersworld.com/index.php/redirecting-old-urls-to-new-in-aspnet-mvc/
	/// </summary>
	public class RedirectRule
	{
		/// <summary>
		/// old URL substring to check for in current URI
		/// </summary>
		public string OldUrlContains;
		/// <summary>
		/// old URL substring to not check for in the current URI
		/// </summary>
		public string OldUrlContainsNot;
		/// <summary>
		/// new URI the to redirect user to
		/// </summary>
		public string NewUrl;
		/// <summary>
		/// if true then check for the OldUrl as exact match rather than substring
		/// </summary>
		public bool IsOldUrlContainsExact;

		/// <summary>
		/// initialize a new RedirectRule object
		/// </summary>
		/// <param name="strOldUrlContains">substring to check if exists in current URI</param>
		/// <param name="strOldUrlContainsNot">substring to check if not exists in current URI</param>
		/// <param name="strNewUrl">URI to redirect the user to</param>
		public RedirectRule(string strOldUrlContains, string strOldUrlContainsNot, string strNewUrl)
		{
			OldUrlContains = strOldUrlContains;
			OldUrlContainsNot = strOldUrlContainsNot;
			NewUrl = strNewUrl;
			IsOldUrlContainsExact = false;
		}

		/// <summary>
		/// initialize a new RedirectRule object
		/// </summary>
		/// <param name="strOldUrlContains">substring to check if exists in current URI</param>
		/// <param name="strOldUrlContainsNot">substring to check if not exists in current URI</param>
		/// <param name="strNewUrl">URI to redirect the user to</param>
		/// <param name="isExact">if true then check for the OldUrl as exact match rather than substring</param>
		public RedirectRule(string strOldUrlContains, string strOldUrlContainsNot, string strNewUrl, bool isExact)
		{
			OldUrlContains = strOldUrlContains;
			OldUrlContainsNot = strOldUrlContainsNot;
			NewUrl = strNewUrl;
			IsOldUrlContainsExact = isExact;
		}
	}
}