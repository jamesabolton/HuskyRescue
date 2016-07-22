using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Events
{
	public class List
	{
		public Guid Id { get; set; }

		public Guid OrganizationId { get; set; }

		[DisplayName("Hosting location")]
		public string OrganziationName { get; set; }

		[DisplayName("Event Date")]
		public DateTime EventDate { get; set; }

		[DisplayName("Active?")]
		public bool IsActive { get; set; }

		[DisplayName("Deleted?")]
		public bool IsDeleted { get; set; }

		public string AddedByUserId { get; set; }

		[DisplayName("Event Name")]
		public string EventName { get; set; }

		[DisplayName("Start Time")]
		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
		public TimeSpan? StartTime { get; set; }

		[DisplayName("End Time")]
		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:hh:mm tt}")]
		public TimeSpan? EndTime { get; set; }

		[DisplayName("All Day Event")]
		public bool? IsAllDayEvent { get; set; }

		[DisplayName("RoughRiders Event")]
		public bool? IsRoughRidersEvent { get; set; }

		[DisplayName("Golf Tournament Event")]
		public bool? IsGolfTournamentEvent { get; set; }

		[DisplayName("Raffle Event")]
		public bool? IsRaffleEvent { get; set; }

		[DisplayName("Meet & Greet Event")]
		public bool? IsMeetAndGreetEvent { get; set; }

		[DisplayName("Dog Wash Event")]
		public bool? IsDogWashEvent { get; set; }

		[DisplayName("5K/Running Event")]
		public bool? Is5KEvent { get; set; }

		[DisplayName("Other Event")]
		public bool? IsOtherEvent { get; set; }

		[DisplayName("Are tickets sold?")]
		public bool? AreTicketsSold { get; set; }

		[DisplayName("Current Ticket Price")]
		[RequiredIf("AreTicketsSold == true", ErrorMessage = "ticket price required if tickets are sold")]
		public decimal TicketPrice { get; set; }

		[DisplayName("Current Other Ticket Price")]
		public decimal? TickPriceOther { get; set; }
	}
}
