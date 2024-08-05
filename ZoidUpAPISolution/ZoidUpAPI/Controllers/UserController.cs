using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ZoidUpAPI.Data.Services.UserService;
using ZoidUpAPI.Models;
using ZoidUpAPI.Models.Others.AuthRelated;

namespace ZoidUpAPI.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class UserController(IUserService _userService) : ControllerBase
    {

        [HttpGet]
        public async Task<ActionResult<User>> GetUser([FromQuery] string token)
        {
            var user = await _userService.GetUser(token);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<AccessTokenResponse>> Login([FromQuery] LoginEntry model)
        {
            var result = await _userService.Login(model);
            if (result == null)
            {
                return BadRequest("Please enter valid credentials");
            }
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<AccessTokenResponse>> Register([FromBody] RegisterEntry model)
        {
            var result = await _userService.Register(model);
            if (result == null)
            {
                return BadRequest("Please enter valid credentials");
            }
            return Ok(result);
        }
    }
}
