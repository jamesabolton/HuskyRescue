using System.Collections.Generic;

namespace HuskyRescue.ViewModel.Admin.Roles
{
	public class RoleResourceEqualityComparer : IEqualityComparer<RoleResource>
	{

		#region IEqualityComparer<List> Members

		public bool Equals(RoleResource x, RoleResource y)
		{
			return x.Id == y.Id;
		}

		public int GetHashCode(RoleResource obj)
		{
			var str = obj.Id + obj.Name;
			return str.GetHashCode();
		}

		#endregion
	}
}