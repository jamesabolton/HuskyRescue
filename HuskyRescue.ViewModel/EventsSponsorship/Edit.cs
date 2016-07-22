using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.EventsSponsorship
{
	public class Edit
	{
		public Edit()
		{
			Events = new List<SelectListItem>();
			Items = new List<SponsorshipItem>();
		}

		public List<SponsorshipItem> Items { get; set; }

		public List<SelectListItem> Events { get; set; }

		[DisplayName("Event")]
		[Required(ErrorMessage = "event required")]
		public Guid EventId { get; set; }

		public Guid Id { get; set; }

		[DisplayName("Is Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Is Deleted?")]
		public bool IsDeleted { get; set; }

		[DisplayName("Name")]
		[Required(ErrorMessage = "event name required")]
		[StringLength(100, ErrorMessage = "name cannot be more than X characters")]
		public string Name { get; set; }

		[DisplayName("Amount")]
		[Required(ErrorMessage = "cost of sponsorship required")]
		[AssertThat("Amount > 0", ErrorMessage = "cost must be greater than zero")]
		[DisplayFormat(DataFormatString = "{0:F2}", ApplyFormatInEditMode = true)]
		public decimal Amount { get; set; }

		[DisplayName("Number of Allowed Sponsors")]
		[AssertThat("Amount >= 0", ErrorMessage = "number of sponsors must be greater than or equal zero")]
		public int NumberOfAllowedSponsors { get; set; }

		[DisplayName("Number of Sponsors")]
		[Required(ErrorMessage = "number of sponsorships required")]
		[AssertThat("Amount > 0", ErrorMessage = "number of sponsors allowed must be greater than zero")]
		public int NumberOfSponsors { get; set; }

		public string AddedByUserId { get; set; }

		public string UpdatedByUserId { get; set; }

		[DisplayName("Public Description (if any)")]
		[StringLength(4000, ErrorMessage = "must be 4000 or fewer characters")]
		public string PublicDescription { get; set; }

		[DisplayName("Internal Notes")]
		[StringLength(4000, ErrorMessage = "must be 4000 or fewer characters")]
		public string InternalNotes { get; set; }
	}
}
