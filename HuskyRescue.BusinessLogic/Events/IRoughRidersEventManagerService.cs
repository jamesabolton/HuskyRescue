using System;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.RoughRiders.Register;

namespace HuskyRescue.BusinessLogic
{
	public interface IRoughRidersEventManagerService
	{
		Task<RequestResult> RegisterForTournamentPublic(CreatePublic register);

		Task<bool> IsEventActive();
	}
}