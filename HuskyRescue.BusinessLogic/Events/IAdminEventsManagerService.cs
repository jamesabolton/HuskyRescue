using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Events;
using System;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminEventsManagerService
	{
		Task<List<List>> GetAllEventsListAsync();
		Task<RequestResult> AddEventAsync(Create eventObj);
		Task<RequestResult> EditEventAsync(Edit eventObj);
		Task<Edit> GetEventByIdAsync(Guid id);
	}
}