//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DEA.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class AspNetFeeChallan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetFeeChallan()
        {
            this.AspNetStudent_Payment = new HashSet<AspNetStudent_Payment>();
        }
    
        public int Id { get; set; }
        public Nullable<int> ClassID { get; set; }
        public Nullable<System.DateTime> DueDate { get; set; }
        public Nullable<int> DurationTypeID { get; set; }
        public Nullable<int> TotalAmount { get; set; }
        public Nullable<System.DateTime> StartDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<int> Penalty { get; set; }
        public Nullable<System.DateTime> ValidDate { get; set; }
        public string Title { get; set; }
        public Nullable<System.DateTime> Created { get; set; }
    
        public virtual AspNetClass AspNetClass { get; set; }
        public virtual AspNetDurationType AspNetDurationType { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetStudent_Payment> AspNetStudent_Payment { get; set; }
    }
}
