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
    
    public partial class Student
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Student()
        {
            this.StudentAttandences = new HashSet<StudentAttandence>();
        }
    
        public int StudentID { get; set; }
        public Nullable<int> UserId { get; set; }
        public Nullable<int> ClassId { get; set; }
        public Nullable<System.DateTime> DOB { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Gender { get; set; }
        public Nullable<int> ParentID { get; set; }
    
        public virtual Class Class { get; set; }
        public virtual Parent Parent { get; set; }
        public virtual User User { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StudentAttandence> StudentAttandences { get; set; }
    }
}