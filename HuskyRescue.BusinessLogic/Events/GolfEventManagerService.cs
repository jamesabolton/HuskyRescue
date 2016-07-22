using System;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Golf.Register;
using Serilog;
using System.Collections.ObjectModel;

namespace HuskyRescue.BusinessLogic
{
	public class GolfEventManagerService : IGolfEventManagerService
	{
		private readonly ILogger _logger;

		private readonly IBraintreePaymentService _braintreePaymentService;

		private readonly IAdminSystemSettingsManagerService _adminSystemSettingsManagerService;

		public GolfEventManagerService(ILogger iLogger, IBraintreePaymentService iBraintreePaymentService, IAdminSystemSettingsManagerService iAdminSystemSettingsManagerService)
		{
			_logger = iLogger;
			_braintreePaymentService = iBraintreePaymentService;
			_adminSystemSettingsManagerService = iAdminSystemSettingsManagerService;
		}

		public async Task<RequestResult> RegisterForTournamentPublic(CreatePublic register)
		{
			var requestResult = new RequestResult();


			var activeGolfEventId = (await _adminSystemSettingsManagerService.GetSettingDetailAsync("GolfTournamentId")).Value;

			// Log request
			_logger.Information("Golf Registration {@golfregister}", register);

			#region Process payment
			var paymentRequestResult = new RequestResult();

			if (register.PaymentMethod.ToLower().Equals("paypal"))
			{
				paymentRequestResult = _braintreePaymentService.SendPayment(register.AmountDue, register.Nonce, true, PaymentType.PayPal, register.DeviceData,
					"golf registration", register.CustomerNotes,
					register.PayeeFirstName, register.PayeeLastName, register.PayeeEmailAddress);
			}
			else
			{

				var stateCode = string.Empty;
				if (register.PayeeAddressStateId.HasValue)
				{
					stateCode = ViewModel.Common.Lists.GetStateCode(register.PayeeAddressStateId.Value);
				}

				paymentRequestResult = _braintreePaymentService.SendPayment(register.AmountDue, register.Nonce, true, PaymentType.CreditCard, register.DeviceData,
					"golf registration", register.CustomerNotes,
					register.PayeeFirstName, register.PayeeLastName, register.PayeeAddressStreet1, register.PayeeAddressStreet2,
					register.PayeeAddressCity, stateCode, register.PayeeAddressPostalCode, register.CountryCodeId);
			}

			if (!paymentRequestResult.Succeeded)
			{
				// TODO: handle failure to pay
				requestResult.Succeeded = false;
				requestResult.Errors.Add("Payment Failure - see below for details: ");
				requestResult.Errors.AddRange(paymentRequestResult.Errors);

				_logger.Error("Golf Registration Payment Failed {@GolfTournamentPaymentErrors}", requestResult.Errors);

				return requestResult;
			}
			#endregion

			var numberOfTickets = 0;
			if (!string.IsNullOrEmpty(register.Attendee1FirstName)) { numberOfTickets += 1; }
			if (!string.IsNullOrEmpty(register.Attendee2FirstName)) { numberOfTickets += 1; }
			if (!string.IsNullOrEmpty(register.Attendee3FirstName)) { numberOfTickets += 1; }
			if (!string.IsNullOrEmpty(register.Attendee4FirstName)) { numberOfTickets += 1; }

			#region Save to attendee information to database
			try
			{
				using (var context = new HuskyRescueEntities())
				{

					#region Event Registration

					var dbEvent = context.Events.Single(e => e.Id.Equals(new Guid(activeGolfEventId)));

					var dbEventRegistration = new EventRegistration
					                          {
						                          DateSubmitted = DateTime.Today,
												  EventId = dbEvent.Id,
						                          HasPaid = true,
						                          RegistrationComments = register.CustomerNotes,
						                          NumberTicketsBought = numberOfTickets,
						                          AmountPaid = register.AmountDue
					                          };
					#endregion

					#region Attendee #1
					var dbPerson = new Person
									{
										AddedOnDate = DateTime.Today,
										DateActive = DateTime.Today,
										FirstName = register.Attendee1FirstName,
										LastName = register.Attendee1LastName,
										IsActive = true,
										Notes = "Golfer 2016"
									};
					if (!string.IsNullOrEmpty(register.Attendee1AddressStreet1))
					{
						dbPerson.Addresses.Add(new Address
						{
							Address1 = register.Attendee1AddressStreet1,
							Address2 = register.Attendee1AddressStreet2,
							AddressStateId = register.Attendee1AddressStateId,
							AddressTypeId = 1, // primary
							City = register.Attendee1AddressCity,
							IsBillingAddress = false,
							ZipCode = register.Attendee1AddressPostalCode
						});
					}

					if (!string.IsNullOrEmpty(register.Attendee1EmailAddress))
					{
						dbPerson.EmailAddresses = new Collection<EmailAddress>
												{
													new EmailAddress
													{
														Address = register.Attendee1EmailAddress,
														EmailAddressTypeId = 0
													}
												};
					}

					if (!string.IsNullOrEmpty(register.Attendee1HomePhoneNumber) || !string.IsNullOrEmpty(register.Attendee1MobilePhoneNumber))
					{
						dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
						if (!string.IsNullOrEmpty(register.Attendee1HomePhoneNumber))
						{
							dbPerson.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = register.Attendee1HomePhoneNumber,
								PhoneNumberTypeId = 1
							});
						}
						if (!string.IsNullOrEmpty(register.Attendee1MobilePhoneNumber))
						{
							dbPerson.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = register.Attendee1MobilePhoneNumber,
								PhoneNumberTypeId = 1
							});
						}
					}

					dbEventRegistration.EventRegistrationPersons.Add(new EventRegistrationPerson
					                                                 {
						                                                 IsPrimaryPerson = true,
																		 Person = dbPerson,
																		 AttendeeType = register.Attendee1Type,
																		 TicketPrice = register.Attendee1TicketPrice
					                                                 });
					#endregion

					#region Attendee #2
					if (register.Attendee2IsAttending)
					{
						dbPerson = new Person
						{
							AddedOnDate = DateTime.Today,
							DateActive = DateTime.Today,
							FirstName = register.Attendee2FirstName,
							LastName = register.Attendee2LastName,
							IsActive = true,
							Notes = "Golfer 2016"
						};
						if (!string.IsNullOrEmpty(register.Attendee2AddressStreet1))
						{
							dbPerson.Addresses.Add(new Address
							{
								Address1 = register.Attendee2AddressStreet1,
								Address2 = register.Attendee2AddressStreet2,
								AddressStateId = register.Attendee2AddressStateId,
								AddressTypeId = 1, // primary
								City = register.Attendee2AddressCity,
								IsBillingAddress = false,
								ZipCode = register.Attendee2AddressPostalCode
							});
						}

						if (!string.IsNullOrEmpty(register.Attendee2EmailAddress))
						{
							dbPerson.EmailAddresses = new Collection<EmailAddress>
												{
													new EmailAddress
													{
														Address = register.Attendee2EmailAddress,
														EmailAddressTypeId = 0
													}
												};
						}

						if (!string.IsNullOrEmpty(register.Attendee2HomePhoneNumber) || !string.IsNullOrEmpty(register.Attendee2MobilePhoneNumber))
						{
							dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
							if (!string.IsNullOrEmpty(register.Attendee2HomePhoneNumber))
							{
								dbPerson.PhoneNumbers.Add(new PhoneNumber
								{
									PhoneNumber1 = register.Attendee2HomePhoneNumber,
									PhoneNumberTypeId = 1
								});
							}
							if (!string.IsNullOrEmpty(register.Attendee2MobilePhoneNumber))
							{
								dbPerson.PhoneNumbers.Add(new PhoneNumber
								{
									PhoneNumber1 = register.Attendee2MobilePhoneNumber,
									PhoneNumberTypeId = 1
								});
							}
						}

						dbEventRegistration.EventRegistrationPersons.Add(new EventRegistrationPerson
						{
							IsPrimaryPerson = true,
							Person = dbPerson,
							AttendeeType = register.Attendee2Type,
							TicketPrice = register.Attendee2TicketPrice
						});
					}
					#endregion

					#region Attendee #3
					if (register.Attendee3IsAttending)
					{
						dbPerson = new Person
						{
							AddedOnDate = DateTime.Today,
							DateActive = DateTime.Today,
							FirstName = register.Attendee3FirstName,
							LastName = register.Attendee3LastName,
							IsActive = true,
							Notes = "Golfer 2016"
						};
						if (!string.IsNullOrEmpty(register.Attendee3AddressStreet1))
						{
							dbPerson.Addresses.Add(new Address
							{
								Address1 = register.Attendee3AddressStreet1,
								Address2 = register.Attendee3AddressStreet2,
								AddressStateId = register.Attendee3AddressStateId,
								AddressTypeId = 1, // primary
								City = register.Attendee3AddressCity,
								IsBillingAddress = false,
								ZipCode = register.Attendee3AddressPostalCode
							});
						}

						if (!string.IsNullOrEmpty(register.Attendee3EmailAddress))
						{
							dbPerson.EmailAddresses = new Collection<EmailAddress>
												{
													new EmailAddress
													{
														Address = register.Attendee3EmailAddress,
														EmailAddressTypeId = 0
													}
												};
						}

						if (!string.IsNullOrEmpty(register.Attendee3HomePhoneNumber) || !string.IsNullOrEmpty(register.Attendee3MobilePhoneNumber))
						{
							dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
							if (!string.IsNullOrEmpty(register.Attendee3HomePhoneNumber))
							{
								dbPerson.PhoneNumbers.Add(new PhoneNumber
								{
									PhoneNumber1 = register.Attendee3HomePhoneNumber,
									PhoneNumberTypeId = 1
								});
							}
							if (!string.IsNullOrEmpty(register.Attendee3MobilePhoneNumber))
							{
								dbPerson.PhoneNumbers.Add(new PhoneNumber
								{
									PhoneNumber1 = register.Attendee3MobilePhoneNumber,
									PhoneNumberTypeId = 1
								});
							}
						}

						dbEventRegistration.EventRegistrationPersons.Add(new EventRegistrationPerson
						{
							IsPrimaryPerson = true,
							Person = dbPerson,
							AttendeeType = register.Attendee3Type,
							TicketPrice = register.Attendee3TicketPrice
						});
					}
					#endregion

					#region Attendee #4
					if (register.Attendee4IsAttending)
					{
						dbPerson = new Person
						{
							AddedOnDate = DateTime.Today,
							DateActive = DateTime.Today,
							FirstName = register.Attendee4FirstName,
							LastName = register.Attendee4LastName,
							IsActive = true,
							Notes = "Golfer 2016"
						};
						if (!string.IsNullOrEmpty(register.Attendee4AddressStreet1))
						{
							dbPerson.Addresses.Add(new Address
							{
								Address1 = register.Attendee4AddressStreet1,
								Address2 = register.Attendee4AddressStreet2,
								AddressStateId = register.Attendee4AddressStateId,
								AddressTypeId = 1, // primary
								City = register.Attendee4AddressCity,
								IsBillingAddress = false,
								ZipCode = register.Attendee4AddressPostalCode
							});
						}

						if (!string.IsNullOrEmpty(register.Attendee4EmailAddress))
						{
							dbPerson.EmailAddresses = new Collection<EmailAddress>
												{
													new EmailAddress
													{
														Address = register.Attendee4EmailAddress,
														EmailAddressTypeId = 0
													}
												};
						}

						if (!string.IsNullOrEmpty(register.Attendee4HomePhoneNumber) || !string.IsNullOrEmpty(register.Attendee4MobilePhoneNumber))
						{
							dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
							if (!string.IsNullOrEmpty(register.Attendee4HomePhoneNumber))
							{
								dbPerson.PhoneNumbers.Add(new PhoneNumber
								{
									PhoneNumber1 = register.Attendee4HomePhoneNumber,
									PhoneNumberTypeId = 1
								});
							}
							if (!string.IsNullOrEmpty(register.Attendee4MobilePhoneNumber))
							{
								dbPerson.PhoneNumbers.Add(new PhoneNumber
								{
									PhoneNumber1 = register.Attendee4MobilePhoneNumber,
									PhoneNumberTypeId = 1
								});
							}
						}

						dbEventRegistration.EventRegistrationPersons.Add(new EventRegistrationPerson
						{
							IsPrimaryPerson = true,
							Person = dbPerson,
							AttendeeType = register.Attendee4Type,
							TicketPrice = register.Attendee4TicketPrice
						});
					}
					#endregion

					dbEventRegistration = context.EventRegistrations.Add(dbEventRegistration);

					context.SaveChanges();
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "golf reg create validation error: {@GolfRegItem} {@DbValidationErrors}", register, ex.EntityValidationErrors);
				foreach (var validationError in ex.EntityValidationErrors)
				{
					requestResult.Errors.Add(validationError.ToString());
				}
			}
			#endregion

			#region send emails
			const string subject = "6th Annual Texas Husky Rescue Golf Registration";
			var bodyText = @" 
Thank you for registering for Texas Husky Rescue's 6th Annual Golf Tournament.  We are very excited for this year's event and we have no doubt you will have a fabulous time.

You will be sent an email closer to the tournament with detailed information.  If you have any questions prior to then, please feel free to email us at golf@texashuskyrescue.org.

Thanks again,
Texas Husky Rescue Golf Committee
1-877-TX-HUSKY (894-8759) (phone/fax)
PO Box 118891, Carrollton, TX 75011
";

			EmailService emailService;
			if (register.Attendee1IsAttending)
			{
				emailService = new EmailService(EmailService.FromGolfMailAddress, new MailAddress(register.Attendee1EmailAddress, register.Attendee1FullName), subject, bodyText);
				emailService.Tag = "golf-registration";
				await emailService.SendAsync();
			}
			if (register.Attendee2IsAttending)
			{
				emailService = new EmailService(EmailService.FromGolfMailAddress, new MailAddress(register.Attendee2EmailAddress, register.Attendee2FullName), subject, bodyText);
				emailService.Tag = "golf-registration";
				await emailService.SendAsync();
			}
			if (register.Attendee3IsAttending)
			{
				emailService = new EmailService(EmailService.FromGolfMailAddress, new MailAddress(register.Attendee3EmailAddress, register.Attendee3FullName), subject, bodyText);
				emailService.Tag = "golf-registration";
				await emailService.SendAsync();
			}
			if (register.Attendee4IsAttending)
			{
				emailService = new EmailService(EmailService.FromGolfMailAddress, new MailAddress(register.Attendee4EmailAddress, register.Attendee4FullName), subject, bodyText);
				emailService.Tag = "golf-registration";
				await emailService.SendAsync();
			}

			var groupEmail = AdminSystemSettings.GetSetting("Email-Golf");
			bodyText = "Golf registration for " + numberOfTickets + " attendees."; 
			bodyText += Environment.NewLine;
			bodyText += Environment.NewLine;
			bodyText += "Attendee 1: " + Environment.NewLine;
			bodyText += "Name: " + register.Attendee1FullName + Environment.NewLine;
			bodyText += "Address: " + register.Attendee1FullAddress + Environment.NewLine;
			bodyText += "Home Phone: " + (string.IsNullOrEmpty(register.Attendee1HomePhoneNumber) ? "not provided" : register.Attendee1HomePhoneNumber) + Environment.NewLine;
			bodyText += "Mobile Phone: " + (string.IsNullOrEmpty(register.Attendee1MobilePhoneNumber) ? "not provided" : register.Attendee1MobilePhoneNumber) + Environment.NewLine;
			bodyText += "Email: " + (string.IsNullOrEmpty(register.Attendee1EmailAddress) ? "not provided" : register.Attendee1EmailAddress) + Environment.NewLine;
			bodyText += "Mailable?: " + register.Attendee1FutureContact + Environment.NewLine;
			bodyText += "Attendance Type: " + register.Attendee1Type + Environment.NewLine;
			bodyText += "Ticket Price: " + register.Attendee1TicketPrice + Environment.NewLine;
			bodyText += "-------------------------------------------------------------" + Environment.NewLine;
			if (register.Attendee2IsAttending)
			{
				bodyText += "Attendee 2: " + Environment.NewLine;
				bodyText += "Name: " + register.Attendee2FullName + Environment.NewLine;
				bodyText += "Address: " + register.Attendee2FullAddress + Environment.NewLine;
				bodyText += "Home Phone: " + (string.IsNullOrEmpty(register.Attendee2HomePhoneNumber) ? "not provided" : register.Attendee2HomePhoneNumber) + Environment.NewLine;
				bodyText += "Mobile Phone: " + (string.IsNullOrEmpty(register.Attendee2MobilePhoneNumber) ? "not provided" : register.Attendee2MobilePhoneNumber) + Environment.NewLine;
				bodyText += "Email: " + (string.IsNullOrEmpty(register.Attendee2EmailAddress) ? "not provided" : register.Attendee2EmailAddress) + Environment.NewLine;
				bodyText += "Mailable?: " + register.Attendee2FutureContact + Environment.NewLine;
				bodyText += "Attendance Type: " + register.Attendee2Type + Environment.NewLine;
				bodyText += "Ticket Price: " + register.Attendee2TicketPrice + Environment.NewLine;
				bodyText += "-------------------------------------------------------------" + Environment.NewLine;
			}
			if (register.Attendee3IsAttending)
			{
				bodyText += "Attendee 3: " + Environment.NewLine;
				bodyText += "Name: " + register.Attendee3FullName + Environment.NewLine;
				bodyText += "Address: " + register.Attendee3FullAddress + Environment.NewLine;
				bodyText += "Home Phone: " + (string.IsNullOrEmpty(register.Attendee3HomePhoneNumber) ? "not provided" : register.Attendee3HomePhoneNumber) + Environment.NewLine;
				bodyText += "Mobile Phone: " + (string.IsNullOrEmpty(register.Attendee3MobilePhoneNumber) ? "not provided" : register.Attendee3MobilePhoneNumber) + Environment.NewLine;
				bodyText += "Email: " + (string.IsNullOrEmpty(register.Attendee3EmailAddress) ? "not provided" : register.Attendee3EmailAddress) + Environment.NewLine;
				bodyText += "Mailable?: " + register.Attendee3FutureContact + Environment.NewLine;
				bodyText += "Attendance Type: " + register.Attendee3Type + Environment.NewLine;
				bodyText += "Ticket Price: " + register.Attendee3TicketPrice + Environment.NewLine;
				bodyText += "-------------------------------------------------------------" + Environment.NewLine;
			}
			if (register.Attendee4IsAttending)
			{
				bodyText += "Attendee 4: " + Environment.NewLine;
				bodyText += "Name: " + register.Attendee4FullName + Environment.NewLine;
				bodyText += "Address: " + register.Attendee4FullAddress + Environment.NewLine;
				bodyText += "Home Phone: " + (string.IsNullOrEmpty(register.Attendee4HomePhoneNumber) ? "not provided" : register.Attendee4HomePhoneNumber) + Environment.NewLine;
				bodyText += "Mobile Phone: " + (string.IsNullOrEmpty(register.Attendee4MobilePhoneNumber) ? "not provided" : register.Attendee4MobilePhoneNumber) + Environment.NewLine;
				bodyText += "Email: " + (string.IsNullOrEmpty(register.Attendee4EmailAddress) ? "not provided" : register.Attendee4EmailAddress) + Environment.NewLine;
				bodyText += "Mailable?: " + register.Attendee4FutureContact + Environment.NewLine;
				bodyText += "Attendance Type: " + register.Attendee4Type + Environment.NewLine;
				bodyText += "Ticket Price: " + register.Attendee4TicketPrice + Environment.NewLine;
				bodyText += "-------------------------------------------------------------" + Environment.NewLine;
			}
			bodyText += "Notes from the register: " + register.CustomerNotes + Environment.NewLine;
			bodyText += "-------------------------------------------------------------" + Environment.NewLine;
			bodyText += "Payee: " + Environment.NewLine;
			bodyText += "Paid with " + register.PaymentMethod + Environment.NewLine;
			if (register.PaymentMethod == "paypal")
			{
				bodyText += "Email: " + register.PayeeEmailAddress + Environment.NewLine;
			}
			else
			{
				bodyText += "Name: " + register.PayeeFullName + Environment.NewLine;
				bodyText += "Address: " + register.PayeeFullAddress + Environment.NewLine;
				bodyText += "Email: " + register.PayeeEmailAddress + Environment.NewLine;
			}

			emailService = new EmailService(EmailService.FromWebAdminMailAddress, EmailService.FromGolfMailAddress, subject, bodyText);
			emailService.Tag = "golf-registration";
			await emailService.SendAsync();

			#endregion

			requestResult.Succeeded = true;
			return requestResult;
		}
	}
}
