// <copyright file="Match.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace DAL
{
    using System;

    /// <summary>
    /// class Match for storing in database.
    /// </summary>
    public partial class Match
    {
        /// <summary>
        /// Gets or sets match id primary key.
        /// </summary>
        public int MatchId { get; set; }

        /// <summary>
        /// Gets or sets used id foreign key.
        /// </summary>
        public int? UserId { get; set; }

        /// <summary>
        /// Gets or sets duration.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// Gets or sets date and time.
        /// </summary>
        public DateTime? DateTime { get; set; }

        /// <summary>
        /// Gets or sets score.
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// Gets or sets result.
        /// </summary>
        public bool? Result { get; set; }

        /// <summary>
        /// Gets or sets hashed layout.
        /// </summary>
        public long? Layout { get; set; }

        /// <summary>
        /// Gets or sets turns.
        /// </summary>
        public int? Turns { get; set; }

        /// <summary>
        /// Gets or sets path to picture.
        /// </summary>
        public string CustomPicture { get; set; }

        public virtual User User { get; set; }
    }
}
