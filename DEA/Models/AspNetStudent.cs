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
    
    public partial class AspNetStudent
    {
        public int Id { get; set; }
        public string StudentID { get; set; }
        public Nullable<int> ClassID { get; set; }
        public string Level { get; set; }
        public string SchoolName { get; set; }
        public string BirthDate { get; set; }
        public string Nationality { get; set; }
        public string Religion { get; set; }
        public string Gender { get; set; }
        public string PhoneNumber { get; set; }
    
        public virtual AspNetClass AspNetClass { get; set; }
        public virtual AspNetUser AspNetUser { get; set; }
    }
}