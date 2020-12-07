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
        /// <param name="matchId"> match ID.</param>
        /// <param name="duration"> duration.</param>
        /// <param name="score"> score.</param>
        /// <param name="turns"> turns.</param>
        /// <param name="date"> date.</param>
        /// <param name="size"> match size.</param>
        public MatchHistoryLine(int? matchId, int? duration, int? score, int? turns, DateTime? date, int? size)
        {
            this.MatchId = matchId;
            this.Duration = duration;
            this.Score = score;
            this.Turns = turns;
            this.Date = date;
            this.Size = size;
        }

        /// <summary>
        /// gets or sets match duration.
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// gets or sets score.
        /// </summary>
        public int? Score { get; set; }

        /// <summary>
        /// gets or sets turns count.
        /// </summary>
        public int? Turns { get; set; }

        /// <summary>
        /// gets or sets time and date when match was played.
        /// </summary>
        public DateTime? Date { get; set; }

        /// <summary>
        /// gets or sets match field size.
        /// </summary>
        public int? Size { get; set; }

        /// <summary>
        /// gets or sets match ID.
        /// </summary>
        public int? MatchId { get; set; }
    }
}
