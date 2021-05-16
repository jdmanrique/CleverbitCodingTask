using Cleverbit.CodingTask.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data.Repository
{
    public class UserMatchRepository : Repository<UserMatch>, IUserMatchRepository
    {

        public UserMatchRepository(CodingTaskContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<UserMatch>> GetAllUserMatches()
        {
            return await this.dbContext.UserMatches.ToListAsync();
        }

        public async Task<List<UserMatch>> GetUserMatchesByMatchId(int matchId)
        {
            var list = this.dbContext.UserMatches.Where(x => x.MatchId == matchId);
            return await list.ToListAsync();
        }

        public async Task<UserMatch> GetUserMatchByUserIdMatchId(int userId, int matchId)
        {
            var match = await this.dbContext.UserMatches.Where(x => x.MatchId == matchId && x.UserId == userId).FirstOrDefaultAsync();

            return match;
        }

        public async Task AddUserMatch(UserMatch userMatch)
        {
            await this.dbContext.UserMatches.AddAsync(userMatch);
            await this.dbContext.SaveChangesAsync();
        }

    }
}
