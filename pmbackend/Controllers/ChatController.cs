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

    [HttpGet("UserChats")]
    public IActionResult GetAllUserChats()
    {
        var user = _userManager
            .FindByNameAsync(User.FindFirst(ClaimTypes.Name)!.Value)
            .GetAwaiter().GetResult();

        var chats = _chatRepository.GetChatsForUser(user);

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

    //TODO Fix this?
    //Needs to be done through usernames only.
    public IActionResult SendMessageToChat(MessageDto incomingMsg, string targetUsername)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value;
        var user = _userManager.FindByNameAsync(username).GetAwaiter().GetResult();
        var trgt = _userManager.FindByNameAsync(targetUsername).GetAwaiter().GetResult();

        var msg = _mapper.Map<Message>(incomingMsg);
        
        if (_chatRepository.AddMessageToChat(incomingMsg.ChatId, msg))
            return Ok("MESSAGE ADDED TO DATABASE!");
        
        //Create chat 
        var chat = new Chat
        {
            IsVisible = true,
            Messages = new List<Message> { msg },
            Users = new List<PmUser> { user, trgt }
        };

        if (_chatRepository.AddChat(chat))
        {
            return Ok(_mapper.Map<ChatDto>(chat));
        }

        return BadRequest("Request could not be parsed!");
        //Do nothing and append to db as entry.
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