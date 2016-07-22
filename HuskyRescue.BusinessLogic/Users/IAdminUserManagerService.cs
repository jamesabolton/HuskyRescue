using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Admin.Users;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminUserManagerService
	{
		/// <summary>
		/// Get a list of roles to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the user roles</returns>
		Task<List<List>> GetUsersListAsync();

		/// <summary>
		/// Get all roles with those for the given user marked as selected
		/// </summary>
		/// <param name="userId">user Id</param>
		/// <param name="includeOtherRoles">indicate if roles not assigned to this user should be included</param>
		/// <returns>list of user's roles</returns>
		List<Role> GetUserRolesListAsync(string userId, bool includeOtherRoles = true);

		/// <summary>
		/// Retrieve the full details of a user
		/// </summary>
		/// <param name="id">id of the user</param>
		/// <returns>Detail object for view model</returns>
		Task<Detail> GetUserDetailAsync(string id);

		/// <summary>
		/// Add a new user to the database
		/// </summary>
		/// <param name="user">user view model object</param>
		/// <returns>RequestResult indicating success</returns>
		Task<RequestResult> AddUserAsync(Create user);

		/// <summary>
		/// Update user in database
		/// </summary>
		/// <param name="user">user view model to update in database</param>
		/// <returns>RequestResult object indicating success or error</returns>
		Task<RequestResult> UpdateUserAsync(Edit user);
	}
}