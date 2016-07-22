using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Golf.Register;

namespace HuskyRescue.BusinessLogic
{
	public interface IGolfEventManagerService
	{
		Task<RequestResult> RegisterForTournamentPublic(CreatePublic register);
	}
}