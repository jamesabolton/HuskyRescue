using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Organization;

namespace HuskyRescue.BusinessLogic
{
	public interface IAdminOrganizationsManagerService
	{
		Task<List<List>> GetFilteredOrgsListAsync(bool? isActive = true, bool? isDeleted = null,
			bool? animalClinicOnly = null, bool? boardingOnly = null, bool? donorOnly = null,
			bool? granterOnly = null, string nameContains = "");
		Task<List<List>> GetAllOrgsListAsync();
		Task<RequestResult> AddOrganizationAsync(Create org);
	}
}