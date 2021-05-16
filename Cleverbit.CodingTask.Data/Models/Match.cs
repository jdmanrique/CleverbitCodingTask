using System;
using System.Collections.Generic;
using System.Text;

namespace Cleverbit.CodingTask.Data.Models
{
    public class Match
    {
        public int MatchId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public int? MatchWinnerUserId { get; set; }
    }
}
