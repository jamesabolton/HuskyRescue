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
    
    public partial class ApplicantResidencePetDepositCoverageType
    {
        public ApplicantResidencePetDepositCoverageType()
        {
            this.Applicants = new HashSet<Applicant>();
        }
    
        public int Id { get; set; }
        public string Value { get; set; }
    
        public virtual ICollection<Applicant> Applicants { get; set; }
    }
}
