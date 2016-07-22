using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Adoption
{
	public class Apply
	{
		public Apply()
		{
			ApplicantTypeId = 0;
			AppAddressStateList = new List<SelectListItem>();
			ResidencePetDepositCoverageList = new List<SelectListItem>();
			ResidenceOwnershipList = new List<SelectListItem>();
			ResidenceTypeList = new List<SelectListItem>();
			StudentTypeList = new List<SelectListItem>();
			GenderList = new List<SelectListItem>();
		}

		public Apply(int applicantType)
		{
			ApplicantTypeId = applicantType;
			AppAddressStateList = new List<SelectListItem>();
			ResidencePetDepositCoverageList = new List<SelectListItem>();
			ResidenceOwnershipList = new List<SelectListItem>();
			ResidenceTypeList = new List<SelectListItem>();
			StudentTypeList = new List<SelectListItem>();
			GenderList = new List<SelectListItem>();
		}

		[DisplayName("Would you like to receive newsletters and event information from Texas Husky Rescue in the future?")]
		public bool? IsEmailable { get; set; }

		public Guid Id { get; set; }

		public Guid PersonId { get; set; }

		public Guid? ApplicantVeterinarianId { get; set; }

		public int ApplicantTypeId { get; set; }

		#region Applicant Contact Information
		[DisplayName("Applicant First Name")]
		[Required(ErrorMessage = "please provide your first name")]
		[AssertThat("Length(AppNameFirst) <= 50 && AppNameFirst != null", ErrorMessage = "first name is required and must be less than 50 characters")]
		public string AppNameFirst { get; set; }

		[DisplayName("Last Name")]
		[Required(ErrorMessage = "please provide your full last name")]
		[AssertThat("Length(AppNameLast) <= 50 && AppNameLast != null", ErrorMessage = "last name is required and must be less than 50 characters")]
		public string AppNameLast { get; set; }

		public string AppFullName { get { return AppNameFirst + " " + AppNameLast; } }

		[DisplayName("Co-applicant First Name")]
		[AssertThat("Length(AppSpouseNameFirst) <= 50", ErrorMessage = "co-applicant first name must be less than 50 characters")]
		public string AppSpouseNameFirst { get; set; }

		[DisplayName("Last Name")]
		[AssertThat("Length(AppSpouseNameFirst) <= 50", ErrorMessage = "co-applicant last name must be less than 50 characters")]
		public string AppSpouseNameLast { get; set; }

		[DisplayName("Cell Phone Number")]
		[RequiredIf("AppHomePhone == null && AppEmail == null", ErrorMessage = "cell phone required if home phone and email not provided")]
		[AssertThat("Length(AppCellPhone) > 8 && Length(AppCellPhone) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string AppCellPhone { get; set; }

		[DisplayName("Home Phone Number")]
		[RequiredIf("AppCellPhone == null && AppEmail == null", ErrorMessage = "home phone required if cell phone and email not provided")]
		[AssertThat("Length(AppHomePhone) > 8 && Length(AppHomePhone) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string AppHomePhone { get; set; }

		[DisplayName("Email Address")]
		[RequiredIf("AppCellPhone == null && AppHomePhone == null", ErrorMessage = "email required if no phone numbers are provided")]
		[AssertThat("IsEmail(AppEmail) && Length(AppEmail) <= 200", ErrorMessage = "valid email address required")]
		public string AppEmail { get; set; }

		[DisplayName("Date of Birth")]
		[AssertThat("AppDateBirth <= AddYears(Today(), -21)", ErrorMessage = "You must be at least 21 years old")]
		public DateTime AppDateBirth { get; set; }

		[DisplayName("Street Address")]
		[Required(ErrorMessage = "home street address is required")]
		[StringLength(50, ErrorMessage = "street address must be less than 50 characters")]
		public string AppAddressStreet1 { get; set; }

		[DisplayName("City")]
		[Required(ErrorMessage = "city of home address is required")]
		[StringLength(50, ErrorMessage = "city name must be less than 50 characters")]
		public string AppAddressCity { get; set; }

		[DisplayName("State")]
		[Required(ErrorMessage = "state of home address is required")]
		public int AppAddressStateId { get; set; }
		public IEnumerable<SelectListItem> AppAddressStateList { get; set; }

		[DisplayName("Postal Code")]
		[DataType(DataType.PostalCode)]
		[Required(ErrorMessage = "postal code of home address is required")]
		[StringLength(10, ErrorMessage = "postal code must be less than 10 digits")]
		public string AppAddressZip { get; set; }

		public DateTime AddYears(DateTime from, int years)
		{
			return from.AddYears(years);
		}

		[DisplayName("Employer Name")]
		[StringLength(50, ErrorMessage = "employer name must be less than 50 characters")]
		public string AppEmployer { get; set; }

		public string AppAddressFull { get { return AppAddressStreet1 + ", " + AppAddressCity + ", " + AppAddressStateId + ", " + AppAddressZip; } }

		public string AppAddressCityStateZip { get { return AppAddressCity + ", " + AppAddressStateId + ", " + AppAddressZip; } }
		#endregion

		#region Residence Information
		[DisplayName("Do you own or rent?")]
		[Required(ErrorMessage = "owning / renting required")]
		public int ResidenceOwnershipId { get; set; }
		public IEnumerable<SelectListItem> ResidenceOwnershipList { get; set; }

		[DisplayName("Residence type?")]
		[Required(ErrorMessage = "residence type required")]
		public int ResidenceTypeId { get; set; }
		public IEnumerable<SelectListItem> ResidenceTypeList { get; set; }

		[DisplayName("Does your landlord allow pets?")]
		[RequiredIf("ResidenceOwnershipId == 2", ErrorMessage = "if pets are allowed is required")]
		public bool? ResidenceIsPetAllowed { get; set; }

		[DisplayName("Is a pet deposit required?")]
		[RequiredIf("ResidenceOwnershipId == 2", ErrorMessage = "need of a pet deposit must be answered")]
		public bool? ResidenceIsPetDepositRequired { get; set; }

		[DisplayName("How much is the deposit?")]
		[AssertThat("ResidencePetDepositAmount >= 0", ErrorMessage = "pet deposit amount must be greater than zero")]
		[RequiredIf("ResidenceOwnershipId == 2 && ResidenceIsPetDepositRequired == true", ErrorMessage = "pet deposit amount required")]
		public decimal? ResidencePetDepositAmount { get; set; }

		[DisplayName("Has the deposit been paid?")]
		[RequiredIf("ResidenceOwnershipId == 2 && ResidenceIsPetDepositRequired == true", ErrorMessage = "indicate if the pet deposit has been payed")]
		public bool? ResidenceIsPetDepositPaid { get; set; }

		[DisplayName("Is the deposit")]
		[RequiredIf("ResidenceOwnershipId == 2 && ResidenceIsPetDepositRequired == true", ErrorMessage = "indicate if the pet deposit covers all pets or one pet")]
		public int? ResidencePetDepositCoverageId { get; set; }
		public IEnumerable<SelectListItem> ResidencePetDepositCoverageList { get; set; }

		//[DataType(DataType.Text)]
		//[AssertThat("Length(ResidencePetDepositCoverage) <= 20", ErrorMessage = "")]
		//[RequiredIf("ResidenceOwnershipId == 2", ErrorMessage = "if renting provide landlord information")]
		//public string ResidencePetDepositCoverage { get; set; }

		[DisplayName("Pet size/weight Limit?")]
		[RequiredIf("ResidenceOwnershipId == 2", ErrorMessage = "indicate if there is a limit on pet size and/or weight")]
		public bool? ResidencePetSizeWeightLimit { get; set; }

		[DisplayName("Name of Apartment Complex or Landlord")]
		[AssertThat("Length(ResidenceLandlordName) <= 50", ErrorMessage = "landlord name must be less than 50 characters")]
		[RequiredIf("ResidenceOwnershipId == 2", ErrorMessage = "landlord or property owner name is required")]
		public string ResidenceLandlordName { get; set; }

		[DisplayName("Complex/Landlord Phone number")]
		[AssertThat("Length(ResidenceLandlordNumber) > 8 && Length(ResidenceLandlordNumber) < 15", ErrorMessage = "landlord phone must be a valid phone number 9 to 14 digits in length")]
		[RequiredIf("ResidenceOwnershipId == 2", ErrorMessage = "landlord phone number required")]
		[AssertThat("Length(ResidenceLandlordNumber) > 8 && Length(ResidenceLandlordNumber) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		public string ResidenceLandlordNumber { get; set; }

		[DisplayName("How long have you lived at your current residence?")]
		[Required(ErrorMessage = "length of residence required")]
		[StringLength(30, ErrorMessage = "length of residence must be less than 30 characters")]
		public string ResidenceLengthOfResidence { get; set; }
		#endregion

		#region Filtering Information
		[DisplayName("Are you or your spouse a student?")]
		[Required(ErrorMessage = "student status required")]
		public bool? IsAppOrSpouseStudent { get; set; }

		[DisplayName("Student Type")]
		[RequiredIf("IsAppOrSpouseStudent == true", ErrorMessage = "type of student is required")]
		public int? StudentTypeId { get; set; }
		public IEnumerable<SelectListItem> StudentTypeList { get; set; }

		[DisplayName("Do you or your spouse travel frequently?")]
		[Required(ErrorMessage = "travel frequency is required")]
		public bool? IsAppTravelFrequent { get; set; }

		[DisplayName("If yes, how often?"), DataType(DataType.Text)]
		[AssertThat("Length(AppTravelFrequency) <= 50", ErrorMessage = "travel frequency must be less than 50 characters")]
		[RequiredIf("IsAppTravelFrequent == true", ErrorMessage = "travel frequency detail is required")]
		public string AppTravelFrequency { get; set; }

		[DisplayName("Where would your pet stay while you are out of town?")]
		[Required(ErrorMessage = "your pets care information while you are out of town is required")]
		[StringLength(4000)]
		public string WhatIfTravelPetPlacement { get; set; }

		[DisplayName("If you had to move, what would you do with your pets?")]
		[Required(ErrorMessage = "what you do with your pets when moving is required")]
		[StringLength(4000)]
		public string WhatIfMovingPetPlacement { get; set; }

		[DisplayName("How many people live in your household")]
		[Required(ErrorMessage = "number of people living in the residence is required")]
		[StringLength(50, ErrorMessage = "household size must be less than 50 characters")]
		public string ResidenceNumberOccupants { get; set; }

		[DisplayName("Ages of children")]
		[StringLength(50, ErrorMessage = "ages of children must be less than 50 characters")]
		public string ResidenceAgesOfChildren { get; set; }

		[DisplayName("Do you have a fenced yard?")]
		[Required(ErrorMessage = "having a fenced yard is required")]
		public bool? ResidenceIsYardFenced { get; set; }

		[DisplayName("Type of fence?")]
		[AssertThat("Length(ResidenceFenceType) <= 50", ErrorMessage = "type of fence must be less than 50 characters")]
		[RequiredIf("ResidenceIsYardFenced == true", ErrorMessage = "type of fence is required")]
		public string ResidenceFenceType { get; set; }

		[DisplayName("Height of the fence?")]
		[AssertThat("Length(ResidenceFenceHeight) <= 50", ErrorMessage = "height of fence must be less than 50 characters")]
		[RequiredIf("ResidenceIsYardFenced == true", ErrorMessage = "height of fence is required")]
		public string ResidenceFenceHeight { get; set; }

		[DisplayName("This pet will be kept...")]
		public string PetKeptLocationInOutDoors { get; set; }

		[DisplayName("Totally Inside")]
		public bool IsPetKeptLocationInOutDoorsTotallyInside { get; set; }
		[DisplayName("Mostly Inside")]
		public bool IsPetKeptLocationInOutDoorsMostlyInside { get; set; }
		[DisplayName("Totally Outside")]
		public bool IsPetKeptLocationInOutDoorsTotallyOutside { get; set; }
		[DisplayName("Mostly Outside")]
		public bool IsPetKeptLocationInOutDoorMostlyOutsides { get; set; }

		[DisplayName("Reason")]
		[StringLength(4000)]
		public string PetKeptLocationInOutDoorsExplain { get; set; }

		[DisplayName("Number of hours a day")]
		[Required(ErrorMessage = "number of hours a day pet is left alone is required")]
		[StringLength(20, ErrorMessage = "number of hours pet left alone must be less than 20 characters")]
		public string PetLeftAloneHours { get; set; }

		[DisplayName("Number of days a week")]
		[Required(ErrorMessage = "number of days a week pet left alone is required")]
		[StringLength(20, ErrorMessage = "number of days a week pet left alone must be less than 20 characters")]
		public string PetLeftAloneDays { get; set; }

		[DisplayName("Where will this pet be kept while you are at work or away from home?")]
		public string PetKeptLocationAloneRestriction { get; set; }
		[DisplayName("Loose indoors")]
		public bool IsPetKeptLocationAloneRestrictionLooseIndoors { get; set; }
		[DisplayName("Garage")]
		public bool IsPetKeptLocationAloneRestrictionGarage { get; set; }
		[DisplayName("Outside kennel or dog run")]
		public bool IsPetKeptLocationAloneRestrictionOutsideKennel { get; set; }
		[DisplayName("Crated indoors")]
		public bool IsPetKeptLocationAloneRestrictionCratedIndoors { get; set; }
		[DisplayName("Crated Outdoors")]
		public bool IsPetKeptLocationAloneRestrictionCratedOutdoors { get; set; }
		[DisplayName("Loose in Backyard")]
		public bool IsPetKeptLocationAloneRestrictionLooseInBackyard { get; set; }
		[DisplayName("Tied Up Outdoors")]
		public bool IsPetKeptLocationAloneRestrictionTiedUpOutdoors { get; set; }
		[DisplayName("Basement")]
		public bool IsPetKeptLocationAloneRestrictionBasement { get; set; }
		[DisplayName("Other")]
		public bool IsPetKeptLocationAloneRestrictionOther { get; set; }

		[DisplayName("Reason")]
		[StringLength(4000)]
		public string PetKeptLocationAloneRestrictionExplain { get; set; }

		[DisplayName("Where will this pet sleep at night?")]
		public string PetKeptLocationSleepingRestriction { get; set; }
		[DisplayName("Loose indoors")]
		public bool IsPetKeptLocationSleepingRestrictionLooseIndoors { get; set; }
		[DisplayName("Garage")]
		public bool IsPetKeptLocationSleepingRestrictionGarage { get; set; }
		[DisplayName("Outside kennel or dog run")]
		public bool IsPetKeptLocationSleepingRestrictionOutsideKennel { get; set; }
		[DisplayName("Crated indoors")]
		public bool IsPetKeptLocationSleepingRestrictionCratedIndoors { get; set; }
		[DisplayName("Crated Outdoors")]
		public bool IsPetKeptLocationSleepingRestrictionCratedOutdoors { get; set; }
		[DisplayName("Loose in Backyard")]
		public bool IsPetKeptLocationSleepingRestrictionLooseInBackyard { get; set; }
		[DisplayName("Tied Up Outdoors")]
		public bool IsPetKeptLocationSleepingRestrictionTiedUpOutdoors { get; set; }
		[DisplayName("Basement")]
		public bool IsPetKeptLocationSleepingRestrictionBasement { get; set; }
		[DisplayName("In bed with owner")]
		public bool IsPetKeptLocationSleepingRestrictionInBedOwner { get; set; }
		[DisplayName("Other")]
		public bool IsPetKeptLocationSleepingRestrictionOther { get; set; }

		[DisplayName("Reason")]
		[StringLength(4000)]
		public string PetKeptLocationSleepingRestrictionExplain { get; set; }

		[DisplayName("Why do you want to adopt a husky at this time?")]
		[StringLength(4000)]
		public string PetAdoptionReason { get; set; }

		[DisplayName("House Pet")]
		public bool IsPetAdoptionReasonHousePet { get; set; }
		[DisplayName("Guard Dog")]
		public bool IsPetAdoptionReasonGuardDog { get; set; }
		[DisplayName("Watch Dog")]
		public bool IsPetAdoptionReasonWatchDog { get; set; }
		[DisplayName("Gift")]
		public bool IsPetAdoptionReasonGift { get; set; }
		[DisplayName("Companion for Child")]
		public bool IsPetAdoptionReasonCompanionChild { get; set; }
		[DisplayName("Companion for Pet")]
		public bool IsPetAdoptionReasonCompanionPet { get; set; }
		[DisplayName("Jogging partner")]
		public bool IsPetAdoptionReasonJoggingPartner { get; set; }
		[DisplayName("Other")]
		public bool IsPetAdoptionReasonOther { get; set; }

		[DisplayName("Reason")]
		[StringLength(4000)]
		public string PetAdoptionReasonExplain { get; set; }

		[DisplayName("Have you ever owned a husky?")]
		[Required(ErrorMessage = "previous ownership of a husky is required")]
		public bool? FilterAppHasOwnedHuskyBefore { get; set; }

		[DisplayName("What traits are you looking for in a Husky (active, lazy, kid friendly, cat friendly agility, etc.)? Be specific so we can find a husky that best fits your lifestyle.")]
		[Required(ErrorMessage = "traits desired in an adoptable husky is required")]
		[StringLength(4000)]
		public string FilterAppTraitsDesired { get; set; }

		[DisplayName("Do you currently own any cats?")]
		[Required(ErrorMessage = "current ownership of cats is required")]
		public bool? FilterAppIsCatOwner { get; set; }

		[DisplayName("If yes, how many?")]
		[AssertThat("Length(FilterAppCatsOwnedCount) <= 20", ErrorMessage = "number of cats owned must be less than 20 characters")]
		[RequiredIf("FilterAppIsCatOwner == true", ErrorMessage = "number of cats owned is required")]
		public string FilterAppCatsOwnedCount { get; set; }

		[DisplayName("Are you aware huskies are diggers, escape artists, heavy shedders, and may not be cat friendly?")]
		[Required(ErrorMessage = "traits of a husky is required")]
		public bool? FilterAppIsAwareHuskyAttributes { get; set; }

		[DisplayName("Are all adults in agreement about the adoption?")]
		[Required(ErrorMessage = "all adults in agreement is required")]
		public bool? IsAllAdultsAgreedOnAdoption { get; set; }

		[DisplayName("If no, why?")]
		[RequiredIf("IsAllAdultsAgreedOnAdoption == false", ErrorMessage = "reason for all adults not agreeing on adoption is required")]
		[AssertThat("Length(IsAllAdultsAgreedOnAdoptionReason) <= 4000")]
		public string IsAllAdultsAgreedOnAdoptionReason { get; set; }

		[DisplayName("Which of our huskies are you interested in?")]
		[StringLength(4000)]
		public string FilterAppDogsInterestedIn { get; set; }
		#endregion

		#region vet
		[DisplayName("Doctor Name")]
		[StringLength(50, ErrorMessage = "vet doctor name must be less than 50 characters")]
		[RequiredIf("Name1 != null || Name2 != null || Name3 != null || Name4 != null", ErrorMessage = "Vet required")]
		public string NameDr { get; set; }

		[DisplayName("Office Name")]
		[AssertThat("Length(NameOffice) <= 50", ErrorMessage = "vet office name must be less than 50 characters")]
		[RequiredIf("NameDr != null", ErrorMessage = "vet office name required")]
		public string NameOffice { get; set; }

		[DisplayName("Phone Number")]
		[AssertThat("Length(PhoneNumber) > 8 && Length(PhoneNumber) < 15", ErrorMessage = "phone number must be 9 to 14 digits")]
		[RequiredIf("NameDr != null || NameOffice != null", ErrorMessage = "vet phone number required")]
		public string PhoneNumber { get; set; }
		#endregion

		#region previous animals
		public IEnumerable<SelectListItem> GenderList { get; set; }

		#region 1
		[DisplayName("Name")]
		[StringLength(50, ErrorMessage = "pet name must be less than 50 characters")]
		public string Name1 { get; set; }

		[DisplayName("Breed")]
		[AssertThat("Length(Breed1) <= 20", ErrorMessage = "breed must be less than 20 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "breed is required")]
		public string Breed1 { get; set; }

		[DisplayName("Sex")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "sex is required")]
		public string Gender1 { get; set; }

		[DisplayName("Age")]
		[AssertThat("Length(AgeMonths1) <= 100", ErrorMessage = "age of pet must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "age of pet is required")]
		public string AgeMonths1 { get; set; }

		[DisplayName("Length of ownership")]
		[AssertThat("Length(OwnershipLengthMonths1) <= 100", ErrorMessage = "length of pet ownership must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "length of pet ownership is required")]
		public string OwnershipLengthMonths1 { get; set; }

		[DisplayName("Altered (spay/neuter)?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "pet alteration required")]
		public bool? IsAltered1 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(AlteredReason1) <= 200", ErrorMessage = "reason for not altering pet must be less than 200 characters")]
		[RequiredIf("IsAltered1 == false", ErrorMessage = "reason for not altering pet required")]
		public string AlteredReason1 { get; set; }

		[DisplayName("On HW Preventative?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "pet heartworm prevention required")]
		public bool? IsHwPrevention1 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(HwPreventionReason1) <= 200", ErrorMessage = "lack of heartworm prevention reason must be less than 200 characters")]
		[RequiredIf("IsHwPrevention1 == false", ErrorMessage = "lack of heartworm prevention reason is required")]
		public string HwPreventionReason1 { get; set; }

		[DisplayName("Fully Vaccinated?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "vaccination is required")]
		public bool? IsFullyVaccinated1 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(FullyVaccinatedReason1) <= 200", ErrorMessage = "lack of full vaccination reason must be less than 200 characters")]
		[RequiredIf("IsFullyVaccinated1 == false", ErrorMessage = "lack of full vaccination reason is required")]
		public string FullyVaccinatedReason1 { get; set; }

		[DisplayName("Do you still own this animal?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name1)", ErrorMessage = "current ownership of pet is required")]
		public bool? IsStillOwned1 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(IsStillOwnedReason1) <= 200", ErrorMessage = "lack of current ownership reason must be less than 200 characters")]
		[RequiredIf("IsStillOwned1 == false", ErrorMessage = "lack of current ownership reason is required")]
		public string IsStillOwnedReason1 { get; set; }
		#endregion

		#region 2
		[DisplayName("Name")]
		[StringLength(50, ErrorMessage = "pet name must be less than 50 characters")]
		public string Name2 { get; set; }

		[DisplayName("Breed")]
		[AssertThat("Length(Breed2) <= 20", ErrorMessage = "breed must be less than 20 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "breed is required")]
		public string Breed2 { get; set; }

		[DisplayName("Sex")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "sex is required")]
		public string Gender2 { get; set; }

		[DisplayName("Age")]
		[AssertThat("Length(AgeMonths2) <= 100", ErrorMessage = "age of pet must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "age of pet is required")]
		public string AgeMonths2 { get; set; }

		[DisplayName("Length of ownership")]
		[AssertThat("Length(OwnershipLengthMonths2) <= 100", ErrorMessage = "length of pet ownership must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "length of pet ownership is required")]
		public string OwnershipLengthMonths2 { get; set; }

		[DisplayName("Altered (spay/neuter)?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "pet alteration required")]
		public bool? IsAltered2 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(AlteredReason2) <= 200", ErrorMessage = "reason for not altering pet must be less than 200 characters")]
		[RequiredIf("IsAltered2 == false", ErrorMessage = "reason for not altering pet required")]
		public string AlteredReason2 { get; set; }

		[DisplayName("On HW Preventative?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "pet heartworm prevention required")]
		public bool? IsHwPrevention2 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(HwPreventionReason2) <= 200", ErrorMessage = "lack of heartworm prevention reason must be less than 200 characters")]
		[RequiredIf("IsHwPrevention2 == false", ErrorMessage = "lack of heartworm prevention reason is required")]
		public string HwPreventionReason2 { get; set; }

		[DisplayName("Fully Vaccinated?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "vaccination is required")]
		public bool? IsFullyVaccinated2 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(FullyVaccinatedReason2) <= 200", ErrorMessage = "lack of full vaccination reason must be less than 200 characters")]
		[RequiredIf("IsFullyVaccinated2 == false", ErrorMessage = "lack of full vaccination reason is required")]
		public string FullyVaccinatedReason2 { get; set; }

		[DisplayName("Do you still own this animal?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name2)", ErrorMessage = "current ownership of pet is required")]
		public bool? IsStillOwned2 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(IsStillOwnedReason2) <= 200", ErrorMessage = "lack of current ownership reason must be less than 200 characters")]
		[RequiredIf("IsStillOwned2 == false", ErrorMessage = "lack of current ownership reason is required")]
		public string IsStillOwnedReason2 { get; set; }
		#endregion

		#region 3
		[DisplayName("Name")]
		[StringLength(50, ErrorMessage = "pet name must be less than 50 characters")]
		public string Name3 { get; set; }

		[DisplayName("Breed")]
		[AssertThat("Length(Breed3) <= 20", ErrorMessage = "breed must be less than 20 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "breed is required")]
		public string Breed3 { get; set; }

		[DisplayName("Sex")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "sex is required")]
		public string Gender3 { get; set; }

		[DisplayName("Age")]
		[AssertThat("Length(AgeMonths3) <= 100", ErrorMessage = "age of pet must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "age of pet is required")]
		public string AgeMonths3 { get; set; }

		[DisplayName("Length of ownership")]
		[AssertThat("Length(OwnershipLengthMonths3) <= 100", ErrorMessage = "length of pet ownership must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "length of pet ownership is required")]
		public string OwnershipLengthMonths3 { get; set; }

		[DisplayName("Altered (spay/neuter)?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "pet alteration required")]
		public bool? IsAltered3 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(AlteredReason3) <= 200", ErrorMessage = "reason for not altering pet must be less than 200 characters")]
		[RequiredIf("IsAltered3 == false", ErrorMessage = "reason for not altering pet required")]
		public string AlteredReason3 { get; set; }

		[DisplayName("On HW Preventative?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "pet heartworm prevention required")]
		public bool? IsHwPrevention3 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(HwPreventionReason3) <= 200", ErrorMessage = "lack of heartworm prevention reason must be less than 200 characters")]
		[RequiredIf("IsHwPrevention3 == false", ErrorMessage = "lack of heartworm prevention reason is required")]
		public string HwPreventionReason3 { get; set; }

		[DisplayName("Fully Vaccinated?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "vaccination is required")]
		public bool? IsFullyVaccinated3 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(FullyVaccinatedReason3) <= 200", ErrorMessage = "lack of full vaccination reason must be less than 200 characters")]
		[RequiredIf("IsFullyVaccinated3 == false", ErrorMessage = "lack of full vaccination reason is required")]
		public string FullyVaccinatedReason3 { get; set; }

		[DisplayName("Do you still own this animal?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name3)", ErrorMessage = "current ownership of pet is required")]
		public bool? IsStillOwned3 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(IsStillOwnedReason3) <= 200", ErrorMessage = "lack of current ownership reason must be less than 200 characters")]
		[RequiredIf("IsStillOwned3 == false", ErrorMessage = "lack of current ownership reason is required")]
		public string IsStillOwnedReason3 { get; set; }
		#endregion

		#region 4
		[DisplayName("Name")]
		[StringLength(50, ErrorMessage = "pet name must be less than 50 characters")]
		public string Name4 { get; set; }

		[DisplayName("Breed")]
		[AssertThat("Length(Breed4) <= 20", ErrorMessage = "breed must be less than 20 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "breed is required")]
		public string Breed4 { get; set; }

		[DisplayName("Sex")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "sex is required")]
		public string Gender4 { get; set; }

		[DisplayName("Age")]
		[AssertThat("Length(AgeMonths4) <= 100", ErrorMessage = "age of pet must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "age of pet is required")]
		public string AgeMonths4 { get; set; }

		[DisplayName("Length of ownership")]
		[AssertThat("Length(OwnershipLengthMonths4) <= 100", ErrorMessage = "length of pet ownership must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "length of pet ownership is required")]
		public string OwnershipLengthMonths4 { get; set; }

		[DisplayName("Altered (spay/neuter)?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "pet alteration required")]
		public bool? IsAltered4 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(AlteredReason4) <= 200", ErrorMessage = "reason for not altering pet must be less than 200 characters")]
		[RequiredIf("IsAltered4 == false", ErrorMessage = "reason for not altering pet required")]
		public string AlteredReason4 { get; set; }

		[DisplayName("On HW Preventative?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "pet heartworm prevention required")]
		public bool? IsHwPrevention4 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(HwPreventionReason4) <= 200", ErrorMessage = "lack of heartworm prevention reason must be less than 200 characters")]
		[RequiredIf("IsHwPrevention4 == false", ErrorMessage = "lack of heartworm prevention reason is required")]
		public string HwPreventionReason4 { get; set; }

		[DisplayName("Fully Vaccinated?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "vaccination is required")]
		public bool? IsFullyVaccinated4 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(FullyVaccinatedReason4) <= 200", ErrorMessage = "lack of full vaccination reason must be less than 200 characters")]
		[RequiredIf("IsFullyVaccinated4 == false", ErrorMessage = "lack of full vaccination reason is required")]
		public string FullyVaccinatedReason4 { get; set; }

		[DisplayName("Do you still own this animal?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name4)", ErrorMessage = "current ownership of pet is required")]
		public bool? IsStillOwned4 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(IsStillOwnedReason4) <= 200", ErrorMessage = "lack of current ownership reason must be less than 200 characters")]
		[RequiredIf("IsStillOwned4 == false", ErrorMessage = "lack of current ownership reason is required")]
		public string IsStillOwnedReason4 { get; set; }
		#endregion

		#region 5
		[DisplayName("Name")]
		[StringLength(50, ErrorMessage = "pet name must be less than 50 characters")]
		public string Name5 { get; set; }

		[DisplayName("Breed")]
		[AssertThat("Length(Breed5) <= 20", ErrorMessage = "breed must be less than 20 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "breed is required")]
		public string Breed5 { get; set; }

		[DisplayName("Sex")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "sex is required")]
		public string Gender5 { get; set; }

		[DisplayName("Age")]
		[AssertThat("Length(AgeMonths5) <= 100", ErrorMessage = "age of pet must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "age of pet is required")]
		public string AgeMonths5 { get; set; }

		[DisplayName("Length of ownership")]
		[AssertThat("Length(OwnershipLengthMonths5) <= 100", ErrorMessage = "length of pet ownership must be less than 100 characters")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "length of pet ownership is required")]
		public string OwnershipLengthMonths5 { get; set; }

		[DisplayName("Altered (spay/neuter)?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "pet alteration required")]
		public bool? IsAltered5 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(AlteredReason5) <= 200", ErrorMessage = "reason for not altering pet must be less than 200 characters")]
		[RequiredIf("IsAltered5 == false", ErrorMessage = "reason for not altering pet required")]
		public string AlteredReason5 { get; set; }

		[DisplayName("On HW Preventative?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "pet heartworm prevention required")]
		public bool? IsHwPrevention5 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(HwPreventionReason5) <= 200", ErrorMessage = "lack of heartworm prevention reason must be less than 200 characters")]
		[RequiredIf("IsHwPrevention5 == false", ErrorMessage = "lack of heartworm prevention reason is required")]
		public string HwPreventionReason5 { get; set; }

		[DisplayName("Fully Vaccinated?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "vaccination is required")]
		public bool? IsFullyVaccinated5 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(FullyVaccinatedReason5) <= 200", ErrorMessage = "lack of full vaccination reason must be less than 200 characters")]
		[RequiredIf("IsFullyVaccinated5 == false", ErrorMessage = "lack of full vaccination reason is required")]
		public string FullyVaccinatedReason5 { get; set; }

		[DisplayName("Do you still own this animal?")]
		[RequiredIf("!IsNullOrWhiteSpace(Name5)", ErrorMessage = "current ownership of pet is required")]
		public bool? IsStillOwned5 { get; set; }

		[DisplayName("If no, please explain why")]
		[AssertThat("Length(IsStillOwnedReason5) <= 200", ErrorMessage = "lack of current ownership reason must be less than 200 characters")]
		[RequiredIf("IsStillOwned5 == false", ErrorMessage = "lack of current ownership reason is required")]
		public string IsStillOwnedReason5 { get; set; }
		#endregion
		#endregion
	}
}