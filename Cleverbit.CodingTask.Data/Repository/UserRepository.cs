using Cleverbit.CodingTask.Data.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Data.Repository
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        public UserRepository(CodingTaskContext dbContext) : base(dbContext)
        {
        }

        public async Task<List<User>> GetAllUsers()
        {
            return await this.dbContext.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int userId)
        {
            var list = this.dbContext.Users.Where(x => x.Id == userId);
            var item =  await list.ToListAsync();
            return item.FirstOrDefault();
        }

        public async Task<User> GetUserByUserName(string userName)
        {
            var list = this.dbContext.Users.Where(x => x.UserName.ToLower() == userName.ToLower());
            var item = await list.ToListAsync();
            return item.FirstOrDefault();
        }

    }
}
