using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel.Common
{
	public class EmailAddress
	{
		public Guid Id { get; set; }

		public int EmailAddressTypeId { get; set; }
		
		[DataType(DataType.EmailAddress)]
		[StringLength(200)]
		public string Address { get; set; }

		[StringLength(200)]
		[DisplayName("Email Type")]
		public string TypeName { get; set; }
	}
}
