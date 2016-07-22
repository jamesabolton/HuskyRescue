using HuskyRescue.ViewModel.Adoption;

namespace HuskyRescue.BusinessLogic
{
	public interface IFormSerivce
	{
		string CreateAdoptionApplicationPdf(Apply app);
	}
}