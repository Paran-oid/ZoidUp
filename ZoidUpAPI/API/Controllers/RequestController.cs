using API.Data.Services.RequestFriendshipService;
using API.Models;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IRequestFriendshipService _rfs;
        public RequestController(IRequestFriendshipService rfs)
        {
            _rfs = rfs;
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<List<User>>> GetAllFriends(int userID)
        {
            var friends = await _rfs.GetAllFriends(userID);

            if (friends == null)
            {
                return NotFound("User wasn't found");
            }

            return Ok(friends);
        }

        [HttpGet("{userID}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllRecommendedFriends(int userID)
        {
            var users = await _rfs.GetAllRecommendedFriends(userID);
            if (users == null)
            {
                return NotFound("user wasn't found");
            }
            return Ok(users);

        }

        [HttpGet("{receiverID}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllReceivedRequests(int receiverID)
        {
            var users = await _rfs.GetAllReceivedRequests(receiverID);
            return Ok(users);
        }

        [HttpGet("{senderID}")]
        public async Task<ActionResult<IEnumerable<User>>> GetAllSentRequests(int senderID)
        {
            var users = await _rfs.GetAllSentRequests(senderID);
            return Ok(users);
        }

        [HttpGet]
        public async Task<ActionResult<string>> SendRequest([FromQuery] int SenderID, [FromQuery] int receiverID)
        {
            string result = await _rfs.SendRequest(SenderID, receiverID);
            if (result == "request doesn't exist")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<string>> RemoveRequest([FromQuery] int senderID, [FromQuery] int receiverID)
        {
            string result = await _rfs.RemoveRequest(senderID, receiverID);
            if (result == "request doesn't exist")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        [HttpDelete]
        public async Task<ActionResult<string>> RemoveFriendship([FromQuery] int userID, [FromQuery] int friendID)
        {
            string result = await _rfs.RemoveFriendship(userID, friendID);
            if (result == "request doesn't exist")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

    }
}
