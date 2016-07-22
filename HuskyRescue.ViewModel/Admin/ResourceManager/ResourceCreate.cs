using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public class ResourceCreate : IResourceCreate
	{
		[Required, StringLength(128)]
		public string Controller { get; set; }
		[Required, StringLength(120)]
		public string ControllerAction { get; set; }
		[Required]
		public string Action { get; set; }

		public SelectList Actions { get; set; }

		[DisplayName("Readable")]
		public bool IsRead { get; set; }
		[DisplayName("Creatable")]
		public bool IsCreate { get; set; }
		[DisplayName("Updateable")]
		public bool IsUpdate { get; set; }
		[DisplayName("Deletable")]
		public bool IsDelete { get; set; }
		[DisplayName("Executable")]
		public bool IsExecute { get; set; }

		public ResourceCreate()
		{
			Actions = new SelectList(new List<string> { "get", "post" });
		}
	}
}
