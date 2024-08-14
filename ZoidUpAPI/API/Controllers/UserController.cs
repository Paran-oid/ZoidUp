using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;
using API.Data.Services.UserService;
using API.Models;
using API.Models.Others.AuthRelated;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController(IUserService _userService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult> GetUser([FromHeader] string token)
        {
            try
            {
                var result = await _userService.GetUser(token);

                if (result == null)
                {
                    return NotFound("User not found");
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                return NotFound("User not found");
            }
        }


        [HttpGet]
        public async Task<ActionResult<AccessTokenResponse>> Login([FromQuery] LoginEntry model)
        {
            var result = await _userService.Login(model);
            if (result == null)
            {
                return BadRequest("This user doesn't exist");
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<string>> Logout()
        {
            await HttpContext.SignOutAsync();
            return Ok("successfully logged out");
        }

        [HttpPost]
        public async Task<ActionResult<AccessTokenResponse>> Register([FromBody] RegisterEntry model)
        {
            var result = await _userService.Register(model);
            if (result == null)
            {
                return BadRequest("This user already exists");
            }
            return Ok(result);
        }
    }
}
