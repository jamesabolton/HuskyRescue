using System;
using System.ComponentModel;

namespace HuskyRescue.ViewModel.Inventory
{
	public class PlacementList
	{
		public Guid Id { get; set; }

		public Guid InventoryId { get; set; }

		[DisplayName("Name")]
		public string InventoryName { get; set; }

		[DisplayName("Quantity")]
		public int QuantityAtLocation { get; set; }

		[DisplayName("Added On")]
		public string AddedOnDate { get; set; }
		
		[DisplayName("Updated On")]
		public string UpdatedOnDate { get; set; }

		public bool IsLocationPerson { get; set; }
		
		public bool IsLocationOrg { get; set; }

		[DisplayName("Location")]
		public string LocationName { get; set; }

		public Guid LocationId { get; set; }
	}
}
