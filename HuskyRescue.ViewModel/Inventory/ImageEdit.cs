using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Inventory
{
	public class ImageEdit
	{
		[Required]
		public Guid Id { get; set; }

		[Required]
		public Guid InventoryId { get; set; }

		[DisplayName("Associated Inventory Item")]
		public IEnumerable<SelectListItem> InventoryItems { get; set; }

		[Required]
		[StringLength(1000)]
		[DisplayName("Relative Path")]
		public string RelativePath { get; set; }

		[Required]
		[StringLength(1000)]
		[DisplayName("Physical Path")]
		public string PhysicalPath { get; set; }

		[StringLength(500)]
		[DisplayName("Alternate Text / Title")]
		public string AltText { get; set; }

		public ImageEdit()
		{
			InventoryItems = new List<SelectListItem>();
		}
	}
}
