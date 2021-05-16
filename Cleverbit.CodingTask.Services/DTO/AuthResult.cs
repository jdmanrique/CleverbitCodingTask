using System;
using System.Collections.Generic;
using System.Text;

namespace Cleverbit.CodingTask.Services.DTO
{
    public class AuthResult
    {
        public bool IsAuthenticated { get; set; }
        public User UserData { get; set; }
        public string AuthErrorMessage { get; set; }
    }
}
