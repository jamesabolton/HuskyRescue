using System.ComponentModel;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public interface IResourceDetail
	{
		string Id { get; set; }
		string Name { get; set; }

		[DisplayName("Operations Allowed")]
		string Operations { get; set; }

		[DisplayName("Associated Roles")]
		string AssociatedRoleNames { get; set; }
	}
}