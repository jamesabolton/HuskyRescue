using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Admin.Roles;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminRolesManagerService
	{
		/// <summary>
		/// Get a list of roles to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the user roles</returns>
		Task<List<List>> GetRolesListAsync();

		/// <summary>
		/// Get all resources with those for the given role marked as selected
		/// </summary>
		/// <param name="roleId">role roleId</param>
		/// <param name="includeOtherResources">indicate if resources not assigned to this role should be included</param>
		/// <returns>list of resources</returns>
		List<RoleResource> GetRoleResourcesListAsync(string roleId, bool includeOtherResources = true);

		/// <summary>
		/// Retrieve the full details of a user role
		/// </summary>
		/// <param name="id">id of the role</param>
		/// <returns>Detail object for view model</returns>
		Task<Detail> GetRoleDetailAsync(string id);

		/// <summary>
		/// Add a new user role to the database
		/// </summary>
		/// <param name="role">user role view model object</param>
		/// <returns>RequestResult indicating success</returns>
		Task<RequestResult> AddRoleAsync(Create role);

		/// <summary>
		/// Update role in database
		/// </summary>
		/// <param name="role">role view model to update in database</param>
		/// <returns>RequestResult object indicating success or error</returns>
		Task<RequestResult> UpdateRoleAsync(Edit role);
	}
}