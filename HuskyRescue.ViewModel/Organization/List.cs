using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Organization
{
	public class List
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
		public DateTime AddedOnDate { get; set; }

		[DisplayName("Added by user")]
		public string AddedByUserName { get; set; }

		[DisplayName("Updated on date")]
		public DateTime? UpdatedOnDate { get; set; }

		[DisplayName("Updated by user")]
		public string UpdatedByUserName { get; set; }

		[DisplayName("Date activated")]
		public DateTime DateActive { get; set; }

		[DisplayName("Date inactive")]
		public DateTime? DateInactive { get; set; }

		[DisplayName("Date deleted (soft)")]
		public DateTime? DateDeleted { get; set; }

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

		#region Mailing Address
		public string MailingAddressFull
		{
			get
			{
				var address = string.Empty;
				address += MailingAddress1;
				if (!string.IsNullOrEmpty(MailingAddress2)) { address += " " + MailingAddress2; }
				if (!string.IsNullOrEmpty(MailingAddress3)) { address += " " + MailingAddress3; }
				address += ", " + MailingAddressCity;
				address += ", " + MailingAddressState;
				address += " " + MailingAddressZipCode;
				return address;
			}

		}

		[DisplayName("Mailing Address Street")]
		public string MailingAddress1 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		public string MailingAddress2 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		public string MailingAddress3 { get; set; }

		[DisplayName("Mailing Address City")]
		public string MailingAddressCity { get; set; }

		[DisplayName("Mailing Address State")]
		public string MailingAddressState { get; set; }

		[DisplayName("Mailing Address ZIP")]
		[DataType(DataType.PostalCode)]
		public string MailingAddressZipCode { get; set; }

		[DisplayName("Mailing Address Country Code")]
		public string MailingAddressCountryId { get; set; }
		#endregion

		#region Physical Address
		public string PhysicalAddressFull
		{
			get
			{
				var address = string.Empty;
				address += PhysicalAddress1;
				if (!string.IsNullOrEmpty(PhysicalAddress2)) { address += " " + PhysicalAddress2; }
				if (!string.IsNullOrEmpty(PhysicalAddress3)) { address += " " + PhysicalAddress3; }
				address += ", " + PhysicalAddressCity;
				address += ", " + PhysicalAddressState;
				address += " " + PhysicalAddressZipCode;
				return address;
			}

		}

		[DisplayName("Physical Address Street")]
		public string PhysicalAddress1 { get; set; }

		[DisplayName("Physical Address Street Cont.")]
		public string PhysicalAddress2 { get; set; }

		[DisplayName("Physical Address Street Cont.")]
		public string PhysicalAddress3 { get; set; }

		[DisplayName("Physical Address City")]
		public string PhysicalAddressCity { get; set; }

		[DisplayName("Physical Address State")]
		public string PhysicalAddressState { get; set; }

		[DisplayName("Physical Address ZIP")]
		[DataType(DataType.PostalCode)]
		public string PhysicalAddressZipCode { get; set; }

		[DisplayName("Physical Address Country Code")]
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
