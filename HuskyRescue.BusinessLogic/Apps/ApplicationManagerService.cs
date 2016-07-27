using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Net.Mail;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Adoption;
using Serilog;

namespace HuskyRescue.BusinessLogic
{
    public class ApplicationManagerService : IApplicationManagerService
    {
        private static ILogger _logger;
        private static IFormSerivce _formService;
        private readonly IBraintreePaymentService _braintreePaymentService;
        public ApplicationManagerService(ILogger ilogger, IFormSerivce iFormService, IBraintreePaymentService iBraintreePaymentService)
        {
            _logger = ilogger;
            _formService = iFormService;
            _braintreePaymentService = iBraintreePaymentService;
        }

        public async Task<RequestResult> AddAdoptionApplication(Apply app)
        {
            _logger.Information("Saving Adoption Application {@app}", app);

            var result = new RequestResult();

            #region Process payment
            var paymentRequestResult = new RequestResult();

            if (app.PaymentMethod.ToLower().Equals("paypal"))
            {
                paymentRequestResult = _braintreePaymentService.SendPayment(app.ApplicationFeeAmount, app.Nonce, true, PaymentType.PayPal, app.DeviceData,
                    "adoption app fee", app.Comments,
                    app.PayeeFirstName, app.PayeeLastName, app.PayeeEmailAddress);
            }
            else
            {
                var stateCode = string.Empty;
                if (app.PayeeAddressStateId.HasValue)
                {
                    stateCode = ViewModel.Common.Lists.GetStateCode(app.PayeeAddressStateId.Value);
                }

                paymentRequestResult = _braintreePaymentService.SendPayment(app.ApplicationFeeAmount, app.Nonce, true, PaymentType.CreditCard, app.DeviceData,
                    "adoption app fee", app.Comments,
                    app.PayeeFirstName, app.PayeeLastName, app.PayeeAddressStreet1, app.PayeeAddressStreet2,
                    app.PayeeAddressCity, stateCode, app.PayeeAddressPostalCode, app.CountryCodeId);
            }

            if (!paymentRequestResult.Succeeded)
            {
                // TODO: handle failure to pay
                result.Succeeded = false;
                result.Errors.Add("Payment Failure - see below for details: ");
                result.Errors.AddRange(paymentRequestResult.Errors);

                _logger.Error("Adoption App Fee Payment Failed {@AdoptionAppFeePaymentErrors}", result.Errors);

                return result;
            }

            // payment is a success. capture the transaction id from braintree
            app.BraintreeTransactionId = paymentRequestResult.NewKey;

            #endregion

            #region Create DB Applicant Object
            var dbApplicant = new Applicant
            {
                AppAddressStateId = app.AppAddressStateId,
                AppAddressCity = app.AppAddressCity,
                AppAddressStreet1 = app.AppAddressStreet1,
                AppAddressZIP = app.AppAddressZip,
                AppCellPhone = app.AppCellPhone,
                AppDateBirth = app.AppDateBirth,
                AppEmail = app.AppEmail,
                AppEmployer = app.AppEmployer,
                AppHomePhone = app.AppHomePhone,
                AppNameFirst = app.AppNameFirst,
                AppNameLast = app.AppNameLast,
                AppSpouseNameFirst = app.AppSpouseNameFirst,
                AppSpouseNameLast = app.AppSpouseNameLast,
                AppTravelFrequency = app.AppTravelFrequency,
                ApplicantTypeId = 1,
                FilterAppCatsOwnedCount = app.FilterAppCatsOwnedCount,
                FilterAppDogsInterestedIn = app.FilterAppDogsInterestedIn,
                FilterAppHasOwnedHuskyBefore = app.FilterAppHasOwnedHuskyBefore != null && app.FilterAppHasOwnedHuskyBefore.Value,
                FilterAppIsAwareHuskyAttributes = app.FilterAppIsAwareHuskyAttributes != null && app.FilterAppIsAwareHuskyAttributes.Value,
                FilterAppIsCatOwner = app.FilterAppIsCatOwner != null && app.FilterAppIsCatOwner.Value,
                FilterAppTraitsDesired = app.FilterAppTraitsDesired,
                DateSubmitted = DateTime.Today,
                IsAllAdultsAgreedOnAdoption = app.IsAllAdultsAgreedOnAdoption != null && app.IsAllAdultsAgreedOnAdoption.Value,
                IsAllAdultsAgreedOnAdoptionReason = app.IsAllAdultsAgreedOnAdoptionReason,
                IsAppOrSpouseStudent = app.IsAppOrSpouseStudent != null && app.IsAppOrSpouseStudent.Value,
                IsAppTravelFrequent = app.IsAppTravelFrequent != null && app.IsAppTravelFrequent.Value,
                IsPetAdoptionReasonCompanionChild = app.IsPetAdoptionReasonCompanionChild,
                IsPetAdoptionReasonCompanionPet = app.IsPetAdoptionReasonCompanionPet,
                IsPetAdoptionReasonGift = app.IsPetAdoptionReasonGift,
                IsPetAdoptionReasonGuardDog = app.IsPetAdoptionReasonGuardDog,
                IsPetAdoptionReasonHousePet = app.IsPetAdoptionReasonHousePet,
                IsPetAdoptionReasonJoggingPartner = app.IsPetAdoptionReasonJoggingPartner,
                IsPetAdoptionReasonOther = app.IsPetAdoptionReasonOther,
                IsPetAdoptionReasonWatchDog = app.IsPetAdoptionReasonWatchDog,
                IsPetKeptLocationAloneRestrictionBasement = app.IsPetKeptLocationAloneRestrictionBasement,
                IsPetKeptLocationAloneRestrictionCratedIndoors = app.IsPetKeptLocationAloneRestrictionCratedIndoors,
                IsPetKeptLocationAloneRestrictionCratedOutdoors = app.IsPetKeptLocationAloneRestrictionCratedOutdoors,
                IsPetKeptLocationAloneRestrictionGarage = app.IsPetKeptLocationAloneRestrictionGarage,
                IsPetKeptLocationAloneRestrictionLooseInBackyard = app.IsPetKeptLocationAloneRestrictionLooseInBackyard,
                IsPetKeptLocationAloneRestrictionLooseIndoors = app.IsPetKeptLocationAloneRestrictionLooseIndoors,
                IsPetKeptLocationAloneRestrictionOther = app.IsPetKeptLocationAloneRestrictionOther,
                IsPetKeptLocationAloneRestrictionOutsIdeKennel = app.IsPetKeptLocationAloneRestrictionOutsideKennel,
                IsPetKeptLocationAloneRestrictionTiedUpOutdoors = app.IsPetKeptLocationAloneRestrictionTiedUpOutdoors,
                IsPetKeptLocationInOutDoorMostlyOutsIdes = app.IsPetKeptLocationInOutDoorMostlyOutsides,
                IsPetKeptLocationInOutDoorsMostlyInsIde = app.IsPetKeptLocationInOutDoorsMostlyInside,
                IsPetKeptLocationInOutDoorsTotallyInsIde = app.IsPetKeptLocationInOutDoorsTotallyInside,
                IsPetKeptLocationInOutDoorsTotallyOutsIde = app.IsPetKeptLocationInOutDoorsTotallyOutside,
                IsPetKeptLocationSleepingRestrictionBasement = app.IsPetKeptLocationSleepingRestrictionBasement,
                IsPetKeptLocationSleepingRestrictionCratedIndoors = app.IsPetKeptLocationSleepingRestrictionCratedIndoors,
                IsPetKeptLocationSleepingRestrictionCratedOutdoors = app.IsPetKeptLocationSleepingRestrictionCratedOutdoors,
                IsPetKeptLocationSleepingRestrictionGarage = app.IsPetKeptLocationSleepingRestrictionGarage,
                IsPetKeptLocationSleepingRestrictionInBedOwner = app.IsPetKeptLocationSleepingRestrictionInBedOwner,
                IsPetKeptLocationSleepingRestrictionLooseInBackyard = app.IsPetKeptLocationSleepingRestrictionLooseInBackyard,
                IsPetKeptLocationSleepingRestrictionLooseIndoors = app.IsPetKeptLocationSleepingRestrictionLooseIndoors,
                IsPetKeptLocationSleepingRestrictionOther = app.IsPetKeptLocationSleepingRestrictionOther,
                IsPetKeptLocationSleepingRestrictionOutsIdeKennel = app.IsPetKeptLocationSleepingRestrictionOutsideKennel,
                IsPetKeptLocationSleepingRestrictionTiedUpOutdoors = app.IsPetKeptLocationSleepingRestrictionTiedUpOutdoors,
                ResIdenceIsYardFenced = app.ResidenceIsYardFenced != null && app.ResidenceIsYardFenced.Value,
                ResidenceAgesOfChildren = app.ResidenceAgesOfChildren,
                ResidenceFenceHeight = app.ResidenceFenceHeight,
                ResidenceIsPetAllowed = app.ResidenceIsPetAllowed,
                ResidenceIsPetDepositPaid = app.ResidenceIsPetDepositPaid,
                ResidenceIsPetDepositRequired = app.ResidenceIsPetDepositRequired,
                ResidenceLandlordName = app.ResidenceLandlordName,
                ResidenceLandlordNumber = app.ResidenceLandlordNumber,
                ResidenceLengthOfResidence = app.ResidenceLengthOfResidence,
                ResidenceNumberOccupants = app.ResidenceNumberOccupants,
                ResidenceOwnershipId = app.ResidenceOwnershipId,
                ResidencePetDepositAmount = app.ResidencePetDepositAmount,
                ResidenceFenceType = app.ResidenceFenceType,
                ResidencePetDepositCoverageId = app.ResidencePetDepositCoverageId,
                ResidencePetSizeWeightLimit = app.ResidencePetSizeWeightLimit,
                ResidenceTypeId = app.ResidenceTypeId,
                PetLeftAloneDays = app.PetLeftAloneDays,
                PetLeftAloneHours = app.PetLeftAloneHours,
                StudentTypeId = app.StudentTypeId,
                PetAdoptionReasonExplain = app.PetAdoptionReasonExplain,
                PetAdoptionReason = app.PetAdoptionReason,
                PetKeptLocationAloneRestriction = app.PetKeptLocationAloneRestriction,
                PetKeptLocationAloneRestrictionExplain = app.PetKeptLocationAloneRestrictionExplain,
                PetKeptLocationInOutDoors = app.PetKeptLocationInOutDoors,
                PetKeptLocationInOutDoorsExplain = app.PetKeptLocationInOutDoorsExplain,
                PetKeptLocationSleepingRestriction = app.PetKeptLocationSleepingRestriction,
                PetKeptLocationSleepingRestrictionExplain = app.PetKeptLocationSleepingRestrictionExplain,
                WhatIfMovingPetPlacement = app.WhatIfMovingPetPlacement,
                WhatIfTravelPetPlacement = app.WhatIfTravelPetPlacement,
                ApplicationFeeAmount = app.ApplicationFeeAmount,
                ApplicationFeePaymentMethod = app.PaymentMethod,
                ApplicationFeeTransactionId = app.BraintreeTransactionId,
                Comments = app.Comments
            };
            if (!string.IsNullOrEmpty(app.NameDr))
            {
                dbApplicant.ApplicantVeterinarian = new ApplicantVeterinarian
                {
                    NameDr = app.NameDr,
                    NameOffice = app.NameOffice,
                    PhoneNumber = app.PhoneNumber
                };
            }
            dbApplicant.ApplicantOwnedAnimals = new Collection<ApplicantOwnedAnimal>();

            if (!string.IsNullOrEmpty(app.Name1))
            {
                dbApplicant.ApplicantOwnedAnimals.Add(new ApplicantOwnedAnimal
                {
                    AgeMonths = app.AgeMonths1,
                    Breed = app.Breed1,
                    Gender = app.Gender1,
                    OwnershipLengthMonths = app.OwnershipLengthMonths1,
                    Name = app.Name1,
                    IsAltered = app.IsAltered1 != null && app.IsAltered1.Value,
                    AlteredReason = app.AlteredReason1,
                    IsFullyVaccinated = app.IsFullyVaccinated1 != null && app.IsFullyVaccinated1.Value,
                    FullyVaccinatedReason = app.FullyVaccinatedReason1,
                    IsHwPrevention = app.IsHwPrevention1 != null && app.IsHwPrevention1.Value,
                    HwPreventionReason = app.HwPreventionReason1,
                    IsStillOwned = app.IsStillOwned1 != null && app.IsStillOwned1.Value,
                    IsStillOwnedReason = app.IsStillOwnedReason1
                });
            }
            if (!string.IsNullOrEmpty(app.Name2))
            {
                dbApplicant.ApplicantOwnedAnimals.Add(new ApplicantOwnedAnimal
                {
                    AgeMonths = app.AgeMonths2,
                    Breed = app.Breed2,
                    Gender = app.Gender2,
                    OwnershipLengthMonths = app.OwnershipLengthMonths2,
                    Name = app.Name2,
                    IsAltered = app.IsAltered2 != null && app.IsAltered2.Value,
                    AlteredReason = app.AlteredReason2,
                    IsFullyVaccinated = app.IsFullyVaccinated2 != null && app.IsFullyVaccinated2.Value,
                    FullyVaccinatedReason = app.FullyVaccinatedReason2,
                    IsHwPrevention = app.IsHwPrevention2 != null && app.IsHwPrevention2.Value,
                    HwPreventionReason = app.HwPreventionReason2,
                    IsStillOwned = app.IsStillOwned2 != null && app.IsStillOwned2.Value,
                    IsStillOwnedReason = app.IsStillOwnedReason2
                });
            }
            if (!string.IsNullOrEmpty(app.Name3))
            {
                dbApplicant.ApplicantOwnedAnimals.Add(new ApplicantOwnedAnimal
                {
                    AgeMonths = app.AgeMonths3,
                    Breed = app.Breed3,
                    Gender = app.Gender3,
                    OwnershipLengthMonths = app.OwnershipLengthMonths3,
                    Name = app.Name3,
                    IsAltered = app.IsAltered3 != null && app.IsAltered3.Value,
                    AlteredReason = app.AlteredReason3,
                    IsFullyVaccinated = app.IsFullyVaccinated3 != null && app.IsFullyVaccinated3.Value,
                    FullyVaccinatedReason = app.FullyVaccinatedReason3,
                    IsHwPrevention = app.IsHwPrevention3 != null && app.IsHwPrevention3.Value,
                    HwPreventionReason = app.HwPreventionReason3,
                    IsStillOwned = app.IsStillOwned3 != null && app.IsStillOwned3.Value,
                    IsStillOwnedReason = app.IsStillOwnedReason3
                });
            }
            if (!string.IsNullOrEmpty(app.Name4))
            {
                dbApplicant.ApplicantOwnedAnimals.Add(new ApplicantOwnedAnimal
                {
                    AgeMonths = app.AgeMonths4,
                    Breed = app.Breed4,
                    Gender = app.Gender4,
                    OwnershipLengthMonths = app.OwnershipLengthMonths4,
                    Name = app.Name4,
                    IsAltered = app.IsAltered4 != null && app.IsAltered4.Value,
                    AlteredReason = app.AlteredReason4,
                    IsFullyVaccinated = app.IsFullyVaccinated4 != null && app.IsFullyVaccinated4.Value,
                    FullyVaccinatedReason = app.FullyVaccinatedReason4,
                    IsHwPrevention = app.IsHwPrevention4 != null && app.IsHwPrevention4.Value,
                    HwPreventionReason = app.HwPreventionReason4,
                    IsStillOwned = app.IsStillOwned4 != null && app.IsStillOwned4.Value,
                    IsStillOwnedReason = app.IsStillOwnedReason4
                });
            }
            if (!string.IsNullOrEmpty(app.Name5))
            {
                dbApplicant.ApplicantOwnedAnimals.Add(new ApplicantOwnedAnimal
                {
                    AgeMonths = app.AgeMonths5,
                    Breed = app.Breed5,
                    Gender = app.Gender5,
                    OwnershipLengthMonths = app.OwnershipLengthMonths5,
                    Name = app.Name5,
                    IsAltered = app.IsAltered5 != null && app.IsAltered5.Value,
                    AlteredReason = app.AlteredReason5,
                    IsFullyVaccinated = app.IsFullyVaccinated5 != null && app.IsFullyVaccinated5.Value,
                    FullyVaccinatedReason = app.FullyVaccinatedReason5,
                    IsHwPrevention = app.IsHwPrevention5 != null && app.IsHwPrevention5.Value,
                    HwPreventionReason = app.HwPreventionReason5,
                    IsStillOwned = app.IsStillOwned5 != null && app.IsStillOwned5.Value,
                    IsStillOwnedReason = app.IsStillOwnedReason5
                });
            }
            #endregion

            #region Create DB Person Object
            var dbPerson = new Person
            {
                AddedOnDate = DateTime.Today,
                //AddedByUserId = 
                DateActive = DateTime.Today,
                FirstName = app.AppNameFirst,
                LastName = app.AppNameLast,
                IsActive = true,
                IsAdopter = true
            };

            dbPerson.Addresses.Add(new Address
            {
                Address1 = app.AppAddressStreet1,
                AddressStateId = app.AppAddressStateId,
                AddressTypeId = 1,
                City = app.AppAddressCity,
                IsShippingAddress = true,
                ZipCode = app.AppAddressZip
            });

            if (!string.IsNullOrEmpty(app.AppEmail))
            {
                dbPerson.EmailAddresses = new Collection<EmailAddress>
                                        {
                                            new EmailAddress
                                            {
                                                Address = app.AppEmail,
                                                EmailAddressTypeId = 0
                                            }
                                        };
            }

            if (!string.IsNullOrEmpty(app.AppHomePhone) || !string.IsNullOrEmpty(app.AppCellPhone))
            {
                dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
                if (!string.IsNullOrEmpty(app.AppHomePhone))
                {
                    dbPerson.PhoneNumbers.Add(new PhoneNumber
                    {
                        PhoneNumber1 = app.AppHomePhone,
                        PhoneNumberTypeId = 1
                    });
                }
                if (!string.IsNullOrEmpty(app.AppCellPhone))
                {
                    dbPerson.PhoneNumbers.Add(new PhoneNumber
                    {
                        PhoneNumber1 = app.AppCellPhone,
                        PhoneNumberTypeId = 1
                    });
                }
            }
            #endregion

            #region save to database

            try
            {
                _logger.Information("Saving application to database: {@dbPerson}", dbPerson);
                using (var context = new HuskyRescueEntities())
                {
                    dbPerson = context.People.Add(dbPerson);

                    context.SaveChanges();
                }
            }
            catch (DbEntityValidationException ex)
            {
                _logger.Error(ex, "Database Validation Exception saving Applicant");
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Database Update Exception saving Applicant");
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Invalid Operation Exception saving Applicant");
            }

            dbApplicant.PersonId = dbPerson.Id;
            foreach (var dbAnimal in dbApplicant.ApplicantOwnedAnimals)
            {
                dbAnimal.PersonId = dbPerson.Id;
            }

            try
            {
                _logger.Information("Saving adoption application to database: {@dbApplicant}", dbApplicant);
                using (var context = new HuskyRescueEntities())
                {
                    context.Applicants.Add(dbApplicant);

                    context.SaveChanges();

                    app.Id = dbApplicant.Id;
                }
            }
            catch (DbEntityValidationException ex)
            {
                _logger.Error(ex, "Database Validation Exception saving AdoptionApplication");
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Database Update Exception saving AdoptionApplication");
            }
            catch (InvalidOperationException ex)
            {
                _logger.Error(ex, "Invalid Operation Exception saving AdoptionApplication");
            }
            #endregion

            #region generate application PDF
            var generatedPdfPath = _formService.CreateAdoptionApplicationPdf(app);
            #endregion

            #region send emails
            var subject = "Adoption Application for " + app.AppFullName;
            var bodyText = string.Format(@" Thank you for your application.
                                Attached is a copy of your application sent to Texas Husky Rescue, Inc.
                                You can respond back to this email if you have any further questions or comments for us.
                                We will get back to you within the next 7 days regarding your application.
                                You're ${0} application fee will be applied towards your adoption fee if you are approved.
                                The confirmation number for your application fee is {1}", app.ApplicationFeeAmount, app.BraintreeTransactionId);
            var attachments = new List<Attachment> { new Attachment(generatedPdfPath) };
            //var emailService = new EmailService(EmailService.EmailFrom.Contact, app.AppEmail, subject, bodyText, bodyText, attachments);
            var emailService = new EmailService(EmailService.FromContactMailAddress, new MailAddress(app.AppEmail, app.AppFullName), subject, bodyText, false, attachments);
            emailService.Tag = "adoption-app";
            await emailService.SendAsync();

            var groupEmail = AdminSystemSettings.GetSetting("Email-Contact");

            bodyText = "Dogs interested in: " + app.FilterAppDogsInterestedIn;
            attachments = new List<Attachment> { new Attachment(generatedPdfPath) };
            //emailService = new EmailService(EmailService.EmailFrom.Admin, groupEmail, subject, bodyText, bodyText, attachments);
            emailService = new EmailService(new MailAddress(app.AppEmail, app.AppFullName), EmailService.FromContactMailAddress, subject, bodyText, false, attachments);
            emailService.Tag = "adoption-app";
            await emailService.SendAsync();
            #endregion

            result.Succeeded = true;

            return result;
        }
    }
}
