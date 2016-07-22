using System;
using System.ComponentModel;

namespace HuskyRescue.ViewModel.Inventory
{
	public class ImageList
	{
		public Guid Id { get; set; }

		public Guid InventoryId { get; set; }

		[DisplayName("Relative Path")]
		public string RelativePath { get; set; }

		[DisplayName("Physicla Path")]
		public string PhysicalPath { get; set; }

		[DisplayName("Alt Text")]
		public string AltText { get; set; }

		[DisplayName("Item Name")]
		public string ItemName { get; set; }
	}
}
