using Cleverbit.CodingTask.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data.Repository
{
    public interface IMatchRepository: IRepository<Match>
    {
        Task<List<Match>> GetAllMatches();
    }
}
