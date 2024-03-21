using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatController : Controller
{
    private readonly IChatRepository _chatRepository;
    private readonly UserManager<PmUser> _userManager;
    private readonly IMapper _mapper;

    public ChatController(IChatRepository chatRepository, UserManager<PmUser>
        userManager, IMapper mapper)
    {
        _chatRepository = chatRepository;
        _userManager = userManager;
        _mapper = mapper;
    }

    [HttpGet("Chats")]
    public IActionResult GetAllChats()
    {
        var chats = _mapper.Map<List<ChatDto>>(_chatRepository.GetAllChats());
        return Ok(chats);
    }

    //TODO When there is no chat create one instead.
    [HttpGet("Chat")]
    public IActionResult GetChat(int id)
    {
        var chat = _mapper.Map<ChatDto>(_chatRepository.GetChat(id));
        return Ok(chat);
    }


    [HttpPost("SendMessage")]
    public IActionResult SendMessageToChat(int id,MessageDto incomingMsg)
    {
        var msg = _mapper.Map<Message>(incomingMsg);

        if (!_chatRepository.AddMessageToChat(id, msg))
        {
            return BadRequest("Message not added to DB!");
        }
        
        return Ok("MESSAGE ADDED TO DATABASE!");
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

        return Ok("Chat has been created!");
    }
}