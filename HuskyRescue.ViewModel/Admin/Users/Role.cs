using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Admin.Users
{
	public class Role
	{
		public bool Selected { get; set; }

		public string Id { get; set; }

		[Display(Name = "Role Name")]
		public string Name { get; set; }
	}
}
