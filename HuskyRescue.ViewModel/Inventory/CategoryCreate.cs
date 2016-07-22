using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Inventory
{
	public class CategoryCreate
	{

		public Guid Id { get; set; }

		[DisplayName("Category Name")]
		[StringLength(200)]
		[Required]
		public string Name { get; set; }

		[DisplayName("Description")]
		[StringLength(4000)]
		public string Description { get; set; }
	}
}
