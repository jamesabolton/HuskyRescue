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
    
    public partial class AspNetRoleResource
    {
        public string RoleId { get; set; }
        public string ResourceId { get; set; }
        public int Operations { get; set; }
    
        public virtual AspNetResource AspNetResource { get; set; }
        public virtual AspNetRole AspNetRole { get; set; }
    }
}
