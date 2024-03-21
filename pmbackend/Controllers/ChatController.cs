using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pmbackend.Models;

namespace pmbackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController: Controller
{
    private readonly IChatRepository _chatRepository;
    private readonly UserManager<PmUser> _userManager;

    public ChatController(IChatRepository chatRepository, UserManager<PmUser> userManager)
    {
        _chatRepository = chatRepository;
        _userManager = userManager;
    }

    [HttpGet("Chats")]
    public IActionResult GetAllChats()
    {
        var chats = _chatRepository.GetAllChats();
        return Ok(chats);
    }

    [HttpGet("Chat")]
    public IActionResult GetChat(int id)
    {
        var chat = _chatRepository.GetChat(id);
        return Ok(chat);
    }
    
    [HttpPost("CreateChat")]
    public IActionResult CreateChat(string targetUsername)
    {
        var result = User.FindFirst(ClaimTypes.Name)?.Value;
        var user1 = _userManager.FindByNameAsync(result).GetAwaiter().GetResult();
        var user2 = _userManager.FindByNameAsync(targetUsername).GetAwaiter().GetResult();

        var chat = new Chat
        {
            IsVisible = true, Messages = new List<Message>(), Users =
                new List<PmUser> { user1, user2 }
        };
        
        _chatRepository.AddChat(chat);
        
        return Ok();
    }
}