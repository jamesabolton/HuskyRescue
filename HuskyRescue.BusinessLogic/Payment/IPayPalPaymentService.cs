using System.Threading.Tasks;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Payment;
using PayPal.Api;

namespace HuskyRescue.BusinessLogic
{
	public interface IPayPalPaymentService
	{
		APIContext ApiContext { get; }

		RequestResult SendPayment(decimal amount, bool isTaxExempt, PaymentType paymentType,
			string transactionDescription = "", string customerNotes = "",
			string firstName = "", string lastName = "",
			string addressStreet1 = "", string addressStreet2 = "", string addressCity = "", string addressStateId = "", string addressPostalCode = "", string countryCode = "US",
			string phoneNumber = "", string email = "", string company = "", string website = "", bool isShipping = false);
		Task<RequestResult> SendDonation(Donation donation);
	}
}