using System.Net.Mail;
using Braintree;
using Braintree.Exceptions;
using HuskyRescue.DataModel;
using HuskyRescue.ViewModel;
using HuskyRescue.ViewModel.Payment;
using Serilog;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Validation;
using System.Threading.Tasks;
using Address = HuskyRescue.DataModel.Address;
using Payment = HuskyRescue.DataModel.Payment;

namespace HuskyRescue.BusinessLogic
{
	public class BraintreePaymentService : IBraintreePaymentService
	{
		private readonly ILogger _logger;

		public BraintreePaymentService(ILogger iLogger)
		{
			_logger = iLogger;
		}

		private string _clientKey;

		public string ClientKey
		{
			get
			{
				if (string.IsNullOrEmpty(_clientKey))
				{
					_clientKey = AdminSystemSettings.GetSetting("BraintreeClientKey");
				}
				return _clientKey;
			}
		}

		private BraintreeGateway _gateway;

		public string GetClientToken(string customerId)
		{
			return string.IsNullOrEmpty(customerId) ? Gateway.ClientToken.generate() : Gateway.ClientToken.generate(new ClientTokenRequest { CustomerId = customerId });
		}

		public BraintreeGateway Gateway
		{
			get
			{
				if (_gateway == null)
				{
					var merchantId = AdminSystemSettings.GetSetting("BraintreeMerchantId");
					var publicKey = AdminSystemSettings.GetSetting("BraintreePublicKey");
					var privateKey = AdminSystemSettings.GetSetting("BraintreePrivateKey");
					var isProduction = AdminSystemSettings.GetSetting("BraintreeIsProduction");

					_gateway = new BraintreeGateway
					{
						Environment = isProduction.ToLower().Equals("true") ? Braintree.Environment.PRODUCTION : Braintree.Environment.SANDBOX,
						MerchantId = merchantId,
						PublicKey = publicKey,
						PrivateKey = privateKey
					};
				}
				return _gateway;
			}
		}

		public RequestResult SendPayment(decimal amount, string nonce, bool isTaxExempt, PaymentType paymentType, string deviceData,
			string transactionDescription = "", string customerNotes = "",
			string firstName = "", string lastName = "",
			string addressStreet1 = "", string addressStreet2 = "", string addressCity = "", string addressStateId = "", string addressPostalCode = "", string countryCode = "US",
			string phoneNumber = "", string email = "", string company = "", string website = "", bool isShipping = false)
		{
			var requestResult = new RequestResult();

			try
			{
				var braintreeRequest = new TransactionRequest
									   {
										   Amount = amount,
										   PaymentMethodNonce = nonce,
										   TaxExempt = isTaxExempt,
										   Type = TransactionType.SALE,
										   DeviceData = deviceData,
										   Options = new TransactionOptionsRequest
													 {
														 StoreInVaultOnSuccess = true,
														 StoreShippingAddressInVault = isShipping,
														 SubmitForSettlement = true
													 },
										   Customer = new CustomerRequest
													  {
														  FirstName = firstName,
														  LastName = lastName,
														  Email = !string.IsNullOrEmpty(email) ? email : string.Empty,
														  Website = !string.IsNullOrEmpty(website) ? website : string.Empty,
														  Phone = !string.IsNullOrEmpty(phoneNumber) ? phoneNumber : string.Empty,
														  Company = !string.IsNullOrEmpty(company) ? company : string.Empty,
													  }
									   };
				if (!string.IsNullOrEmpty(transactionDescription))
				{
					braintreeRequest.CustomFields.Add("transaction_desc", transactionDescription);
				}
				if (!string.IsNullOrEmpty(customerNotes))
				{
                    braintreeRequest.CustomFields.Add("customer_comments", customerNotes.Length > 255 ? customerNotes.Substring(0,254) : customerNotes);
				}
				if (paymentType == PaymentType.CreditCard)
				{
					braintreeRequest.Options.AddBillingAddressToPaymentMethod = true;
					
					braintreeRequest.BillingAddress = new AddressRequest
					{
						Company = !string.IsNullOrEmpty(company) ? company : string.Empty,
						CountryCodeAlpha2 = countryCode,
						FirstName = firstName,
						LastName = lastName,
						PostalCode = addressPostalCode,
						StreetAddress = addressStreet1,
						ExtendedAddress = addressStreet2,
						Locality = addressCity,
						Region = addressStateId
					};
				}
				if(paymentType == PaymentType.PayPal)
				{

					braintreeRequest.Options.PayPal = new TransactionOptionsPayPalRequest
					{
						//CustomField = "PayPal custom field",
						Description = String.IsNullOrEmpty(transactionDescription) ? "TXHR Payment" : transactionDescription
					};
				}

				var result = Gateway.Transaction.Sale(braintreeRequest);

				// check if success
				if (result.IsSuccess())
				{
					requestResult.Succeeded = true;
                    requestResult.NewKey = result.Target.Id;
                    var transTarget = result.Target;
					if (transTarget.PaymentInstrumentType == PaymentInstrumentType.CREDIT_CARD)
					{
					}
					if (transTarget.PaymentInstrumentType == PaymentInstrumentType.PAYPAL_ACCOUNT)
					{
					}
				}
				else
				{
					requestResult.Succeeded = false;
					requestResult.Errors = new List<string>(1) { result.Message };
					if (result.Transaction != null)
					{
						if (result.Transaction.Status == TransactionStatus.SETTLEMENT_DECLINED)
						{

						}
						if (result.Transaction.Status == TransactionStatus.FAILED)
						{

						}
						if (result.Transaction.Status == TransactionStatus.GATEWAY_REJECTED)
						{

						}
						if (result.Transaction.Status == TransactionStatus.PROCESSOR_DECLINED)
						{
							// https://developers.braintreepayments.com/javascript+dotnet/reference/general/processor-responses/authorization-responses
							// 1000 >= code < 2000 Success
							// 2000 >= code < 3000 Decline
							// 3000 >= code        Failure
						}
						if (result.Transaction.Status == TransactionStatus.UNRECOGNIZED)
						{

						}

						if (result.Errors.DeepCount > 0)
						{
							_logger.Information("Braintree validation errors: {Message} -- {@BraintreeValidationErrors}", result.Message, result.Errors.DeepAll());
						}
						else
						{
							_logger.Information("Braintree transaction failure: {@BraintreeResult}", result);
						}
					}
				}
			}
			catch (AuthenticationException authenticationException)
			{
				// API keys are incorrect
				// TODO send email to admin
				_logger.Error(authenticationException, "Braintree authentication error");
				throw;
			}
			catch (AuthorizationException authorizationException)
			{
				// not authorized to perform the attempted action according to the roles assigned to the user who owns the API key
				// TODO send email to admin
				_logger.Error(authorizationException, "Braintree authorization error");
				throw;
			}
			catch (ServerException serverException)
			{
				// something went wrong on the braintree server
				// user should try again
				_logger.Error(serverException, "Braintree server error");
				throw;
			}
			catch (UpgradeRequiredException upgradeRequiredException)
			{
				// TODO send email to admin
				_logger.Error(upgradeRequiredException, "Braintree upgrade required error");
				throw;
			}
			catch (BraintreeException braintreeException)
			{
				// user should try again
				_logger.Error(braintreeException, "Braintree general error");
				throw;
			}

			return requestResult;
		}

		/// <summary>
		/// Send donation to braintree for processing, send email to group and to donor, save to database
		/// </summary>
		/// <param name="donation">donation view model</param>
		/// <returns>RequestResult object</returns>
		public async Task<RequestResult> SendDonation(Donation donation)
		{
			var requestResult = new RequestResult();

			try
			{
				var braintreeRequest = new TransactionRequest
							  {
								  Amount = donation.Amount,
								  PaymentMethodNonce = donation.Nonce,
								  TaxExempt = true,
								  Type = TransactionType.SALE,
								  Options = new TransactionOptionsRequest
											{
												StoreInVault = true,
												StoreInVaultOnSuccess = true,
												StoreShippingAddressInVault = true,
												//PayeeEmail = !string.IsNullOrEmpty(donation.Email) ? donation.Email : string.Empty,
												SubmitForSettlement = true
											},
								  Customer = new CustomerRequest
											  {
												  FirstName = donation.FirstName,
												  LastName = donation.LastName,
												  Email = !string.IsNullOrEmpty(donation.Email) ? donation.Email : string.Empty,
												  Website = !string.IsNullOrEmpty(donation.Website) ? donation.Website : string.Empty,
												  Phone = !string.IsNullOrEmpty(donation.MobilePhoneNumber) ? donation.MobilePhoneNumber : string.Empty,
												  Company = !string.IsNullOrEmpty(donation.CompanyName) ? donation.CompanyName : string.Empty
											  }
							  };

				//if (!string.IsNullOrEmpty(donation.StreetAddress1))
				//{
				//	braintreeRequest.Options.AddBillingAddressToPaymentMethod = true;

				//	braintreeRequest.BillingAddress = new AddressRequest
				//							 {
				//								 Company = !string.IsNullOrEmpty(donation.CompanyName) ? donation.CompanyName : string.Empty,
				//								 CountryCodeAlpha2 = donation.CountryCodeId,
				//								 FirstName = donation.FirstName,
				//								 LastName = donation.LastName,
				//								 PostalCode = donation.PostalCode,
				//								 StreetAddress = donation.StreetAddress1,
				//								 ExtendedAddress = donation.StreetAddress2,
				//								 Locality = donation.City,
				//								 Region = donation.StateId.ToString()
				//							 };
				//}

				if (donation.PaymentMethod.Equals("creditcard"))
				{
					braintreeRequest.Options.AddBillingAddressToPaymentMethod = true;
					
					braintreeRequest.BillingAddress = new AddressRequest
					{
						Company = !string.IsNullOrEmpty(donation.CompanyName) ? donation.CompanyName : string.Empty,
						CountryCodeAlpha2 = donation.CountryCodeId,
						FirstName = donation.FirstName,
						LastName = donation.LastName,
						PostalCode = donation.PostalCode,
						StreetAddress = donation.StreetAddress1,
						ExtendedAddress = donation.StreetAddress2,
						Locality = donation.City,
						Region = donation.StateId.ToString()
					};
				}
				if (donation.PaymentMethod.Equals("paypal"))
				{

					braintreeRequest.Options.PayPal = new TransactionOptionsPayPalRequest
					{
						//CustomField = "PayPal custom field",
						Description = String.IsNullOrEmpty(donation.CustomerNotes) ? "TXHR Payment" : donation.CustomerNotes
					};
				}

				// send request to braintree
				var result = Gateway.Transaction.Sale(braintreeRequest);

				// check if success
				if (result.IsSuccess())
				{
					requestResult.Succeeded = true;

					var transTarget = result.Target;
					if (transTarget.PaymentInstrumentType == PaymentInstrumentType.CREDIT_CARD)
					{
					}
					if (transTarget.PaymentInstrumentType == PaymentInstrumentType.PAYPAL_ACCOUNT)
					{
					}

					#region send emails

					var subject = string.Join(" ", "Donation from", donation.FirstName, donation.LastName, "to Texas Husky Rescue, Inc");
					var bodyText = string.Join(" ", "Thank you for your donation of", donation.Amount);

					EmailService emailService;
					if (!string.IsNullOrEmpty(donation.Email))
					{
						//emailService = new EmailService(EmailService.EmailFrom.Contact, donation.Email, subject, bodyText, bodyText);
						emailService = new EmailService(EmailService.FromContactMailAddress, new MailAddress(donation.Email, donation.FirstName + " " + donation.LastName), subject, bodyText);
						emailService.Tag = "payment-donation";
						await emailService.SendAsync();
					}

					var groupEmail = AdminSystemSettings.GetSetting("Email-Contact");
					bodyText = string.Join(" ", "Donation from", donation.FirstName, donation.LastName, "for", donation.Amount, ". ");
					bodyText += System.Environment.NewLine;
					bodyText += "Does this person want to receive email from us in the future? " + donation.FutureContact;
					bodyText += System.Environment.NewLine;
					bodyText += "Notes from the donor: " + donation.CustomerNotes;
					bodyText += System.Environment.NewLine;
					bodyText += "Donor's email: " + donation.Email;
					//emailService = new EmailService(EmailService.EmailFrom.Contact, groupEmail, subject, bodyText, bodyText);
					emailService = new EmailService(new MailAddress(donation.Email, donation.FirstName + " " + donation.LastName), EmailService.FromContactMailAddress, subject, bodyText);
					emailService.Tag = "payment-donation";
					await emailService.SendAsync();

					#endregion

					#region save to database
					#region create objects
					var dbPerson = new Person
					{
						AddedOnDate = DateTime.Today,
						AddedByUserId = donation.AddedByUserId,
						DateActive = DateTime.Today,
						FirstName = donation.FirstName,
						LastName = donation.LastName,
						IsActive = true,
						IsDonor = true
					};

					if (!string.IsNullOrEmpty(donation.StreetAddress1))
					{
						dbPerson.Addresses.Add(new Address
											   {
												   Address1 = donation.StreetAddress1,
												   Address2 = donation.StreetAddress2,
												   AddressStateId = donation.StateId,
												   AddressTypeId = 4, // billing
												   City = donation.City,
												   IsBillingAddress = true,
												   ZipCode = donation.PostalCode
											   });
					}

					if (!string.IsNullOrEmpty(donation.Email))
					{
						dbPerson.EmailAddresses = new Collection<EmailAddress>
				                        {
					                        new EmailAddress
					                        {
						                        Address = donation.Email,
												EmailAddressTypeId = 0
					                        }
				                        };
					}

					if (!string.IsNullOrEmpty(donation.HomePhoneNumber) || !string.IsNullOrEmpty(donation.MobilePhoneNumber))
					{
						dbPerson.PhoneNumbers = new Collection<PhoneNumber>();
						if (!string.IsNullOrEmpty(donation.HomePhoneNumber))
						{
							dbPerson.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = donation.HomePhoneNumber,
								PhoneNumberTypeId = 1
							});
						}
						if (!string.IsNullOrEmpty(donation.MobilePhoneNumber))
						{
							dbPerson.PhoneNumbers.Add(new PhoneNumber
							{
								PhoneNumber1 = donation.MobilePhoneNumber,
								PhoneNumberTypeId = 1
							});
						}
					}


					var dbPaymentDonation = new PaymentDonation
									{
										HasThankYouCardBeenSent = false,
										DonatedItemCashValue = donation.Amount,
										DonatedItemDescription = "online donation",
										Payment = new Payment
										{
											AddedByUserId = donation.AddedByUserId,
											AddedOnDate = DateTime.Today,
											Amount = donation.Amount,
											DateSubmitted = DateTime.Today,
											IsCash = false,
											IsCheck = false,
											IsOnline = true,
											Notes = donation.CustomerNotes,
											PayeeName = donation.FirstName + " " + donation.LastName,
											PaymentTransactionId = transTarget.Id
										}
									};
					#endregion

					#region save to database
					try
					{
						using (var context = new HuskyRescueEntities())
						{
							dbPerson = context.People.Add(dbPerson);

							context.SaveChanges();

							dbPaymentDonation.Payment.PersonId = dbPerson.Id;

							context.PaymentDonations.Add(dbPaymentDonation);

							context.SaveChanges();
						}
					}
					catch (DbEntityValidationException ex)
					{
						_logger.Error(ex, "donation create validation error: {@DonationItem} {@DbValidationErrors}", donation, ex.EntityValidationErrors);
					}
					#endregion
					#endregion

				}
				else
				{
					requestResult.Succeeded = false;
					requestResult.Errors = new List<string>(1) { result.Message };
					if (result.Transaction != null)
					{
						if (result.Transaction.Status == TransactionStatus.SETTLEMENT_DECLINED)
						{

						}
						if (result.Transaction.Status == TransactionStatus.FAILED)
						{

						}
						if (result.Transaction.Status == TransactionStatus.GATEWAY_REJECTED)
						{

						}
						if (result.Transaction.Status == TransactionStatus.PROCESSOR_DECLINED)
						{
							// https://developers.braintreepayments.com/javascript+dotnet/reference/general/processor-responses/authorization-responses
							// 1000 >= code < 2000 Success
							// 2000 >= code < 3000 Decline
							// 3000 >= code        Failure
						}
						if (result.Transaction.Status == TransactionStatus.UNRECOGNIZED)
						{

						}

						if (result.Errors.DeepCount > 0)
						{
							_logger.Information("Braintree validation errors: {Message} -- {@BraintreeValidationErrors}", result.Message, result.Errors.DeepAll());
						}
						else
						{
							_logger.Information("Braintree transaction failure: {@BraintreeResult}", result);
						}
					}
				}
			}
			catch (AuthenticationException authenticationException)
			{
				// API keys are incorrect
				// TODO send email to admin
				_logger.Error(authenticationException, "Braintree authentication error");
				throw;
			}
			catch (AuthorizationException authorizationException)
			{
				// not authorized to perform the attempted action according to the roles assigned to the user who owns the API key
				// TODO send email to admin
				_logger.Error(authorizationException, "Braintree authorization error");
				throw;
			}
			catch (ServerException serverException)
			{
				// something went wrong on the braintree server
				// user should try again
				_logger.Error(serverException, "Braintree server error");
				throw;
			}
			catch (UpgradeRequiredException upgradeRequiredException)
			{
				// TODO send email to admin
				_logger.Error(upgradeRequiredException, "Braintree upgrade required error");
				throw;
			}
			catch (BraintreeException braintreeException)
			{
				// user should try again
				_logger.Error(braintreeException, "Braintree general error");
				throw;
			}

			return requestResult;
		}
	}
}
