using System.ComponentModel;

namespace HuskyRescue.ViewModel.SystemSetting
{
	public class List
	{
		public string Name { get; set; }

		public string Value { get; set; }

		[DisplayName("Added on")]
		public string AddedOnDate { get; set; }

		[DisplayName("Added by User")]
		public string AddedByUserId { get; set; }

		[DisplayName("Updated by User")]
		public string UpdatedByUserId { get; set; }

		[DisplayName("Updated on")]
		public string UpdatedOnDate { get; set; }

	}
}
