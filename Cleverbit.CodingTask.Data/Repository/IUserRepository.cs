using Cleverbit.CodingTask.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data.Repository
{
    public interface IUserRepository: IRepository<User>
    {
        Task<List<User>> GetAllUsers();
        Task<User> GetUserById(int userId);
        Task<User> GetUserByUserName(string userName);

    }
}
