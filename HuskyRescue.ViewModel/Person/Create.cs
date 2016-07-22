using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Person
{
	public class Create
	{
		public Create()
		{
			AddressStates = new List<SelectListItem>();
			PhoneNumberTypes = new List<SelectListItem>();
			EmailTypes = new List<SelectListItem>();
		}

		public Guid Id { get; set; }

		public Nullable<Guid> UserId { get; set; }

		[DisplayName("Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Volunteer?")]
		public bool IsVolunteer { get; set; }

		[DisplayName("Foster?")]
		public bool IsFoster { get; set; }

		[DisplayName("Available to foster?")]
		public bool IsAvailableFoster { get; set; }

		[DisplayName("Adopter?")]
		public bool IsAdopter { get; set; }

		[DisplayName("Donor?")]
		public bool IsDonor { get; set; }

		[DisplayName("Sponsor?")]
		public bool IsSponsor { get; set; }

		[DisplayName("On Board of Directors?")]
		public bool IsBoardMember { get; set; }

		[DisplayName("System User?")]
		public bool IsSystemUser { get; set; }

		[DisplayName("DO NOT ADOPT?")]
		public bool IsDoNotAdopt { get; set; }

		[DisplayName("First Name")]
		[StringLength(200)]
		[Required]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		[StringLength(200)]
		public string LastName { get; set; }

		[DisplayName("Sex (M or F)")]
		[StringLength(1)]
		public string Sex { get; set; }

		[DisplayName("Drivers License Number")]
		[StringLength(50)]
		public string DriverLicenseNumber { get; set; }

		[DisplayName("Notes")]
		[DataType(DataType.MultilineText)]
		[StringLength(4000)]
		public string Notes { get; set; }

		#region Phone Numbers
		[DisplayName("Phone Type")]
		public List<SelectListItem> PhoneNumberTypes { get; set; }

		public int PhoneNumber1TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber1 { get; set; }

		public int PhoneNumber2TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber2 { get; set; }

		public int PhoneNumber3TypeId { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		public string PhoneNumber3 { get; set; }
		#endregion

		#region Email Addresses
		[DisplayName("Email Type")]
		public List<SelectListItem> EmailTypes { get; set; }

		public int Email1TypeId { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email1 { get; set; }

		public int Email2TypeId { get; set; }

		[DisplayName("Email")]
		[DataType(DataType.EmailAddress)]
		public string Email2 { get; set; }
		#endregion

		public List<SelectListItem> AddressStates { get; set; }

		#region Home Address
		[DisplayName("Home Address Street")]
		[StringLength(200)]
		public string HomeAddress1 { get; set; }

		[DisplayName("Home Address Street Cont.")]
		[StringLength(200)]
		public string HomeAddress2 { get; set; }

		[DisplayName("Home Address Street Cont.")]
		[StringLength(200)]
		public string HomeAddress3 { get; set; }

		[DisplayName("Home Address City")]
		[StringLength(200)]
		public string HomeCity { get; set; }

		[DisplayName("Home Address State")]
		public Nullable<int> HomeAddressStateId { get; set; }

		[DisplayName("Home Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10)]
		public string HomeZipCode { get; set; }

		[DisplayName("Home Address Country Code")]
		[StringLength(3)]
		public string HomeCountryId { get; set; }
		#endregion

		#region Mailing Address
		[DisplayName("Mailing Address Street")]
		[StringLength(200)]
		public string MailingAddress1 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		[StringLength(200)]
		public string MailingAddress2 { get; set; }

		[DisplayName("Mailing Address Street Cont.")]
		[StringLength(200)]
		public string MailingAddress3 { get; set; }

		[DisplayName("Mailing Address City")]
		[StringLength(200)]
		public string MailingCity { get; set; }

		[DisplayName("Mailing Address State")]
		public Nullable<int> MailingAddressStateId { get; set; }

		[DisplayName("Mailing Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10)]
		public string MailingZipCode { get; set; }

		[DisplayName("Mailing Address Country Code")]
		[StringLength(3)]
		public string MailingCountryId { get; set; }
		#endregion

		#region Billing Address
		[DisplayName("Billing Address Street")]
		[StringLength(200)]
		public string BillingAddress1 { get; set; }

		[DisplayName("Billing Address Street Cont.")]
		[StringLength(200)]
		public string BillingAddress2 { get; set; }

		[DisplayName("Billing Address Street Cont.")]
		[StringLength(200)]
		public string BillingAddress3 { get; set; }

		[DisplayName("Billing Address City")]
		[StringLength(200)]
		public string BillingCity { get; set; }

		[DisplayName("Billing Address State")]
		public Nullable<int> BillingAddressStateId { get; set; }

		[DisplayName("Billing Address ZIP")]
		[DataType(DataType.PostalCode)]
		[StringLength(10)]
		public string BillingZipCode { get; set; }

		[DisplayName("Billing Address Country Code")]
		[StringLength(3)]
		public string BillingCountryId { get; set; }
		#endregion
	}
}
