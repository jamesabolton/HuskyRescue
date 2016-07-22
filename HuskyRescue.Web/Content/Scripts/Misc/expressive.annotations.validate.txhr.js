
ea.addMethod('IsValidMonth', function(month) {
	var monthInt = parseInt(month);

	if (!isNaN(monthInt) && isFinite(monthInt)) {
		if (monthInt >= 1 && monthInt <= 12) {
			return true;
		}
	}
	return false;
});

ea.addMethod('IsValidCreditCardYear', function(year) {
	var yearInt = parseInt(year);
	
	if (!isNaN(yearInt) && isFinite(yearInt)) {
		var currentYear=new Date().getFullYear();
		if (yearInt >= currentYear) {
			return true;
		}
	}
	return false;
});

ea.addMethod('IsNotPaypalPayment', function(method) {
	return $('#PaymentMethod').val() != "paypal";
});