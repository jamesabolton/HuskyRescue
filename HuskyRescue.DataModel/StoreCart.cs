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
    
    public partial class StoreCart
    {
        public StoreCart()
        {
            this.StoreCartItems = new HashSet<StoreCartItem>();
        }
    
        public System.Guid Id { get; set; }
        public System.DateTime CreatedOnDate { get; set; }
        public Nullable<System.DateTime> UpdatedOnDate { get; set; }
        public System.Guid UserId { get; set; }
    
        public virtual ICollection<StoreCartItem> StoreCartItems { get; set; }
    }
}