using System;
using System.Collections.Generic;
using System.Text;

namespace Cleverbit.CodingTask.Data.Models
{
    public class UserMatch
    {
        public int UserMatchId { get; set; }
        public int UserId { get; set; }
        public int MatchId { get; set; }
        public int? NumberInput { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
