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
    
    public partial class AspNetLoginTime
    {
        public int ID { get; set; }
        public System.DateTime StartTime { get; set; }
        public Nullable<System.DateTime> EndTime { get; set; }
        public Nullable<System.DateTime> Duration { get; set; }
        public string UserName { get; set; }
        public string UserId { get; set; }
    }
}
