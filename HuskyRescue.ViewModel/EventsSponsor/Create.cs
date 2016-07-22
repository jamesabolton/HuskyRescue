using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;
using HuskyRescue.ViewModel.Common;

namespace HuskyRescue.ViewModel.EventsSponsor
{
	public class Create
	{
		public Create()
		{
			Organizations = new List<SelectListItem>();
			Events = new List<SelectListItem>();
			People = new List<SelectListItem>();
			EventSponsorships = new List<SelectListItem>();

			AddressStates = new List<SelectListItem>();
			AddressStates = Lists.GetStateList();
			PhoneNumberTypes = new List<SelectListItem>();
			PhoneNumberTypes = Lists.GetPhoneTypeList();
			EmailTypes = new List<SelectListItem>();
			EmailTypes = Lists.GetEmailTypeList();
		}

		public string LogoFilePhysicalPath { get; set; }
		public string LogoFileName { get; set; }

		public byte[] FileLogoContent { get; set; }
		public string FileLogoName { get; set; }
		public int FileTypeId = 4;
		public string FileLogoContentType { get; set; }

		public List<SelectListItem> Organizations { get; set; }

		public List<SelectListItem> People { get; set; }

		public List<SelectListItem> Events { get; set; }

		public List<SelectListItem> EventSponsorships { get; set; }

		[DisplayName("New or existing organization?")]
		public bool? IsNewOrg { get; set; }

		[DisplayName("New or existing person?")]
		public bool? IsNewPerson { get; set; }

		[DisplayName("Organization")]
		[RequiredIf("SponsorType == 'B'", ErrorMessage = "organization required")]
		public Guid OrganizationId { get; set; }

		[DisplayName("Person")]
		[RequiredIf("SponsorType == 'P'", ErrorMessage = "person required")]
		public Guid PeopleId { get; set; }

		[DisplayName("Event")]
		[Required(ErrorMessage = "event required")]
		public Guid EventId { get; set; }

		[DisplayName("Sponsorship")]
		[Required(ErrorMessage = "sponsorship required")]
		public Guid EventSponsorshipId { get; set; }

		[DisplayName("Logo received?")]
		public bool HaveReceivedLogo { get; set; }

		[DisplayName("Signage received?")]
		public bool HaveReceivedSingage { get; set; }

		[DisplayName("Paid?")]
		public bool HasPaid { get; set; }

		[DisplayName("Amount paid")]
		public decimal AmountPaid { get; set; }

		public string AddedByUserId { get; set; }

		[DisplayName("Internal Notes About The Event")]
		[StringLength(4000, ErrorMessage = "must be less than 4000 characters")]
		public string InternalNotes { get; set; }

		[DisplayName("Is this a new sponsor?")]
		public bool IsNewSponsor { get; set; }

		[DisplayName("Is the sponsor a business or a person?")]
		public bool IsBusinessSponsor { get; set; }

		[DisplayName("Is the sponsor a business or a person?")]
		public bool IsPersonSponsor { get; set; }

		[DisplayName("Person or Business sponsor?")]
		[Required(ErrorMessage = "sponsor type required")]
		public string SponsorType { get; set; }

		#region new sponsor fields
		#region new organization
		[DisplayName("Business Name")]
		[StringLength(200, ErrorMessage = "no more than 200 characters")]
		[RequiredIf("IsNewOrg == true", ErrorMessage = "org name required")]
		public string OrgName { get; set; }
		#endregion

		#region new person
		[DisplayName("First Name")]
		[StringLength(200)]
		[RequiredIf("IsNewPerson == true")]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		[StringLength(200)]
		[RequiredIf("IsNewPerson == true")]
		public string LastName { get; set; }
		#endregion

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
		#endregion
	}
}
