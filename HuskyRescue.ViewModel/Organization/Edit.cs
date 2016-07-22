using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Organization
{
	public class Edit
	{
		public Edit()
		{
			AddressStates = new List<SelectListItem>();
			PhoneNumberTypes = new List<SelectListItem>();
			EmailTypes = new List<SelectListItem>();
		}

		[Required]
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

		[DisplayName("Name")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		[Required(ErrorMessage = "org name required")]
		public string Name { get; set; }

		[DisplayName("Tax ID")]
		[StringLength(10, ErrorMessage = "no more than 10 numbers")]
		public string EmployeeIdNumber { get; set; }

		[DisplayName("Notes")]
		[DataType(DataType.MultilineText)]
		[StringLength(4000, ErrorMessage = "no more than 4000 characters")]
		public string Notes { get; set; }

		#region Phone Numbers
		public List<SelectListItem> PhoneNumberTypes { get; set; }

		[DisplayName("Phone Type")]
		public int PhoneNumber1TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber1 { get; set; }

		[DisplayName("Phone Type")]
		public int PhoneNumber2TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber2 { get; set; }

		[DisplayName("Phone Type")]
		public int PhoneNumber3TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber3 { get; set; }
		#endregion

		#region Email Addresses
		public List<SelectListItem> EmailTypes { get; set; }

		[DisplayName("Email Type")]
		public int Email1TypeId { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email1 { get; set; }

		[DisplayName("Email Type")]
		public int Email2TypeId { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email2 { get; set; }
		#endregion

		public List<SelectListItem> AddressStates { get; set; }

		#region Home Address
		[DisplayName("Home Address Street")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string HomeAddress1 { get; set; }

		[DisplayName("Home Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string HomeAddress2 { get; set; }

		[DisplayName("Home Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string HomeAddress3 { get; set; }

		[DisplayName("Home Address City")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string HomeAddressCity { get; set; }

		[DisplayName("Home Address State")]
		public int? HomeAddressStateId { get; set; }

		[DisplayName("Home Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10, ErrorMessage = "no more than 10 characters")]
		public string HomeAddressZipCode { get; set; }

		[DisplayName("Home Address Country Code")]
		[StringLength(3, ErrorMessage = "no more than 3 characters")]
		public string HomeAddressCountryId { get; set; }
		#endregion

		#region Mailing Address
		[DisplayName("Mailing Address Street")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddress1 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddress2 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddress3 { get; set; }

		[DisplayName("Mailing Address City")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddressCity { get; set; }

		[DisplayName("Mailing Address State")]
		public int? MailingAddressStateId { get; set; }

		[DisplayName("Mailing Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10, ErrorMessage = "no more than 10 characters")]
		public string MailingAddressZipCode { get; set; }

		[DisplayName("Mailing Address Country Code")]
		[StringLength(3, ErrorMessage = "no more than 3 characters")]
		public string MailingAddressCountryId { get; set; }
		#endregion

		#region Billing Address
		[DisplayName("Billing Address Street")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string BillingAddress1 { get; set; }

		[DisplayName("Billing Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string BillingAddress2 { get; set; }

		[DisplayName("Billing Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string BillingAddress3 { get; set; }

		[DisplayName("Billing Address City")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string BillingAddressCity { get; set; }

		[DisplayName("Billing Address State")]
		public int? BillingAddressStateId { get; set; }

		[DisplayName("Billing Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10, ErrorMessage = "no more than 10 characters")]
		public string BillingAddressZipCode { get; set; }

		[DisplayName("Billing Address Country Code")]
		[StringLength(3, ErrorMessage = "no more than 3 characters")]
		public string BillingAddressCountryId { get; set; }
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
