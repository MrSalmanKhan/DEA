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
    
    public partial class Term
    {
        public int TermID { get; set; }
        public string TermName { get; set; }
        public Nullable<System.DateTime> TermStartDate { get; set; }
        public Nullable<System.DateTime> TermEndDate { get; set; }
        public Nullable<int> SessionID { get; set; }
        public Nullable<bool> Status { get; set; }
    
        public virtual Session Session { get; set; }
    }
}