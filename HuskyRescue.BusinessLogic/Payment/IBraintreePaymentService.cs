using System.Threading.Tasks;
using Braintree;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Payment;

namespace HuskyRescue.BusinessLogic
{
	public interface IBraintreePaymentService
	{
		string ClientKey { get; }
		BraintreeGateway Gateway { get; }
		string GetClientToken(string customerId);

		RequestResult SendPayment(decimal amount, string nonce, bool isTaxExempt, PaymentType paymentType, string deviceData,
			string transactionDescription = "", string customerNotes = "",
			string firstName = "", string lastName = "",
			string addressStreet1 = "", string addressStreet2 = "", string addressCity = "", string addressStateId = "", string addressPostalCode = "", string countryCode = "US",
			string phoneNumber = "", string email = "", string company = "", string website = "", bool isShipping = false);
		Task<RequestResult> SendDonation(Donation donation);
	}
}