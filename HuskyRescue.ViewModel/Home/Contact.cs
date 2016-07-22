using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.Home
{
	public class Contact
	{
		public Contact()
		{
			ContactReasonList = new List<SelectListItem>();
			PostedFiles = new List<HttpPostedFileBase>();
		}

		[DisplayName("First Name")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "first name required")]
		[AssertThat("Length(NameFirst) <= 100", ErrorMessage = "first name cannot be longer than 100 characters")]
		public string NameFirst { get; set; }

		[DisplayName("Last Name")]
		[DataType(DataType.Text)]
		[Required(ErrorMessage = "last name required")]
		[AssertThat("Length(NameLast) <= 100", ErrorMessage = "last name cannot be longer than 100 characters")]
		public string NameLast { get; set; }

		public string FullName
		{
			get { return NameFirst + " " + NameLast; }
		}

		[DisplayName("Email Address")]
		[DataType(DataType.EmailAddress)]
		//[Required(ErrorMessage = "your email address is required")]
		[AssertThat("Length(EmailAddress) <= 200", ErrorMessage = "email address cannot be longer than 200 characters")]
		[AssertThat("IsEmail(EmailAddress)", ErrorMessage = "valid email address required")]
		public string EmailAddress { get; set; }

		[DisplayName("Phone Number")]
		[DataType(DataType.PhoneNumber)]
		[AssertThat("Length(Number) > 8 && Length(Number) < 16", ErrorMessage = "phone number must be between 9 and 15 digits")]
		[AssertThat(@"IsRegexMatch(Number, '^\\d+$')", ErrorMessage = "must be a valid phone number")]
		public string Number { get; set; }

		[DisplayName("Message to Texas Husky Rescue")]
		[DataType(DataType.MultilineText)]
		[Required(ErrorMessage = "message must be provided")]
		[AssertThat("Length(Message) <= 4000", ErrorMessage = "message cannot be longer than 4000 characters")]
		public string Message { get; set; }

		[DisplayName("Would you like to be added to our emailing list?")]
		public bool IsEmailable { get; set; }

		[DisplayName("Contact Reason")]
		[Required(ErrorMessage = "reason for contacting is required")]
		public string ContactReasonId { get; set; }
		public IEnumerable<SelectListItem> ContactReasonList { get; set; }

		public List<HttpPostedFileBase> PostedFiles { get; set; }
	}


}
