using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Admin.Roles
{
	public class Create
	{
		[Display(Name = "Role Name")]
		[Required(ErrorMessage = "role name is required")]
		[AssertThat("Length(Name) <= 256", ErrorMessage = "role name can be no more than 256 characters")]
		public string Name { get; set; }

		public IEnumerable<RoleResource> Resources { get; set; }

		public Create()
		{
			Resources = new List<RoleResource>();
		}
	}
}
