using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HuskyRescue.ViewModel.Admin.Roles
{
	public class RoleResource : IEqualityComparer<RoleResource>
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

		public string Id { get; set; }

		public string Name { get; set; }

		public bool Selected { get; set; }

		public int Operations { get; set; }

		public string OperationDesc { get; set; }

		public RoleResource()
		{
			Selected = false;
		}
	}
}