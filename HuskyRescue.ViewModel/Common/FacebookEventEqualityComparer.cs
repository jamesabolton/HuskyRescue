using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel.Common
{
	class FacebookEventEqualityComparer : IEqualityComparer<FacebookEvent>
	{
		public bool Equals(FacebookEvent x, FacebookEvent y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(FacebookEvent obj)
		{
			var str = obj.Id + obj.StartTime.ToLongDateString();
			return str.GetHashCode();
		}
	}
}
