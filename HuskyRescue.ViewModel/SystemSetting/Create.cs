using System;
using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;

namespace HuskyRescue.ViewModel.SystemSetting
{
	public class Create
	{
		[Required(ErrorMessage = "setting name is required")]
		[AssertThat("Length(Name) <= 200", ErrorMessage = "setting name must be 200 characters or less")]
		public string Name { get; set; }
		
		[Required(ErrorMessage = "setting value is required")]
		[AssertThat("Length(Value) <= 1000", ErrorMessage = "setting value must be 1000 characters or less")]
		public string Value { get; set; }

		[AssertThat("Length(Notes) <= 4000", ErrorMessage = "notes can be no more than 4000 characters")]
		public string Notes { get; set; }

		public string AddedByUserId { get; set; }
	}
}
