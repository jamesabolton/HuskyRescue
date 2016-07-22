using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.EventsSponsor
{
	public class Edit
	{
		public Edit()
		{
			Organizations = new List<SelectListItem>();
			Events = new List<SelectListItem>();
			People = new List<SelectListItem>();
			EventSponsorships = new List<SelectListItem>();
		}

		public Guid Id { get; set; }

		public List<SelectListItem> Organizations { get; set; }

		public List<SelectListItem> People { get; set; }

		public List<SelectListItem> Events { get; set; }

		public List<SelectListItem> EventSponsorships { get; set; }

		[DisplayName("Organization")]
		[RequiredIf("PeopleId == null", ErrorMessage = "organization or person required")]
		public Guid OrganizationId { get; set; }

		[DisplayName("Person")]
		[RequiredIf("OrganizationId == null", ErrorMessage = "person or organization required")]
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
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
		public decimal AmountPaid { get; set; }

		public string AddedByUserId { get; set; }

		public string UpdatedByUserId { get; set; }

		[DisplayName("Internal Notes About The Event")]
		[StringLength(4000, ErrorMessage = "must be less than 4000 characters")]
		public string InternalNotes { get; set; }

	}
}
