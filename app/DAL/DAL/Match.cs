using System;
using System.Collections.Generic;

#nullable disable

namespace DAL
{
    public partial class Match
    {
        public int MatchId { get; set; }
        public int? UserId { get; set; }
        public int? Duration { get; set; }
        public DateTime? DateTime { get; set; }
        public int? Score { get; set; }
        public bool? Result { get; set; }
        public long? Layout { get; set; }
        public int? Turns { get; set; }
        public string CustomPicture { get; set; }

        public virtual User User { get; set; }
    }
}
