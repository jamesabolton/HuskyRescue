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
    
    public partial class InventoryCategory
    {
        public InventoryCategory()
        {
            this.Inventories = new HashSet<Inventory>();
        }
    
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    
        public virtual ICollection<Inventory> Inventories { get; set; }
    }
}
