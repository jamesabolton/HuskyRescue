using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Admin.Roles
{
	public class List
	{
		public string Id { get; set; }

		[Display(Name = "Role Name")]
		public string Name { get; set; }
	}
}
