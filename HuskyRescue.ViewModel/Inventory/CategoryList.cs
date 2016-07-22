using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Inventory
{
	public class CategoryList
	{

		public Guid Id { get; set; }

		[DisplayName("Category Name")]
		public string Name { get; set; }

		[DisplayName("Description")]
		public string Description { get; set; }
	}
}
