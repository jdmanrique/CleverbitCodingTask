using System;
using System.Threading.Tasks;
using Cleverbit.CodingTask.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Cleverbit.CodingTask.Host.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize("BasicAuthentication")]
    public class NumberGameController : ControllerBase
    {
        private readonly INumberGameService service;
        private readonly ILogger<NumberGameController> logger;

        public NumberGameController(INumberGameService _service, ILogger<NumberGameController> _logger)
        {
            this.service = _service;
            this.logger = _logger;
        }

        [HttpGet]
        [Route("GetExpiredMatches")]
        [AllowAnonymous]
        public async Task<ActionResult> GetExpiredMatches()
        {
            try
            {
                var matches = await this.service.GetExpiredMatches();
                return this.Ok(matches);
            }
            catch(Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                return this.BadRequest();
            }
        }

        [HttpGet]
        [Route("GetActiveMatch")]
        public async Task<ActionResult> GetActiveMatch(int userId)
        {
            try
            {
                var match = await this.service.GetActiveMatch(userId);
                return this.Ok(match);
            }
            catch(NullReferenceException ex)
            {
                this.logger.LogError(ex.Message, ex);
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                return this.BadRequest();
            }
        }

        [HttpPost]
        [Route("AddUserMatch")]
        public async Task<ActionResult> AddUserMatch([FromBody]int userId)
        {
            try
            {
                var input = await this.service.PlayNow(userId);
                return this.Ok(input);
            }
            catch (NullReferenceException ex)
            {
                this.logger.LogError(ex.Message, ex);
                return this.NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                this.logger.LogError(ex.Message, ex);
                return this.BadRequest();
            }
        }


    }
}