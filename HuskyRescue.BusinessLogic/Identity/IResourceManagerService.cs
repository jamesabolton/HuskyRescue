using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Admin.ResourceManager;

namespace HuskyRescue.BusinessLogic.Identity
{
	public interface IResourceManagerService
	{
		/// <summary>
		/// Get a list of resources to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the site's resources</returns>
		Task<List<ResourceIndex>> GetResourceListAsync();

		/// <summary>
		/// Add a new Resource to the database
		/// </summary>
		/// <param name="id">Optional key; GUID as string - will create if Null or Empty</param>
		/// <param name="name">Name of the result</param>
		/// <param name="operations">Operations allowed on the resource</param>
		/// <returns>RequestResult indicating success and if successful the key is returned</returns>
		Task<RequestResult> AddResourceAsync(string id, string name, int operations);

		Task<RequestResult> AddResourceAsync(ResourceCreate resourceViewModel);

		/// <summary>
		/// Determine if a resource exists
		/// </summary>
		/// <param name="id">resource key</param>
		/// <returns>RequestResult</returns>
		Task<RequestResult> ResourceExistsAsync(string id);

		/// <summary>
		/// Add resource to a role
		/// </summary>
		/// <param name="resourceId">key of the resource</param>
		/// <param name="operation">operations available to the role for this resource</param>
		/// <param name="roleId">key of the role to own the resource</param>
		/// <returns>RequestResult</returns>
		Task<RequestResult> AddRoleResourceAsync(string resourceId, ResourceOperations operation, string roleId);

		string GetRolesAsCsv(string resourceId, ResourceOperations operation);

		/// <summary>
		/// Check if a given resource and operation are assigned to a set of roles (typically a user's)
		/// </summary>
		/// <param name="resourceId">resource ID</param>
		/// <param name="roleNames">array of user's roles</param>
		/// <returns>true or false</returns>
		bool Authorize(string resourceId, string[] roleNames);

		/// <summary>
		/// Retrieve the full details of a Resource which includes which roles are using the resource
		/// </summary>
		/// <param name="id">resource id</param>
		/// <returns><see cref="ResourceDetail"/></returns>
		Task<ResourceDetail> GetResourceDetailAsync(string id);
	}
}