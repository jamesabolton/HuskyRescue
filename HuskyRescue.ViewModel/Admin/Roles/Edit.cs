using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using HuskyRescue.ViewModel.Admin.ResourceManager;

namespace HuskyRescue.ViewModel.Admin.Roles
{
	public class Edit
	{
		public string Id { get; set; }

		[Display(Name = "Role Name")]
		[Required(ErrorMessage = "role name is required")]
		[AssertThat("Length(Name) <= 256", ErrorMessage = "role name can be no more than 256 characters")]
		public string Name { get; set; }

		public IEnumerable<RoleResource> Resources { get; set; }

		public Edit()
		{
			Resources = new List<RoleResource>();
		}
	}
}
