using Cleverbit.CodingTask.Services.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Services
{
    public interface INumberGameService
    {
        Task<Match> GetActiveMatch(int userId);
        Task<List<Match>> GetExpiredMatches();
        Task<List<MatchParticipant>> GetMatchWinner(int matchId);
        Task<int> PlayNow(int userId);
    }
}
