using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pmbackend.Models.Dto;

namespace pmbackend.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessageController: Controller
{
    private readonly IMessageRepository _messageRepository;
    private readonly IMapper _mapper;
    public MessageController(IMessageRepository messageRepository, IMapper mapper)
    {
        _messageRepository = messageRepository;
        _mapper = mapper;
    }

    [HttpPost]
    public IActionResult SendMessage(MessageDto messageDto)
    {
        return Ok();
    }

}