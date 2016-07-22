using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel.Inventory
{
	public class CategoryEdit
	{
		[Required]
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
