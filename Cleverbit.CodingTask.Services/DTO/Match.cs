using System;
using System.Collections.Generic;
using System.Text;

namespace Cleverbit.CodingTask.Services.DTO
{
    public class Match
    {
        public int MatchId { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string Winner { get; set; }
        public int NumberInput { get; set; }

    }
}
