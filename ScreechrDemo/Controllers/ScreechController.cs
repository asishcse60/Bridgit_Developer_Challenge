using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ScreechrDemo.Api.Validators;
using ScreechrDemo.Business.Core.Interface;
using ScreechrDemo.Contracts.Constants;
using ScreechrDemo.Contracts.Model;
using ScreechrDemo.Databases.Utils;

namespace ScreechrDemo.Api.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]
    [Consumes("application/json")]
    [Produces("application/json")]
    // [Authorize]
    public class ScreechController : Controller
    {
        private readonly ILogger<ScreechController> _logger;
        private readonly IScreechService _screechService; 
        public ScreechController(IScreechService screechService,ILogger<ScreechController> logger)
        {
            _logger = logger;
            _screechService = screechService;
        }


        [HttpPost("add")]
        public async Task<IActionResult> AddScreech([FromBody]ScreechModel screech)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res = await _screechService.AddScreechAsync(screech);
            if (res.Result == Result.SUCCESS)
            {
                return Created("/screech/{id}", new { Id = res.Screech.Id });
            }
            return BadRequest();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetScreech(ulong id)
        {
            if (id <= 0)
            {
                return BadRequest();
            }
            var result = await _screechService.GetScreechrByIdAsync(id);
            if (result.Result == Result.SUCCESS)
            {
                return Ok(result.Screech);
            }
            return BadRequest();
        }

        [HttpPatch("update-text")]
        public async Task<IActionResult> UpdateScreech([FromBody] UpdateScreechModel model, ulong id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var res = await _screechService.UpdateScreechrContentByIdAsync(id, model);
            if (res.Result == Result.SUCCESS)
            {
                return NoContent();
            }

            return BadRequest();
        }

        [HttpGet("all-screech")]
        [AllowAnonymous]
        public async Task<IActionResult> AllScreech(ulong? userId, int pageNumber = FieldLimit.DEFAULT_PAGE_NUM,  int pageSize = FieldLimit.DEFAULT_PAGE_SIZE,  string sortOrder = FieldLimit.SortDsc)
        {
            var pageModel = new PageModel
            {
                UserId = userId,
                PageNo = pageNumber,
                PageSize = pageSize,
                SortDir = sortOrder
            };

            var validator = new PageModelValidation();
            var vResult =validator.ValidateAsync(pageModel);
            if(vResult?.Result.IsValid == false) return BadRequest();

            var res = _screechService.GetAllScreechAsync(pageModel);

            return Ok(res.Results);

        }

        [HttpGet("health")]
        public async Task<IActionResult> GetHealth()
        {
            return Ok("Api is healthy!");
        }
    }
}
