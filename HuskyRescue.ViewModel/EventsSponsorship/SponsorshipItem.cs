using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.EventsSponsorship
{
	public class SponsorshipItem
	{
		public int Id { get; set; }

		public Guid EventSponsorshipId { get; set; }

		[DisplayName("Item")]
		[StringLength(4000, ErrorMessage = "max length is 4000 characters")]
		public string Value { get; set; }
	}
}
