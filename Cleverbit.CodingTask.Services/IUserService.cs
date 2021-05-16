using Cleverbit.CodingTask.Services.DTO;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.Services
{
    public interface IUserService
    {
        Task<AuthResult> Authenticate(string userName, string password);
    }
}
