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
    
    public partial class PhoneNumber
    {
        public PhoneNumber()
        {
            this.People = new HashSet<Person>();
            this.Organizations = new HashSet<Organization>();
        }
    
        public System.Guid Id { get; set; }
        public int PhoneNumberTypeId { get; set; }
        public string PhoneNumber1 { get; set; }
    
        public virtual PhoneNumberType PhoneNumberType { get; set; }
        public virtual ICollection<Person> People { get; set; }
        public virtual ICollection<Organization> Organizations { get; set; }
    }
}
