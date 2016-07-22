using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Inventory
{
	public class CategoryDetail
	{
		public CategoryDetail()
		{
			AssociatedItems = new List<KeyValuePair<Guid, string>>();
		}

		[Required]
		public Guid Id { get; set; }

		[DisplayName("Category Name")]
		[StringLength(200)]
		[Required]
		public string Name { get; set; }

		[DisplayName("Description")]
		[StringLength(4000)]
		public string Description { get; set; }

		[DisplayName("Items In This Category")]
		public List<KeyValuePair<Guid, String>> AssociatedItems { get; set; } 
	}
}
