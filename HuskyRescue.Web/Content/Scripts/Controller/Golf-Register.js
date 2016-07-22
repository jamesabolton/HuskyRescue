braintree.setup($('#clientToken').val(),
	'custom',
	{
		id: 'golfRegisterForm',
		onError: function (response) {
			console.log('braintree onError: [type=' + response.type + '][message=' + response.message + ']');
			$('input[name^="Attendee"]').prop('readonly', false);
			$('input[name^="Payee"]').prop('readonly', false);
			$('option:not(:selected)').prop('disabled', false);
			$('input[name="PaymentMethod"]').prop('readonly', false);
		},
		onPaymentMethodReceived: function (response) {
			// performed when submitting the form
			console.log('braintree onPaymentMethodReceived: [nonce=' + response.nonce + '][type=' + response.type + '][details=' + response.details.cardType + ' -- ' + response.details.lastTwo + ']');

			if (response.details.cardType !== 'Unknown' && response.nonce !== '' && $('input[name="payment_method_nonce"]')) {
				$('input[name="payment_method_nonce"]').val(response.nonce);

				var isValid = $('#golfRegisterForm').valid();
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
					$('#golfRegisterForm').submit();
				}
			}
			else if ($('input[name="payment_method_nonce"]')) {
				$('input[name="payment_method_nonce"]').val('');
			}
		}
		/*,
		paypal: {
			container: 'paypal-button',
			singleUse: true,
			amount: parseFloat($('#AmountDue')),
			currency: 'USD',
			onSuccess: function (nonce, email) {
				console.log('paypal onSuccess');
				$('input[name^="Attendee"]').prop('readonly', true);
				$('input[name^="Payee"]').prop('readonly', true);
				$('option:not(:selected)').prop('disabled', true);
				$('input[name="PaymentMethod"]').prop('readonly', true);

				if (nonce !== '' && $('input[name="payment_method_nonce"]')) {
					$('input[name="payment_method_nonce"]').val(nonce);
				}
				// set paypal email to email address on form if one has not been provided already
				//if (email !== '' && $('input[name="Email"]').val() !== '') {
				//	$('input[name="Email"]').val(email);
				//}
			},
			onCancelled: function () {
				console.log('paypal onCancelled');
				$('input[name^="Attendee"]').prop('readonly', false);
				$('input[name^="Payee"]').prop('readonly', false);
				$('option:not(:selected)').prop('disabled', false);
				$('input[name="PaymentMethod"]').prop('readonly', false);
			},
			onUnsupported: function () {
				console.log('paypal onUnsupported');
				$('input[name^="Attendee"]').prop('readonly', false);
				$('input[name^="Payee"]').prop('readonly', false);
				$('option:not(:selected)').prop('disabled', false);
				$('input[name="PaymentMethod"]').prop('readonly', false);
			}
		}*/
	});
var env = $('#environment').val() === true ? BraintreeData.environments.production : BraintreeData.environments.sandbox;

BraintreeData.setup($('#merchantId').valueOf(), 'golfRegisterForm', env);

function UpdatePayeeList() {
	UpdateAmountDue();

	var inputName = $(this).prop('id');
	var playerNumber = parseInt(inputName.charAt(8));

	var firstNameInput = $('#Attendee' + (playerNumber) + 'FirstName');
	var lastNameInput = $('#Attendee' + (playerNumber) + 'LastName');

	var playerListSelectOptions = $('#payeelist option');

	// check if the option is already there
	var exists = false;
	playerListSelectOptions.each(function () {
		if (parseInt(this.value) === playerNumber) {
			exists = true;
			return false;
		}
	});

	// either name input has value
	if (firstNameInput.val() !== '' || lastNameInput.val() !== '') {
		if (!exists) {
			var newOption = '<option value="' + playerNumber + '">' + firstNameInput.val() + ' ' + lastNameInput.val() + '</option>';
			$('#payeelist').append(newOption);
		} else {
			$('#payeelist option[value=\'' + playerNumber + '\']').text(firstNameInput.val() + ' ' + lastNameInput.val());
		}
	}
	else {
		// remove existing option if no name provided
		if (exists) {
			$('#payeelist option[value=\'' + playerNumber + '\']').remove();
		}
	}
}

function PopulatePaymentInformation() {
	var playerList = $('#payeelist');
	var selectedPlayer = playerList.val();

	if (parseInt($('#payeelist').val()) >= 0) {
		var firstName = $('#Attendee' + selectedPlayer + 'FirstName').val();
		var lastName = $('#Attendee' + selectedPlayer + 'LastName').val();
		var street1 = $('#Attendee' + selectedPlayer + 'AddressStreet1').val();
		var street2 = $('#Attendee' + selectedPlayer + 'AddressStreet2').val();
		var stateId = $('#Attendee' + selectedPlayer + 'AddressStateId').val();
		var city = $('#Attendee' + selectedPlayer + 'AddressCity').val();
		var zip = $('#Attendee' + selectedPlayer + 'AddressPostalCode').val();
		var email = $('#Attendee' + selectedPlayer + 'EmailAddress').val();

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

function UpdateAmountDue() {
	var amountdue = 0;
	var golfTicketCost = parseInt($('#GolfTicketCost').val());
	var banquetTicketCost = parseInt($('#BanquetTicketCost').val());
	if ($('#Attendee1FirstName').val()) {
		switch ($('input[name="Attendee1Type"]:checked').val()) {
			case 'golfing':
				amountdue += golfTicketCost;
				$('#Attendee1TicketPrice').val(golfTicketCost);
				break;
			case 'banquet':
				amountdue += banquetTicketCost;
				$('#Attendee1TicketPrice').val(banquetTicketCost);
				break;
		}
	}
	if ($('#Attendee2FirstName').val()) {
		switch ($('input[name="Attendee2Type"]:checked').val()) {
			case 'golfing':
				amountdue += golfTicketCost;
				$('#Attendee2TicketPrice').val(golfTicketCost);
				break;
			case 'banquet':
				amountdue += banquetTicketCost;
				$('#Attendee2TicketPrice').val(banquetTicketCost);
				break;
		}
	}
	if ($('#Attendee3FirstName').val()) {
		switch ($('input[name="Attendee3Type"]:checked').val()) {
			case 'golfing':
				amountdue += golfTicketCost;
				$('#Attendee3TicketPrice').val(golfTicketCost);
				break;
			case 'banquet':
				amountdue += banquetTicketCost;
				$('#Attendee3TicketPrice').val(banquetTicketCost);
				break;
		}
	}
	if ($('#Attendee4FirstName').val()) {
		switch ($('input[name="Attendee4Type"]:checked').val()) {
			case 'golfing':
				amountdue += golfTicketCost;
				$('#Attendee4TicketPrice').val(golfTicketCost);
				break;
			case 'banquet':
				amountdue += banquetTicketCost;
				$('#Attendee4TicketPrice').val(banquetTicketCost);
				break;
		}
	}

	$('#AmountDue').val(parseInt(amountdue));
}

// assign functions to events
for (var i = 1; i <= 4; i++) {
	var firstNameInput = $('#Attendee' + (i) + 'FirstName');
	var lastNameInput = $('#Attendee' + (i) + 'LastName');
	var attendeeType = $('input[name="Attendee' + (i) + 'Type"]:radio');
	firstNameInput.on('change', UpdatePayeeList);
	lastNameInput.on('change', UpdatePayeeList);
	attendeeType.on('change', UpdateAmountDue);
}

$('#payeelist').on('change', PopulatePaymentInformation);

//$('input[name="PaymentMethod"]:radio').on('change', PaymentType);
$('#PaymentMethodcreditcard').prop('checked', true);
PaymentType();
$('#PaymentMethodcreditcard').parent().hide();
//$('#PaymentMethodpaypal').hide();

UpdateAmountDue();

$('input[name$="PhoneNumber"]').mask('000-000-0000');
$('input[name$="PostalCode"]').mask('00000');

document.getElementById('Attendee1FirstName').focus();
