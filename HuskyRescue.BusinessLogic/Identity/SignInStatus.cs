using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.BusinessLogic.Identity
{
	public enum SignInStatus
	{
		Success,
		LockedOut,
		RequiresVerification,
		Failure,
		RequiresEmailConfirmation
	}
}
