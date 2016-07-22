using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Adoption;

namespace HuskyRescue.BusinessLogic
{
	public interface IApplicationManagerService
	{
		Task<RequestResult> AddAdoptionApplication(Apply app);
	}
}