using System;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.RoughRiders.Register;
using Serilog;
using System.Collections.ObjectModel;

namespace HuskyRescue.BusinessLogic
{
	public class RoughRidersEventManagerService : IRoughRidersEventManagerService
	{
		private readonly ILogger _logger;

		private readonly IBraintreePaymentService _braintreePaymentService;

		private readonly IAdminSystemSettingsManagerService _adminSystemSettingsManagerService;

		public RoughRidersEventManagerService(ILogger iLogger, IBraintreePaymentService iBraintreePaymentService, IAdminSystemSettingsManagerService iAdminSystemSettingsManagerService)
		{
			_logger = iLogger;
			_braintreePaymentService = iBraintreePaymentService;
			_adminSystemSettingsManagerService = iAdminSystemSettingsManagerService;
		}

		public async Task<RequestResult> RegisterForTournamentPublic(CreatePublic register)
		{
			var requestResult = new RequestResult();


			var activeRoughRidersEventId = (await _adminSystemSettingsManagerService.GetSettingDetailAsync("RoughRidersEventId")).Value;

			// Log request
			_logger.Information("RoughRiders Registration {@RoughRidersRegister}", register);

			#region Process payment
			var paymentRequestResult = new RequestResult();

			if (register.PaymentMethod.ToLower().Equals("paypal"))
			{
				paymentRequestResult = _braintreePaymentService.SendPayment(register.AmountDue, register.Nonce, true, PaymentType.PayPal, register.DeviceData,
					"RoughRiders registration", register.CustomerNotes,
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
					"RoughRiders registration", register.CustomerNotes,
					register.PayeeFirstName, register.PayeeLastName, register.PayeeAddressStreet1, register.PayeeAddressStreet2,
					register.PayeeAddressCity, stateCode, register.PayeeAddressPostalCode, register.CountryCodeId);
			}

			if (!paymentRequestResult.Succeeded)
			{
				// TODO: handle failure to pay
				requestResult.Succeeded = false;
				requestResult.Errors.Add("Payment Failure - see below for details: ");
				requestResult.Errors.AddRange(paymentRequestResult.Errors);

				_logger.Error("RoughRiders Registration Payment Failed {@RoughRidersPaymentErrors}", requestResult.Errors);

				return requestResult;
			}
			#endregion

			#region Save to attendee information to database
			try
			{
				using (var context = new HuskyRescueEntities())
				{

					#region Event Registration

					var dbEvent = context.Events.Single(e => e.Id.Equals(new Guid(activeRoughRidersEventId)));

					var dbEventRegistration = new EventRegistration
											  {
												  DateSubmitted = DateTime.Today,
												  EventId = dbEvent.Id,
												  HasPaid = true,
												  RegistrationComments = register.CustomerNotes,
												  NumberTicketsBought = register.NumberOfTickets,
												  AmountPaid = register.AmountDue
											  };
					#endregion

					#region Attendee
					var dbPerson = new Person
									{
										AddedOnDate = DateTime.Today,
										DateActive = DateTime.Today,
										FirstName = register.AttendeeFirstName,
										LastName = register.AttendeeLastName,
										IsActive = true,
										Notes = "RoughRiders 2015"
									};
					if (!string.IsNullOrEmpty(register.AttendeeAddressStreet1))
					{
						dbPerson.Addresses.Add(new Address
						{
							Address1 = register.AttendeeAddressStreet1,
							Address2 = register.AttendeeAddressStreet2,
							AddressStateId = register.AttendeeAddressStateId,
							AddressTypeId = 1, // primary
							City = register.AttendeeAddressCity,
							IsBillingAddress = false,
							ZipCode = register.AttendeeAddressPostalCode
						});
					}

					if (!string.IsNullOrEmpty(register.AttendeeEmailAddress))
					{
						dbPerson.EmailAddresses = new Collection<EmailAddress>
												{
													new EmailAddress
													{
														Address = register.AttendeeEmailAddress,
														EmailAddressTypeId = 0
													}
												};
					}

					if (!string.IsNullOrEmpty(register.AttendeeHomePhoneNumber) || !string.IsNullOrEmpty(register.AttendeeMobilePhoneNumber))
					{
						dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
						if (!string.IsNullOrEmpty(register.AttendeeHomePhoneNumber))
						{
							dbPerson.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = register.AttendeeHomePhoneNumber,
								PhoneNumberTypeId = 1
							});
						}
						if (!string.IsNullOrEmpty(register.AttendeeMobilePhoneNumber))
						{
							dbPerson.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = register.AttendeeMobilePhoneNumber,
								PhoneNumberTypeId = 1
							});
						}
					}

					dbEventRegistration.EventRegistrationPersons.Add(new EventRegistrationPerson
																	 {
																		 IsPrimaryPerson = true,
																		 Person = dbPerson,
																		 AttendeeType = register.AttendeeType,
																		 TicketPrice = register.AttendeeTicketPrice
																	 });
					#endregion

					dbEventRegistration = context.EventRegistrations.Add(dbEventRegistration);

					context.SaveChanges();
				}
			}
			catch (DbEntityValidationException ex)
			{
				_logger.Error(ex, "RoughRider reg create validation error: {@RoughRiderRegItem} {@DbValidationErrors}", register, ex.EntityValidationErrors);
				foreach (var validationError in ex.EntityValidationErrors)
				{
					requestResult.Errors.Add(validationError.ToString());
				}
			}
			#endregion

			#region send emails
			const string subject = "3rd Annual Texas Husky Rescue RoughRiders Registration";
			var bodyText = @" 
Thank you for supporting Texas Husky Rescue and the Frisco RoughRiders!  Tickets will be available at Will Call the day of the game, April 11th.  The Will Call window is located at the Dr Pepper Ballpark Ticket Office in the Home Plate Building and will open 2 hours prior to the start of the game.

Advance parking passes can also be purchased from Jessica (972-334-1936 or jenglish@ridersbaseball.com) with the RoughRiders for $5. Parking passes purchased the day of the game are $10.

A reminder e-mail will be sent closer to game day.  If you have any questions prior to then, please feel free to email us at roughriders@texashuskyrescue.org.

Thanks again,
Texas Husky Rescue
1-877-TX-HUSKY (894-8759) (phone/fax)
PO Box 118891, Carrollton, TX 75011
";

			EmailService emailService;
			if (register.AttendeeIsAttending)
			{
				emailService = new EmailService(EmailService.FromRoughRiderMailAddress, new MailAddress(register.AttendeeEmailAddress, register.AttendeeFullName), subject, bodyText);
				emailService.Tag = "roughrider-registration";
				await emailService.SendAsync();
			}

			var groupEmail = AdminSystemSettings.GetSetting("Email-RoughRider");
			bodyText = "RoughRiders registration for " + register.NumberOfTickets + " tickets.";
			bodyText += Environment.NewLine;
			bodyText += Environment.NewLine;
			bodyText += "Attendee:: " + Environment.NewLine;
			bodyText += "Name: " + register.AttendeeFullName + Environment.NewLine;
			bodyText += "Address: " + register.AttendeeFullAddress + Environment.NewLine;
			bodyText += "Home Phone: " + (string.IsNullOrEmpty(register.AttendeeHomePhoneNumber) ? "not provided" : register.AttendeeHomePhoneNumber) + Environment.NewLine;
			bodyText += "Mobile Phone: " + (string.IsNullOrEmpty(register.AttendeeMobilePhoneNumber) ? "not provided" : register.AttendeeMobilePhoneNumber) + Environment.NewLine;
			bodyText += "Email: " + (string.IsNullOrEmpty(register.AttendeeEmailAddress) ? "not provided" : register.AttendeeEmailAddress) + Environment.NewLine;
			bodyText += "Mailable?: " + register.AttendeeFutureContact + Environment.NewLine;
			bodyText += "Ticket Price: " + register.GameTicketCost + Environment.NewLine;
			bodyText += "-------------------------------------------------------------" + Environment.NewLine;


			bodyText += "Notes from the attendee: " + register.CustomerNotes + Environment.NewLine;
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

			emailService = new EmailService(EmailService.FromWebAdminMailAddress, EmailService.FromRoughRiderMailAddress, subject, bodyText);
			emailService.Tag = "roughrider-registration";
			await emailService.SendAsync();

			#endregion

			requestResult.Succeeded = true;
			return requestResult;
		}

		public async Task<bool> IsEventActive()
		{
			var isActive = false;

			using (var context = new HuskyRescueEntities())
			{
				var activeRoughRidersEventId = (await _adminSystemSettingsManagerService.GetSettingDetailAsync("RoughRidersEventId")).Value;
				if (activeRoughRidersEventId == null) return isActive;

				var rrEvent = await context.Events.SingleAsync(e => e.Id.ToString().Equals(activeRoughRidersEventId));

				if (rrEvent != null)
				{
					isActive = rrEvent.IsActive;
				}
			}
			return isActive;
		}
	}
}
