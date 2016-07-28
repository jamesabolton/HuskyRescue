ea.addMethod('AddYears', function (from, years) {
    from = new Date(from);
    var to = from.getFullYear() + years;
    from.setFullYear(to);
    return from.getTime();
});

braintree.setup($('#clientToken').val(),
    'custom',
    {
        id: 'applyForm',
        onError: function (response) {
            console.log('braintree onError: [type=' + response.type + '][message=' + response.message + ']');
            $('option:not(:selected)').prop('disabled', false);
            $('input[name="payment_method_nonce"]').val('');
        },
        onPaymentMethodReceived: function (response) {
            // performed when submitting the form
            console.log('braintree onPaymentMethodReceived: [nonce=' + response.nonce + '][type=' + response.type + '][details=' + response.details.cardType + ' -- ' + response.details.lastTwo + ']');

            if (response.details.cardType !== 'Unknown' && response.nonce !== '' && $('input[name="payment_method_nonce"]')) {
                $('input[name="payment_method_nonce"]').val(response.nonce);

                var isValid = $('#applyForm').valid();
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
                    $('#applyForm').submit();
                }
            }
            else if ($('input[name="payment_method_nonce"]')) {
                $('input[name="payment_method_nonce"]').val('');
            }
        },
        paypal: {
            container: 'paypal-button',
            singleUse: true,
            amount: parseFloat($('#ApplicationFeeAmountReadOnly')),
            currency: 'USD',
            onSuccess: function (nonce, email) {
                // This will be called as soon as the user completes the PayPal flow
                console.log('paypal onSuccess');
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
                $('option:not(:selected)').prop('disabled', false);
                $('input[name="payment_method_nonce"]').val('');
            },
            onUnsupported: function () {
                console.log('paypal onUnsupported');
                $('option:not(:selected)').prop('disabled', false);
                $('input[name="payment_method_nonce"]').val('');
            }
        }
    });

var env = $('#environment').val() === true ? BraintreeData.environments.production : BraintreeData.environments.sandbox;
BraintreeData.setup($('#merchantId').valueOf(), 'applyForm', env);

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

$('input[name="PaymentMethod"]:radio').on('change', PaymentType);
PaymentType();

var past = new Date();
if ($('#AppDateBirth').val() === '') {
    past.setTime(past.valueOf() - (21 * 365 * 24 * 60 * 60 * 1000));
}
$('#AppDateBirth').fdatepicker('update', past);

$('#IsAllAdultsAgreedOnAdoptionReason_div').hide();
$('input[name="IsAllAdultsAgreedOnAdoption"]').click(function () {
    if ($('input[name="IsAllAdultsAgreedOnAdoption"]:checked').val() === 'True') {
        $('#IsAllAdultsAgreedOnAdoptionReason_div').hide();
    } else {
        $('#IsAllAdultsAgreedOnAdoptionReason_div').show();
    }
});

$('.ResidenceRent').hide();
$('#ResidenceOwnershipId').change(function () {
    if ($('#ResidenceOwnershipId').val() === '2') {
        $('.ResidenceRent').show();
    } else {
        $('.ResidenceRent').hide();
    }
});

$('.ResidencePetDeposit').hide();
$('input[name="ResidenceIsPetDepositRequired"]').click(function () {
    if ($('input[name="ResidenceIsPetDepositRequired"]:checked').val() === 'False') {
        $('.ResidencePetDeposit').hide();
    } else {
        $('.ResidencePetDeposit').show();
    }
});

$('.ResidenceFence').hide();
$('input[name="ResidenceIsYardFenced"]').click(function () {
    if ($('input[name="ResidenceIsYardFenced"]:checked').val() === 'False') {
        $('.ResidenceFence').hide();
    } else {
        $('.ResidenceFence').show();
    }
});

$('.StudentType').hide();
$('input[name="IsAppOrSpouseStudent"]').click(function () {
    if ($('input[name="IsAppOrSpouseStudent"]:checked').val() === 'False') {
        $('.StudentType').hide();
    } else {
        $('.StudentType').show();
    }
});

$('.TravelFrequent').hide();
$('input[name="IsAppTravelFrequent"]').click(function () {
    if ($('input[name="IsAppTravelFrequent"]:checked').val() === 'False') {
        $('.TravelFrequent').hide();
    } else {
        $('.TravelFrequent').show();
    }
});

$('.CatOwner').hide();
$('input[name="FilterAppIsCatOwner"]').click(function () {
    if ($('input[name="FilterAppIsCatOwner"]:checked').val() === 'False') {
        $('.CatOwner').hide();
    } else {
        $('.CatOwner').show();
    }
});

$('.Pet1').hide();
$('.Pet1Alter').hide();
$('.Pet1HW').hide();
$('.Pet1Vacc').hide();
$('.Pet1StillOwned').hide();

$('#Name1').change(function () {
    if ($('#Name1').val() !== '') {
        $('.Pet1').show();
    } else {
        $('.Pet1').hide();
        $('.Pet1Alter').hide();
        $('.Pet1HW').hide();
        $('.Pet1Vacc').hide();
        $('.Pet1StillOwned').hide();
    }
});
$('#Name1').blur(function () {
    if ($('#Name1').val() !== '') {
        $('#Breed1').focus();
    } else {
        $('#Name2').focus();
    }
});

$('input[name="IsAltered1"]').click(function () {
    if ($('input[name="IsAltered1"]:checked').val() === 'False') {
        $('.Pet1Alter').show();
    } else {
        $('.Pet1Alter').hide();
    }
});
$('input[name="IsHwPrevention1"]').click(function () {
    if ($('input[name="IsHwPrevention1"]:checked').val() === 'False') {
        $('.Pet1HW').show();
    } else {
        $('.Pet1HW').hide();
    }
});
$('input[name="IsFullyVaccinated1"]').click(function () {
    if ($('input[name="IsFullyVaccinated1"]:checked').val() === 'False') {
        $('.Pet1Vacc').show();
    } else {
        $('.Pet1Vacc').hide();
    }
});
$('input[name="IsStillOwned1"]').click(function () {
    if ($('input[name="IsStillOwned1"]:checked').val() === 'False') {
        $('.Pet1StillOwned').show();
    } else {
        $('.Pet1StillOwned').hide();
    }
});

$('.Pet2').hide();
$('.Pet2Alter').hide();
$('.Pet2HW').hide();
$('.Pet2Vacc').hide();
$('.Pet2StillOwned').hide();
$('#Name2').change(function () {
    if ($('#Name2').val() !== '') {
        $('.Pet2').show();
    } else {
        $('.Pet2').hide();
        $('.Pet2Alter').hide();
        $('.Pet2HW').hide();
        $('.Pet2Vacc').hide();
        $('.Pet2StillOwned').hide();
    }
});
$('#Name2').blur(function () {
    if ($('#Name2').val() !== '') {
        $('#Breed2').focus();
    } else {
        $('#Name3').focus();
    }
});

$('input[name="IsAltered2"]').click(function () {
    if ($('input[name="IsAltered2"]:checked').val() === 'False') {
        $('.Pet2Alter').show();
    } else {
        $('.Pet2Alter').hide();
    }
});
$('input[name="IsHwPrevention2"]').click(function () {
    if ($('input[name="IsHwPrevention2"]:checked').val() === 'False') {
        $('.Pet2HW').show();
    } else {
        $('.Pet2HW').hide();
    }
});
$('input[name="IsFullyVaccinated2"]').click(function () {
    if ($('input[name="IsFullyVaccinated2"]:checked').val() === 'False') {
        $('.Pet2Vacc').show();
    } else {
        $('.Pet2Vacc').hide();
    }
});
$('input[name="IsStillOwned2"]').click(function () {
    if ($('input[name="IsStillOwned2"]:checked').val() === 'False') {
        $('.Pet2StillOwned').show();
    } else {
        $('.Pet2StillOwned').hide();
    }
});

$('.Pet3').hide();
$('.Pet3Alter').hide();
$('.Pet3HW').hide();
$('.Pet3Vacc').hide();
$('.Pet3StillOwned').hide();
$('#Name3').change(function () {
    if ($('#Name3').val() !== '') {
        $('.Pet3').show();
    } else {
        $('.Pet3').hide();
        $('.Pet3Alter').hide();
        $('.Pet3HW').hide();
        $('.Pet3Vacc').hide();
        $('.Pet3StillOwned').hide();
    }
});
$('#Name3').blur(function () {
    if ($('#Name3').val() !== '') {
        $('#Breed3').focus();
    } else {
        $('#Name4').focus();
    }
});

$('input[name="IsAltered3"]').click(function () {
    if ($('input[name="IsAltered3"]:checked').val() === 'False') {
        $('.Pet3Alter').show();
    } else {
        $('.Pet3Alter').hide();
    }
});
$('input[name="IsHwPrevention3"]').click(function () {
    if ($('input[name="IsHwPrevention3"]:checked').val() === 'False') {
        $('.Pet3HW').show();
    } else {
        $('.Pet3HW').hide();
    }
});
$('input[name="IsFullyVaccinated3"]').click(function () {
    if ($('input[name="IsFullyVaccinated3"]:checked').val() === 'False') {
        $('.Pet3Vacc').show();
    } else {
        $('.Pet3Vacc').hide();
    }
});
$('input[name="IsStillOwned3"]').click(function () {
    if ($('input[name="IsStillOwned3"]:checked').val() === 'False') {
        $('.Pet3StillOwned').show();
    } else {
        $('.Pet3StillOwned').hide();
    }
});

$('.Pet4').hide();
$('.Pet4Alter').hide();
$('.Pet4HW').hide();
$('.Pet4Vacc').hide();
$('.Pet4StillOwned').hide();
$('#Name4').change(function () {
    if ($('#Name4').val() !== '') {
        $('.Pet4').show();
    } else {
        $('.Pet4').hide();
        $('.Pet4Alter').hide();
        $('.Pet4HW').hide();
        $('.Pet4Vacc').hide();
        $('.Pet4StillOwned').hide();
    }
});
$('#Name4').blur(function () {
    if ($('#Name4').val() !== '') {
        $('#Breed4').focus();
    } else {
        $('#Name5').focus();
    }
});

$('input[name="IsAltered4"]').click(function () {
    if ($('input[name="IsAltered4"]:checked').val() === 'False') {
        $('.Pet4Alter').show();
    } else {
        $('.Pet4Alter').hide();
    }
});
$('input[name="IsHwPrevention4"]').click(function () {
    if ($('input[name="IsHwPrevention4"]:checked').val() === 'False') {
        $('.Pet4HW').show();
    } else {
        $('.Pet4HW').hide();
    }
});
$('input[name="IsFullyVaccinated4"]').click(function () {
    if ($('input[name="IsFullyVaccinated4"]:checked').val() === 'False') {
        $('.Pet4Vacc').show();
    } else {
        $('.Pet4Vacc').hide();
    }
});
$('input[name="IsStillOwned4"]').click(function () {
    if ($('input[name="IsStillOwned4"]:checked').val() === 'False') {
        $('.Pet4StillOwned').show();
    } else {
        $('.Pet4StillOwned').hide();
    }
});

$('.Pet5').hide();
$('.Pet5Alter').hide();
$('.Pet5HW').hide();
$('.Pet5Vacc').hide();
$('.Pet5StillOwned').hide();
$('#Name5').change(function () {
    if ($('#Name5').val() !== '') {
        $('.Pet5').show();
    } else {
        $('.Pet5').hide();
        $('.Pet5Alter').hide();
        $('.Pet5HW').hide();
        $('.Pet5Vacc').hide();
        $('.Pet5StillOwned').hide();
    }
});
$('#Name5').blur(function () {
    if ($('#Name5').val() !== '') {
        $('#Breed5').focus();
    } else {
        $('#submitButton').focus();
    }
});

$('input[name="IsAltered5"]').click(function () {
    if ($('input[name="IsAltered5"]:checked').val() === 'False') {
        $('.Pet5Alter').show();
    } else {
        $('.Pet5Alter').hide();
    }
});
$('input[name="IsHwPrevention5"]').click(function () {
    if ($('input[name="IsHwPrevention5"]:checked').val() === 'False') {
        $('.Pet5HW').show();
    } else {
        $('.Pet5HW').hide();
    }
});
$('input[name="IsFullyVaccinated5"]').click(function () {
    if ($('input[name="IsFullyVaccinated5"]:checked').val() === 'False') {
        $('.Pet5Vacc').show();
    } else {
        $('.Pet5Vacc').hide();
    }
});
$('input[name="IsStillOwned5"]').click(function () {
    if ($('input[name="IsStillOwned5"]:checked').val() === 'False') {
        $('.Pet5StillOwned').show();
    } else {
        $('.Pet5StillOwned').hide();
    }
});


$('input[name$="AppHomePhone"]').mask('000-000-0000');
$('input[name$="AppCellPhone"]').mask('000-000-0000');
$('input[name$="PhoneNumber"]').mask('000-000-0000');
$('input[name$="ResidenceLandlordNumber"]').mask('000-000-0000');
$('input[name$="AppAddressZip"]').mask('00000');
