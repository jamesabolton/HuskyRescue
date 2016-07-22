using System.ComponentModel;

namespace HuskyRescue.ViewModel.Admin.ResourceManager
{
	public interface IResourceIndex
	{
		string Id { get; set; }
		string Name { get; set; }

		[DisplayName("Allowed Access")]
		string OperationNames { get; set; }
	}
}