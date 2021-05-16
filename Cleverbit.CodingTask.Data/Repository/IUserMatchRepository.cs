using Cleverbit.CodingTask.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data.Repository
{
    public interface IUserMatchRepository: IRepository<UserMatch>
    {
        Task<List<UserMatch>> GetUserMatchesByMatchId(int matchId);
        Task<UserMatch> GetUserMatchByUserIdMatchId(int userId, int matchId);
        Task AddUserMatch(UserMatch match);
    }
}
