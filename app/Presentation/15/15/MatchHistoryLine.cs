// <copyright file="MatchHistoryLine.cs" company="OnceDaughtersAlwaysDaughters">
// Copyright (c) OnceDaughtersAlwaysDaughters. All rights reserved.
// </copyright>

namespace Fifteens
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Class for displaying all match in MatchHistoryWindow.
    /// </summary>
    public class MatchHistoryLine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MatchHistoryLine"/> class.
        /// </summary>
        /// <param name="matchHistoryLine">
        /// Database string which information about match.
        /// </param>
        public MatchHistoryLine()
        {
            this.Duration = 10;
            this.Score = 228;
            this.Turns = 4;
            this.Date = DateTime.Now;
        }

        /// <summary>
        /// gets or sets match duration.
        /// </summary>
        public int Duration { get; set; }

        /// <summary>
        /// gets or sets score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        /// gets or sets turns count.
        /// </summary>
        public int Turns { get; set; }

        /// <summary>
        /// gets or sets time and date when match was played.
        /// </summary>
        public DateTime Date { get; set; }
    }
}
