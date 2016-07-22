using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Admin.Users;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public class AdminUserManagerService : IAdminUserManagerService
	{
		private readonly ILogger _logger;

		public AdminUserManagerService(ILogger ilogger)
		{
			_logger = ilogger;
		}

		/// <summary>
		/// Get a list of roles to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the user roles</returns>
		public async Task<List<List>> GetUsersListAsync()
		{
			var users = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var dbUsers = await context.AspNetUsers.ToListAsync();

				if (dbUsers == null) return users;
				users.AddRange(dbUsers.Select(dbUser => new List
				{
					AccessFailedCount = dbUser.AccessFailedCount,
					Email = dbUser.Email,
					EmailConfirmed = dbUser.EmailConfirmed,
					Id = dbUser.Id,
					LockoutEnabled = dbUser.LockoutEnabled,
					LockoutEndDateUtc = dbUser.LockoutEndDateUtc.ToString(),
					PhoneNumber = dbUser.PhoneNumber,
					PhoneNumberConfirmed = dbUser.PhoneNumberConfirmed,
					TwoFactorEnabled = dbUser.TwoFactorEnabled,
					UserName = dbUser.UserName,
					Roles = string.Join(", ", dbUser.AspNetRoles.Select(role => role.Name).ToArray())
				}));

				_logger.Verbose("UsersList {@users}", users);
			}
			return users;
		}

		/// <summary>
		/// Get all roles with those for the given user marked as selected
		/// </summary>
		/// <param name="userId">user Id</param>
		/// <param name="includeOtherRoles">indicate if roles not assigned to this user should be included</param>
		/// <returns>list of user's roles</returns>
		public List<Role> GetUserRolesListAsync(string userId, bool includeOtherRoles = true)
		{
			var roles = new List<Role>();

			using (var context = new HuskyRescueEntities())
			{
				if (!string.IsNullOrEmpty(userId) && !includeOtherRoles)
				{
					roles = context.AspNetUsers.Single(user => user.Id.Equals(userId)).AspNetRoles.Select(role => new Role
					                                                                                              {
						                                                                                              Id = role.Id,
						                                                                                              Name =
							                                                                                              role.Name,
						                                                                                              Selected =
							                                                                                              true
					                                                                                              }).ToList();
				}
				else if (!string.IsNullOrEmpty(userId) && includeOtherRoles)
				{
					roles = context.AspNetUsers.Single(user => user.Id.Equals(userId)).AspNetRoles.Select(role => new Role
					{
						Id = role.Id,
						Name = role.Name,
						Selected = true
					}).ToList();

					roles.AddRange(context.AspNetRoles.Where(role => role.AspNetUsers.Any(user => !user.Id.Equals(userId))).Select(role => new Role
					{
						Id = role.Id,
						Name = role.Name,
						Selected = false
					}).ToList());
				}
				else if (string.IsNullOrEmpty(userId))
				{
					roles = context.AspNetRoles.Select(role => new Role
					{
						Id = role.Id,
						Name = role.Name,
						Selected = false
					}).ToList();
				}
			}
			return roles;
		}

		/// <summary>
		/// Retrieve the full details of a user
		/// </summary>
		/// <param name="id">id of the user</param>
		/// <returns>Detail object for view model</returns>
		public async Task<Detail> GetUserDetailAsync(string id)
		{
			var userDetail = new Detail();

			using (var context = new HuskyRescueEntities())
			{
				var dbUser = await context.AspNetUsers.FindAsync(id);
				if (dbUser != null)
				{
					userDetail = new Detail
					{
						AccessFailedCount = dbUser.AccessFailedCount,
						Email = dbUser.Email,
						EmailConfirmed = dbUser.EmailConfirmed,
						Id = dbUser.Id,
						LockoutEnabled = dbUser.LockoutEnabled,
						LockoutEndDateUtc = dbUser.LockoutEndDateUtc.ToString(),
						PhoneNumber = dbUser.PhoneNumber,
						PhoneNumberConfirmed = dbUser.PhoneNumberConfirmed,
						TwoFactorEnabled = dbUser.TwoFactorEnabled,
						UserName = dbUser.UserName
					};
					var userClaims = dbUser.AspNetRoles.ToList();

					var allRoles = context.AspNetRoles.ToList();
					
					var userRoles = userClaims.Where(allRoles.Contains).ToList();

					var nonUserRoles = allRoles.Except(userRoles);

					userDetail.Roles.AddRange(userRoles.Select(role => new Role
					{
						Id = role.Id,
						Name = role.Name,
						Selected = true
					}));

					userDetail.Roles.AddRange(nonUserRoles.Select(role => new Role
					{
						Id = role.Id,
						Name = role.Name,
						Selected = false
					}));

					//userDetail.Roles.RemoveAll(role => !dbAllRoles.Contains(role));
					//userDetail.Roles = userDetail.Roles.Union(allRoles, new RoleEqualityComparer()).ToList();
				}
			}

			return userDetail;
		}

		/// <summary>
		/// Add a new user to the database
		/// </summary>
		/// <param name="user">user view model object</param>
		/// <returns>RequestResult indicating success</returns>
		public async Task<RequestResult> AddUserAsync(Create user)
		{

			var result = new RequestResult();
			
			using (var context = new HuskyRescueEntities())
			{
				int numberOfSaves;
				var dbUser = new AspNetUser
				{
					Id = Guid.NewGuid().ToString(),
					AccessFailedCount = 0,
					Email = user.Email,
					EmailConfirmed = false,
					LockoutEnabled = false,
					PhoneNumber = user.PhoneNumber,
					PhoneNumberConfirmed = false,
					TwoFactorEnabled = user.TwoFactorEnabled,
					UserName = user.UserName
				};
				try
				{
					foreach (var role in user.Roles.Where(role => role.Selected))
					{
						dbUser.AspNetRoles.Add(context.AspNetRoles.Single(dbRole => dbRole.Id.Equals(role.Id)));
					}

					// add user to database
					dbUser = context.AspNetUsers.Add(dbUser);

					// save changes - returns number of database changes
					numberOfSaves = await context.SaveChangesAsync();
				}
				catch (Exception)
				{
					throw;
				}

				// if there are changes then return success and the key of the new user
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = dbUser.Id;
				}
			}

			return result;
		}

		/// <summary>
		/// Update user in database
		/// </summary>
		/// <param name="user">user view model to update in database</param>
		/// <returns>RequestResult object indicating success or error</returns>
		public async Task<RequestResult> UpdateUserAsync(Edit user)
		{
			var result = new RequestResult();

			using (var context = new HuskyRescueEntities())
			{
				int numberOfSaves;
				AspNetUser dbUser;

				try
				{
					dbUser = await context.AspNetUsers.FindAsync(user.Id);

					dbUser.Email = user.Email;
					dbUser.EmailConfirmed = user.EmailConfirmed;
					dbUser.LockoutEnabled = dbUser.LockoutEnabled;
					if (dbUser.LockoutEnabled)
					{
						dbUser.LockoutEndDateUtc = null;
					}
					else
					{
						dbUser.LockoutEndDateUtc = DateTime.Today;
					}
					dbUser.PhoneNumber = user.PhoneNumber;
					dbUser.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
					dbUser.TwoFactorEnabled = user.TwoFactorEnabled;
					dbUser.UserName = user.UserName;

					var dbAllRoles = context.AspNetRoles.ToList();
					var dbCurrentRoles = dbUser.AspNetRoles.Where(dbAllRoles.Contains).ToList();
					var dbCount = dbCurrentRoles.Count();
					foreach (var submittedRole in user.Roles)
					{
						// find roles that are selected and not associated with the user - add them
						if ((!dbCurrentRoles.Any(currentRole => submittedRole.Id.Equals(currentRole.Id)) && submittedRole.Selected) || (dbCount == 0 && submittedRole.Selected))
						{
							var dbRole = context.AspNetRoles.Single(role => role.Id.Equals(submittedRole.Id));

							dbUser.AspNetRoles.Add(dbRole);
						}
						// find roles that are not selected and associated with the user - delete them
						if (dbCurrentRoles.Any(currentRole => submittedRole.Id.Equals(currentRole.Id)) && !submittedRole.Selected)
						{
							dbUser.AspNetRoles.Remove(context.AspNetRoles.Single(role => role.Id.Equals(submittedRole.Id)));
						}
					}

					context.Entry(dbUser).State = EntityState.Modified;

					// save changes - returns number of database changes
					numberOfSaves = await context.SaveChangesAsync();
				}
				catch (Exception)
				{

					throw;
				}

				// if there are changes then return success and the key of the new user
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = dbUser.Id;
				}
			}

			return result;
		}

	}
}
