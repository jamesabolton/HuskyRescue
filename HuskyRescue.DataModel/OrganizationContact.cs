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
    
    public partial class OrganizationContact
    {
        public System.Guid OrganizationId { get; set; }
        public System.Guid PersonId { get; set; }
        public string Role { get; set; }
    
        public virtual Person Person { get; set; }
        public virtual Organization Organization { get; set; }
    }
}
