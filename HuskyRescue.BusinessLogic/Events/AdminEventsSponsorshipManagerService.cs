using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.EventsSponsorship;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public class AdminEventsSponsorshipManagerService : IAdminEventsSponsorshipManagerService
	{
		private readonly ILogger _logger;

		public AdminEventsSponsorshipManagerService(ILogger iLogger)
		{
			_logger = iLogger;
		}

		public async Task<List<List>> GetAllSponsorshipsListAsync()
		{
			var sponsorships = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var sponsorshipsDb = await context.EventSponsorships.ToListAsync();

				if (sponsorshipsDb == null) return sponsorships;
				sponsorships.AddRange(sponsorshipsDb.Select(dbSponsorship => new List
				{
					EventName = dbSponsorship.Event.Name,
					AddedByUserId = dbSponsorship.AddedByUserId,
					IsActive = dbSponsorship.IsActive,
					IsDeleted = dbSponsorship.IsDeleted,
					Id = dbSponsorship.Id,
					Amount = dbSponsorship.Cost,
					Name = dbSponsorship.Name,
					EventId = dbSponsorship.EventId,
					NumberOfAllowedSponsors = dbSponsorship.NumberOfAllowedSponsors,
					NUmberOfSponsors = dbSponsorship.NumberOfSponsors
				}));

				_logger.Verbose("All Event Sponsorships List {@allEventSponsorships}", sponsorships);
			}
			return sponsorships;
		}

		public async Task<List<List>> GetEventSponsorshipsListAsync(Guid eventId)
		{
			var sponsorships = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var sponsorshipsDb = await context.EventSponsorships.Where(s => s.EventId.Equals(eventId)).ToListAsync();

				if (sponsorshipsDb == null) return sponsorships;
				sponsorships.AddRange(sponsorshipsDb.Select(dbSponsorship => new List
				{
					EventName = dbSponsorship.Event.Name,
					AddedByUserId = dbSponsorship.AddedByUserId,
					IsActive = dbSponsorship.IsActive,
					IsDeleted = dbSponsorship.IsDeleted,
					Id = dbSponsorship.Id,
					Amount = dbSponsorship.Cost,
					Name = dbSponsorship.Name,
					EventId = dbSponsorship.EventId,
					NumberOfAllowedSponsors = dbSponsorship.NumberOfAllowedSponsors,
					NUmberOfSponsors = dbSponsorship.NumberOfSponsors
				}));

				_logger.Verbose("Event Sponsorships List {@eventSponsorships}", sponsorships);
			}
			return sponsorships;
		}


		public Detail GetSponsorshipDetail(Guid id)
		{
			Detail sponsorship;
			using (var context = new HuskyRescueEntities())
			{
				var sponsorshipDb = context.EventSponsorships.Single(e => e.Id.Equals(id));

				if (sponsorshipDb == null) return null;
				sponsorship = new Detail
				{
					EventName = sponsorshipDb.Name,
					AddedByUserId = sponsorshipDb.AddedByUserId,
					UpdatedByUserId = sponsorshipDb.UpdatedByUserId,
					IsActive = sponsorshipDb.IsActive,
					IsDeleted = sponsorshipDb.IsDeleted,
					Id = sponsorshipDb.Id,
					Amount = sponsorshipDb.Cost,
					Name = sponsorshipDb.Name,
					EventId = sponsorshipDb.EventId,
					NumberOfAllowedSponsors = sponsorshipDb.NumberOfAllowedSponsors,
					NumberOfSponsors = sponsorshipDb.NumberOfSponsors,
					InternalNotes = sponsorshipDb.Notes,
					PublicDescription = sponsorshipDb.PublicDescription
				};

				var sponsorshipItemsDb = context.EventSponsorshipItems.Where(i => i.EventSponsorshipId.Equals(id));

				sponsorship.Items = new List<SponsorshipItem>();
				foreach (var itemDb in sponsorshipItemsDb)
				{
					sponsorship.Items.Add(new SponsorshipItem
					                      {
						                      EventSponsorshipId = id,
						                      Id = itemDb.Id,
						                      Value = itemDb.Value
					                      });
				}
				_logger.Verbose("Event Sponsorship Detail {@eventSponsorship}", sponsorship);
			}
			return sponsorship;
		}

		public Edit GetSponsorshipDetailForEdit(Guid id)
		{
			Edit sponsorship;
			using (var context = new HuskyRescueEntities())
			{
				var sponsorshipDb = context.EventSponsorships.Single(e => e.Id.Equals(id));

				if (sponsorshipDb == null) return null;
				sponsorship = new Edit
				{
					AddedByUserId = sponsorshipDb.AddedByUserId,
					UpdatedByUserId = sponsorshipDb.UpdatedByUserId,
					IsActive = sponsorshipDb.IsActive,
					IsDeleted = sponsorshipDb.IsDeleted,
					Id = sponsorshipDb.Id,
					Amount = sponsorshipDb.Cost,
					Name = sponsorshipDb.Name,
					EventId = sponsorshipDb.EventId,
					NumberOfAllowedSponsors = sponsorshipDb.NumberOfAllowedSponsors,
					NumberOfSponsors = sponsorshipDb.NumberOfSponsors,
					InternalNotes = sponsorshipDb.Notes,
					PublicDescription = sponsorshipDb.PublicDescription
				};

				var sponsorshipItemsDb = context.EventSponsorshipItems.Where(i => i.EventSponsorshipId.Equals(id));

				//Get existing detail items
				sponsorship.Items = new List<SponsorshipItem>();
				foreach (var itemDb in sponsorshipItemsDb)
				{
					sponsorship.Items.Add(new SponsorshipItem
					{
						EventSponsorshipId = id,
						Id = itemDb.Id,
						Value = itemDb.Value
					});
				}

				//pad with blank items to make up to 10
				for (var i = sponsorship.Items.Count; i < 10; i++)
				{
					sponsorship.Items.Add(new SponsorshipItem
					                      {
						                      EventSponsorshipId = id
					                      });
				}

				_logger.Verbose("Event Sponsorship Detail {@eventSponsorship}", sponsorship);
			}
			return sponsorship;
		}

		public async Task<RequestResult> AddEventSponsorshipAsync(Create eventSponsorshipObj)
		{
			_logger.Information("New Event Sponsorship Created {@eventSponsorshipCreateObject}", eventSponsorshipObj);

			var result = new RequestResult();

			try
			{

				using (var context = new HuskyRescueEntities())
				{
					var dbEventSponsorship = new EventSponsorship
					{
						Name = eventSponsorshipObj.Name,
						IsActive = true,//eventSponsorshipObj.IsActive,
						IsDeleted = false,
						Notes = eventSponsorshipObj.InternalNotes,
						AddedOnDate = DateTime.Today,
						Cost = eventSponsorshipObj.Amount,
						EventId = eventSponsorshipObj.EventId,
						NumberOfAllowedSponsors = eventSponsorshipObj.NumberOfAllowedSponsors,
						NumberOfSponsors = eventSponsorshipObj.NumberOfSponsors,
						PublicDescription = eventSponsorshipObj.PublicDescription,
						AddedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(eventSponsorshipObj.AddedByUserId))).UserName
					};

					// add setting to database
					dbEventSponsorship = context.EventSponsorships.Add(dbEventSponsorship);

					// save changes - returns number of database changes
					var numberOfSaves = await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
						result.NewKey = dbEventSponsorship.Id.ToString();
					}

					// if there are any sponsorship items save them now.
					foreach (var item in eventSponsorshipObj.Items.Where(e => !string.IsNullOrEmpty(e)))
					{
						context.EventSponsorshipItems.Add(new EventSponsorshipItem
						                                  {
							                                  EventSponsorshipId = dbEventSponsorship.Id,
							                                  Value = item
						                                  });
					}

					numberOfSaves = await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						//result.Succeeded = true;
						//result.NewKey = dbEventSponsorship.Id.ToString();
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "new event sponsorship create validation error: {@NewEventSponsorshipItem} {@DbValidationErrors}", eventSponsorshipObj, ex.EntityValidationErrors);
			}

			return result;
		}

		public async Task<RequestResult> UpdateEventSponsorshipAsync(Edit eventSponsorshipObj)
		{
			_logger.Information("Updating Event Sponsorship {@eventSponsorshipUpdateObject}", eventSponsorshipObj);

			var result = new RequestResult();

			try
			{
				using (var context = new HuskyRescueEntities())
				{
					var dbEventSponsorship = context.EventSponsorships.Single(e => e.Id.Equals(eventSponsorshipObj.Id));

					if(dbEventSponsorship != null){
						dbEventSponsorship.Name = eventSponsorshipObj.Name;
						dbEventSponsorship.IsActive = eventSponsorshipObj.IsActive;
						dbEventSponsorship.IsDeleted = eventSponsorshipObj.IsDeleted;
						dbEventSponsorship.Notes = eventSponsorshipObj.InternalNotes;
						dbEventSponsorship.UpdatedOnDate = DateTime.Today;
						dbEventSponsorship.Cost = eventSponsorshipObj.Amount;
						dbEventSponsorship.EventId = eventSponsorshipObj.EventId;
						dbEventSponsorship.NumberOfAllowedSponsors = eventSponsorshipObj.NumberOfAllowedSponsors;
						dbEventSponsorship.NumberOfSponsors = eventSponsorshipObj.NumberOfSponsors;
						dbEventSponsorship.PublicDescription = eventSponsorshipObj.PublicDescription;
						dbEventSponsorship.UpdatedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(eventSponsorshipObj.UpdatedByUserId))).UserName;
					}

					// Update sponsorship in database
					context.Entry(dbEventSponsorship).State = EntityState.Modified;

					// save changes - returns number of database changes
					var numberOfSaves = await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
					}

					// if there are any sponsorship items save them now.
					foreach (var item in eventSponsorshipObj.Items)
					{
						if (item.Id == 0)
						{
							// do not add if value is empty
							if (string.IsNullOrEmpty(item.Value)) continue;

							// item is new
							context.EventSponsorshipItems.Add(new EventSponsorshipItem
							{
								EventSponsorshipId = item.EventSponsorshipId,
								Value = item.Value
							});
						}
						else
						{
							var dbEventSponsorshipItem = context.EventSponsorshipItems.Single(e => e.Id.Equals(item.Id) && e.EventSponsorshipId.Equals(item.EventSponsorshipId));

							if (dbEventSponsorshipItem == null) continue;
							if (!string.IsNullOrEmpty(item.Value))
							{
								// skip items that have no value change
								if(item.Value == dbEventSponsorshipItem.Value) continue;

								// update modified item
								dbEventSponsorshipItem.Value = item.Value;
								context.Entry(dbEventSponsorshipItem).State = EntityState.Modified;
							}
							else
							{
								//item has been deleted
								context.Entry(dbEventSponsorshipItem).State = EntityState.Deleted;
							}
						}
					}

					numberOfSaves += await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "event sponsorship update validation error: {@UpdatedEventSponsorshipItem} {@DbValidationErrors}", eventSponsorshipObj, ex.EntityValidationErrors);
			}

			return result;
		}
	
		public async Task<RequestResult> UpdateEventSponsorshipAvailableCount(Guid eventSponsorshipId, Guid eventId, int number)
		{
			var result = new RequestResult(false);

			using (var context = new HuskyRescueEntities())
			{
				var sponsorshipsDb = await context.EventSponsorships.FindAsync(eventSponsorshipId, eventId);

				if (sponsorshipsDb == null) return result;

				sponsorshipsDb.NumberOfSponsors += number;

				context.Entry(sponsorshipsDb).State = EntityState.Modified;

				// save changes - returns number of database changes
				var numberOfSaves = await context.SaveChangesAsync();

				// if there are changes then return success 
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
				}

				_logger.Verbose("Event Sponsorship count adjusted by {number} {@eventSponsorship}", number, sponsorshipsDb);
			}

			return result;
		}
	}
}
