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
    
    public partial class Payment
    {
        public Payment()
        {
            this.PaymentDonations = new HashSet<PaymentDonation>();
            this.PaymentEventRegistrations = new HashSet<PaymentEventRegistration>();
            this.PaymentStores = new HashSet<PaymentStore>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> PersonId { get; set; }
        public Nullable<System.Guid> OrganizationId { get; set; }
        public string PaymentTransactionId { get; set; }
        public string PayeeName { get; set; }
        public System.DateTime DateSubmitted { get; set; }
        public bool IsCash { get; set; }
        public bool IsCheck { get; set; }
        public bool IsOnline { get; set; }
        public decimal Amount { get; set; }
        public Nullable<System.DateTime> AddedOnDate { get; set; }
        public string AddedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedOnDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public string Notes { get; set; }
    
        public virtual ICollection<PaymentDonation> PaymentDonations { get; set; }
        public virtual ICollection<PaymentEventRegistration> PaymentEventRegistrations { get; set; }
        public virtual ICollection<PaymentStore> PaymentStores { get; set; }
    }
}
