using API.Data.Services.RequestFriendshipService;
using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/requests")]
    [ApiController]
    [Authorize]
    public class RequestController : ControllerBase
    {
        private readonly IRequestFriendshipService _rfs;
        public RequestController(IRequestFriendshipService rfs)
        {
            _rfs = rfs;
        }


        [HttpGet("has/{userId}")]
        public async Task<ActionResult<bool>> HasRequests(int userId)
        {
            bool result = await _rfs.HasRequests(userId);
            return result;
        }

        //add this to friendship controller and service preferably

        //[HttpGet("{userId}")]
        //public async Task<ActionResult<IEnumerable<User>>> GetAllFriends(int userId)
        //{
        //    var friends = await _rfs.GetAllFriends(userId);

        //    if (friends == null)
        //    {
        //        return NotFound("User wasn't found");
        //    }

        //    return Ok(friends);
        //}

        [HttpGet("recommendations/{userId}")]
        public async Task<IActionResult> GetAllRecommended(int userId)
        {
            try
            {
                var users = await _rfs.GetAllRecommended(userId);
                return Ok(users);
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

        [HttpGet("{receiverId}")]
        public async Task<IActionResult> GetAllReceivedRequests(int receiverId)
        {
            var users = await _rfs.GetAllReceivedRequests(receiverId);
            return Ok(users);
        }

        [HttpGet("sent/{senderId}")]
        public async Task<IActionResult> GetAllSentRequests(int senderId)
        {
            var users = await _rfs.GetAllSentRequests(senderId);
            return Ok(users);
        }

        [HttpPost]
        public async Task<IActionResult> SendRequest([FromBody] SendRequestDTO model)
        {
            try
            {
                string result = await _rfs.SendRequest(model.senderId, model.receiverId);
                return CreatedAtAction(nameof(SendRequest), result);
            }
            catch (Exception e)
            {
                if (e.Message == "friendship already exists, can't send request")
                {
                    return Conflict(new { message = e.Message });
                }
                else if (e.Message == "request already sent")
                {
                    return Conflict(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }

        [HttpDelete]
        public async Task<IActionResult> UnsendRequest([FromBody] SendRequestDTO model)
        {
            try
            {
                string result = await _rfs.UnsendRequest(model.senderId, model.receiverId);
                return Ok(result);
            }
            catch (Exception e)
            {
                if (e.Message == "friendship already exists, can't unsend request")
                {
                    return Conflict(new { message = e.Message });
                }
                else if (e.Message == "request doesn't exist")
                {
                    return NotFound(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }


        //add this to friendship controller and service preferably

        //[HttpDelete]
        //public async Task<ActionResult<string>> RemoveFriendship([FromQuery] int userId, [FromQuery] int friendId)
        //{
        //    string result = await _rfs.RemoveFriendship(userId, friendId);
        //    if (result == "request doesn't exist")
        //    {
        //        return NotFound(result);
        //    }

        //    return Ok(result);
        //}

    }
}
