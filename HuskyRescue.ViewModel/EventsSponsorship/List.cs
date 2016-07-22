using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.EventsSponsorship
{
	public class List
	{
		public List()
		{
		}

		[DisplayName("Event")]
		public string EventName { get; set; }

		public Guid EventId { get; set; }

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
		public int NUmberOfSponsors { get; set; }

		public string AddedByUserId { get; set; }

		public string UpdatedByUserId { get; set; }
	}
}
