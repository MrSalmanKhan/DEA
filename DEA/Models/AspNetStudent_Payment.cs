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
    
    public partial class AspNetStudent_Payment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public AspNetStudent_Payment()
        {
            this.AspNetStudent_PaymentDetail = new HashSet<AspNetStudent_PaymentDetail>();
            this.AspNetStudent_PaymentDetail1 = new HashSet<AspNetStudent_PaymentDetail>();
        }
    
        public int Id { get; set; }
        public string StudentID { get; set; }
        public Nullable<int> FeeChallanID { get; set; }
        public Nullable<int> TotalAmount { get; set; }
        public Nullable<int> PaymentAmount { get; set; }
        public Nullable<System.DateTime> PaymentDate { get; set; }
        public string Status { get; set; }
        public string Remarks { get; set; }
    
        public virtual AspNetFeeChallan AspNetFeeChallan { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetStudent_PaymentDetail> AspNetStudent_PaymentDetail { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<AspNetStudent_PaymentDetail> AspNetStudent_PaymentDetail1 { get; set; }
    }
}
