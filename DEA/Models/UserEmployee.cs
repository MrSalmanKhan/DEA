using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEA.Models
{
    public class UserEmployee
    {
        public User user { get; set; }
        public Employee employee { get; set; }
        public Nullable<int> RoleID { get; set; }

        public virtual Role Role { get; set; }
    }
}