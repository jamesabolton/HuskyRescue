using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Admin.Roles
{
	public class Detail
	{
		public string Id { get; set; }

		[Display(Name = "Role Name")]
		public string Name { get; set; }

		public List<RoleResource> Resources { get; set; }

		public Detail()
		{
			Resources = new List<RoleResource>();
		}
	}
}
