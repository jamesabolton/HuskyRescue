using System.Collections.Generic;
using HuskyRescue.ViewModel.Common;

namespace HuskyRescue.BusinessLogic
{
	public interface IFacebookManagerService
	{
		List<FacebookEvent> GetEvents();
	}
}