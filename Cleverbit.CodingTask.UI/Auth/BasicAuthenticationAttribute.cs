using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cleverbit.CodingTask.UI.Auth
{
    public class BasicAuthenticationAttribute : AuthorizeAttribute
    {
        public BasicAuthenticationAttribute()
        {
            Policy = "BasicAuthentication";
        }
    }
}
