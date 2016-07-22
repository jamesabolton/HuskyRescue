using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Organization;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public class AdminOrganizationsManagerService : IAdminOrganizationsManagerService
	{
		private readonly ILogger _logger;

		public AdminOrganizationsManagerService(ILogger iLogger)
		{
			_logger = iLogger;
		}

		public async Task<List<List>> GetFilteredOrgsListAsync(bool? isActive = true, bool? isDeleted = null,
			bool? animalClinicOnly = null, bool? boardingOnly = null, bool? donorOnly = null,
			bool? granterOnly = null, string nameContains = "")
		{
			var orgs = new List<List>();

			using (var context = new HuskyRescueEntities())
			{
				IQueryable<Organization> query = context.Organizations;

				if (isActive.HasValue)
				{
					query = query.Where(org => org.IsActive == isActive.Value);
				}
				if (isDeleted.HasValue)
				{
					query = query.Where(org => org.IsDeleted == isDeleted.Value);
				}
				if (animalClinicOnly.HasValue)
				{
					query = query.Where(org => org.IsAnimalClinic == animalClinicOnly.Value);
				}
				if (boardingOnly.HasValue)
				{
					query = query.Where(org => org.IsBoardingPlace == boardingOnly.Value);
				}
				if (donorOnly.HasValue)
				{
					query = query.Where(org => org.IsDonor == donorOnly.Value);
				}
				if (granterOnly.HasValue)
				{
					query = query.Where(org => org.IsGrantGiver == granterOnly.Value);
				}
				if (!string.IsNullOrEmpty(nameContains))
				{
					query = query.Where(org => org.Name.Contains(nameContains));
				}

				var orgsDb = await query.ToListAsync();

				if (orgsDb.Any())
				{
					orgs.AddRange(orgsDb.Select(dbOrg => new List
					{
						IsActive = dbOrg.IsActive,
						IsDeleted = dbOrg.IsDeleted.HasValue && dbOrg.IsDeleted.Value,
						DateActive = dbOrg.DateActive,
						DateInactive = dbOrg.DateInactive,
						DateDeleted = dbOrg.DateDeleted,
						EIN = dbOrg.EIN,
						IsAnimalClinic = dbOrg.IsAnimalClinic,
						IsBoardingPlace = dbOrg.IsBoardingPlace,
						IsGrantGiver = dbOrg.IsGrantGiver,
						IsDonor = dbOrg.IsDonor,
						Id = dbOrg.Id,
						Name = dbOrg.Name,
 					}));
				}
				_logger.Verbose("Filtered Organizations List {@filteredOrgs}", orgs);
			}
			return orgs;
		}

		public async Task<List<List>> GetAllOrgsListAsync()
		{
			var orgs = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var orgsDb = await context.Organizations.ToListAsync();

				if (orgsDb == null) return orgs;
				orgs.AddRange(orgsDb.Select(dbOrg => new List
				{
					//AddedByUserName = context.AspNetUsers.Find(dbOrg.AddedByUserId.ToString()).UserName,
					//AddedOnDate = dbOrg.AddedOnDate,
					IsActive = dbOrg.IsActive,
					IsDeleted = dbOrg.IsDeleted.HasValue && dbOrg.IsDeleted.Value,
					DateActive = dbOrg.DateActive,
					DateInactive = dbOrg.DateInactive,
					DateDeleted = dbOrg.DateDeleted,
					EIN = dbOrg.EIN,
					IsAnimalClinic = dbOrg.IsAnimalClinic,
					IsBoardingPlace = dbOrg.IsBoardingPlace,
					IsGrantGiver = dbOrg.IsGrantGiver,
					IsDonor = dbOrg.IsDonor,
					Id = dbOrg.Id,
					Name = dbOrg.Name,
					//UpdatedOnDate = dbOrg.UpdatedOnDate,
					//UpdatedByUserName = context.AspNetUsers.Find(dbOrg.UpdatedByUserId.ToString()).UserName
				}));

				_logger.Verbose("All Organizations List {@allOrgs}", orgs);
			}
			return orgs;
		}

		public async Task<RequestResult> AddOrganizationAsync(Create org)
		{
			_logger.Information("New Org Created {@orgCreatedObject}", org);

			var result = new RequestResult();

			try
			{

				using (var context = new HuskyRescueEntities())
				{
					var dbOrg = new Organization
					{
						AddedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(org.AddedByUserId))).UserName,
						AddedOnDate = DateTime.Today,
						DateActive = DateTime.Today,
						EIN = org.EmployeeIdNumber,
						IsActive = true,
						IsDonor = org.IsDonor,
						IsAnimalClinic = org.IsAnimalClinic,
						IsBoardingPlace = org.IsBoardingPlace,
						IsGrantGiver = org.IsGrantGiver,
						Name = org.Name,
						Notes = org.Notes
					};

					if (!string.IsNullOrEmpty(org.Email1) || !string.IsNullOrEmpty(org.Email2))
					{
						dbOrg.EmailAddresses = new Collection<EmailAddress>();
						if (!string.IsNullOrEmpty(org.Email1))
						{
							dbOrg.EmailAddresses.Add( new EmailAddress
							                          {
														  Address = org.Email1,
														  EmailAddressTypeId = 0
							                          });
						}
						if (!string.IsNullOrEmpty(org.Email2))
						{
							dbOrg.EmailAddresses.Add(new EmailAddress
							{
								Address = org.Email2,
								EmailAddressTypeId = 0
							});
						}
					}

					if (!string.IsNullOrEmpty(org.PhoneNumber1) || !string.IsNullOrEmpty(org.PhoneNumber2) || !string.IsNullOrEmpty(org.PhoneNumber3))
					{
						dbOrg.PhoneNumbers = new Collection<PhoneNumber>();
						if (!string.IsNullOrEmpty(org.PhoneNumber1))
						{
							dbOrg.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = org.PhoneNumber1,
								PhoneNumberTypeId = org.PhoneNumber1TypeId.HasValue ? org.PhoneNumber1TypeId.Value : 0
							});
						}
						if (!string.IsNullOrEmpty(org.PhoneNumber2))
						{
							dbOrg.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = org.PhoneNumber2,
								PhoneNumberTypeId = org.PhoneNumber2TypeId.HasValue ? org.PhoneNumber2TypeId.Value : 0
							});
						}
						if (!string.IsNullOrEmpty(org.PhoneNumber3))
						{
							dbOrg.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = org.PhoneNumber3,
								PhoneNumberTypeId = org.PhoneNumber3TypeId.HasValue ? org.PhoneNumber3TypeId.Value : 0
							});
						}
					}

					if (!string.IsNullOrEmpty(org.PhysicalAddressStreet1) || 
						!string.IsNullOrEmpty(org.MailingAddressStreet1))
					{
						dbOrg.Addresses = new Collection<Address>();
						if (!string.IsNullOrEmpty(org.PhysicalAddressStreet1))
						{
							dbOrg.Addresses.Add(
								new Address
								{
									Address1 = org.PhysicalAddressStreet1,
									Address2 = org.PhysicalAddressStreet2,
									Address3 = org.PhysicalAddressStreet3,
									City = org.PhysicalAddressCity,
									AddressStateId = org.PhysicalAddressStateId,
									ZipCode = org.PhysicalAddressPostalCode,
									CountryId = org.PhysicalAddressCountryId,
									IsBillingAddress = true,
									IsShippingAddress = false,
									AddressTypeId = 1 // primary
								});
						}
						if (!string.IsNullOrEmpty(org.MailingAddressStreet1))
						{
							dbOrg.Addresses.Add(
								new Address
								{
									Address1 = org.MailingAddressStreet1,
									Address2 = org.MailingAddressStreet2,
									Address3 = org.MailingAddressStreet3,
									City = org.MailingAddressCity,
									AddressStateId = org.MailingAddressStateId,
									ZipCode = org.MailingAddressPostalCode,
									CountryId = org.MailingAddressCountryId,
									IsBillingAddress = false,
									IsShippingAddress = false,
									AddressTypeId = 2 // Mailing
								});
						}

						if (!string.IsNullOrEmpty(org.Website))
						{
							dbOrg.Websites.Add(
								new Website
								{
									Name = org.Name,
									Website1 = org.Website
								});
						}

					}

					// add setting to database
					dbOrg = context.Organizations.Add(dbOrg);

					// save changes - returns number of database changes
					var numberOfSaves = await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
						result.NewKey = dbOrg.Id.ToString();
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "new event create validation error: {@NewOrganizationItem} {@DbValidationErrors}", org, ex.EntityValidationErrors);
			}

			return result;
		}

	}
}
