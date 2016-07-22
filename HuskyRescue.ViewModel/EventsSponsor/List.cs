using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.EventsSponsor
{
	public class List
	{
		public List()
		{
		}

		public Guid Id { get; set; }

		[DisplayName("Sponsor Name")]
		public string SponsorName { get; set; }

		public bool IsPerson { get; set; }

		public Guid OrgPersonId { get; set; }

		[DisplayName("Event")]
		public Guid EventId { get; set; }

		[DisplayName("Event")]
		public string EventName { get; set; }

		[DisplayName("Sponsorship")]
		public Guid EventSponsoshipId { get; set; }

		[DisplayName("Sponsorship")]
		public string EventSponsoshipName { get; set; }

		[DisplayName("Logo received?")]
		public bool HaveReceivedLogo { get; set; }

		[DisplayName("Signage received?")]
		public bool HaveReceivedSingage { get; set; }

		[DisplayName("Paid?")]
		public bool HasPaid { get; set; }

		[DisplayName("Amount paid")]
		public decimal AmountPaid { get; set; }

		public string AddedByUserId { get; set; }

		public string UpdatedByUserId { get; set; }
	}
}
