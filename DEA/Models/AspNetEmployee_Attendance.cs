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
    
    public partial class AspNetEmployee_Attendance
    {
        public int Id { get; set; }
        public Nullable<int> AttendanceID { get; set; }
        public Nullable<int> EmployeeID { get; set; }
        public string Status { get; set; }
        public string Reason { get; set; }
        public string Month { get; set; }
        public string Time { get; set; }
    
        public virtual AspNetEmployee AspNetEmployee { get; set; }
        public virtual AspNetEmployeeAttendance AspNetEmployeeAttendance { get; set; }
    }
}
