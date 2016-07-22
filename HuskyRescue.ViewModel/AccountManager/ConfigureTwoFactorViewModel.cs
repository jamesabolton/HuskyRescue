using System.Collections.Generic;

namespace HuskyRescue.ViewModel.AccountManager
{
	public class ConfigureTwoFactorViewModel
	{
		public string SelectedProvider { get; set; }
		public ICollection<System.Web.Mvc.SelectListItem> Providers { get; set; }
	}
}
