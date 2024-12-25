using Microsoft.AspNetCore.Mvc;
using EducationManagementSystem.Server.Data.DTOs;
using EducationManagementSystem.Server.Interfaces;

namespace EducationManagementSystem.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ChatMessagesController : ControllerBase
{
    private readonly IChatMessageService _chatService;
    private readonly ILogger<ChatMessagesController> _logger;

    public ChatMessagesController(IChatMessageService chatService, ILogger<ChatMessagesController> logger)
    {
        _chatService = chatService;
        _logger = logger;
    }

    [HttpPost]
    public async Task<ActionResult<ChatMessageDTO>> SendMessage([FromBody] CreateChatMessageDTO messageDto)
    {
        try
        {
            var message = await _chatService.SendMessageAsync(messageDto);
            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("conversation")]
    public async Task<ActionResult<IEnumerable<ChatMessageDTO>>> GetConversation(
        [FromQuery] int user1Id, [FromQuery] int user2Id)
    {
        try
        {
            var messages = await _chatService.GetConversationAsync(user1Id, user2Id);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving conversation");
            return StatusCode(500, "Internal server error");
        }
    }
}
