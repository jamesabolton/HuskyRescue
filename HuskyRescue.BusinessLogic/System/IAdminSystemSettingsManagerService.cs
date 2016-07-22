using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.SystemSetting;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminSystemSettingsManagerService
	{
		/// <summary>
		/// Get a list of resources to display on the client
		/// </summary>
		/// <returns>list of view model objects describing the system settings</returns>
		Task<List<List>> GetSettingListAsync();

		/// <summary>
		/// Retrieve the full details of a system setting
		/// </summary>
		/// <param name="id">name of the system setting</param>
		/// <returns>Detail object for view model</returns>
		Task<Detail> GetSettingDetailAsync(string id);

		/// <summary>
		/// Add a new system setting to the database
		/// </summary>
		/// <param name="setting">system setting view model object</param>
		/// <returns>RequestResult indicating success</returns>
		Task<RequestResult> AddSettingAsync(Create setting);

		/// <summary>
		/// Update setting in database
		/// </summary>
		/// <param name="setting">setting view model to update in database</param>
		/// <returns>RequestResult object indicating success or error</returns>
		Task<RequestResult> UpdateSettingAsync(Edit setting);
	}
}