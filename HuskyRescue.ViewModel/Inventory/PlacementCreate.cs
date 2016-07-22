using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Inventory
{
	public class PlacementCreate
	{
		public Guid Id { get; set; }

		[Required]
		public Guid InventoryId { get; set; }

		[DisplayName("Associated Inventory Item")]
		public IEnumerable<SelectListItem> InventoryItems { get; set; }

		[Required]
		[DisplayName("Quantity")]
		public int QuantityAtLocation { get; set; }

		[DisplayName("People")]
		public IEnumerable<SelectListItem> People { get; set; }

		[DisplayName("Organizations")]
		public IEnumerable<SelectListItem> Orgs { get; set; }

		[Required]
		public Guid LocationId { get; set; }

		/// <summary>
		/// Hard coded to P or B in the view based off of the selected People or Orgs
		/// </summary>
		[Required]
		public string LocationType { get; set; }

		[StringLength(4000)]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		public PlacementCreate()
		{
			InventoryItems = new List<SelectListItem>();
			People = new List<SelectListItem>();
			Orgs = new List<SelectListItem>();
		}
	}
}
