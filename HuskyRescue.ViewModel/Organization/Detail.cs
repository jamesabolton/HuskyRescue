using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Organization
{
	public class Detail
	{
		public Guid Id { get; set; }

		[DisplayName("Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Deleted?")]
		public bool IsDeleted { get; set; }

		[DisplayName("Boarding?")]
		public bool IsBoardingPlace { get; set; }

		[DisplayName("Animal Clinic?")]
		public bool IsAnimalClinic { get; set; }

		[DisplayName("Donor?")]
		public bool IsDonor { get; set; }

		[DisplayName("Grant Giver?")]
		public bool IsGrantGiver { get; set; }

		[DisplayName("Sponsor?")]
		public bool IsSponsor { get; set; }

		[DisplayName("Added on Date")]
		public string AddedOnDate { get; set; }

		[DisplayName("Added by user")]
		public string AddedByUserName { get; set; }

		[DisplayName("Updated on date")]
		public string UpdatedOnDate { get; set; }

		[DisplayName("Updated by user")]
		public string UpdatedByUserName { get; set; }

		[DisplayName("Date activated")]
		public DateTime DateActive { get; set; }

		[DisplayName("Date inactive")]
		public string DateInactive { get; set; }

		[DisplayName("Date deleted (soft)")]
		public string DateDeleted { get; set; }

		[DisplayName("Name")]
		public string Name { get; set; }

		[DisplayName("Tax ID")]
		public string EIN { get; set; }

		[DisplayName("Notes")]
		[DataType(DataType.MultilineText)]
		public string Notes { get; set; }

		#region Phone Numbers
		[DisplayName("Phone Type")]
		public string PhoneNumber1Type { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber1 { get; set; }

		[DisplayName("Phone Type")]
		public string PhoneNumber2Type { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber2 { get; set; }

		[DisplayName("Phone Type")]
		public string PhoneNumber3Type { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber3 { get; set; }
		#endregion

		#region Email Addresses
		[DisplayName("Email Type")]
		public string Email1Type { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email1 { get; set; }

		[DisplayName("Email Type")]
		public string Email2Type { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email2 { get; set; }
		#endregion

		#region Home Address
		[DisplayName("Home Address Street")]
		public string HomeAddress1 { get; set; }

		[DisplayName("Home Address Street Cont.")]
		public string HomeAddress2 { get; set; }

		[DisplayName("Home Address Street Cont.")]
		public string HomeAddress3 { get; set; }

		[DisplayName("Home Address City")]
		public string HomeCity { get; set; }

		[DisplayName("Home Address State")]
		public string HomeAddressState { get; set; }

		[DisplayName("Home Address ZIP")]
		[DataType(DataType.PostalCode)]
		public string HomeZipCode { get; set; }

		[DisplayName("Home Address Country Code")]
		public string HomeCountryId { get; set; }
		#endregion

		#region Mailing Address
		[DisplayName("Mailing Address Street")]
		public string MailingAddress1 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		public string MailingAddress2 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		public string MailingAddress3 { get; set; }

		[DisplayName("Mailing Address City")]
		public string MailingCity { get; set; }

		[DisplayName("Mailing Address State")]
		public string MailingAddressState { get; set; }

		[DisplayName("Mailing Address ZIP")]
		[DataType(DataType.PostalCode)]
		public string MailingZipCode { get; set; }

		[DisplayName("Mailing Address Country Code")]
		public string MailingCountryId { get; set; }
		#endregion

		#region Billing Address
		[DisplayName("Billing Address Street")]
		public string BillingAddress1 { get; set; }

		[DisplayName("Billing Address Street Cont.")]
		public string BillingAddress2 { get; set; }

		[DisplayName("Billing Address Street Cont.")]
		public string BillingAddress3 { get; set; }

		[DisplayName("Billing Address City")]
		public string BillingCity { get; set; }

		[DisplayName("Billing Address State")]
		public string BillingAddressState { get; set; }

		[DisplayName("Billing Address ZIP")]
		[DataType(DataType.PostalCode)]
		public string BillingZipCode { get; set; }

		[DisplayName("Billing Address Country Code")]
		public string BillingCountryId { get; set; }
		#endregion

		//public virtual ICollection<AnimalPlacement> AnimalPlacements { get; set; }
		//public virtual ICollection<Event> Events { get; set; }
		//public virtual ICollection<EventSponsor> EventSponsors { get; set; }
		//public virtual ICollection<InventoryPlacement> InventoryPlacements { get; set; }
		//public virtual ICollection<MicrochipManufacturer> MicrochipManufacturers { get; set; }
		//public virtual ICollection<OrganizationContact> OrganizationContacts { get; set; }
		//public virtual ICollection<PaymentMethod> PaymentMethods { get; set; }
		//public virtual ICollection<MarketingPreference> MarketingPreferences { get; set; }
		//public virtual ICollection<Website> Websites { get; set; }
	}
}
