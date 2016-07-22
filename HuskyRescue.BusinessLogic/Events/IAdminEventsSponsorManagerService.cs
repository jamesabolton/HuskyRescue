using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.EventsSponsor;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminEventsSponsorManagerService
	{
		Task<List<List>> GetAllSponsorsListAsync();
		Task<RequestResult> AddEventSponsorAsync(Create eventSponsorObj);
	}
}