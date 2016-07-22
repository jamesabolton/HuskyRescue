using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using HuskyRescue.ViewModel.Common;

namespace HuskyRescue.ViewModel.Organization
{
	public class Create
	{
		public Create()
		{
			AddressStates = new List<SelectListItem>();
			AddressStates = Lists.GetStateList();
			PhoneNumberTypes = new List<SelectListItem>();
			PhoneNumberTypes = Lists.GetPhoneTypeList();
			EmailTypes = new List<SelectListItem>();
			EmailTypes = Lists.GetEmailTypeList();
		}

		public string AddedByUserId { get; set; }

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
		public int? PhoneNumber1TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber1 { get; set; }

		[DisplayName("Phone Type")]
		public int? PhoneNumber2TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber2 { get; set; }

		[DisplayName("Phone Type")]
		public int? PhoneNumber3TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber3 { get; set; }
		#endregion

		#region Email Addresses
		public List<SelectListItem> EmailTypes { get; set; }

		[DisplayName("Type")]
		public int? Email1TypeId { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email1 { get; set; }

		[DisplayName("Type")]
		public int? Email2TypeId { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email2 { get; set; }
		#endregion

		[DisplayName("Website")]
		[StringLength(1000, ErrorMessage = "no more than 1000 characters")]
		public string Website { get; set; }

		public List<SelectListItem> AddressStates { get; set; }

		#region Mailing Address
		[DisplayName("Address Street")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddressStreet1 { get; set; }

		[DisplayName("Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddressStreet2 { get; set; }

		[DisplayName("Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddressStreet3 { get; set; }

		[DisplayName("Address City")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string MailingAddressCity { get; set; }

		[DisplayName("Address State")]
		public int? MailingAddressStateId { get; set; }

		[DisplayName("Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10, ErrorMessage = "no more than 10 characters")]
		public string MailingAddressPostalCode { get; set; }

		[DisplayName("Address Country Code")]
		[StringLength(3, ErrorMessage = "no more than 3 characters")]
		public string MailingAddressCountryId { get; set; }
		#endregion

		#region Physical Address
		[DisplayName("Address Street")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string PhysicalAddressStreet1 { get; set; }

		[DisplayName("Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string PhysicalAddressStreet2 { get; set; }

		[DisplayName("Address Street Cont.")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string PhysicalAddressStreet3 { get; set; }

		[DisplayName("Address City")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		public string PhysicalAddressCity { get; set; }

		[DisplayName("Address State")]
		public int? PhysicalAddressStateId { get; set; }

		[DisplayName("Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10, ErrorMessage = "no more than 10 characters")]
		public string PhysicalAddressPostalCode { get; set; }

		[DisplayName("Address Country Code")]
		[StringLength(3, ErrorMessage = "no more than 3 characters")]
		public string PhysicalAddressCountryId { get; set; }
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
