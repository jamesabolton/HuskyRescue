using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace HuskyRescue.ViewModel.EventsSponsorship
{
	public class Detail
	{
		public Detail()
		{
			Items = new List<SponsorshipItem>();
		}

		public List<SponsorshipItem> Items { get; set; }

		[DisplayName("Event")]
		public Guid EventId { get; set; }

		[DisplayName("Event Name")]
		public string EventName { get; set; }

		public Guid Id { get; set; }

		[DisplayName("Is Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Is Deleted?")]
		public bool IsDeleted { get; set; }

		[DisplayName("Name")]
		public string Name { get; set; }

		[DisplayName("Amount")]
		public decimal Amount { get; set; }

		[DisplayName("Number of Allowed Sponsors")]
		public int NumberOfAllowedSponsors { get; set; }

		[DisplayName("Number of Sponsors")]
		public int NumberOfSponsors { get; set; }

		public string AddedByUserId { get; set; }

		public string UpdatedByUserId { get; set; }

		[DisplayName("Public Description (if any)")]
		public string PublicDescription { get; set; }

		[DisplayName("Internal Notes")]
		public string InternalNotes { get; set; }
	}
}
