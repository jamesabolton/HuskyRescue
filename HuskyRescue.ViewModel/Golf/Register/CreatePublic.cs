using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;
using HuskyRescue.ViewModel.Common;

namespace HuskyRescue.ViewModel.Golf.Register
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

			AttendeeTypes = new List<KeyValuePair<string, object>>
			                {
				                new KeyValuePair<string, object>("Golfing", "golfing"),
								new KeyValuePair<string, object>("Banquet Only", "banquet")
			                };
		}



		public List<SelectListItem> States { get; set; }

		public List<KeyValuePair<string, object>> AttendeeTypes { get; set; }

		#region Attendee1

		public bool Attendee1IsAttending
		{
			get { return !string.IsNullOrEmpty(Attendee1FirstName); }
		}

		[DisplayName("Will you be golfing or attending the banquet only?")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "type of attendee required")]
		public string Attendee1Type { get; set; }

		[DisplayName("First Name")]
		[Required(ErrorMessage = "please provide your first name")]
		[AssertThat("Length(Attendee1FirstName) <= 50 && Attendee1FirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string Attendee1FirstName { get; set; }

		[DisplayName("Last Name")]
		[Required(ErrorMessage = "please provide your full last name")]
		[AssertThat("Length(Attendee1LastName) <= 50 && Attendee1LastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "last name required")]
		public string Attendee1LastName { get; set; }

		public string Attendee1FullName
		{
			get { return Attendee1FirstName + " " + Attendee1LastName; }
		}

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "street required")]
		public string Attendee1AddressStreet1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string Attendee1AddressStreet2 { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "city required")]
		public string Attendee1AddressCity { get; set; }

		[DisplayName("State")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "state required")]
		public int? Attendee1AddressStateId { get; set; }

		[DisplayName("Postal Code")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "ZIP required")]
		public string Attendee1AddressPostalCode { get; set; }

		public string Attendee1FullAddress
		{
			get
			{
				var address = Attendee1AddressStreet1;
				if (!string.IsNullOrEmpty(Attendee1AddressStreet2)) { address = " " + Attendee1AddressStreet2; }
				address += ", " + Attendee1AddressCity;
				if (Attendee1AddressStateId.HasValue) { address += ", " + Lists.GetStateCode(Attendee1AddressStateId.Value); }
				address += " " + Attendee1AddressPostalCode;
				return address;
			}
		}

		[DisplayName("Email")]
		[RequiredIf("Length(Attendee1FirstName) > 0", ErrorMessage = "email address is required")]
		[AssertThat("IsEmail(Attendee1EmailAddress) && Length(Attendee1EmailAddress) <= 200", ErrorMessage = "valid email address required")]
		public string Attendee1EmailAddress { get; set; }

		[DisplayName("Mobile Number")]
		//[AssertThat(@"IsRegexMatch(Attendee1MobilePhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(Attendee1MobilePhoneNumber) > 8 && Length(Attendee1MobilePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee1MobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		//[AssertThat(@"IsRegexMatch(Attendee1HomePhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(Attendee1HomePhoneNumber) > 8 && Length(Attendee1HomePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee1HomePhoneNumber { get; set; }

		[DisplayName("May TXHR contact you in the future via email regarding events, donation drives, promotions, or other correspondence?")]
		public bool Attendee1FutureContact { get; set; }

		public decimal Attendee1TicketPrice { get; set; }
		#endregion

		#region Attendee2

		public bool Attendee2IsAttending
		{
			get { return !string.IsNullOrEmpty(Attendee2FirstName); }
		}

		[DisplayName("Will you be golfing or attending the banquet only?")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "type of attendee required")]
		public string Attendee2Type { get; set; }

		[DisplayName("First Name")]
		[RequiredIf("Length(Attendee2LastName) > 0", ErrorMessage = "please provide your first name")]
		[AssertThat("Length(Attendee2FirstName) <= 50 && Attendee2FirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string Attendee2FirstName { get; set; }

		[DisplayName("Last Name")]
		[AssertThat("Length(Attendee2LastName) <= 50 && Attendee2LastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "last name required")]
		public string Attendee2LastName { get; set; }

		public string Attendee2FullName
		{
			get { return Attendee2FirstName + " " + Attendee2LastName; }
		}

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "street required")]
		public string Attendee2AddressStreet1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string Attendee2AddressStreet2 { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "city required")]
		public string Attendee2AddressCity { get; set; }

		[DisplayName("State")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "state required")]
		public int? Attendee2AddressStateId { get; set; }

		[DisplayName("Postal Code")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "ZIP required")]
		public string Attendee2AddressPostalCode { get; set; }

		public string Attendee2FullAddress
		{
			get
			{
				var address = Attendee2AddressStreet1;
				if (!string.IsNullOrEmpty(Attendee2AddressStreet2)) { address = " " + Attendee2AddressStreet2; }
				address += ", " + Attendee2AddressCity;
				if (Attendee2AddressStateId.HasValue) { address += ", " + Lists.GetStateCode(Attendee2AddressStateId.Value); }
				address += " " + Attendee2AddressPostalCode;
				return address;
			}
		}

		[DisplayName("Email")]
		[RequiredIf("Length(Attendee2FirstName) > 0", ErrorMessage = "email address is required")]
		[AssertThat("IsEmail(Attendee2EmailAddress) && Length(Attendee2EmailAddress) <= 200", ErrorMessage = "valid email address required")]
		public string Attendee2EmailAddress { get; set; }

		[DisplayName("Mobile Number")]
		//[AssertThat(@"IsRegexMatch(Attendee2MobilePhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(Attendee2MobilePhoneNumber) > 8 && Length(Attendee2MobilePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee2MobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		//[AssertThat(@"IsRegexMatch(Attendee2HomePhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(Attendee2HomePhoneNumber) > 8 && Length(Attendee2HomePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee2HomePhoneNumber { get; set; }

		[DisplayName("May TXHR contact you in the future via email regarding events, donation drives, promotions, or other correspondence?")]
		public bool Attendee2FutureContact { get; set; }

		public decimal Attendee2TicketPrice { get; set; }
		#endregion

		#region Attendee3

		public bool Attendee3IsAttending
		{
			get { return !string.IsNullOrEmpty(Attendee3FirstName); }
		}

		[DisplayName("Will you be golfing or attending the banquet only?")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "type of attendee required")]
		public string Attendee3Type { get; set; }
		
		[DisplayName("First Name")]
		[RequiredIf("Length(Attendee3LastName) > 0", ErrorMessage = "please provide your first name")]
		[AssertThat("Length(Attendee3FirstName) <= 50 && Attendee3FirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string Attendee3FirstName { get; set; }

		[DisplayName("Last Name")]
		[AssertThat("Length(Attendee3LastName) <= 50 && Attendee3LastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "last name required")]
		public string Attendee3LastName { get; set; }

		public string Attendee3FullName
		{
			get { return Attendee3FirstName + " " + Attendee3LastName; }
		}

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "street required")]
		public string Attendee3AddressStreet1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string Attendee3AddressStreet2 { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "city required")]
		public string Attendee3AddressCity { get; set; }

		[DisplayName("State")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "state required")]
		public int? Attendee3AddressStateId { get; set; }

		[DisplayName("Postal Code")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "ZIP required")]
		public string Attendee3AddressPostalCode { get; set; }

		public string Attendee3FullAddress
		{
			get
			{
				var address = Attendee3AddressStreet1;
				if (!string.IsNullOrEmpty(Attendee3AddressStreet2)) { address = " " + Attendee3AddressStreet2; }
				address += ", " + Attendee3AddressCity;
				if (Attendee3AddressStateId.HasValue) { address += ", " + Lists.GetStateCode(Attendee3AddressStateId.Value); }
				address += " " + Attendee3AddressPostalCode;
				return address;
			}
		}

		[DisplayName("Email")]
		[RequiredIf("Length(Attendee3FirstName) > 0", ErrorMessage = "email address is required")]
		[AssertThat("IsEmail(Attendee3EmailAddress) && Length(Attendee3EmailAddress) <= 200", ErrorMessage = "valid email address required")]
		public string Attendee3EmailAddress { get; set; }

		[DisplayName("Mobile Number")]
		[AssertThat("Length(Attendee3MobilePhoneNumber) > 8 && Length(Attendee3MobilePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee3MobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		[AssertThat("Length(Attendee3HomePhoneNumber) > 8 && Length(Attendee3HomePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee3HomePhoneNumber { get; set; }

		[DisplayName("May TXHR contact you in the future via email regarding events, donation drives, promotions, or other correspondence?")]
		public bool Attendee3FutureContact { get; set; }

		public decimal Attendee3TicketPrice { get; set; }
		#endregion

		#region Attendee4

		public bool Attendee4IsAttending
		{
			get { return !string.IsNullOrEmpty(Attendee4FirstName); }
		}

		[DisplayName("Will you be golfing or attending the banquet only?")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "type of attendee required")]
		public string Attendee4Type { get; set; }
		
		[DisplayName("First Name")]
		[RequiredIf("Length(Attendee4LastName) > 0", ErrorMessage = "please provide your first name")]
		[AssertThat("Length(Attendee4FirstName) <= 50 && Attendee4FirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string Attendee4FirstName { get; set; }

		[DisplayName("Last Name")]
		[AssertThat("Length(Attendee4LastName) <= 50 && Attendee4LastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "last name required")]
		public string Attendee4LastName { get; set; }

		public string Attendee4FullName
		{
			get { return Attendee4FirstName + " " + Attendee4LastName; }
		}

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "street required")]
		public string Attendee4AddressStreet1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string Attendee4AddressStreet2 { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "city required")]
		public string Attendee4AddressCity { get; set; }

		[DisplayName("State")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "state required")]
		public int? Attendee4AddressStateId { get; set; }

		[DisplayName("Postal Code")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "ZIP required")]
		public string Attendee4AddressPostalCode { get; set; }

		public string Attendee4FullAddress
		{
			get
			{
				var address = Attendee4AddressStreet1;
				if (!string.IsNullOrEmpty(Attendee4AddressStreet2)) { address = " " + Attendee4AddressStreet2; }
				address += ", " + Attendee4AddressCity;
				if (Attendee4AddressStateId.HasValue) { address += ", " + Lists.GetStateCode(Attendee4AddressStateId.Value); }
				address += " " + Attendee4AddressPostalCode;
				return address;
			}
		}

		[DisplayName("Email")]
		[RequiredIf("Length(Attendee4FirstName) > 0", ErrorMessage = "email address is required")]
		[AssertThat("IsEmail(Attendee4EmailAddress) && Length(Attendee4EmailAddress) <= 200", ErrorMessage = "valid email address required")]
		public string Attendee4EmailAddress { get; set; }

		[DisplayName("Mobile Number")]
		//[AssertThat(@"IsRegexMatch(Attendee4MobilePhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(Attendee4MobilePhoneNumber) > 8 && Length(Attendee4MobilePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee4MobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		//[AssertThat(@"IsRegexMatch(Attendee4HomePhoneNumber, '^\\d+$')", ErrorMessage = "cell must be a valid phone number")]
		[AssertThat("Length(Attendee4HomePhoneNumber) > 8 && Length(Attendee4HomePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string Attendee4HomePhoneNumber { get; set; }

		[DisplayName("May TXHR contact you in the future via email regarding events, donation drives, promotions, or other correspondence?")]
		public bool Attendee4FutureContact { get; set; }

		public decimal Attendee4TicketPrice { get; set; }
		#endregion

		[DisplayName("Notes / Comments")]
		[AssertThat("Length(CustomerNotes) <= 4000", ErrorMessage = "notes must be less than 4000 characters")]
		public string CustomerNotes { get; set; }

		#region Payment
		/// <summary>
		/// 1-4 to go with Attendee 1 to 4
		/// </summary>
		public int AttendeePayerId { get; set; }

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

		public string CountryCodeId { get; set; }

		public string BraintreeTransactionId { get; set; }

		public string Nonce { get; set; }

		public string DeviceData { get; set; }

		public decimal GolfTicketCost { get; set; }

		public bool IsDiscounted { get; set; }

		public decimal BanquetTicketCost { get; set; }

		[DisplayName("Amount Due Today")]
		[DataType(DataType.Currency)]
		public decimal AmountDue { get; set; }

		public string OrderId { get; set; }

		[DisplayName("Payment Method")]
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
