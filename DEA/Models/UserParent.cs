using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DEA.Models
{
    public class UserParent
    {
        public User user { get; set; }
        public Parent parent { get; set; }
    }
}