braintree.setup($('#clientToken').val(),
	'custom',
	{
		container: "checkout",
		id: "roughRidersRegisterForm",

		//paypal: {
		//	container: 'paypal-button',
		//	singleUse: true,
		//	onSuccess: function (nonce, email) {
		//		$('#PaymentMethod').val('paypal');
		//		$('#CardNumber').val('').prop('disabled', true);
		//		$('#CardCvv').val('').prop('disabled', true);
		//		$('#CardExpireMonth').val('').prop('disabled', true);
		//		$('#CardExpireYear').val('').prop('disabled', true);
		//		$('#Email').val(email);
		//	},
		//	onCancelled: function () {
		//		$('#PaymentMethod').val('');
		//		$('#CardNumber').prop('disabled', false);
		//		$('#CardCvv').prop('disabled', false);
		//		$('#CardExpireMonth').prop('disabled', false);
		//		$('#CardExpireYear').prop('disabled', false);
		//	}
		//}
	});
var env = $('#environment').val() == true ? BraintreeData.environments.production : BraintreeData.environments.sandbox;

BraintreeData.setup($('#merchantId').valueOf(), 'roughRidersRegisterForm', env);

$('#paymentButton').on('click', function (e) {
	e.preventDefault();
	var isValid = $('#roughRidersRegisterForm').valid();
	if (isValid) {
		if ($('#PaymentMethod').val() != "paypal") {
			var client = new braintree.api.Client({ clientToken: $('#clientToken').val() });
			client.tokenizeCard({
				cardholderName: $('#PayeeFirstName').val() + " " + $('#PayeeLastName').val(),
				number: $('#CardNumber').val(),
				expirationMonth: $('#CardExpireMonth').val(),
				expirationYear: $('#CardExpireYear').val(),
				cvv: $('#CardCvv').val(),
				billingAddress: {
					postalCode: $('#PostalCode').val()
				}
			}, function (err, nonce) {
				if ($.find('#payment_method_nonce').length) {
					$.find('#payment_method_nonce').val(nonce);
				} else {
					$('#roughRidersRegisterForm').append('<input type="hidden" id="payment_method_nonce" name="payment_method_nonce" value="' + nonce + '" />');
				}
				//$('#payment_method_nonce').val(nonce);
				$('#roughRidersRegisterForm').submit();
			});
		} else {
			$('#roughRidersRegisterForm').submit();
		}
	}
});

function PopulatePaymentInformation() {
	if (document.getElementById('UseAttendeeForPayee').checked) {
		var firstName = $('#AttendeeFirstName').val();
		var lastName = $('#AttendeeLastName').val();
		var street1 = $('#AttendeeAddressStreet1').val();
		var street2 = $('#AttendeeAddressStreet2').val();
		var stateId = $('#AttendeeAddressStateId').val();
		var city = $('#AttendeeAddressCity').val();
		var zip = $('#AttendeeAddressPostalCode').val();
		var email = $('#AttendeeEmailAddress').val();

		$('#PayeeFirstName').val(firstName);
		$('#PayeeLastName').val(lastName);
		$('#PayeeAddressStreet1').val(street1);
		$('#PayeeAddressStreet2').val(street2);
		$('#PayeeAddressStateId').val(stateId);
		$('#PayeeAddressCity').val(city);
		$('#PayeeAddressPostalCode').val(zip);
		$('#PayeeEmailAddress').val(email);
	} else {
		$('#PayeeFirstName').val('');
		$('#PayeeLastName').val('');
		$('#PayeeAddressStreet1').val('');
		$('#PayeeAddressStreet2').val('');
		$('#PayeeAddressStateId').val('');
		$('#PayeeAddressCity').val('');
		$('#PayeeAddressPostalCode').val('');
		$('#PayeeEmailAddress').val('');
	}
}

function PaymentType() {
	var paymentType = $('input[name="PaymentMethod"]:checked').val();

	switch (paymentType) {
		case "paypal":
			$('#paypalrow').show();
			$('#creditcardrow').hide();
			break;
		case "creditcard":
			$('#paypalrow').hide();
			$('#creditcardrow').show();
			break;
	}

}

function UpdateAmountDue() {
	var amountdue = 0;
	var gameTicketCost = parseInt($('#GameTicketCost').val());

	var numberOfTickets = parseInt($('#NumberOfTickets').val());

	$('#AmountDue').val(parseInt(gameTicketCost * numberOfTickets));
}

// assign functions to events
$('#NumberOfTickets').on('change', UpdateAmountDue);

$('input[name="PaymentMethod"]:radio').on('change', PaymentType);
$('#UseAttendeeForPayee').on('change', PopulatePaymentInformation);

$('#PaymentMethod').val('creditcard');

PaymentType();
UpdateAmountDue();

//document.getElementById('AttendeeFirstName').focus();
