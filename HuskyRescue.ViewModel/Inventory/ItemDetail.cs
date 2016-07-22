using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Inventory
{
	public class ItemDetail
	{
		public ItemDetail()
		{
			Placements = new List<PlacementList>();
		}

		public Guid Id { get; set; }
		public Guid CategoryId { get; set; }

		[DisplayName("Category")]
		public string CatagoryName { get; set; }

		[DisplayName("Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Deleted?")]
		public bool IsDeleted { get; set; }

		[DisplayName("Item Name")]
		public string Name { get; set; }

		[DisplayName("Number of Items")]
		public int Quantity { get; set; }

		[DisplayName("Cost To The Group")]
		[DataType(DataType.Currency)]
		public decimal CostToBuy { get; set; }

		[DisplayName("Available For Online Store Sales?")]
		public bool IsAvailableInStore { get; set; }

		[DisplayName("Price To Sell For In Online Store")]
		[DataType(DataType.Currency)]
		public decimal StoreSellPrice { get; set; }

		[DisplayName("Cost to Ship One Item")]
		[DataType(DataType.Currency)]
		public decimal ShippingCost { get; set; }

		[DisplayName("Brand of the Item/Product (if any)")]
		public string Brand { get; set; }

		[DisplayName("Model (Number) of the Item/Product (if any)")]
		public string Model { get; set; }

		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		#region Placement Info
		public List<PlacementList> Placements { get; set; }
		#endregion

		#region Image Info
		public string ImagePath { get; set; }

		public string ImageAltText { get; set; }

		public Guid ImageId { get; set; }
		#endregion
	}
}
