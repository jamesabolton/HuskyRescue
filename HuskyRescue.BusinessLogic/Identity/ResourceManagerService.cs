using System.Collections.Generic;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.ViewModel.Admin.ResourceManager;
using Serilog;

namespace HuskyRescue.BusinessLogic.Identity
{
	public class ResourceManagerService : IResourceManagerService
	{
		private static ILogger _logger;
		public ResourceManagerService(ILogger ilogger)
		{
			_logger = ilogger;
		}

		/// <summary>
		/// Get a list of resources to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the site's resources</returns>
		public async Task<List<ResourceIndex>> GetResourceListAsync()
		{
			var resources = new List<ResourceIndex>();
			using (var context = new HuskyRescueEntities())
			{
				
				var dbResources = await context.AspNetResources.ToListAsync();

				if (dbResources == null) return resources;
				resources.AddRange(dbResources.Select(dbResource => new ResourceIndex
				{
					Id = dbResource.Id,
					Name = dbResource.Name,
					OperationNames = string.Join(", ", OperationIdToName(dbResource.Operations))
				}));

				_logger.Verbose("ResourceIndex {@resources}", resources);
			}
			return resources;
		}

		/// <summary>
		/// Convert Operation as number to a list of strings describing the operation(s)
		/// </summary>
		/// <param name="oper">numeric representation of the operations</param>
		/// <returns>list of operations</returns>
		public static IEnumerable<string> OperationIdToName(int oper)
		{
			var operationNames = new List<string>();
			if ((oper & (int)ResourceOperations.Create) != 0)
			{
				operationNames.Add("Create");
			}
			if ((oper & (int)ResourceOperations.Delete) != 0)
			{
				operationNames.Add("Delete");
			}
			if ((oper & (int)ResourceOperations.Execute) != 0)
			{
				operationNames.Add("Execute");
			}
			if ((oper & (int)ResourceOperations.Read) != 0)
			{
				operationNames.Add("Read");
			}
			if ((oper & (int)ResourceOperations.Update) != 0)
			{
				operationNames.Add("Update");
			}
			if (oper == (int)ResourceOperations.All)
			{
				operationNames.Add("All");
			}
			if ((oper & (int)ResourceOperations.None) != 0)
			{
				operationNames.Add("None");
			}
			return operationNames;
		}

		/// <summary>
		/// Add a new Resource to the database
		/// </summary>
		/// <param name="id">Optional key; GUID as string - will create if Null or Empty</param>
		/// <param name="name">Name of the result</param>
		/// <param name="operations">Operations allowed on the resource</param>
		/// <returns>RequestResult indicating success and if successful the key is returned</returns>
		public async Task<RequestResult> AddResourceAsync(string id, string name, int operations)
		{
			var result = new RequestResult();

			// If the key is blank then generate a new one
			if (string.IsNullOrEmpty(id) || id.Equals(Guid.Empty.ToString()))
			{
				id = Guid.NewGuid().ToString();
			}

			var resource = new AspNetResource
			               {
				Id = id,
				Name = name,
				Operations = operations
			};

			using (var context = new HuskyRescueEntities())
			{
				// add resource to database
				resource = context.AspNetResources.Add(resource);

				// save changes - returns number of database changes
				var numberOfSaves = await context.SaveChangesAsync();
				// if there are changes then return success and the key of the new resource
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = resource.Id;
				}
			}

			return result;
		}

		public async Task<RequestResult> AddResourceAsync(ResourceCreate resourceViewModel)
		{
			var operations = 0;

			if (!resourceViewModel.IsCreate && !resourceViewModel.IsDelete && !resourceViewModel.IsExecute &&
			    !resourceViewModel.IsRead && !resourceViewModel.IsUpdate)
			{
				operations = (int) ResourceOperations.None;
			}
			else if (resourceViewModel.IsCreate && resourceViewModel.IsDelete && resourceViewModel.IsExecute &&
			         resourceViewModel.IsRead && resourceViewModel.IsUpdate)
			{
				operations = (int) ResourceOperations.All;
			}
			else
			{
				if (resourceViewModel.IsCreate)
				{
					operations = (int) ResourceOperations.Create;
				}
				if (resourceViewModel.IsDelete)
				{
					operations = operations != 0 ? operations | (int)ResourceOperations.Delete : (int)ResourceOperations.Delete;
				}
				if (resourceViewModel.IsExecute)
				{
					operations = operations != 0 ? operations | (int)ResourceOperations.Execute : (int)ResourceOperations.Execute;
				}
				if (resourceViewModel.IsRead)
				{
					operations = operations != 0 ? operations | (int)ResourceOperations.Read : (int)ResourceOperations.Read;
				}
				if (resourceViewModel.IsUpdate)
				{
					operations = operations != 0 ? operations | (int)ResourceOperations.Update : (int)ResourceOperations.Update;
				}
			}

			var result =
				await
					AddResourceAsync(string.Empty,
					                 string.Join("-", resourceViewModel.Controller, resourceViewModel.ControllerAction,
					                             resourceViewModel.Action), operations);

			return result;
		}

		/// <summary>
		/// Determine if a resource exists
		/// </summary>
		/// <param name="id">resource key</param>
		/// <returns>RequestResult</returns>
		public async Task<RequestResult> ResourceExistsAsync(string id)
		{
			AspNetResource resource;
			using (var context = new HuskyRescueEntities())
			{
				resource = await context.AspNetResources.FindAsync(id);
			}
			return new RequestResult(resource != null);
		}

		/// <summary>
		/// Add resource to a role
		/// </summary>
		/// <param name="resourceId">key of the resource</param>
		/// <param name="operation">operations available to the role for this resource</param>
		/// <param name="roleId">key of the role to own the resource</param>
		/// <returns>RequestResult</returns>
		public async Task<RequestResult> AddRoleResourceAsync(string resourceId, ResourceOperations operation, string roleId)
		{
			var result = new RequestResult();
			var roleResource = new AspNetRoleResource
			                   {
				                   RoleId = roleId,
				                   ResourceId = resourceId,
				                   Operations = (int) operation
			                   };

			using (var context = new HuskyRescueEntities())
			{
				context.AspNetRoleResources.Add(roleResource);

				var numberOfSaves = await context.SaveChangesAsync();
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
				}
			}

			return result;
		}

		/// <summary>
		/// Check if a given resource and operation are assigned to a set of roles (typically a user's)
		/// </summary>
		/// <param name="resourceId">resource ID</param>
		/// <param name="roleNames">array of user's roles</param>
		/// <returns>true or false</returns>
		public bool Authorize(string resourceId, string[] roleNames)
		{
			var authorized = false;

			using (var context = new HuskyRescueEntities())
			{
				// the roles the current user has
				var userRoles = context.AspNetRoles.Where(role => roleNames.Contains(role.Name)).ToArray();
				var userRoleIds = userRoles.Select(role => role.Id).ToArray();
				// the resource requested
				AspNetResource resource = null;
				try
				{
					resource = context.AspNetResources.Single(res => res.Name == resourceId);
				}
				catch (InvalidOperationException ex)
				{
					// this resource is not defined in the database - deny access by default
					_logger.Debug(ex, "Exception for {resourceId} and @{roles}", resourceId, roleNames);
				}

				if (resource != null)
				{
					// check if there are any matches before doing the select
					if (context.AspNetRoleResources.Any(roleResources => userRoleIds.Contains(roleResources.RoleId) && roleResources.ResourceId.Equals(resource.Id)))
					{
						// the role resource relationship - used to get the allowed operations on the resource for the role
						var roleResource =
							context.AspNetRoleResources.Single(roleResources => userRoleIds.Contains(roleResources.RoleId) && roleResources.ResourceId.Equals(resource.Id));
						if ((roleResource.Operations & resource.Operations) != 0)
						{
							authorized = true;
						}
					}
				}
			}

			return authorized;
		}

		public string GetRolesAsCsv(string resourceId, ResourceOperations operation)
		{
			string rolesCsv;
			using (var context = new HuskyRescueEntities())
			{
				var roleIds =
					context.AspNetRoleResources.Where(r => r.ResourceId.Equals(resourceId) && (r.Operations & (int) operation) != 0)
					       .Select(r => r.RoleId)
					       .Distinct();
				var roles =  context.AspNetRoles.Where(role => roleIds.Contains(role.Id)).Select(r => r.Name).ToArray();
				rolesCsv = String.Join(",", roles);
			}
			return rolesCsv;
		}

		/// <summary>
		/// Retrieve the full details of a Resource which includes which roles are using the resource
		/// </summary>
		/// <param name="id">resource id</param>
		/// <returns><see cref="ResourceDetail"/></returns>
		public async Task<ResourceDetail> GetResourceDetailAsync(string id)
		{
			var resourceDetail = new ResourceDetail();
			using (var context = new HuskyRescueEntities())
			{
				var resourceDetailDb = context.AspNetResources.Where(resource => resource.Id.Equals(id))
						.Include(a => a.AspNetRoleResources)
						.Single();
					//await context.AspNetResources.SingleAsync(resource => resource.Id.Equals(id));

				if (resourceDetailDb != null)
				{
					resourceDetail = new ResourceDetail
					                 {
										 Id = resourceDetailDb.Id,
										 Name = resourceDetailDb.Name,
										 Operations = string.Join(", ", OperationIdToName(resourceDetailDb.Operations))

					                 };

					if (resourceDetailDb.AspNetRoleResources.Count > 0)
					{
						var roles = new List<string>();
						foreach (var rr in resourceDetailDb.AspNetRoleResources)
						{
							 roles.Add((await context.AspNetRoles.SingleAsync(role => role.Id.Equals(rr.RoleId))).Name);
						}

						resourceDetail.AssociatedRoleNames = string.Join(", ", roles.ToArray());
					}
				}
			}

			return resourceDetail;
		}
	}
}
