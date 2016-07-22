using System.Data.Entity;
using HuskyRescue.BusinessLogic.Identity;
using HuskyRescue.DataModel;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.ViewModel.Admin.Roles;
using HuskyRescue.ViewModel;

namespace HuskyRescue.BusinessLogic
{
	public class AdminRolesManagerService : IAdminRolesManagerService
	{
		private readonly ILogger _logger;

		public AdminRolesManagerService(ILogger ilogger)
		{
			_logger = ilogger;
		}

		/// <summary>
		/// Get a list of roles to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the user roles</returns>
		public async Task<List<List>> GetRolesListAsync()
		{
			var roles = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var dbRoles = await context.AspNetRoles.ToListAsync();

				if (dbRoles == null) return roles;
				roles.AddRange(dbRoles.Select(dbRole => new List
				{
					Name = dbRole.Name,
					Id = dbRole.Id
				}));

				_logger.Verbose("SystemSettingList {@roles}", roles);
			}
			return roles;
		}

		/// <summary>
		/// Get all resources with those for the given role marked as selected
		/// </summary>
		/// <param name="roleId">role roleId</param>
		/// <param name="includeOtherResources">indicate if resources not assigned to this role should be included</param>
		/// <returns>list of resources</returns>
		public List<RoleResource> GetRoleResourcesListAsync(string roleId, bool includeOtherResources = true)
		{
			List<RoleResource> resources;

			using (var context = new HuskyRescueEntities())
			{
				resources = context.AspNetResources.Select(r => new RoleResource
				{
					Id = r.Id,
					Name = r.Name,
					Operations = r.Operations,
					Selected = false
				}).ToList();

				if (!string.IsNullOrEmpty(roleId))
				{
					var dbRolesResources = context.AspNetRoleResources.Where(r => r.RoleId.Equals(roleId)).ToList();

					// all resources with those associated with a roleId marked as true and those not marked as false
					if (includeOtherResources)
					{
						foreach (var resource in resources.Where(resource => dbRolesResources.Any(r => r.ResourceId.Equals(resource.Id))))
						{
							resource.Selected = true;
						}
					}
					// remove all resources not marked as selected
					else
					{
						resources.RemoveAll(r => !dbRolesResources.Exists(rr => rr.ResourceId == r.Id));
					}
				}
			}
			resources.ForEach(r => r.OperationDesc = string.Join(", ", ResourceManagerService.OperationIdToName(r.Operations)));
			return resources;
		}

		/// <summary>
		/// Retrieve the full details of a user role
		/// </summary>
		/// <param name="id">id of the role</param>
		/// <returns>Detail object for view model</returns>
		public async Task<Detail> GetRoleDetailAsync(string id)
		{
			var roleDetail = new Detail();

			using (var context = new HuskyRescueEntities())
			{
				var dbRole = await context.AspNetRoles.FindAsync(id);
				if (dbRole != null)
				{
					roleDetail = new Detail
									{
										Name = dbRole.Name,
										Id = dbRole.Id,
										Resources = dbRole.AspNetRoleResources.Select(r => new RoleResource { Id = r.ResourceId, Name = r.AspNetResource.Name, Selected = true, Operations = r.Operations }).ToList()
									};
					var allResources = context.AspNetResources.Select(r => new RoleResource { Id = r.Id, Name = r.Name, Selected = false, Operations = r.Operations });
					roleDetail.Resources = roleDetail.Resources.Union(allResources, new RoleResourceEqualityComparer()).ToList();
					roleDetail.Resources.ForEach(r => r.OperationDesc = string.Join(", ", ResourceManagerService.OperationIdToName(r.Operations)));
					//roleDetail.Resources = roleDetail.Resources.OrderBy(r => r.Name).ToList();
					roleDetail.Resources.Sort((r1, r2) => r1.Name.CompareTo(r2.Name));
				}
			}

			return roleDetail;
		}

		/// <summary>
		/// Add a new user role to the database
		/// </summary>
		/// <param name="role">user role view model object</param>
		/// <returns>RequestResult indicating success</returns>
		public async Task<RequestResult> AddRoleAsync(Create role)
		{

			var result = new RequestResult();
			var dbRole = new AspNetRole
			{
				Name = role.Name,
				Id = Guid.NewGuid().ToString()
			};

			using (var context = new HuskyRescueEntities())
			{
				var numberOfSaves = 0;
				try
				{
					// add role to database
					dbRole = context.AspNetRoles.Add(dbRole);

					// save changes - returns number of database changes
					numberOfSaves = await context.SaveChangesAsync();
				}
				catch (Exception)
				{
					throw;
				}

				try
				{
					foreach (var resourceRole in role.Resources.Where(r => r.Selected))
					{
						context.AspNetRoleResources.Add(new AspNetRoleResource
														{
															ResourceId = resourceRole.Id,
															RoleId = dbRole.Id,
															Operations = resourceRole.Operations
														});
					}

					numberOfSaves += await context.SaveChangesAsync();
				}
				catch (Exception)
				{
					throw;
				}

				// if there are changes then return success and the key of the new role
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = dbRole.Id;
				}
			}

			return result;
		}

		/// <summary>
		/// Update role in database
		/// </summary>
		/// <param name="role">role view model to update in database</param>
		/// <returns>RequestResult object indicating success or error</returns>
		public async Task<RequestResult> UpdateRoleAsync(Edit role)
		{
			var result = new RequestResult();

			using (var context = new HuskyRescueEntities())
			{
				var numberOfSaves = 0;
				AspNetRole dbRole;

				try
				{
					dbRole = await context.AspNetRoles.FindAsync(role.Id);

					if (!dbRole.Name.Equals(role.Name))
					{
						dbRole.Name = role.Name;

						context.Entry(dbRole).State = EntityState.Modified;

						// save changes - returns number of database changes
						numberOfSaves = await context.SaveChangesAsync();
					}
				}
				catch (Exception)
				{

					throw;
				}

				try
				{
					var dbCurrentRoleResources = context.AspNetRoleResources.Where(rr => rr.RoleId.Equals(role.Id));
					var dbCount = dbCurrentRoleResources.Count();
					foreach (var submittedRoleResources in role.Resources)
					{
						// find resources that are selected and not associated with the role - add them
						if ((!dbCurrentRoleResources.Any(currentResource => submittedRoleResources.Id.Equals(currentResource.ResourceId)) && submittedRoleResources.Selected) || (dbCount == 0 && submittedRoleResources.Selected))
						{
							// add resource
							context.AspNetRoleResources.Add(new AspNetRoleResource
							{
								ResourceId = submittedRoleResources.Id,
								RoleId = dbRole.Id,
								Operations = submittedRoleResources.Operations
							});
						}
						// find resources that are not selected and associated with the role - delete them
						if (dbCurrentRoleResources.Any(currentResource => submittedRoleResources.Id.Equals(currentResource.ResourceId)) && !submittedRoleResources.Selected)
						{
							context.AspNetRoleResources.Remove(
								dbCurrentRoleResources.Single(r => r.RoleId.Equals(dbRole.Id) && r.ResourceId.Equals(submittedRoleResources.Id)));
						}
					}

					numberOfSaves += await context.SaveChangesAsync();
				}
				catch (Exception)
				{

					throw;
				}

				// if there are changes then return success and the key of the new role
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = dbRole.Name;
				}
			}

			return result;
		}
	}
}
