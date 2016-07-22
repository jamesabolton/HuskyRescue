using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Payment
{
	public class Donation
	{
		public string DonationId { get; set; }

		[DisplayName("May TXHR contact you in the future via email regarding events, donation drives, promotions, or other correspondence?")]
		public bool FutureContact { get; set; }

		[DisplayName("Braintree Customer Id")]
		public string CustomerId { get; set; }

		[DisplayName("First Name")]
		[Required(ErrorMessage = "please provide your first name")]
		[AssertThat("Length(FirstName) <= 50 && FirstName != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		[Required(ErrorMessage = "please provide your full last name")]
		[AssertThat("Length(LastName) <= 50 && LastName != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		public string LastName { get; set; }

		[DisplayName("Business Name")]
		public string CompanyName { get; set; }

		[DisplayName("Street Address")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing street required")]
		public string StreetAddress1 { get; set; }

		[DisplayName("Street Address Cont.")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string StreetAddress2 { get; set; }

		[DisplayName("City")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing city required")]
		public string City { get; set; }

		public List<SelectListItem> States { get; set; }

		[DisplayName("State")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing state required")]
		public int? StateId { get; set; }

		[DisplayName("Postal Code")]
		[RequiredIf("IsNotPaypalPayment(PaymentMethod)", ErrorMessage = "billing postal code required")]
		[StringLength(5, ErrorMessage = "postal code must be 5 or fewer digits")]
		public string PostalCode { get; set; }

		[DisplayName("Country Code")]
		public string CountryCodeId { get; set; }

		[DisplayName("Mobile Number")]
		[AssertThat("Length(MobilePhoneNumber) > 8 && Length(MobilePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string MobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		[AssertThat("Length(HomePhoneNumber) > 8 && Length(HomePhoneNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string HomePhoneNumber { get; set; }

		[DisplayName("Website Link")]
		public string Website { get; set; }

		[DisplayName("Email")]
		[RequiredIf("FutureContact == true")]
		[AssertThat("IsEmail(Email) && Length(Email) <= 200", ErrorMessage = "valid email address required")]
		public string Email { get; set; }

		[DisplayName("Braintree Transaction Id")]
		public string BraintreeTransactionId { get; set; }

		public string Nonce { get; set; }

		public string DeviceData { get; set; }

		[DisplayName("Gift Amount")]
		[Required(ErrorMessage = "donation amount required")]
		[DataType(DataType.Currency)]
		public decimal Amount { get; set; }

		[DisplayName("Braintree Order Id")]
		public string OrderId { get; set; }

		[DisplayName("Notes / Comments")]
		[AssertThat("Length(CustomerNotes) <= 4000", ErrorMessage = "notes must be less than 4000 characters")]
		public string CustomerNotes { get; set; }

		[DisplayName("Payment Method")]
		public string PaymentMethod { get; set; }

		[DisplayName("Card Number")]
		public string CardNumber { get; set; }

		[DisplayName("Month")]
		public string CardExpireMonth { get; set; }

		[DisplayName("Year")]
		public string CardExpireYear { get; set; }

		[DisplayName("CVV")]
		public string CardCvv { get; set; }

		public string AddedByUserId { get; set; }

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

		[DisplayName("Payment Method")]
		[Required(ErrorMessage = "payment method is required")]
		public List<KeyValuePair<string, object>> PaymentMethods { get; set; }

		public Donation()
		{
			States = new List<SelectListItem>();
			CountryCodeId = "US";
			Amount = 25;

			PaymentMethods = new List<KeyValuePair<string, object>>
			                 {
				                 new KeyValuePair<string, object>("PayPal", "paypal"),
				                 new KeyValuePair<string, object>("Credit Card", "creditcard")
			                 };
		}
	}
}
