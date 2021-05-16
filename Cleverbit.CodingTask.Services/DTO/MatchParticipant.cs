using System;
using System.Collections.Generic;
using System.Text;

namespace Cleverbit.CodingTask.Services.DTO
{
    public class MatchParticipant
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int MatchId { get; set; }
        public int? NumberInput { get; set; }
    }
}
