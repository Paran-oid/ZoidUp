using API.Data.Services.FriendshipService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/friends")]
    [Authorize]
    [ApiController]
    public class FriendshipController : ControllerBase
    {
        private readonly IFriendshipService _friendshipService;

        public FriendshipController(IFriendshipService friendshipService)
        {
            _friendshipService = friendshipService;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetAllFriends(int userId)
        {
            try
            {
                var friends = await _friendshipService.GetAllFriends(userId);
                return Ok(friends);
            }
            catch (Exception e)
            {
                if (e.Message == "user doesn't exist")
                {
                    return NotFound(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }

        [HttpDelete("{userId}/{friendId}")]
        public async Task<IActionResult> RemoveFriendship(int userId, int friendId)
        {
            try
            {
                string result = await _friendshipService.RemoveFriendship(userId, friendId);
                return Ok(new { message = result });
            }
            catch (Exception e)
            {
                if (e.Message == "request doesn't exist")
                {
                    return NotFound(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }
    }
}
