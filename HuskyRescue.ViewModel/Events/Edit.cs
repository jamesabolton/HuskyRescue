using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Events
{
	public class Edit
	{
		public Edit()
		{
			IsActive = true;
			EventDate = DateTime.Today;
			Organizations = new List<SelectListItem>();
		}

		public Guid Id { get; set; }

		public List<SelectListItem> Organizations { get; set; }

		[DisplayName("Organization hosting the event")]
		[Required(ErrorMessage = "hosting organization required")]
		public Guid OrganizationId { get; set; }

		public string OrganizationName { get; set; }

		[DisplayName("Event Date")]
		[Required(ErrorMessage = "event date required")]
		//[DataType(DataType.Date)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
		public DateTime EventDate { get; set; }

		[DisplayName("Is Active?")]
		public bool IsActive { get; set; }

		public string AddedByUserId { get; set; }

		public string UpdatedByUserId { get; set; }

		[DisplayName("Event Name")]
		[Required(ErrorMessage = "event name required")]
		[StringLength(200, ErrorMessage = "event name must be less than 200 characters")]
		public string EventName { get; set; }

		[DisplayName("Start Time")]
		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
		public TimeSpan? StartTime { get; set; }

		[DisplayName("End Time")]
		[DataType(DataType.Time)]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:c}")]
		public TimeSpan? EndTime { get; set; }

		[DisplayName("All Day Event")]
		public bool IsAllDayEvent { get; set; }

		[DisplayName("RoughRiders Event")]
		public bool IsRoughRidersEvent { get; set; }

		[DisplayName("Golf Tournament Event")]
		public bool IsGolfTournamentEvent { get; set; }

		[DisplayName("Raffle Event")]
		public bool IsRaffleEvent { get; set; }

		[DisplayName("Meet & Greet Event")]
		public bool IsMeetAndGreetEvent { get; set; }

		[DisplayName("Dog Wash Event")]
		public bool IsDogWashEvent { get; set; }

		[DisplayName("5K/Running Event")]
		public bool Is5KEvent { get; set; }

		[DisplayName("Other Event")]
		public bool IsOtherEvent { get; set; }

		[DisplayName("Other Event Description")]
		[StringLength(200, ErrorMessage = "must be less than 200 characters")]
		[RequiredIf("IsOtherEvent == true")]
		public string EventDescription { get; set; }

		[DisplayName("Are tickets sold?")]
		public bool AreTicketsSold { get; set; }

		[DisplayName("Current Ticket Price")]
		//[RequiredIf("AreTicketsSold == true", ErrorMessage = "ticket price required if tickets are sold")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F}")]
		public decimal TicketPrice { get; set; }

		[DisplayName("Current Other Ticket Price")]
		[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:F}")]
		public decimal TicketPriceOther { get; set; }

		[DisplayName("Public Event Description")]
		[StringLength(4000, ErrorMessage = "must be less than 4000 characters")]
		public string PublicEventDescription { get; set; }

		[DisplayName("Internal Notes About The Event")]
		[StringLength(4000, ErrorMessage = "must be less than 4000 characters")]
		public string InternalNotes { get; set; }

	}
}
