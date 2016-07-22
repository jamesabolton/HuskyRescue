//$('#slide').slick();
$('#slide').camera({
	portrait: true,
	pagination: false
});

var eventDay = new Date(2015, 9, 23);
$('#countdown-time').countdown({
	until: eventDay
});

$('input:radio[name="donate-amount"]').on('click', function () {
	var amount = $("input[name=donate-amount]:checked").val();
	$('#donateNowLink').prop('href', '/Donate/Donate?Amount=' + amount);
});