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
    
    public partial class SystemAuditLog
    {
        public int Id { get; set; }
        public System.DateTime DateAdded { get; set; }
        public System.Guid UserId { get; set; }
        public int SystemAuditLogEntityTypeId { get; set; }
        public string EntitySurrogateId { get; set; }
        public bool Deleted { get; set; }
        public bool Added { get; set; }
        public bool Updated { get; set; }
    
        public virtual SystemAuditLogEntityType SystemAuditLogEntityType { get; set; }
    }
}
