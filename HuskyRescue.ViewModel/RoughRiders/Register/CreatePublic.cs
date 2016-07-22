using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;
using HuskyRescue.ViewModel.Common;

namespace HuskyRescue.ViewModel.RoughRiders.Register
{
	public class CreatePublic
	{
		public CreatePublic()
		{
			States = new List<SelectListItem>();
			CountryCodeId = "US";

			PaymentMethods = new List<KeyValuePair<string, object>>
			                 {
				                 new KeyValuePair<string, object>("PayPal", "paypal"),
				                 new KeyValuePair<string, object>("Credit Card", "creditcard")
			                 };
		}

		public List<SelectListItem> States { get; set; }

		#region Attendee

		public bool AttendeeIsAttending
		{
			get { return !string.IsNullOrEmpty(AttendeeFirstName); }
		}

		public string AttendeeType { get; set; }

		[DisplayName("First Name")]
		[Required(ErrorMessage = "please provide your first name")]
		[AssertThat("Length(AttendeeFirstName) <= 50 && AttendeeFirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string AttendeeFirstName { get; set; }

		[DisplayName("Last Name")]
		[Required(ErrorMessage = "please provide your full last name")]
		[AssertThat("Length(AttendeeLastName) <= 50 && AttendeeLastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		[RequiredIf("Length(AttendeeFirstName) > 0", ErrorMessage = "last name required")]
		public string AttendeeLastName { get; set; }

		public string AttendeeFullName
		{
			get { return AttendeeFirstName + " " + AttendeeLastName; }
		}

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("Length(AttendeeFirstName) > 0", ErrorMessage = "street required")]
		public string AttendeeAddressStreet1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string AttendeeAddressStreet2 { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("Length(AttendeeFirstName) > 0", ErrorMessage = "city required")]
		public string AttendeeAddressCity { get; set; }

		[DisplayName("State")]
		[RequiredIf("Length(AttendeeFirstName) > 0", ErrorMessage = "state required")]
		public int? AttendeeAddressStateId { get; set; }

		[DisplayName("Postal Code")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		[RequiredIf("Length(AttendeeFirstName) > 0", ErrorMessage = "ZIP required")]
		public string AttendeeAddressPostalCode { get; set; }

		public string AttendeeFullAddress
		{
			get
			{
				var address = AttendeeAddressStreet1;
				if (!string.IsNullOrEmpty(AttendeeAddressStreet2)) { address = " " + AttendeeAddressStreet2; }
				address += ", " + AttendeeAddressCity;
				if (AttendeeAddressStateId.HasValue) { address += ", " + Lists.GetStateCode(AttendeeAddressStateId.Value); }
				address += " " + AttendeeAddressPostalCode;
				return address;
			}
		}

		[DisplayName("Email")]
		[RequiredIf("Length(AttendeeFirstName) > 0", ErrorMessage = "email address is required")]
		[AssertThat("IsEmail(AttendeeEmailAddress) && Length(AttendeeEmailAddress) <= 200", ErrorMessage = "valid email address required")]
		public string AttendeeEmailAddress { get; set; }

		[DisplayName("Mobile Number")]
		[AssertThat("Length(AttendeeMobilePhoneNumber) > 8 && Length(AttendeeMobilePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string AttendeeMobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		[AssertThat("Length(AttendeeHomePhoneNumber) > 8 && Length(AttendeeHomePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string AttendeeHomePhoneNumber { get; set; }

		[DisplayName("May TXHR contact you in the future via email regarding events, donation drives, promotions, or other correspondence?")]
		public bool AttendeeFutureContact { get; set; }

		public decimal AttendeeTicketPrice { get; set; }
		#endregion

		[DisplayName("Notes / Comments")]
		[AssertThat("Length(CustomerNotes) <= 4000", ErrorMessage = "notes must be less than 4000 characters")]
		public string CustomerNotes { get; set; }

		#region Payment
		[DisplayName("First Name")]
		[Required(ErrorMessage = "please provide your first name")]
		[AssertThat("Length(PayeeFirstName) <= 50 && PayeeFirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string PayeeFirstName { get; set; }

		[DisplayName("Last Name")]
		[Required(ErrorMessage = "please provide your full last name")]
		[AssertThat("Length(PayeeLastName) <= 50 && PayeeLastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		public string PayeeLastName { get; set; }

		public string PayeeFullName { get { return PayeeFirstName + " " + PayeeLastName; } }

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing street required")]
		public string PayeeAddressStreet1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address 2 must be less than 50 characters")]
		public string PayeeAddressStreet2 { get; set; }

		[DisplayName("Email")]
		[Required(ErrorMessage = "email address is required")]
		[AssertThat("IsEmail(PayeeEmailAddress) && Length(PayeeEmailAddress) <= 200", ErrorMessage = "valid email address required")]
		public string PayeeEmailAddress { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing city required")]
		public string PayeeAddressCity { get; set; }

		[DisplayName("State")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing state required")]
		public int? PayeeAddressStateId { get; set; }

		[DisplayName("Postal Code")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing postal code required")]
		public string PayeeAddressPostalCode { get; set; }

		public string PayeeFullAddress
		{
			get
			{
				var address = PayeeAddressStreet1;
				if (!string.IsNullOrEmpty(PayeeAddressStreet2)) { address = " " + PayeeAddressStreet2; }
				address += ", " + PayeeAddressCity;
				if (PayeeAddressStateId.HasValue) { address += ", " + Lists.GetStateCode(PayeeAddressStateId.Value); }
				address += " " + PayeeAddressPostalCode;
				return address;
			}
		}

		[DisplayName("Use attendee information for billing?")]
		public bool UseAttendeeForPayee { get; set; }

		public string CountryCodeId { get; set; }

		public string BraintreeTransactionId { get; set; }

		public string Nonce { get; set; }

		public string DeviceData { get; set; }

		public decimal GameTicketCost { get; set; }

		[DisplayName("# of Tickets")]
		[AssertThat("NumberOfTickets > 0", ErrorMessage = "you must purchase at least one ticket")]
		public int NumberOfTickets { get; set; }

		[DisplayName("Amount Due Today")]
		[DataType(DataType.Currency)]
		public decimal AmountDue { get; set; }

		public string OrderId { get; set; }

		public string PaymentMethod { get; set; }

		[DisplayName("Payment Method")]
		[Required(ErrorMessage = "payment method is required")]
		public List<KeyValuePair<string, object>> PaymentMethods { get; set; }

		[DisplayName("Card Number")]
		public string CardNumber { get; set; }

		[DisplayName("Month")]
		public string CardExpireMonth { get; set; }

		[DisplayName("Year")]
		public string CardExpireYear { get; set; }

		[DisplayName("CVV")]
		public string CardCvv { get; set; }
		#endregion

		public bool IsNotPaypalPayment(string method)
		{
			if (string.IsNullOrEmpty(method)) return true;
			return !PaymentMethod.ToLower().Equals("paypal");
		}

		public bool IsValidMonth(string month)
		{
			if (string.IsNullOrEmpty(month)) return false;
			int monthInt;
			var result = int.TryParse(month, out monthInt);

			if (result)
			{
				if (monthInt >= 1 && monthInt <= 12) result = true;
			}
			return result;
		}

		public bool IsValidCreditCardYear(string year)
		{
			if (string.IsNullOrEmpty(year)) return false;
			int yearInt;
			var result = int.TryParse(year, out yearInt);

			if (result)
			{
				if (yearInt >= DateTime.Now.Year) result = true;
			}
			return result;
		}
	}
}
