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
    
    public partial class PaymentEventRegistration
    {
        public PaymentEventRegistration()
        {
            this.EventRegistrations = new HashSet<EventRegistration>();
        }
    
        public System.Guid Id { get; set; }
        public System.Guid PaymentId { get; set; }
    
        public virtual Payment Payment { get; set; }
        public virtual ICollection<EventRegistration> EventRegistrations { get; set; }
    }
}
