using API.Data.Services.MessageService;
using API.Models;
using API.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/messages")]
    [ApiController]
    [Authorize]
    public class MessageController : ControllerBase
    {
        private readonly IMessageService _messageService;
        public MessageController(IMessageService messageService)
        {
            _messageService = messageService;
        }

        [HttpGet("{messageId}")]
        public async Task<IActionResult> Get(int messageId)
        {
            try
            {
                var message = await _messageService.Get(messageId);
                return Ok(message);
            }
            catch (Exception e)
            {
                if (e.Message == "message wasn't found")
                {
                    return NotFound(new { message = e.Message });
                }
                throw;
            }
        }

        [HttpGet("users/{userId}")]
        public async Task<IActionResult> GetAll(int userId)
        {
            try
            {
                var messages = await _messageService.GetAll(userId);
                return Ok(messages);
            }
            catch (Exception e)
            {
                if (e.Message == "user not found")
                {
                    return NotFound(new { message = e.Message });
                }
                else
                {
                    return Conflict();
                }
            }
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] CreateMessageDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = await _messageService.Post(model);
                    return CreatedAtAction(nameof(Get), message);
                }
                return BadRequest(new { message = "something went wrong" });
            }
            catch (Exception e)
            {
                if (e.Message == "user not found")
                {
                    return NotFound(new { message = "user not found" });
                }
                else
                {
                    return Conflict();
                }
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] EditMessageDTO model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var message = await _messageService.Put(model);
                    return Ok(message);
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                if (e.Message == "message wasn't found")
                {
                    return NotFound(new { message = "message not found" });
                }
                else
                {
                    return Conflict();
                }
            }
        }

        [HttpDelete("{messageId}")]
        public async Task<IActionResult> Delete(int messageId)
        {
            try
            {
                var result = await _messageService.Delete(messageId);
                return Ok(result);
            }
            catch (Exception e)
            {

                if (e.Message == "message not found")
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
