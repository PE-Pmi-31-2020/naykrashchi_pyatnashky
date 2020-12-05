using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class User
    {
        public User()
        {
            Matches = new HashSet<Match>();
        }

        public int UserId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Match> Matches { get; set; }
    }
}
