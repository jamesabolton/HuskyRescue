using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace HuskyRescue.ViewModel.Payment
{
	public class Address
	{
		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[DisplayName("Business Name")]
		public string CompanyName { get; set; }

		[DisplayName("Street Address")]
		public string StreetAddress1 { get; set; }

		[DisplayName("Street Address Cont.")]
		public string StreetAddress2 { get; set; }

		public List<SelectListItem> States { get; set; }

		[DisplayName("State")]
		public Nullable<int> MailingAddressStateId { get; set; }

		[DisplayName("Postal Code")]
		public string PostalCode { get; set; }

		[DisplayName("Country Code")]
		public string CountryCodeId { get; set; }

		public Address()
		{
			CountryCodeId = "US";
		}
	}
}
