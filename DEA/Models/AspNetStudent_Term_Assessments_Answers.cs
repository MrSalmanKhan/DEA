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
    
    public partial class AspNetStudent_Term_Assessments_Answers
    {
        public int Id { get; set; }
        public Nullable<int> STAID { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public string Catageory { get; set; }
        public string FirstTermGrade { get; set; }
        public string SecondTermGrade { get; set; }
        public string ThirdTermGrade { get; set; }
    
        public virtual AspNetStudent_Term_Assessment AspNetStudent_Term_Assessment { get; set; }
    }
}
