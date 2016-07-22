using System.ComponentModel;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public class ResourceDetail : IResourceDetail
	{
		public string Id { get; set; }

		public string Name { get; set; }

		[DisplayName("Operations Allowed")]
		public string Operations { get; set; }

		[DisplayName("Associated Roles")]
		public string AssociatedRoleNames { get; set; }
	}
}
