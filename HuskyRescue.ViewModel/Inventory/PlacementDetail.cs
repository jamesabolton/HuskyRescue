using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel.Inventory
{
	public class PlacementDetail
	{
		public Guid Id { get; set; }

		public Guid InventoryId { get; set; }

		[DisplayName("Name")]
		public string InventoryName { get; set; }

		[DisplayName("Quantity")]
		public int QuantityAtLocation { get; set; }

		[DisplayName("Added On")]
		public string AddedOnDate { get; set; }

		[DisplayName("Added By")]
		public string AddedByUserName { get; set; }

		public Guid AddedByUserId { get; set; }

		[DisplayName("Updated On")]
		public string UpdatedOnDate { get; set; }

		[DisplayName("Updated By")]
		public string UpdatedByUserName { get; set; }

		public Nullable<Guid> UpdatedByUserId { get; set; }

		public bool IsLocationPerson { get; set; }

		public bool IsLocationOrg { get; set; }

		[DisplayName("Location")]
		public string LocationName { get; set; }

		[DisplayName("Address")]
		public string LocationAddress { get; set; }

		public Guid LocationId { get; set; }

		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }
	}
}
