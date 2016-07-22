using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel.Payment
{
	public class Customer
	{
		[DisplayName("Braintree Customer Id")]
		public string Id { get; set; }

		[DisplayName("First Name")]
		public string FirstName { get; set; }

		[DisplayName("Last Name")]
		public string LastName { get; set; }

		[DisplayName("Mobile Number")]
		public string MobilePhoneNumber { get; set; }

		[DisplayName("Home Number")]
		public string HomePhoneNumber { get; set; }

		[DisplayName("Website Link")]
		public string Website { get; set; }

		[DisplayName("Email")]
		public string Email { get; set; }
	}
}
