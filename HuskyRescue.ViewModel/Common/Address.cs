using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HuskyRescue.ViewModel.Common
{
	public partial class Address
	{
		public Guid Id { get; set; }
		public int AddressTypeId { get; set; }
		public string Address1 { get; set; }
		public string Address2 { get; set; }
		public string Address3 { get; set; }
		public string City { get; set; }
		public Nullable<int> AddressStateId { get; set; }
		public string ZipCode { get; set; }
		public string CountryId { get; set; }
		public bool IsBillingAddress { get; set; }
		public bool IsShippingAddress { get; set; }

		public virtual AddressState AddressState { get; set; }
		public virtual AddressType AddressType { get; set; }
	}
}
