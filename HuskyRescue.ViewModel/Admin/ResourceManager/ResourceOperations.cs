using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public enum ResourceOperations
	{
		[Display(Name="None")]
		None = 0,
		[Display(Name = "Create")]
		Create = 1,
		[Display(Name = "Read")]
		Read = 2,
		[Display(Name = "Update")]
		Update = 4,
		[Display(Name = "Delete")]
		Delete = 8,
		[Display(Name = "Execute")]
		Execute = 16,
		[Display(Name = "All")]
		All = 31
	}
}
