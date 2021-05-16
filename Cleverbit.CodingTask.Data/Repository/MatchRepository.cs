using Cleverbit.CodingTask.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data.Repository
{
    public class MatchRepository: Repository<Match>, IMatchRepository
    {
        public MatchRepository(CodingTaskContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<Match>> GetAllMatches()
        {
            return await this.dbContext.Matches.ToListAsync();
        }

    }
}
