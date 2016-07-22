using System;
using System.ComponentModel;

namespace HuskyRescue.ViewModel.Inventory
{
	public class ImageDetail
	{
		public Guid Id { get; set; }

		public Guid InventoryId { get; set; }

		public Guid InventoryName { get; set; }

		[DisplayName("Relative Path")]
		public string RelativePath { get; set; }

		[DisplayName("Physical Path")]
		public string PhysicalPath { get; set; }

		[DisplayName("Alternate Text / Title")]
		public string AltText { get; set; }
	}
}
