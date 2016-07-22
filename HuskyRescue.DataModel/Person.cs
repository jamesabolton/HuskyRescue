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
    
    public partial class Person
    {
        public Person()
        {
            this.AnimalPlacements = new HashSet<AnimalPlacement>();
            this.InventoryPlacements = new HashSet<InventoryPlacement>();
            this.OrganizationContacts = new HashSet<OrganizationContact>();
            this.Addresses = new HashSet<Address>();
            this.EmailAddresses = new HashSet<EmailAddress>();
            this.MarketingPreferences = new HashSet<MarketingPreference>();
            this.PhoneNumbers = new HashSet<PhoneNumber>();
            this.Websites = new HashSet<Website>();
            this.EventRegistrationPersons = new HashSet<EventRegistrationPerson>();
            this.EventSponsors = new HashSet<EventSponsor>();
        }
    
        public System.Guid Id { get; set; }
        public Nullable<System.Guid> UserId { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsVolunteer { get; set; }
        public bool IsFoster { get; set; }
        public bool IsAvailableFoster { get; set; }
        public bool IsAdopter { get; set; }
        public bool IsDonor { get; set; }
        public bool IsBoardMember { get; set; }
        public bool IsSystemUser { get; set; }
        public bool IsDoNotAdopt { get; set; }
        public Nullable<System.DateTime> AddedOnDate { get; set; }
        public string AddedByUserId { get; set; }
        public Nullable<System.DateTime> UpdatedOnDate { get; set; }
        public string UpdatedByUserId { get; set; }
        public System.DateTime DateActive { get; set; }
        public Nullable<System.DateTime> DateInactive { get; set; }
        public Nullable<System.DateTime> DateDeleted { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Sex { get; set; }
        public string DriverLicenseNumber { get; set; }
        public string Notes { get; set; }
        public bool IsSponsor { get; set; }
    
        public virtual ICollection<AnimalPlacement> AnimalPlacements { get; set; }
        public virtual ICollection<InventoryPlacement> InventoryPlacements { get; set; }
        public virtual ICollection<OrganizationContact> OrganizationContacts { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
        public virtual ICollection<EmailAddress> EmailAddresses { get; set; }
        public virtual ICollection<MarketingPreference> MarketingPreferences { get; set; }
        public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
        public virtual ICollection<Website> Websites { get; set; }
        public virtual ICollection<EventRegistrationPerson> EventRegistrationPersons { get; set; }
        public virtual ICollection<EventSponsor> EventSponsors { get; set; }
    }
}