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
    
    public partial class EventRegistrationPerson
    {
        public System.Guid Id { get; set; }
        public System.Guid EventRegistrationId { get; set; }
        public System.Guid PersonId { get; set; }
        public bool IsPrimaryPerson { get; set; }
        public decimal TicketPrice { get; set; }
        public string AttendeeType { get; set; }
    
        public virtual EventRegistration EventRegistration { get; set; }
        public virtual Person Person { get; set; }
    }
}
