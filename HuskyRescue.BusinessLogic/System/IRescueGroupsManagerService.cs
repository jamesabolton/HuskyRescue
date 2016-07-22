using System.Collections.Generic;
using System.Threading.Tasks;
using HuskyRescue.ViewModel.Common;

namespace HuskyRescue.BusinessLogic
{
	public interface IRescueGroupsManagerService
	{
		List<RescueGroupAnimal> GetAdoptableHuskiesAsync();
		Task<List<RescueGroupAnimal>> GetFosterableHuskiesAsync();
		Task<RescueGroupAnimal> GetHuskyProfileAsync(string id);
	}
}