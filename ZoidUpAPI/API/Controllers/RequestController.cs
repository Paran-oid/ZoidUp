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

        [HttpPost()]
        public async Task<ActionResult<string>> SendRequest([FromQuery] int SenderID, [FromQuery] int receiverID)
        {
            string result = await _rfs.SendRequest(SenderID, receiverID);
            if (result == "request doesn't exist")
            {
                return NotFound(result);
            }
            return Ok(result);
        }

        [HttpDelete()]
        public async Task<ActionResult<string>> RemoveRequest([FromQuery] int senderID, [FromQuery] int receiverID)
        {
            string result = await _rfs.RemoveRequest(senderID, receiverID);
            if (result == "request doesn't exist")
            {
                return NotFound(result);
            }

            return Ok(result);
        }

        //test this controller then do the message controller and do friendship controller
    }
}
