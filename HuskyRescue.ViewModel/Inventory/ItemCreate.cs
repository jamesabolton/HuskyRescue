using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Inventory
{
	public class ItemCreate
	{
		public ItemCreate()
		{
			Catagories = new List<SelectListItem>();
			PlacementPeople = new List<SelectListItem>();
			PlacementOrgs = new List<SelectListItem>();
			Images = new List<SelectListItem>();
		}

		public Guid Id { get; set; }

		[Required]
		public Guid CategoryId { get; set; }

		[DisplayName("Category")]
		public IEnumerable<SelectListItem> Catagories { get; set; }

		[DisplayName("Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Deleted?")]
		public bool IsDeleted { get; set; }

		[DisplayName("Item Name")]
		[StringLength(200)]
		[Required]
		public string Name { get; set; }

		[Required]
		[DisplayName("Number of Items")]
		public int Quantity { get; set; }

		[Required]
		[DisplayName("Cost To The Group")]
		[DataType(DataType.Currency)]
		public decimal CostToBuy { get; set; }

		[Required]
		[DisplayName("Available For Online Store Sales?")]
		public bool IsAvailableInStore { get; set; }

		[Required]
		[DisplayName("Price To Sell For In Online Store")]
		[DataType(DataType.Currency)]
		public decimal StoreSellPrice { get; set; }

		[Required]
		[DisplayName("Cost to Ship One Item")]
		[DataType(DataType.Currency)]
		public decimal ShippingCost { get; set; }

		[DisplayName("Brand of the Item/Product (if any)")]
		[StringLength(200)]
		public string Brand { get; set; }

		[DisplayName("Model (Number) of the Item/Product (if any)")]
		[StringLength(50)]
		public string Model { get; set; }

		[StringLength(4000)]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }


		#region Placement Info
		[DisplayName("People")]
		public IEnumerable<SelectListItem> PlacementPeople { get; set; }

		[DisplayName("Organizations")]
		public IEnumerable<SelectListItem> PlacementOrgs { get; set; }

		/// <summary>
		/// Hard coded to P or B in the view based off of the selected People or Orgs
		/// </summary>
		[Required]
		public string PlacementType { get; set; }

		[Required]
		public Guid PlacementId { get; set; }

		#endregion

		#region Image Info
		[DisplayName("Image")]
		public IEnumerable<SelectListItem> Images { get; set; }

		public Guid ImageId { get; set; }

		[Required]
		[StringLength(1000)]
		[DisplayName("Relative Path")]
		public string NewImageRelativePath { get; set; }

		[Required]
		[StringLength(1000)]
		[DisplayName("Physical Path")]
		public string NewImagePhysicalPath { get; set; }

		[StringLength(500)]
		[DisplayName("Alternate Text / Title")]
		public string NewImageAltText { get; set; }
		#endregion
	}

}
