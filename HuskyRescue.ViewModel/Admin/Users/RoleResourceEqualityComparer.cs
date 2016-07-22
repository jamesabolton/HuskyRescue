using System.Collections.Generic;

namespace HuskyRescue.ViewModel.Admin.Users
{
	public class RoleEqualityComparer : IEqualityComparer<Role>
	{

		#region IEqualityComparer<List> Members

		public bool Equals(Role x, Role y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(Role obj)
		{
			var str = obj.Id + obj.Name;
			return str.GetHashCode();
		}

		#endregion
	}
}