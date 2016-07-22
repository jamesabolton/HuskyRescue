using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.SystemSetting;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
	public static class AdminSystemSettings
	{
		private static Dictionary<string, string> _settings = new Dictionary<string, string>();

		public static string GetSetting(string key, bool forceRefresh = false)
		{
			if (_settings.Count == 0 || forceRefresh)
			{
				using (var context = new HuskyRescueEntities())
				{
					_settings = context.SystemSettings.ToList().ToDictionary(m => m.Name, m => m.Value);
				}
			}

			string settingValue;

			try
			{
				settingValue = _settings[key];
			}
			catch (KeyNotFoundException ex)
			{
				settingValue = "KEY NOT FOUND";
			}

			return settingValue;
		}

		public static void RefreshSettings()
		{
			using (var context = new HuskyRescueEntities())
			{
				_settings = context.SystemSettings.ToList().ToDictionary(m => m.Name, m => m.Value);
			}
		}
	}

	public class AdminSystemSettingsManagerService : IAdminSystemSettingsManagerService
	{
		private readonly ILogger _logger;

		public AdminSystemSettingsManagerService(ILogger ilogger)
		{
			_logger = ilogger;
		}

		/// <summary>
		/// Get a list of resources to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the system settings</returns>
		public async Task<List<List>> GetSettingListAsync()
		{
			var settings = new List<List>();
			using (var context = new HuskyRescueEntities())
			{
				var systemSettings = await context.SystemSettings.ToListAsync();

				if (systemSettings == null) return settings;
				settings.AddRange(systemSettings.Select(dbSetting => new List
				{
					Name = dbSetting.Name,
					Value = dbSetting.Value,
					AddedByUserId = context.AspNetUsers.Find(dbSetting.AddedByUserId.ToString()).UserName,
					AddedOnDate = dbSetting.AddedOnDate.ToShortDateString(),
					UpdatedByUserId = dbSetting.UpdatedByUserId != null ? context.AspNetUsers.Find(dbSetting.UpdatedByUserId.ToString()).UserName : string.Empty,
					UpdatedOnDate = dbSetting.UpdatedOnDate != null ? dbSetting.UpdatedOnDate.Value.ToShortDateString() : null
				}));

				_logger.Verbose("SystemSettingList {@resources}", settings);
			}
			return settings;
		}

		/// <summary>
		/// Retrieve the full details of a system setting
		/// </summary>
		/// <param name="id">name of the system setting</param>
		/// <returns>Detail object for view model</returns>
		public async Task<Detail> GetSettingDetailAsync(string id)
		{
			var settingDetail = new Detail();

			using (var context = new HuskyRescueEntities())
			{
				var dbSetting = await context.SystemSettings.FindAsync(id);

				if (dbSetting != null)
				{
					settingDetail = new Detail
					                {
						                Name = dbSetting.Name,
						                Value = dbSetting.Value,
						                AddedByUserId = (await context.AspNetUsers.FindAsync(dbSetting.AddedByUserId)).UserName,
						                AddedOnDate = dbSetting.AddedOnDate.ToShortDateString(),
						                UpdatedByUserId =
							                dbSetting.UpdatedByUserId != null
								                ? (await context.AspNetUsers.FindAsync(dbSetting.UpdatedByUserId)).UserName
								                : string.Empty,
						                UpdatedOnDate =
							                dbSetting.UpdatedOnDate != null ? dbSetting.UpdatedOnDate.Value.ToShortDateString() : null
					                };
				}
				else
				{
					settingDetail = null;
				}
			}

			return settingDetail;
		}

		/// <summary>
		/// Add a new system setting to the database
		/// </summary>
		/// <param name="setting">system setting view model object</param>
		/// <returns>RequestResult indicating success</returns>
		public async Task<RequestResult> AddSettingAsync(Create setting)
		{
			var result = new RequestResult();

			using (var context = new HuskyRescueEntities())
			{
				var dbSetting = new SystemSetting
				{
					Name = setting.Name,
					Value = setting.Value,
					Notes = setting.Notes,
					AddedOnDate = DateTime.Today,
				};

				var userIdGuid = new Guid();

				if (Guid.TryParse(setting.AddedByUserId, out userIdGuid))
				{
					dbSetting.AddedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(setting.AddedByUserId))).Id;
				}
				else
				{
					dbSetting.AddedByUserId = (await context.AspNetUsers.SingleAsync(u => u.UserName.Equals(setting.AddedByUserId))).Id;
				}

				// add setting to database
				dbSetting = context.SystemSettings.Add(dbSetting);

				// save changes - returns number of database changes
				var numberOfSaves = await context.SaveChangesAsync();
				// if there are changes then return success and the key of the new setting
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = dbSetting.Name;
				}
			}

			AdminSystemSettings.RefreshSettings();

			return result;
		}

		/// <summary>
		/// Update setting in database
		/// </summary>
		/// <param name="setting">setting view model to update in database</param>
		/// <returns>RequestResult object indicating success or error</returns>
		public async Task<RequestResult> UpdateSettingAsync(Edit setting)
		{
			var result = new RequestResult();

			using (var context = new HuskyRescueEntities())
			{
				var dbSetting = await context.SystemSettings.FindAsync(setting.Name);

				dbSetting.Value = setting.Value;
				dbSetting.Notes = setting.Notes;
				dbSetting.UpdatedOnDate = DateTime.Today;

				var userIdGuid = new Guid();

				if (Guid.TryParse(setting.UpdatedByUserId, out userIdGuid))
				{
					dbSetting.UpdatedByUserId = (await context.AspNetUsers.SingleAsync(u => u.Id.Equals(setting.UpdatedByUserId))).Id;
				}
				else
				{
					dbSetting.UpdatedByUserId = (await context.AspNetUsers.SingleAsync(u => u.UserName.Equals(setting.UpdatedByUserId))).Id;
				}

				context.Entry(dbSetting).State = EntityState.Modified;

				// save changes - returns number of database changes
				var numberOfSaves = await context.SaveChangesAsync();

				// if there are changes then return success and the key of the new setting
				if (numberOfSaves > 0)
				{
					result.Succeeded = true;
					result.NewKey = dbSetting.Name;
				}
			}

			AdminSystemSettings.RefreshSettings();

			return result;
		}
	}
}
