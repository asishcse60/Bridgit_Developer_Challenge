using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    public class UserProfileController : Controller
    {
        private readonly ILogger<UserProfileController> _logger;
        private readonly IUserProfileService _userProfileService;

        public UserProfileController(IUserProfileService userProfileService,ILogger<UserProfileController> logger)
        {
            _userProfileService = userProfileService;
            _logger = logger;
        }

        [HttpPost("add")]
        [AllowAnonymous]
        public async Task<IActionResult>AddUser([FromBody]UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var res = await _userProfileService.AddUserAsync(user);
            if (res.ResultStatus == Result.SUCCESS)
            {
                return Created("/profile/{userId}", new { id = res.UserProfile.Id });

            }

            return BadRequest();
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetUserProfile(ulong userKey)
        {
            if (userKey <= 0)
            {
                return BadRequest();
            }

            var res = await _userProfileService.GetUserProfileByKeyAsync(userKey);
            if (res.ResultStatus == Result.SUCCESS)
            {
                return Ok(res.UserProfile);

            }

            return BadRequest();
        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileModel model, ulong userKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userProfileService.UpdateProfileAsync(model, userKey);
            if (result.ResultStatus == Result.SUCCESS)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpPatch("update-image")]
        public async Task<IActionResult> UpdateProfilePicture([FromBody] UpdateProfileImageModel model, ulong userKey)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var result = await _userProfileService.UpdateProfilePictureAsync(userKey, model);
            if (result.ResultStatus == Result.SUCCESS)
            {
                return NoContent();
            }
            return BadRequest();
        }

        [HttpGet("all-profile")] // validation: maximum 500
        public async Task<IActionResult> GetAllProfiles(int pageNo = FieldLimit.DEFAULT_PAGE_NUM, int pageSize = FieldLimit.DEFAULT_PAGE_SIZE)
        {

            var res = await _userProfileService.GetProfilesAsync(pageNo, pageSize);
            return Ok(res.Results);
        }
    }
}
