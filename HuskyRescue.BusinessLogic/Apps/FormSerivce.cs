using System;
using System.IO;
using System.Security;
using HuskyRescue.ViewModel.Common;
using iTextSharp.text;
using iTextSharp.text.pdf;
using HuskyRescue.ViewModel.Adoption;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
    public class FormSerivce : IFormSerivce
    {
        private static ILogger _logger;
        public FormSerivce(ILogger ilogger)
        {
            _logger = ilogger;
        }

        public string CreateAdoptionApplicationPdf(Apply app)
        {
            // Path to newly created file based on PDF template
            var pdfNewFilePath = string.Empty;
            try
            {
                //if (appType.Equals("A"))
                //{
                var genPath = AdminSystemSettings.GetSetting("AdoptionApplicationOutputPath");

                // get location of template
                var pdfTempFilePath = AdminSystemSettings.GetSetting("AdoptionApplicationFormPath");
                //}
                //else
                //{
                //	genPath = Settings.Default.PathToGeneratedFosterApplications;
                //	// get location of template
                //	pdfTempFilePath = HttpContext.Current.Server.MapPath(Settings.Default.FosterApplicationLocation);
                //}

                // create path to new file that will be generated
                var fileName = app.AppNameLast + " - " + app.Id + ".pdf";
                pdfNewFilePath = Path.Combine(genPath, fileName);

                using (var tempPdfFileStream = new FileStream(pdfTempFilePath, FileMode.Open))
                using (var newPdfFileStream = new FileStream(pdfNewFilePath, FileMode.Create))
                {
                    // Open template PDF
                    var pdfReader = new PdfReader(tempPdfFileStream);

                    // PdfStamper is used to read the field keys and flatten the PDF after generating
                    using (var pdfStamper = new PdfStamper(pdfReader, newPdfFileStream))
                    {
                        try
                        {
                            // if "true" then checkbox and radio buttons will maintain format set in the PDF
                            var saveAppearance = true;

                            // get list of all field names (keys) in the template
                            var pdfFormFields = pdfStamper.AcroFields;

                            pdfFormFields.SetField("Comments", app.Comments);

                            pdfFormFields.SetField("AppFeeAmount", app.ApplicationFeeAmount.ToString());
                            pdfFormFields.SetField("AppFeePaymentMethod", "online");// app.PaymentMethod.ToString()); // change to literal as payment is required and this will always be "online"
                            pdfFormFields.SetField("AppFeePaymentTranId", app.BraintreeTransactionId.ToString());

                            pdfFormFields.SetField("AppName", app.AppNameFirst + " " + app.AppNameLast);
                            pdfFormFields.SetField("AppSpouseName", app.AppSpouseNameFirst + " " + app.AppSpouseNameLast);
                            pdfFormFields.SetField("AppAddressStreet", app.AppAddressStreet1);
                            var stateName = Lists.GetStateList().Find(x => x.Value.Equals(app.AppAddressStateId.ToString())).Text;
                            pdfFormFields.SetField("AppAddressCityStateZip", app.AppAddressCity + ", " + stateName + " " + app.AppAddressZip);
                            pdfFormFields.SetField("AppHomePhone", app.AppHomePhone);
                            pdfFormFields.SetField("AppCellPhone", app.AppCellPhone);
                            pdfFormFields.SetField("AppEmail", app.AppEmail);
                            pdfFormFields.SetField("AppEmployer", app.AppEmployer);
                            pdfFormFields.SetField("AppDateBirth", app.AppDateBirth.ToShortDateString());
                            pdfFormFields.SetField("DateSubmitted", DateTime.Today.ToShortDateString());
                            pdfFormFields.SetField("IsAllAdultsAgreedOnAdoption", IsTrueFalse(app.IsAllAdultsAgreedOnAdoption), saveAppearance);
                            pdfFormFields.SetField("IsAllAdultsAgreedOnAdoptionReason", app.IsAllAdultsAgreedOnAdoptionReason, saveAppearance);
                            pdfFormFields.SetField("ResidenceOwnership",
                                                   Lists.GetResidenceOwnershipTypeList(false, false)
                                                        .Find(x => x.Value.Equals(app.ResidenceOwnershipId.ToString()))
                                                        .Value, saveAppearance);
                            pdfFormFields.SetField("ResidenceType",
                                                   Lists.GetResidencTypeList(false, false)
                                                        .Find(x => x.Value.Equals(app.ResidenceTypeId.ToString()))
                                                        .Value, saveAppearance);

                            if (app.ResidenceOwnershipId.Equals(2))
                            {
                                //Rent
                                pdfFormFields.SetField("ResidenceIsPetAllowed", IsTrueFalse(app.ResidenceIsPetAllowed), saveAppearance);
                                pdfFormFields.SetField("ResidenceIsPetDepositRequired", IsTrueFalse(app.ResidenceIsPetDepositRequired), saveAppearance);

                                if (app.ResidenceIsPetDepositRequired.HasValue)
                                {
                                    if (app.ResidenceIsPetDepositRequired.Value)
                                    {
                                        pdfFormFields.SetField("ResidencePetDepositAmount", app.ResidencePetDepositAmount.ToString());
                                        if (app.ResidencePetDepositCoverageId.HasValue)
                                        {
                                            pdfFormFields.SetField("ResidencePetDepositCoverage",
                                                                   Lists.GetResidenceCoverageTypeList(false, false)
                                                                        .Find(x => x.Value.Equals(app.ResidencePetDepositCoverageId.Value.ToString()))
                                                                        .Value, saveAppearance);
                                        }
                                        pdfFormFields.SetField("ResidenceIsPetDepositPaid", IsTrueFalse(app.ResidenceIsPetDepositPaid), saveAppearance);
                                    }
                                }
                                pdfFormFields.SetField("ResidenceIsPetSizeWeightLimit", IsTrueFalse(app.ResidencePetSizeWeightLimit), saveAppearance);
                                pdfFormFields.SetField("ResidenceLandlordName", app.ResidenceLandlordName);
                                pdfFormFields.SetField("ResidenceLandlordNumber", app.ResidenceLandlordNumber);
                            }
                            pdfFormFields.SetField("ResidenceLengthOfResidence", app.ResidenceLengthOfResidence);
                            pdfFormFields.SetField("WhatIfMovingPetPlacement", app.WhatIfMovingPetPlacement);
                            if (app.IsAppOrSpouseStudent.HasValue)
                            {
                                pdfFormFields.SetField("IsAppOrSpouseStudent", IsTrueFalse(app.IsAppOrSpouseStudent), saveAppearance);
                                if (app.StudentTypeId != null && app.IsAppOrSpouseStudent.Value)
                                {
                                    pdfFormFields.SetField("StudentType",
                                                           Lists.GetStudentTypeList(false, false)
                                                                .Find(x => x.Value.Equals(app.StudentTypeId.Value.ToString()))
                                                                .Value, saveAppearance);
                                }
                            }
                            pdfFormFields.SetField("IsAppTravelFrequent", IsTrueFalse(app.IsAppTravelFrequent), saveAppearance);
                            pdfFormFields.SetField("AppTravelFrequency", app.AppTravelFrequency);
                            pdfFormFields.SetField("WhatIfTravelPetPlacement", app.WhatIfTravelPetPlacement);
                            pdfFormFields.SetField("ResidenceNumberOccupants", app.ResidenceNumberOccupants);
                            pdfFormFields.SetField("ResidenceAgesOfChildren", app.ResidenceAgesOfChildren);
                            pdfFormFields.SetField("ResidenceIsYardFenced", IsTrueFalse(app.ResidenceIsYardFenced), saveAppearance);
                            pdfFormFields.SetField("ResidenceFenceType", app.ResidenceFenceType + " " + app.ResidenceFenceHeight);

                            pdfFormFields.SetField("IsPetKeptLocationInOutDoorsTotallyInside", IsYesNo(app.IsPetKeptLocationInOutDoorsTotallyInside), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationInOutDoorsMostlyInside", IsYesNo(app.IsPetKeptLocationInOutDoorsMostlyInside), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationInOutDoorsTotallyOutside", IsYesNo(app.IsPetKeptLocationInOutDoorsTotallyOutside), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationInOutDoorMostlyOutsides", IsYesNo(app.IsPetKeptLocationInOutDoorMostlyOutsides), saveAppearance);
                            pdfFormFields.SetField("PetKeptLocationInOutDoorsExplain", app.PetKeptLocationInOutDoorsExplain);

                            pdfFormFields.SetField("PetKeptAloneHoursPerDay", app.PetLeftAloneHours);
                            pdfFormFields.SetField("PetKeptAloneNumberDays", app.PetLeftAloneDays);

                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionLooseIndoors", IsYesNo(app.IsPetKeptLocationAloneRestrictionLooseIndoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionGarage", IsYesNo(app.IsPetKeptLocationAloneRestrictionGarage), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionOutsideKennel", IsYesNo(app.IsPetKeptLocationAloneRestrictionOutsideKennel), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionCratedIndoors", IsYesNo(app.IsPetKeptLocationAloneRestrictionCratedIndoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionLooseInBackyard", IsYesNo(app.IsPetKeptLocationAloneRestrictionLooseInBackyard), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionTiedUpOutdoors", IsYesNo(app.IsPetKeptLocationAloneRestrictionTiedUpOutdoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionBasement", IsYesNo(app.IsPetKeptLocationAloneRestrictionBasement), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionCratedOutdoors", IsYesNo(app.IsPetKeptLocationAloneRestrictionCratedOutdoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionInBedOwner", IsYesNo(app.IsPetKeptLocationSleepingRestrictionInBedOwner), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationAloneRestrictionOther", IsYesNo(app.IsPetKeptLocationAloneRestrictionOther), saveAppearance);
                            pdfFormFields.SetField("PetKeptLocationAloneRestrictionExplain", app.PetKeptLocationAloneRestrictionExplain);

                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionLooseIndoors", IsYesNo(app.IsPetKeptLocationSleepingRestrictionLooseIndoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionGarage", IsYesNo(app.IsPetKeptLocationSleepingRestrictionGarage), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionOutsideKennel", IsYesNo(app.IsPetKeptLocationSleepingRestrictionOutsideKennel), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionCratedIndoors", IsYesNo(app.IsPetKeptLocationSleepingRestrictionCratedIndoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionLooseInBackyard", IsYesNo(app.IsPetKeptLocationSleepingRestrictionLooseInBackyard), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionTiedUpOutdoors", IsYesNo(app.IsPetKeptLocationSleepingRestrictionTiedUpOutdoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionBasement", IsYesNo(app.IsPetKeptLocationSleepingRestrictionBasement), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionCratedOutdoors", IsYesNo(app.IsPetKeptLocationSleepingRestrictionCratedOutdoors), saveAppearance);
                            pdfFormFields.SetField("IsPetKeptLocationSleepingRestrictionOther", IsYesNo(app.IsPetKeptLocationSleepingRestrictionOther), saveAppearance);
                            pdfFormFields.SetField("PetKeptLocationSleepingRestrictionExplain", app.PetKeptLocationSleepingRestrictionExplain);

                            //if (appType.Equals("A"))
                            //{
                            pdfFormFields.SetField("IsPetAdoptionReasonHousePet", IsYesNo(app.IsPetAdoptionReasonHousePet), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonGuardDog", IsYesNo(app.IsPetAdoptionReasonGuardDog), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonWatchDog", IsYesNo(app.IsPetAdoptionReasonWatchDog), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonGift", IsYesNo(app.IsPetAdoptionReasonGift), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonCompanionChild", IsYesNo(app.IsPetAdoptionReasonCompanionChild), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonCompanionPet", IsYesNo(app.IsPetAdoptionReasonCompanionPet), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonJoggingPartner", IsYesNo(app.IsPetAdoptionReasonJoggingPartner), saveAppearance);
                            pdfFormFields.SetField("IsPetAdoptionReasonOther", IsYesNo(app.IsPetAdoptionReasonOther), saveAppearance);
                            pdfFormFields.SetField("PetAdoptionReasonExplain", app.PetAdoptionReasonExplain);
                            //}

                            pdfFormFields.SetField("FilterAppHasOwnedHuskyBefore", IsTrueFalse(app.FilterAppHasOwnedHuskyBefore), saveAppearance);
                            pdfFormFields.SetField("FilterAppIsAwareHuskyAttributes", IsTrueFalse(app.FilterAppIsAwareHuskyAttributes), saveAppearance);

                            pdfFormFields.SetField("FilterAppTraitsDesired", app.FilterAppTraitsDesired);

                            pdfFormFields.SetField("FilterAppIsCatOwner", IsTrueFalse(app.FilterAppIsCatOwner), saveAppearance);
                            pdfFormFields.SetField("FilterAppCatsOwnedCount", app.FilterAppCatsOwnedCount);

                            pdfFormFields.SetField("FilterAppDogsInterestedIn", app.FilterAppDogsInterestedIn);

                            pdfFormFields.SetField("Veterinarian.NameOffice", app.NameOffice);
                            pdfFormFields.SetField("Veterinarian.NameDr", app.NameDr);
                            pdfFormFields.SetField("Veterinarian.PhoneNumber", app.PhoneNumber);

                            if (!string.IsNullOrEmpty(app.Name1))
                            {
                                pdfFormFields.SetField("AdopterAnimal.Name1", app.Name1);
                                pdfFormFields.SetField("AdopterAnimal.Breed1", app.Breed1);
                                pdfFormFields.SetField("AdopterAnimal.Gender1", Lists.GetSexTypeList(false).Find(x => x.Value.Equals(app.Gender1)).Value);
                                pdfFormFields.SetField("AdopterAnimal.Age1", app.AgeMonths1);
                                pdfFormFields.SetField("AdopterAnimal.OwnershipLengthMonths1", app.OwnershipLengthMonths1);
                                pdfFormFields.SetField("AdopterAnimal.IsAltered1", IsTrueFalse(app.IsAltered1), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.AlteredReason1", app.AlteredReason1);
                                pdfFormFields.SetField("AdopterAnimal.IsHwPrevention1", IsTrueFalse(app.IsHwPrevention1), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.HwPreventionReason1", app.HwPreventionReason1);
                                pdfFormFields.SetField("AdopterAnimal.IsFullyVaccinated1", IsTrueFalse(app.IsFullyVaccinated1), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.FullyVaccinatedReason1", app.FullyVaccinatedReason1);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwned1", IsTrueFalse(app.IsStillOwned1), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwnedReason1", app.IsStillOwnedReason1);
                            }
                            if (!string.IsNullOrEmpty(app.Name2))
                            {
                                pdfFormFields.SetField("AdopterAnimal.Name2", app.Name2);
                                pdfFormFields.SetField("AdopterAnimal.Breed2", app.Breed2);
                                pdfFormFields.SetField("AdopterAnimal.Gender2", Lists.GetSexTypeList(false).Find(x => x.Value.Equals(app.Gender2)).Value);
                                pdfFormFields.SetField("AdopterAnimal.Age2", app.AgeMonths2);
                                pdfFormFields.SetField("AdopterAnimal.OwnershipLengthMonths2", app.OwnershipLengthMonths2);
                                pdfFormFields.SetField("AdopterAnimal.IsAltered2", IsTrueFalse(app.IsAltered2), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.AlteredReason2", app.AlteredReason2);
                                pdfFormFields.SetField("AdopterAnimal.IsHwPrevention2", IsTrueFalse(app.IsHwPrevention2), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.HwPreventionReason2", app.HwPreventionReason2);
                                pdfFormFields.SetField("AdopterAnimal.IsFullyVaccinated2", IsTrueFalse(app.IsFullyVaccinated2), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.FullyVaccinatedReason2", app.FullyVaccinatedReason2);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwned2", IsTrueFalse(app.IsStillOwned2), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwnedReason2", app.IsStillOwnedReason2);
                            }
                            if (!string.IsNullOrEmpty(app.Name3))
                            {
                                pdfFormFields.SetField("AdopterAnimal.Name3", app.Name3);
                                pdfFormFields.SetField("AdopterAnimal.Breed3", app.Breed3);
                                pdfFormFields.SetField("AdopterAnimal.Gender3", Lists.GetSexTypeList(false).Find(x => x.Value.Equals(app.Gender3)).Value);
                                pdfFormFields.SetField("AdopterAnimal.Age3", app.AgeMonths3);
                                pdfFormFields.SetField("AdopterAnimal.OwnershipLengthMonths3", app.OwnershipLengthMonths3);
                                pdfFormFields.SetField("AdopterAnimal.IsAltered3", IsTrueFalse(app.IsAltered3), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.AlteredReason3", app.AlteredReason3);
                                pdfFormFields.SetField("AdopterAnimal.IsHwPrevention3", IsTrueFalse(app.IsHwPrevention3), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.HwPreventionReason3", app.HwPreventionReason3);
                                pdfFormFields.SetField("AdopterAnimal.IsFullyVaccinated3", IsTrueFalse(app.IsFullyVaccinated3), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.FullyVaccinatedReason3", app.FullyVaccinatedReason3);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwned3", IsTrueFalse(app.IsStillOwned3), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwnedReason3", app.IsStillOwnedReason3);
                            }
                            if (!string.IsNullOrEmpty(app.Name4))
                            {
                                pdfFormFields.SetField("AdopterAnimal.Name4", app.Name4);
                                pdfFormFields.SetField("AdopterAnimal.Breed4", app.Breed4);
                                pdfFormFields.SetField("AdopterAnimal.Gender4", Lists.GetSexTypeList(false).Find(x => x.Value.Equals(app.Gender4)).Value);
                                pdfFormFields.SetField("AdopterAnimal.Age4", app.AgeMonths4);
                                pdfFormFields.SetField("AdopterAnimal.OwnershipLengthMonths4", app.OwnershipLengthMonths4);
                                pdfFormFields.SetField("AdopterAnimal.IsAltered4", IsTrueFalse(app.IsAltered4), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.AlteredReason4", app.AlteredReason4);
                                pdfFormFields.SetField("AdopterAnimal.IsHwPrevention4", IsTrueFalse(app.IsHwPrevention4), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.HwPreventionReason4", app.HwPreventionReason4);
                                pdfFormFields.SetField("AdopterAnimal.IsFullyVaccinated4", IsTrueFalse(app.IsFullyVaccinated4), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.FullyVaccinatedReason4", app.FullyVaccinatedReason4);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwned4", IsTrueFalse(app.IsStillOwned4), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwnedReason4", app.IsStillOwnedReason4);
                            }
                            if (!string.IsNullOrEmpty(app.Name5))
                            {
                                pdfFormFields.SetField("AdopterAnimal.Name5", app.Name5);
                                pdfFormFields.SetField("AdopterAnimal.Breed5", app.Breed5);
                                pdfFormFields.SetField("AdopterAnimal.Gender5", Lists.GetSexTypeList(false).Find(x => x.Value.Equals(app.Gender5)).Value);
                                pdfFormFields.SetField("AdopterAnimal.Age5", app.AgeMonths5);
                                pdfFormFields.SetField("AdopterAnimal.OwnershipLengthMonths5", app.OwnershipLengthMonths5);
                                pdfFormFields.SetField("AdopterAnimal.IsAltered5", IsTrueFalse(app.IsAltered5), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.AlteredReason5", app.AlteredReason5);
                                pdfFormFields.SetField("AdopterAnimal.IsHwPrevention5", IsTrueFalse(app.IsHwPrevention5), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.HwPreventionReason5", app.HwPreventionReason5);
                                pdfFormFields.SetField("AdopterAnimal.IsFullyVaccinated5", IsTrueFalse(app.IsFullyVaccinated5), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.FullyVaccinatedReason5", app.FullyVaccinatedReason5);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwned5", IsTrueFalse(app.IsStillOwned5), saveAppearance);
                                pdfFormFields.SetField("AdopterAnimal.IsStillOwnedReason5", app.IsStillOwnedReason5);
                            }
                        }
                        catch (DocumentException docEx)
                        {
                            // handle pdf document exception if any
                            _logger.Error(docEx, "ApplicantGen - DocumentException {exception_message}", docEx.Message);
                        }
                        catch (IOException ioEx)
                        {
                            // handle IO exception
                            _logger.Error(ioEx, "ApplicantGen - IOException {exception_message}", ioEx.Message);
                        }
                        catch (Exception ex)
                        {
                            // handle other exception
                            _logger.Error(ex, "ApplicantGen - GeneralException {exception_message}", ex.Message);
                        }
                        finally
                        {
                            pdfStamper.FormFlattening = true;
                        }
                    }
                    pdfReader.Close();
                }
            }
            catch (ArgumentNullException ex)
            {
                _logger.Error(ex, "App PDF Gen Error - ArgumentNullException");
            }
            catch (ArgumentException ex)
            {
                _logger.Error(ex, "App PDF Gen Error - ArgumentException");
            }
            catch (SecurityException ex)
            {
                _logger.Error(ex, "App PDF Gen Error - SecurityException");
            }
            catch (FileNotFoundException ex)
            {
                _logger.Error(ex, "App PDF Gen Error - FileNotFoundException");
            }
            catch (DirectoryNotFoundException ex)
            {
                _logger.Error(ex, "App PDF Gen Error - DirectoryNotFoundException");
            }
            catch (IOException ex)
            {
                _logger.Error(ex, "App PDF Gen Error - IOException");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "App PDF Gen Error");
            }

            return pdfNewFilePath;
        }

        /// <summary>
        /// return string literal "true" or "false" from object true or false
        /// </summary>
        /// <param name="value">Boolean true or false</param>
        /// <returns>string "true" or "false"</returns>
        private string IsTrueFalse(bool? value)
        {
            if (value.HasValue)
            {
                return value == true ? "true" : "false";
            }
            return "false";
        }

        /// <summary>
        /// return string literal "yes" or "no" from object true or false
        /// </summary>
        /// <param name="value">Boolean true or false</param>
        /// <returns>string "yes" or "no"</returns>
        private string IsYesNo(bool? value)
        {
            if (value.HasValue)
            {
                return value == true ? "Yes" : "No";
            }
            return "No";
        }
    }
}
