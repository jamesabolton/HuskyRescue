using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.SystemSetting;
using Serilog;
using Create = HuskyRescue.ViewModel.Events.Create;
using List = HuskyRescue.ViewModel.Events.List;
using Edit = HuskyRescue.ViewModel.Events.Edit;

namespace HuskyRescue.BusinessLogic
{
	public class AdminEventsManagerService : IAdminEventsManagerService
	{
		private readonly ILogger _logger;
		private readonly IAdminSystemSettingsManagerService _adminSystemSettingsManagerService;

		public AdminEventsManagerService(ILogger iLogger, IAdminSystemSettingsManagerService iAdminSystemSettingsManagerService)
		{
			_logger = iLogger;
			_adminSystemSettingsManagerService = iAdminSystemSettingsManagerService;
		}

		public async Task<List<List>> GetAllEventsListAsync()
		{
			var events = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var eventsDb = await context.Events.ToListAsync();

				if (eventsDb == null) return events;
				events.AddRange(eventsDb.Select(dbEvent => new List
				{
					EventName = dbEvent.Name,
					AddedByUserId = dbEvent.AddedByUserId,
					IsActive = dbEvent.IsActive,
					IsDeleted = dbEvent.IsDeleted,
					Is5KEvent = dbEvent.Is5kEvent,
					IsAllDayEvent = dbEvent.IsAllDayEvent,
					IsDogWashEvent = dbEvent.IsDogWashEvent,
					IsGolfTournamentEvent = dbEvent.IsGolfTournamentEvent,
					IsMeetAndGreetEvent = dbEvent.IsMeetAndGreetEvent,
					IsOtherEvent = dbEvent.IsOtherEvent,
					IsRaffleEvent = dbEvent.IsRaffleEvent,
					IsRoughRidersEvent = dbEvent.IsRoughRidersEvent,
					StartTime = dbEvent.StartTime,
					EndTime = dbEvent.EndTime,
					EventDate = dbEvent.EventDate.HasValue ? dbEvent.EventDate.Value : DateTime.MinValue,
					AreTicketsSold = dbEvent.AreTicketsSold,
					TickPriceOther = dbEvent.TicketPriceOther,
					TicketPrice = dbEvent.TicketPrice,
					OrganizationId = dbEvent.OrganizationId,
					Id = dbEvent.Id,
					OrganziationName = dbEvent.Organization.Name

				}));

				_logger.Verbose("All Events List {@allEvents}", events);
			}
			return events;
		}

		public async Task<RequestResult> AddEventAsync(Create eventObj)
		{
			_logger.Information("New Event Created {@eventCreateObject}", eventObj);

			var result = new RequestResult();

			try
			{
				using (var context = new HuskyRescueEntities())
				{
					var dbEvent = new Event
					{
						Name = eventObj.EventName,
						IsActive = eventObj.IsActive.HasValue && eventObj.IsActive.Value,
						IsDeleted = false,
						Is5kEvent = eventObj.Is5KEvent,
						IsDogWashEvent = eventObj.IsDogWashEvent,
						IsGolfTournamentEvent = eventObj.IsGolfTournamentEvent,
						IsMeetAndGreetEvent = eventObj.IsMeetAndGreetEvent,
						IsRaffleEvent = eventObj.IsRaffleEvent,
						IsRoughRidersEvent = eventObj.IsRoughRidersEvent,
						IsOtherEvent = eventObj.IsOtherEvent,
						IsAllDayEvent = eventObj.IsAllDayEvent,
						Notes = eventObj.InternalNotes,
						AddedOnDate = DateTime.Today,
						EventDate = eventObj.EventDate,
						AreTicketsSold = eventObj.AreTicketsSold,
						TicketPrice = eventObj.TicketPrice,
						TicketPriceOther = eventObj.TickPriceOther,
						StartTime = eventObj.StartTime.HasValue ? eventObj.StartTime.Value : TimeSpan.MinValue,
						EndTime = eventObj.EndTime.HasValue ? eventObj.EndTime.Value : TimeSpan.MinValue,
						OrganizationId = eventObj.OrganizationId,
						OtherEventDescription = eventObj.EventDescription,
						PublicDescription = eventObj.PublicEventDescription,
						AddedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(eventObj.AddedByUserId))).UserName
					};

					// add setting to database
					dbEvent = context.Events.Add(dbEvent);

					// save changes - returns number of database changes
					var numberOfSaves = await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
						result.NewKey = dbEvent.Id.ToString();
					}

					var settingResult = new RequestResult();

					//check if this is a new event of a single type
					if (dbEvent.IsGolfTournamentEvent)
					{
						var activeGolfTournamentId = await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfTournamentId");
						var activeGolfBanquetTicketCost = await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfBanquetTicketCost");
						var activeGolfTicketCost = await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfTicketCost");

						#region Update Golf Tournament Id
						if (activeGolfTournamentId == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
																									 {
																										 AddedByUserId = eventObj.AddedByUserId,
																										 Name = "GolfTournamentId",
																										 Notes = "Added automatically while creating event " + eventObj.EventName,
																										 Value = dbEvent.Id.ToString()
																									 });
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
																										{
																											Name = activeGolfTournamentId.Name,
																											Notes = activeGolfTournamentId.Notes + Environment.NewLine + "Updated automatically while creating event " + eventObj.EventName,
																											UpdatedByUserId = eventObj.AddedByUserId,
																											Value = dbEvent.Id.ToString()
																										});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("GolfTournamentId updated successfully with new value {@GolfEventId}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("GolfTournamentId NOT updated successfully with new value {@GolfEventId}", dbEvent.Id.ToString());
						}
						#endregion

						#region Golf Tournament Banquet Ticket Cost
						if (activeGolfBanquetTicketCost == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "GolfBanquetTicketCost",
								Notes = "Added automatically while creating event " + eventObj.EventName,
								Value = dbEvent.TicketPriceOther.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeGolfBanquetTicketCost.Name,
								Notes = activeGolfBanquetTicketCost.Notes + Environment.NewLine + "Updated automatically while creating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.TicketPriceOther.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("GolfBanquetTicketCost updated successfully with new value {@GolfBanquetTicketCost}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("GolfBanquetTicketCost NOT updated successfully with new value {@GolfBanquetTicketCost}", dbEvent.Id.ToString());
						}
						#endregion

						#region Golf Tournament Ticket Cost
						if (activeGolfTicketCost == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "GolfTicketCost",
								Notes = "Added automatically while creating event " + eventObj.EventName,
								Value = dbEvent.TicketPrice.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeGolfTicketCost.Name,
								Notes = activeGolfTicketCost.Notes + Environment.NewLine + "Updated automatically while creating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.TicketPrice.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("GolfTicketCost updated successfully with new value {@GolfTicketCost}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("GolfTicketCost NOT updated successfully with new value {@GolfTicketCost}", dbEvent.Id.ToString());
						}
						#endregion
					}
					else if (dbEvent.IsRoughRidersEvent)
					{
						var activeRoughRidersEventId = await _adminSystemSettingsManagerService.GetSettingDetailAsync("RoughRidersEventId");
						var activeRoughRidersTicketCost = await _adminSystemSettingsManagerService.GetSettingDetailAsync("RoughRidersTicketCost");

						#region Update RoughRiders Id
						if (activeRoughRidersEventId == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "RoughRidersEventId",
								Notes = "Added automatically while creating event " + eventObj.EventName,
								Value = dbEvent.Id.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeRoughRidersEventId.Name,
								Notes = activeRoughRidersEventId.Notes + Environment.NewLine + "Updated automatically while creating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.Id.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("RoughRidersEventId updated successfully with new value {@RoughRidersEventId}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("RoughRidersEventId NOT updated successfully with new value {@RoughRidersEventId}", dbEvent.Id.ToString());
						}
						#endregion

						#region RoughRiders Ticket Cost
						if (activeRoughRidersTicketCost == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "RoughRidersTicketCost",
								Notes = "Added automatically while creating event " + eventObj.EventName,
								Value = dbEvent.TicketPrice.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeRoughRidersTicketCost.Name,
								Notes = activeRoughRidersTicketCost.Notes + Environment.NewLine + "Updated automatically while creating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.TicketPrice.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("RoughRidersTicketCost updated successfully with new value {@RoughRidersTicketCost}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("RoughRidersTicketCost NOT updated successfully with new value {@RoughRidersTicketCost}", dbEvent.Id.ToString());
						}
						#endregion
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "new event create validation error: {@NewEventItem} {@DbValidationErrors}", eventObj, ex.EntityValidationErrors);
			}

			return result;
		}

		public async Task<RequestResult> EditEventAsync(Edit eventObj)
		{
			_logger.Information("Update Event {@eventUpdateObject}", eventObj);

			var result = new RequestResult();

			try
			{
				using (var context = new HuskyRescueEntities())
				{
					var dbEvent = await context.Events.SingleAsync(e => e.Id.Equals(eventObj.Id));

					if (dbEvent == null)
					{
						result.Succeeded = false;
						result.Errors.Add("Existing event not found");
						return result;
					}


					dbEvent.Name = eventObj.EventName;
					dbEvent.IsActive = eventObj.IsActive;
					//dbEvent.IsDeleted = false;
					dbEvent.Is5kEvent = eventObj.Is5KEvent;
					dbEvent.IsDogWashEvent = eventObj.IsDogWashEvent;
					dbEvent.IsGolfTournamentEvent = eventObj.IsGolfTournamentEvent;
					dbEvent.IsMeetAndGreetEvent = eventObj.IsMeetAndGreetEvent;
					dbEvent.IsRaffleEvent = eventObj.IsRaffleEvent;
					dbEvent.IsRoughRidersEvent = eventObj.IsRoughRidersEvent;
					dbEvent.IsOtherEvent = eventObj.IsOtherEvent;
					dbEvent.IsAllDayEvent = eventObj.IsAllDayEvent;
					dbEvent.Notes = eventObj.InternalNotes;
					dbEvent.UpdatedOnDate = DateTime.Today;
					dbEvent.EventDate = eventObj.EventDate;
					dbEvent.AreTicketsSold = eventObj.AreTicketsSold;
					dbEvent.TicketPrice = eventObj.TicketPrice;
					dbEvent.TicketPriceOther = eventObj.TicketPriceOther;
					dbEvent.StartTime = eventObj.StartTime.HasValue ? eventObj.StartTime.Value : TimeSpan.Zero;
					dbEvent.EndTime = eventObj.EndTime.HasValue ? eventObj.EndTime.Value : TimeSpan.Zero;
					dbEvent.OrganizationId = eventObj.OrganizationId;
					dbEvent.OtherEventDescription = eventObj.EventDescription;
					dbEvent.PublicDescription = eventObj.PublicEventDescription;
					dbEvent.UpdatedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(eventObj.UpdatedByUserId))).UserName;

					// update data in database
					context.Entry(dbEvent).State = EntityState.Modified;

					// save changes - returns number of database changes
					var numberOfSaves = await context.SaveChangesAsync();
					// if there are changes then return success and the key of the new setting
					if (numberOfSaves > 0)
					{
						result.Succeeded = true;
					}

					var settingResult = new RequestResult();

					if (dbEvent.IsGolfTournamentEvent)
					{
						var activeGolfTournamentId = await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfTournamentId");
						var activeGolfBanquetTicketCost = await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfBanquetTicketCost");
						var activeGolfTicketCost = await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfTicketCost");

						#region Update Golf Tournament Id
						if (activeGolfTournamentId == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "GolfTournamentId",
								Notes = "Added automatically while updating event " + eventObj.EventName,
								Value = dbEvent.Id.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeGolfTournamentId.Name,
								Notes = activeGolfTournamentId.Notes + Environment.NewLine + "Updated automatically while updating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.Id.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("GolfTournamentId updated successfully with new value {@GolfEventId}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("GolfTournamentId NOT updated successfully with new value {@GolfEventId}", dbEvent.Id.ToString());
						}
						#endregion

						#region Golf Tournament Banquet Ticket Cost
						if (activeGolfBanquetTicketCost == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "GolfBanquetTicketCost",
								Notes = "Added automatically while updating event " + eventObj.EventName,
								Value = dbEvent.TicketPriceOther.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeGolfBanquetTicketCost.Name,
								Notes = activeGolfBanquetTicketCost.Notes + Environment.NewLine + "Updated automatically while updating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.TicketPriceOther.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("GolfBanquetTicketCost updated successfully with new value {@GolfBanquetTicketCost}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("GolfBanquetTicketCost NOT updated successfully with new value {@GolfBanquetTicketCost}", dbEvent.Id.ToString());
						}
						#endregion

						#region Golf Tournament Ticket Cost
						if (activeGolfTicketCost == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "GolfTicketCost",
								Notes = "Added automatically while updating event " + eventObj.EventName,
								Value = dbEvent.TicketPrice.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeGolfTicketCost.Name,
								Notes = activeGolfTicketCost.Notes + Environment.NewLine + "Updated automatically while updating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.TicketPrice.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("GolfTicketCost updated successfully with new value {@GolfTicketCost}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("GolfTicketCost NOT updated successfully with new value {@GolfTicketCost}", dbEvent.Id.ToString());
						}
						#endregion
					}
					else if (dbEvent.IsRoughRidersEvent)
					{
						var activeRoughRidersEventId = await _adminSystemSettingsManagerService.GetSettingDetailAsync("RoughRidersEventId");
						var activeRoughRidersTicketCost = await _adminSystemSettingsManagerService.GetSettingDetailAsync("RoughRidersTicketCost");

						#region Update RoughRiders Id
						if (activeRoughRidersEventId == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "RoughRidersEventId",
								Notes = "Added automatically while updating event " + eventObj.EventName,
								Value = dbEvent.Id.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeRoughRidersEventId.Name,
								Notes = activeRoughRidersEventId.Notes + Environment.NewLine + "Updated automatically while updating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.Id.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("RoughRidersEventId updated successfully with new value {@RoughRidersEventId}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("RoughRidersEventId NOT updated successfully with new value {@RoughRidersEventId}", dbEvent.Id.ToString());
						}
						#endregion

						#region RoughRiders Ticket Cost
						if (activeRoughRidersTicketCost == null)
						{
							settingResult = await _adminSystemSettingsManagerService.AddSettingAsync(new ViewModel.SystemSetting.Create
							{
								AddedByUserId = eventObj.AddedByUserId,
								Name = "RoughRidersTicketCost",
								Notes = "Added automatically while updating event " + eventObj.EventName,
								Value = dbEvent.TicketPrice.ToString()
							});
						}
						else
						{
							settingResult = await _adminSystemSettingsManagerService.UpdateSettingAsync(new ViewModel.SystemSetting.Edit
							{
								Name = activeRoughRidersTicketCost.Name,
								Notes = activeRoughRidersTicketCost.Notes + Environment.NewLine + "Updated automatically while updating event " + eventObj.EventName,
								UpdatedByUserId = eventObj.AddedByUserId,
								Value = dbEvent.TicketPrice.ToString()
							});

						}
						if (settingResult.Succeeded)
						{
							_logger.Information("RoughRidersTicketCost updated successfully with new value {@RoughRidersTicketCost}", dbEvent.Id.ToString());
						}
						else
						{
							_logger.Error("RoughRidersTicketCost NOT updated successfully with new value {@RoughRidersTicketCost}", dbEvent.Id.ToString());
						}
						#endregion
					}
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "event update validation error: {@ExistingEventItem} {@DbValidationErrors}", eventObj, ex.EntityValidationErrors);
			}

			return result;
		}

		public async Task<Edit> GetEventByIdAsync(Guid id)
		{
			var eventObj = new Edit();
			using (var context = new HuskyRescueEntities())
			{
				var dbEvent = await context.Events.SingleAsync(e => e.Id.Equals(id));

				if (dbEvent == null) return eventObj;

				eventObj.EventName = dbEvent.Name;
				eventObj.AddedByUserId = dbEvent.AddedByUserId;
				eventObj.IsActive = dbEvent.IsActive;
				//eventObj.IsDeleted = dbEvent.IsDeleted;
				eventObj.Is5KEvent = dbEvent.Is5kEvent;
				eventObj.IsAllDayEvent = dbEvent.IsAllDayEvent;
				eventObj.IsDogWashEvent = dbEvent.IsDogWashEvent;
				eventObj.IsGolfTournamentEvent = dbEvent.IsGolfTournamentEvent;
				eventObj.IsMeetAndGreetEvent = dbEvent.IsMeetAndGreetEvent;
				eventObj.IsOtherEvent = dbEvent.IsOtherEvent;
				eventObj.IsRaffleEvent = dbEvent.IsRaffleEvent;
				eventObj.IsRoughRidersEvent = dbEvent.IsRoughRidersEvent;
				eventObj.StartTime = dbEvent.StartTime;
				eventObj.EndTime = dbEvent.EndTime;
				eventObj.EventDate = dbEvent.EventDate.HasValue ? dbEvent.EventDate.Value : DateTime.MinValue;
				eventObj.AreTicketsSold = dbEvent.AreTicketsSold;
				eventObj.TicketPriceOther = dbEvent.TicketPriceOther.HasValue ? dbEvent.TicketPriceOther.Value : Decimal.Zero;
				eventObj.TicketPrice = dbEvent.TicketPrice;
				eventObj.OrganizationId = dbEvent.OrganizationId;
				eventObj.Id = dbEvent.Id;
				eventObj.OrganizationName = dbEvent.Organization.Name;


				_logger.Verbose("Get Event {@event}", eventObj);
			}
			return eventObj;
		}
	}


}
