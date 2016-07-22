using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public interface IResourceCreate
	{
		[Required, StringLength(128)]
		string Controller { get; set; }

		[Required, StringLength(120)]
		string ControllerAction { get; set; }

		[Required]
		string Action { get; set; }

		SelectList Actions { get; set; }

		[DisplayName("Readable")]
		bool IsRead { get; set; }

		[DisplayName("Creatable")]
		bool IsCreate { get; set; }

		[DisplayName("Updateable")]
		bool IsUpdate { get; set; }

		[DisplayName("Deletable")]
		bool IsDelete { get; set; }

		[DisplayName("Executable")]
		bool IsExecute { get; set; }
	}
}