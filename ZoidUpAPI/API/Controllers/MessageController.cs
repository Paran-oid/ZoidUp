using API.Data.Services.MessageService;
using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpDelete("{messageID}")]
        public async Task<ActionResult<string>> Delete(int messageID)
        {
            var result = await _messageService.Delete(messageID);
            if (result == null)
            {
                return NotFound("message wasn't found");
            }

            return Ok(result);
        }

        [HttpGet("{MessageID}")]
        public async Task<ActionResult<Message>> Get(int messageID)
        {
            var message = await _messageService.Get(messageID);
            if (message == null)
            {
                return NotFound("message wasn't found");
            }

            return Ok(message);
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Message>>> GetAll([FromQuery] int userID)
        {
            var messages = await _messageService.GetAll(userID);
            return Ok(messages);
        }

        [HttpPost]
        public async Task<ActionResult<Message>> Post([FromBody] CreateMessageDTO model)
        {
            var message = await _messageService.Post(model);
            if (message == null)
            {
                return Conflict("There was an error creating the message");
            }
            return Ok(message);
        }

        [HttpPut]
        public async Task<ActionResult<Message>> Put([FromBody] EditMessageDTO model)
        {
            var message = await _messageService.Put(model);
            if (message == null)
            {
                return NotFound("message wasn't found");
            }
            return Ok(message);
        }
    }
}
