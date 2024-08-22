using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Principal;
using API.Data.Services.AuthService;
using API.Models;
using API.Models.DTOs.AuthRelated;

namespace API.Controllers
{
    [Route("auth")]
    [ApiController]
    public class AuthController(IAuthService _AuthService) : ControllerBase
    {
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetUser([FromHeader(Name = "Authorization")] string token)
        {
            try
            {
                var result = await _AuthService.GetUser(token);

                if (result == null)
                {
                    return NotFound("User not found");
                }

                return Ok(result);
            }
            catch (Exception e)
            {
                if (e.Message == "invalid token")
                {
                    return BadRequest(new { message = e.Message });
                }

                else if (e.Message == "user not found")
                {
                    return NotFound(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginEntry model)
        {
            try
            {
                var result = await _AuthService.Login(model);
                return Ok(result);
            }
            catch (Exception e)
            {
                if (e.Message == "user not found")
                {
                    return NotFound(new { message = e.Message });
                }

                else if (e.Message == "not same password")
                {
                    return BadRequest(new { message = e.Message });
                }

                else
                {
                    return Conflict();
                }

            }
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterEntry model)
        {
            try
            {
                var result = await _AuthService.Register(model);
                return CreatedAtAction(nameof(Register), result);
            }
            catch (Exception e)
            {
                if (e.Message == "user with username already exists")
                {
                    return BadRequest(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }

        //create logout endpoint
    }
}
