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
    
    public partial class AspNetClass_FeeType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetClass_FeeType()
        {
            this.AspNetStudent_Discount_Applicable = new HashSet<AspNetStudent_Discount_Applicable>();
        }
    
        public int Id { get; set; }
        public Nullable<int> ClassID { get; set; }
        public Nullable<int> LedgerID { get; set; }
        public Nullable<int> Amount { get; set; }
        public string Type { get; set; }
    
        public virtual AspNetClass AspNetClass { get; set; }
        public virtual AspNetFinanceLedger AspNetFinanceLedger { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetStudent_Discount_Applicable> AspNetStudent_Discount_Applicable { get; set; }
    }
}
