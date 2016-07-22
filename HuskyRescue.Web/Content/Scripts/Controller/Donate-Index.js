braintree.setup($('#clientToken').val(),
	'custom',
	{
		id: 'donateForm',
		onError: function (response) {
			console.log('braintree onError: [type=' + response.type + '][message=' + response.message + ']');
			$('input[name="Amount"]').prop('readonly', false);
			$('option:not(:selected)').prop('disabled', false);
			$('input[name="payment_method_nonce"]').val('');
		},
		onPaymentMethodReceived: function (response) {
			// performed when submitting the form
			console.log('braintree onPaymentMethodReceived: [nonce=' + response.nonce + '][type=' + response.type + '][details=' + response.details.cardType + ' -- ' + response.details.lastTwo + ']');

			if (response.details.cardType !== 'Unknown' && response.nonce !== '' && $('input[name="payment_method_nonce"]')) {
				$('input[name="payment_method_nonce"]').val(response.nonce);

				var isValid = $('#donateForm').valid();
				if (isValid) {
					$.blockUI({
						css: {
							border: 'none',
							padding: '15px',
							backgroundColor: '#000',
							'-webkit-border-radius': '10px',
							'-moz-border-radius': '10px',
							opacity: .5,
							color: '#fff'
						}
					});
					$('#donateForm').submit();
				}
			}
			else if ($('input[name="payment_method_nonce"]')) {
				$('input[name="payment_method_nonce"]').val('');
			}
		},
		paypal: {
			container: 'paypal-button',
			singleUse: true,
			amount: parseFloat($('#Amount')),
			currency: 'USD',
			onSuccess: function (nonce, email) {
				// This will be called as soon as the user completes the PayPal flow
				console.log('paypal onSuccess');
				$('input[name="Amount"]').prop('readonly', true);
				$('option:not(:selected)').prop('disabled', true);

				if (nonce !== '' && $('input[name="payment_method_nonce"]')) {
					$('input[name="payment_method_nonce"]').val(nonce);
				}
				// set paypal email to email address on form if one has not been provided already
				if (email !== '' && $('input[name="Email"]').val() !== '') {
					$('input[name="Email"]').val(email);
				}
			},
			onCancelled: function () {
				console.log('paypal onCancelled');
				$('input[name="Amount"]').prop('readonly', false);
				$('option:not(:selected)').prop('disabled', false);
				$('input[name="payment_method_nonce"]').val('');
			},
			onUnsupported: function () {
				console.log('paypal onUnsupported');
				$('input[name="Amount"]').prop('readonly', false);
				$('option:not(:selected)').prop('disabled', false);
				$('input[name="payment_method_nonce"]').val('');
			}
		}
	});

var env = $('#environment').val() === true ? BraintreeData.environments.production : BraintreeData.environments.sandbox;
BraintreeData.setup($('#merchantId').valueOf(), 'donateForm', env);

function PaymentType() {
	var paymentType = $('input[name="PaymentMethod"]:checked').val();

	switch (paymentType) {
		case 'paypal':
			$('#paypalrow').show();
			$('#creditcardrow').hide();
			break;
		case 'creditcard':
			$('#paypalrow').hide();
			$('#creditcardrow').show();
			break;
	}
}

$(document).ready(function () {
	// parse the query string
	var vars = [], hash;
	var queryString = document.URL.split('?')[1];
	if (queryString != undefined) {
		var queryStringArray = queryString.split('&');
		for (var i = 0; i < queryStringArray.length; i++) {
			hash = queryStringArray[i].split('=');
			vars.push(hash[1]);
			vars[hash[0]] = hash[1];
		}

		if (!isNaN(vars['Amount'])) {
			$('#Amount').val(vars['Amount']);
		}
	}

	$('input[name$="PhoneNumber"]').mask('000-000-0000');
	$('input[name$="PostalCode"]').mask('00000');

	$('input[name="PaymentMethod"]:radio').on('change', PaymentType);

	PaymentType();

	document.getElementById("FirstName").focus();
});