using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Payment
{
	public class Transaction
	{
		[DisplayName("Braintree Transaction Id")]
		public string Id { get; set; }

		public string Nonce { get; set; }

		[DisplayName("Gift Amount")]
		[Required(ErrorMessage = "donation amount required")]
		[AssertThat("IsNumber(Amount)", ErrorMessage = "donation amount must be a number")]
		public decimal Amount { get; set; }

		[DisplayName("Braintree Order Id")]
		public string OrderId { get; set; }

		[DisplayName("Notes / Comments")]
		[AssertThat("Length(CustomerNotes) <= 4000", ErrorMessage = "notes must be less than 4000 characters")]
		public string CustomerNotes { get; set; }

		[DisplayName("Card Number")]
		[Required(ErrorMessage = "card number is required")]
		[AssertThat("IsNumber(CardNumber) && Length(CardNumber) >= 16 && Length(CardNumber) <= 19", ErrorMessage = "valid card number required")]
		public string CardNumber { get; set; }

		[DisplayName("Month")]
		[Required(ErrorMessage = "card expire month required")]
		[AssertThat("IsNumber(CardExpireMonth) && Length(CardExpireMonth) >= 1 && Length(CardExpireMonth) <= 2 && CardExpireMonth >= 1 && CardExpireMonth <= 12", ErrorMessage = "valid card expire month required")]
		public string CardExpireMonth { get; set; }

		[DisplayName("Year")]
		[Required(ErrorMessage = "card expire year required")]
		[AssertThat("IsNumber(CardExpireYear) && (Length(CardExpireYear) == 2 || Length(CardExpireYear) <= 4)", ErrorMessage = "valid card expire year required")]
		public string CardExpireYear { get; set; }

		[DisplayName("CVV")]
		[Required(ErrorMessage = "card security code required")]
		[AssertThat("IsNumber(CardCvv)", ErrorMessage = "valid card security card required")]
		public string CardCvv { get; set; }
	}
}
