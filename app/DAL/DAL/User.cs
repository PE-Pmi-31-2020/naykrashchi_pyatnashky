// <copyright file="User.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

#nullable disable

namespace DAL
{
    using System.Collections.Generic;

    /// <summary>
    /// Class user for storing in database.
    /// </summary>
    public partial class User
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        public User()
        {
            this.Matches = new HashSet<Match>();
        }

        /// <summary>
        /// Gets or sets user id primary key.
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// Gets or sets login.
        /// </summary>
        public string Login { get; set; }

        /// <summary>
        /// Gets or sets password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets matches for relations.
        /// </summary>
        public virtual ICollection<Match> Matches { get; set; }
    }
}
