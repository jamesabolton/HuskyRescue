using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.EventsSponsor;
using Serilog;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace HuskyRescue.BusinessLogic
{
	public class AdminEventsSponsorManagerService : IAdminEventsSponsorManagerService
	{
		private readonly ILogger _logger;
		private readonly IAdminSystemSettingsManagerService _adminSystemSettingsManagerService;

		public AdminEventsSponsorManagerService(ILogger iLogger, IAdminSystemSettingsManagerService iAdminSystemSettingsManagerService)
		{
			_logger = iLogger;
			_adminSystemSettingsManagerService = iAdminSystemSettingsManagerService;
		}

		public async Task<List<List>> GetAllSponsorsListAsync()
		{
			var sponsors = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var sponsorsDb = await context.EventSponsors.ToListAsync();

				if (sponsorsDb == null) return sponsors;
				sponsors.AddRange(sponsorsDb.Select(dbSponsor => new List
				{
					Id = dbSponsor.Id,
					AmountPaid = dbSponsor.AmountPaid,
					EventName = dbSponsor.Event.Name,
					EventId = dbSponsor.EventId,
					HaveReceivedLogo = dbSponsor.HaveReceviedLogo,
					HaveReceivedSingage = dbSponsor.HaveReceivedSingage,
					IsPerson = dbSponsor.PersonId.HasValue, // if true then sponsor is 'person' else it is an 'org'/'business'
					OrgPersonId = dbSponsor.PersonId.HasValue ? dbSponsor.PersonId.Value : dbSponsor.OrganizationId.Value, // 'if person id has value then use the person id else use the org id'
					SponsorName = dbSponsor.PersonId.HasValue ? dbSponsor.Person.FirstName + " " + dbSponsor.Person.LastName : dbSponsor.Organization.Name,
					EventSponsoshipId = dbSponsor.EventSponsorshipId,
					EventSponsoshipName = dbSponsor.EventSponsorship.Name
				}));

				_logger.Verbose("All Events List {@allEventSponsors}", sponsors);
			}
			return sponsors;
		}

		public async Task<RequestResult> AddEventSponsorAsync(Create eventSponsorObj)
		{
			_logger.Information("New Event Sponsor Created {@eventSponsorCreateObject}", eventSponsorObj);

			var result = new RequestResult();

			try
			{
				using (var context = new HuskyRescueEntities())
				{
					var eventSponsor = new EventSponsor
					                   {
						                   EventId = eventSponsorObj.EventId,
						                   EventSponsorshipId = eventSponsorObj.EventSponsorshipId,
						                   AmountPaid = eventSponsorObj.AmountPaid,
						                   Notes = eventSponsorObj.InternalNotes,
						                   AddedOnDate = DateTime.Today,
						                   HaveReceviedLogo = eventSponsorObj.HaveReceivedLogo,
						                   HaveReceivedSingage = eventSponsorObj.HaveReceivedSingage,
						                   AddedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(eventSponsorObj.AddedByUserId))).UserName
					                   };

					eventSponsor.OrganizationId = null;
					eventSponsor.PersonId = null;
					if (eventSponsorObj.OrganizationId != Guid.Empty)
					{
						eventSponsor.OrganizationId = eventSponsorObj.OrganizationId;
					}
					if (eventSponsorObj.PeopleId != Guid.Empty)
					{
						eventSponsor.PersonId = eventSponsorObj.PeopleId;
					}

					// add sponsor to database
					eventSponsor = context.EventSponsors.Add(eventSponsor);

					// if a logo was uploaded save it
					if (eventSponsorObj.FileLogoContent.Length > 0)
					{
						eventSponsor.Files.Add(new File
						{
							ContentType = eventSponsorObj.FileLogoContentType,
							FileTypeId = eventSponsorObj.FileTypeId,
							Content = eventSponsorObj.FileLogoContent
						});
					}

					// save changes to the database - returns number of database changes
					var numberOfSaves = await context.SaveChangesAsync();

					// if there are changes then return success and the key of the new sponsor
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
						result.NewKey = eventSponsor.Id.ToString();

						// update number of available sponsorships
						var sponsorshipDb = await context.EventSponsorships.FindAsync(eventSponsorObj.EventSponsorshipId);
						sponsorshipDb.NumberOfSponsors += 1;

						// save changes to the database
						numberOfSaves = await context.SaveChangesAsync();

						// if there are changes then return success and the key of the new sponsor
						if (numberOfSaves > 0)
						{
							//result.Succeeded = true;
							//result.NewKey = dbEventSponsorship.Id.ToString();
						}
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "new event sponsor create validation error: {@NewEventSponsor} {@DbValidationErrors}",
				              eventSponsorObj, ex.EntityValidationErrors);
			}
			catch (ApplicationException ex)
			{
				_logger.Error(ex, "new event sponsor create app error: {@NewEventSponsor} {@applicationError}",
				  eventSponsorObj, ex);
			}
			catch (Exception ex)
			{
				_logger.Error(ex, "new event sponsor create general error: {@NewEventSponsor} {@generalerror}",
				  eventSponsorObj, ex);
			}

			return result;
		}

		public async Task<RequestResult> UpdateEventSponsorAsync(Edit eventSponsorObj)
		{
			_logger.Information("Event Sponsor Updated {@eventSponsorUpdateObject}", eventSponsorObj);

			var result = new RequestResult();

			try
			{
				using (var context = new HuskyRescueEntities())
				{

					var eventSponsorDb = await context.EventSponsors.SingleAsync(es => es.Id.Equals(eventSponsorObj.Id));

					if (eventSponsorDb != null)
					{

						eventSponsorDb.EventId = eventSponsorObj.EventId;
						eventSponsorDb.EventSponsorshipId = eventSponsorObj.EventSponsorshipId;
						eventSponsorDb.OrganizationId = eventSponsorObj.OrganizationId != Guid.Empty ? eventSponsorObj.OrganizationId : Guid.Empty;
						eventSponsorDb.PersonId = eventSponsorObj.PeopleId != Guid.Empty ? eventSponsorObj.PeopleId : Guid.Empty;
						eventSponsorDb.AmountPaid = eventSponsorObj.AmountPaid;
						eventSponsorDb.HasSponsorMoneyBeenReceived = eventSponsorObj.HasPaid;
						eventSponsorDb.HaveReceivedSingage = eventSponsorObj.HaveReceivedSingage;
						eventSponsorDb.HaveReceviedLogo = eventSponsorObj.HaveReceivedLogo;
						eventSponsorDb.Notes = eventSponsorObj.InternalNotes;
						eventSponsorDb.UpdatedOnDate = DateTime.Now;
						eventSponsorDb.UpdatedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(eventSponsorObj.UpdatedByUserId))).UserName;

						// update sponsor to database
						context.Entry(eventSponsorDb).State = EntityState.Modified;

						// save changes - returns number of database changes
						var numberOfSaves = await context.SaveChangesAsync();

						// if there are changes then return success and the key of the new setting
						if (numberOfSaves > 0)
						{
							result.Succeeded = true;

							// update number of available sponsorships
							var sponsorshipDb = await context.EventSponsorships.FindAsync(eventSponsorObj.EventSponsorshipId);
							sponsorshipDb.NumberOfSponsors += 1;

							numberOfSaves = await context.SaveChangesAsync();
							// if there are changes then return success and the key of the new setting
							if (numberOfSaves > 0)
							{
								//result.Succeeded = true;
								//result.NewKey = dbEventSponsorship.Id.ToString();
							}
						}
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "new event sponsor create validation error: {@NewEventSponsor} {@DbValidationErrors}", eventSponsorObj, ex.EntityValidationErrors);
			}

			return result;
		}

	}
}
