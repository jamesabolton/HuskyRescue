using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.EventsSponsorship;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminEventsSponsorshipManagerService
	{
		Task<List<List>> GetAllSponsorshipsListAsync();
		Task<List<List>> GetEventSponsorshipsListAsync(Guid eventId);
		Task<RequestResult> AddEventSponsorshipAsync(Create eventSponsorshipObj);
		Task<RequestResult> UpdateEventSponsorshipAvailableCount(Guid eventSponsorshipId, Guid eventId, int number);
		Detail GetSponsorshipDetail(Guid id);
		Edit GetSponsorshipDetailForEdit(Guid id);
		Task<RequestResult> UpdateEventSponsorshipAsync(Edit eventSponsorshipObj);
	}
}