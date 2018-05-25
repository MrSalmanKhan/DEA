using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEA.Models
{
    public class UserStudent
    {
        public User user{ get; set; }
        public Student student { get; set; }
        public Nullable<int> ClassID { get; set; }
        public Nullable<int> ParentID { get; set; }

        public virtual Class Class { get; set; }
        public virtual Parent Parent { get; set; }
    }
}