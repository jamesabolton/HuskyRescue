//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HuskyRescue.DataModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class StoreOrder
    {
        public StoreOrder()
        {
            this.StoreOrderDetails = new HashSet<StoreOrderDetail>();
        }
    
        public System.Guid Id { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public Nullable<System.DateTime> UpdatedOnDate { get; set; }
        public Nullable<System.DateTime> SubmittedOnDate { get; set; }
        public Nullable<System.Guid> PaymentStoreId { get; set; }
        public Nullable<System.Guid> StoreShippingMethodId { get; set; }
        public System.Guid UserId { get; set; }
        public System.Guid Status { get; set; }
        public decimal AmountDue { get; set; }
        public string CustomerComments { get; set; }
    
        public virtual PaymentStore PaymentStore { get; set; }
        public virtual ICollection<StoreOrderDetail> StoreOrderDetails { get; set; }
        public virtual StoreShippingMethod StoreShippingMethod { get; set; }
    }
}
