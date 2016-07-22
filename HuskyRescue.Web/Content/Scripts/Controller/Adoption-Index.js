function equalheight() {
	$('.equalheight').each(function (index) {
		var maxHeight = 0;
		$(this).children('heightset').each(function (index) {
			if ($(this).height() > maxHeight)
				maxHeight = $(this).height();
		});
		$(this).children('heightset').height(maxHeight);
	});
}
$(window).bind("load", equalheight);
$(window).bind("resize", equalheight);

function displayAnimal() {
	// get the id of the Animal from the div's 'id' property that fired this function
	var AnimalId = this.id;
	var selectedAnimal = null;
	// find the Animal in the AnimalObj (JSON) using the AnimalId
	for (var count = 0; count < AnimalsObj.length; count++) {
		if (AnimalsObj[count].AnimalId === AnimalId) {
			selectedAnimal = AnimalsObj[count];
			break;
		}
	}

	if (selectedAnimal != null) {
		$('#AnimalDetailModal').foundation('reveal', 'open');
		$('#petName').text(selectedAnimal.AnimalName);
		var fixed = '';
		if (selectedAnimal.AnimalSex == 'Female' && selectedAnimal.AnimalAltered == 'Yes') {
			fixed = 'spayed';
		}
		if (selectedAnimal.AnimalSex == 'Female' && selectedAnimal.AnimalAltered != 'Yes') {
			fixed = 'not spayed';
		}
		if (selectedAnimal.AnimalSex == 'Male' && selectedAnimal.AnimalAltered == 'Yes') {
			fixed = 'neutered';
		}
		if (selectedAnimal.AnimalSex == 'Male' && selectedAnimal.AnimalAltered != 'Yes') {
			fixed = 'not neutered';
		}
		var petSexAge = selectedAnimal.AnimalBreed;
		if (selectedAnimal.AnimalSex != '') petSexAge += ' :: ' + selectedAnimal.AnimalSex;
		if (fixed != '') petSexAge += ' (' + fixed + ') ';
		if (selectedAnimal.AnimalGeneralAge != '') petSexAge += ' :: ' + selectedAnimal.AnimalGeneralAge;

		$('#petSexAge').text(petSexAge);
		$('#petBio').html(selectedAnimal.AnimalDescription);
		$('#petAttributes').empty();
		var attributes = $('#petAttributes').append('<ul></ul>').find('ul');
		if (selectedAnimal.AnimalNeedsFoster.length > 0) {
			attributes.append('<li>Foster home needed: ' + selectedAnimal.AnimalNeedsFoster + '</li>');
		}
		if (selectedAnimal.AnimalStatus.length > 0) {
			attributes.append('<li>Adoption status: ' + selectedAnimal.AnimalStatus + '</li>');
		}
		if (selectedAnimal.AnimalColor.length > 0) {
			attributes.append('<li>Coat color: ' + selectedAnimal.AnimalColor + '</li>');
		}
		if (selectedAnimal.AnimalCratetrained.length > 0) {
			attributes.append('<li>Crate trained: ' + selectedAnimal.AnimalCratetrained + '</li>');
		}
		if (selectedAnimal.AnimalEnergyLevel.length > 0) {
			attributes.append('<li>Energy level: ' + selectedAnimal.AnimalEnergyLevel + '</li>');
		}
		if (selectedAnimal.AnimalExerciseNeeds.length > 0) {
			attributes.append('<li>Exercise needs: ' + selectedAnimal.AnimalExerciseNeeds + '</li>');
		}
		if (selectedAnimal.AnimalEyeColor.length > 0) {
			attributes.append('<li>Eye color: ' + selectedAnimal.AnimalEyeColor + '</li>');
		}
		if (selectedAnimal.AnimalHousetrained.length > 0) {
			attributes.append('<li>House trained: ' + selectedAnimal.AnimalHousetrained + '</li>');
		}
		if (selectedAnimal.AnimalLeashtrained.length > 0) {
			attributes.append('<li>Leash trained: ' + selectedAnimal.AnimalLeashtrained + '</li>');
		}
		if (selectedAnimal.AnimalMixedBreed.length > 0) {
			attributes.append('<li>Mixed breed: ' + selectedAnimal.AnimalMixedBreed + '</li>');
		}
		if (selectedAnimal.AnimalOkWithAdults.length > 0) {
			attributes.append('<li>ok with adults: ' + selectedAnimal.AnimalokWithAdults + '</li>');
		}
		if (selectedAnimal.AnimalOkWithCats.length > 0) {
			attributes.append('<li>ok with cats: ' + selectedAnimal.AnimalokWithCats + '</li>');
		}
		if (selectedAnimal.AnimalOkWithDogs.length > 0) {
			attributes.append('<li>ok with other dogs: ' + selectedAnimal.AnimalokWithDogs + '</li>');
		}
		if (selectedAnimal.AnimalOkWithKids.length > 0) {
			attributes.append('<li>ok with kids: ' + selectedAnimal.AnimalokWithKids + '</li>');
		}
		if (selectedAnimal.AnimalSpecialneeds.length > 0) {
			if (selectedAnimal.AnimalSpecialneeds == 'Yes') {
				attributes.append('<li>Special needs: ' + selectedAnimal.AnimalSpecialneeds + ' (contact us for details)</li>');
			} else {
				attributes.append('<li>Special needs: ' + selectedAnimal.AnimalSpecialneeds + '</li>');
			}
		}

		if (selectedAnimal.AnimalPictures.length > 0) {
			// setup HTML for Animal pictures
			$('#petPictures').empty();

			for (var pictureCount = 0; pictureCount < selectedAnimal.AnimalPictures.length; pictureCount++) {
				var pictureObject = selectedAnimal.AnimalPictures[pictureCount];
				var pictureDiv = '<div data-src="' + pictureObject.UrlSecureFullsize + '"></div>';
				$('#petPictures').append(pictureDiv);
			}

			// start the picture slide show
			$('#petPictures').camera({
				portrait: true
			});
		}
	}
}

$('.AnimalCell').on('mouseover', function () { $(this).addClass('AnimalCellHover'); });
$('.AnimalCell').on('mouseout', function () { $(this).removeClass('AnimalCellHover'); });
$('.AnimalCell').on('click', displayAnimal);

$('#revealClose').on('click', function () { $('#AnimalDetailModal').foundation('reveal', 'close'); });