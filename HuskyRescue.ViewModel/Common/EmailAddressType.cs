using System.ComponentModel.DataAnnotations;

namespace HuskyRescue.ViewModel.Common
{
	public class EmailAddressType
	{
		public int Id { get; set; }

		[StringLength(200)]
		public string Name { get; set; }
	}
}
