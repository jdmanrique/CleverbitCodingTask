using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cleverbit.CodingTask.Services;
using Cleverbit.CodingTask.Services.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cleverbit.CodingTask.UI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize("BasicAuthentication")]
    public class UserController : ControllerBase
    {

        private readonly IUserService userService;
        private readonly ILogger<UserController> logger;

        public UserController(IUserService _userService, ILogger<UserController> _logger)
        {
            this.userService = _userService;
            this.logger = _logger;
        }

        [HttpPost]
        [Route("Authenticate")]
        [AllowAnonymous]
        public async Task<ActionResult> Authenticate(UserInfo info)
        {
            try
            {
                var result = await this.userService.Authenticate(info.UserName, info.Password);
                if (result.IsAuthenticated)
                {
                    return this.Ok(result);
                }
                else
                {
                    return this.Unauthorized(result);
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                return this.BadRequest();
            }
        }


    }
}