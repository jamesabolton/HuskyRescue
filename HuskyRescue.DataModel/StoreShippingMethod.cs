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
    
    public partial class StoreShippingMethod
    {
        public StoreShippingMethod()
        {
            this.StoreOrders = new HashSet<StoreOrder>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsFlatAmountPerItem { get; set; }
        public decimal FlatAmountPerItem { get; set; }
        public bool IsFlatAmountPerOrder { get; set; }
        public decimal FlatAmountPerOrder { get; set; }
        public bool IsPercentAmountPerItem { get; set; }
        public decimal PercentAmountPerItem { get; set; }
        public bool IsPercentAmountPerOrder { get; set; }
        public decimal PercentAmountPerOrder { get; set; }
        public decimal BaseAmount { get; set; }
        public System.DateTime AddOnDate { get; set; }
        public System.Guid AddedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedOnDate { get; set; }
        public Nullable<System.Guid> UpdatedByUserId { get; set; }
        public Nullable<System.DateTime> DeletedOnDate { get; set; }
        public string TrackingWebsite { get; set; }
        public string Notes { get; set; }
    
        public virtual ICollection<StoreOrder> StoreOrders { get; set; }
    }
}