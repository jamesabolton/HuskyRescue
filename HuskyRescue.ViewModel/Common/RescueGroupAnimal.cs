using System.Collections.Generic;

namespace HuskyRescue.ViewModel.Common
{
	public class RescueGroupAnimal
	{
		public string AnimalId { get; set; }
		public string AnimalAltered { get; set; }
		public string AnimalBirthdate { get; set; }
		public string AnimalBreed { get; set; }
		public string AnimalColor { get; set; }
		public string AnimalCratetrained { get; set; }
		public string AnimalDescription { get; set; }
		public string AnimalEnergyLevel { get; set; }
		public string AnimalExerciseNeeds { get; set; }
		public string AnimalEyeColor { get; set; }
		public string AnimalGeneralAge { get; set; }
		public string AnimalHousetrained { get; set; }
		public string AnimalLeashtrained { get; set; }
		public string AnimalMixedBreed { get; set; }
		public string AnimalName { get; set; }
		public string AnimalNeedsFoster { get; set; }
		public string AnimalOkWithAdults { get; set; }
		public string AnimalOkWithCats { get; set; }
		public string AnimalOkWithDogs { get; set; }
		public string AnimalOkWithKids { get; set; }
		public string AnimalOrgId { get; set; }
		public string AnimalOthernames { get; set; }
		public string AnimalRescueId { get; set; }
		public string AnimalSex { get; set; }
		public string AnimalSpecialneeds { get; set; }
		public string AnimalSpecialneedsDescription { get; set; }
		public string AnimalStatus { get; set; }
		public string AnimalSummarypublic { get; set; }
		public string AnimalThumbnailUrl { get; set; }
		public List<RescueGroupPicture> AnimalPictures { get; set; }

		public RescueGroupAnimal()
		{
			AnimalPictures = new List<RescueGroupPicture>();
		}
	}
}