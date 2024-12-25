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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ChatMessageDTO>>> GetAllMessages()
    {
        try
        {
            var messages = await _chatService.GetAllMessagesAsync();
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving messages");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ChatMessageDTO>> GetMessage(int id)
    {
        try
        {
            var message = await _chatService.GetMessageByIdAsync(id);
            if (message == null) return NotFound();
            return Ok(message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving message {MessageId}", id);
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
            _logger.LogError(ex, "Error retrieving conversation between users {User1Id} and {User2Id}",
                user1Id, user2Id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPost]
    public async Task<ActionResult<ChatMessageDTO>> SendMessage(CreateChatMessageDTO messageDto)
    {
        try
        {
            var message = await _chatService.SendMessageAsync(messageDto);
            return CreatedAtAction(nameof(GetMessage), new { id = message.MessageId }, message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message");
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteMessage(int id)
    {
        try
        {
            await _chatService.DeleteMessageAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting message {MessageId}", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpPut("{id}/read")]
    public async Task<IActionResult> MarkAsRead(int id)
    {
        try
        {
            await _chatService.MarkMessageAsReadAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error marking message {MessageId} as read", id);
            return StatusCode(500, "Internal server error");
        }
    }

    [HttpGet("unread/{userId}")]
    public async Task<ActionResult<IEnumerable<ChatMessageDTO>>> GetUnreadMessages(int userId)
    {
        try
        {
            var messages = await _chatService.GetUnreadMessagesAsync(userId);
            return Ok(messages);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving unread messages for user {UserId}", userId);
            return StatusCode(500, "Internal server error");
        }
    }
}