function HasPaidChange() {
	if ($('input[name="HasPaid"]').prop('checked')) {
		$('#AmountPaid').prop('readonly', '');
	} else {
		$('#AmountPaid').prop('readonly', 'true');
	}
}
$('input[name="HasPaid"]').on('change', HasPaidChange);
HasPaidChange();

function SponsorTypeChange() {
	var sponsorType = $('input[name="SponsorType"]:checked').val();

	switch (sponsorType) {
		case "P":
			$('#IsBusinessSponsor').val('false');
			$('#IsPersonSponsor').val('true');
			$('#personFields').show();
			$('#businessFields').hide();
			break;
		case "B":
			$('#IsBusinessSponsor').val('true');
			$('#IsPersonSponsor').val('false');
			$('#businessFields').show();
			$('#personFields').hide();
			break;
		default:
			$('#IsBusinessSponsor').val('false');
			$('#IsPersonSponsor').val('false');
			$('#businessFields').hide();
			$('#personFields').hide();
			break;
	}
}
$('input[name="SponsorType"]:radio').on('change', SponsorTypeChange);
SponsorTypeChange();

function IsNewOrgChange() {
	var isNewOrg = $('input[name="IsNewOrg"]:checked').val();

	switch (isNewOrg) {
		case "true":
			$('#RowOrgNew').show();
			$('#RowNewEntity').show();
			$('#RowOrgExisting').hide();
			break;
		case "false":
			$('#RowOrgExisting').show();
			$('#RowNewEntity').hide();
			$('#RowOrgNew').hide();
			break;
		default:
			$('#RowOrgNew').hide();
			$('#RowNewEntity').hide();
			$('#RowOrgExisting').hide();
			break;
	}
}
$('input[name="IsNewOrg"]:radio').on('change', IsNewOrgChange);
IsNewOrgChange();

function IsNewPersonChange() {
	var isNewPerson = $('input[name="IsNewPerson"]:checked').val();

	switch (isNewPerson) {
		case "true":
			$('#RowPersonNew').show();
			$('#RowNewEntity').show();
			$('#RowPersonExisting').hide();
			break;
		case "false":
			$('#RowPersonExisting').show();
			$('#RowNewEntity').hide();
			$('#RowPersonNew').hide();
			break;
		default:
			$('#RowPersonNew').hide();
			$('#RowNewEntity').hide();
			$('#RowPersonExisting').hide();
			break;
	}
}
$('input[name="IsNewPerson"]:radio').on('change', IsNewPersonChange);
IsNewPersonChange();

$("#EventId").change(function () {
	var eventName = $("#EventId :selected").text();  // Name of event selected
	var eventId = $("#EventId :selected").val();  // Id of event selected
	var url = '/AdminEventsSponsorship/ListByEvent';
	var data1 = { "eventId": eventId };
	$.post(url, data1, function(data) { //ajax call
		var items = [];
		items.push("<option value=''>" + "-- select sponsorship --" + "</option>"); //first item
		for (var i = 0; i < data.length; i++) {
			items.push("<option value=" + data[i].Id + ">" + data[i].Name + "</option>");
		} //all data from the team table push into array
		$("#EventSponsorshipId").html(items.join(' ')); //array object bind to dropdown list
	});
});

Dropzone.options.adminEventsSponsorsForm = {
	autoProcessQueue: false,
	uploadMultiple: false,
	parallelUploads: 1,
	maxFiles: 1,
	maxFilesize: 1, // MBs
	previewsContainer: ".dropzone-previews",
	acceptedFiles: "image/*",
	//autoDiscover: false, // prevent uploading file twice
	init: function () {
		//var myDropzone = this;

		//// First change the button to actually tell Dropzone to process the queue.
		//this.element.querySelector("input[type=submit]").addEventListener("click", function (e) {
		//	// Make sure that the form isn't being sent by default jquery logic
		//	e.preventDefault();
		//	//e.stopPropagation();
		//	var isValid = $('#adminEventsSponsorsForm').valid();
		//	if (isValid) {
		//		if (myDropzone.getQueuedFiles().length > 0) {
		//			myDropzone.processQueue();
		//		} else {
		//			$("#adminEventsSponsorsForm").submit();
		//		}
		//	}
		//});

		// Listen to the sendingmultiple event. In this case, it's the sendingmultiple event instead
		// of the sending event because uploadMultiple is set to true.
		this.on("sendingmultiple", function () {
			// Gets triggered when the form is actually being sent.
			// Hide the success button or the complete form.
		});
		this.on("successmultiple", function (files, response) {
			// Gets triggered when the files have successfully been sent.
			// Redirect user or notify of success.
		});
		this.on("errormultiple", function (files, response) {
			// Gets triggered when there was an error sending the files.
			// Maybe show form again, and notify user of error
		});
		this.on("success", function (files, response) {
			// gets triggered when the files have successfully been sent.
			// redirect user or notify of success
			alert(response);
			// window.location.replace("/AdminEventsSponsors/Index");
		});
	}
}