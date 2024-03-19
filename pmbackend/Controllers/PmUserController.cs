using System.ComponentModel;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using pmbackend.ErrorTypes;
using pmbackend.Models;
using pmbackend.Models.Dto;

namespace pmbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //TODO In production re-enable this, for testing no need to authorize
    // [Authorize]
    public class PmUserController : Controller
    {
        private readonly IPmUserRepository _userRepository;
        private readonly UserManager<PmUser> _userManager;
        private readonly IMapper _mapper;

        public PmUserController(IPmUserRepository userRepository, IMapper
                mapper,
            UserManager<PmUser> manager)
        {
            _userRepository = userRepository;
            _mapper = mapper;
            _userManager = manager;
        }


        [HttpGet("AllUsers")]
        public IActionResult GetAllUsers(string? searchTarget = null)
        {
            var list = _mapper.Map<List<PmUserDto>>(_userRepository.GetAllUsers());

            if (list is null)
                return BadRequest("No users has been found!");

            if (searchTarget.IsNullOrEmpty())
            {
                return Ok(list);
            }
            
            var newList = list.FindAll(usr => usr.Username.ToLower().Contains
                (searchTarget!.ToLower()));

            return Ok(newList);
        }

        [HttpGet("GetUser")]
        public IActionResult GetUser(string username)
        {
            //TODO reset Auth to handle this.
            // if (username == User.FindFirst(ClaimTypes.Name)!.Value)
            // {
            // return BadRequest("You can't type in your own name, damnit!");
            // }
            var user =
                _mapper.Map<PmUserDto>(_userManager.FindByNameAsync(username)
                    .GetAwaiter().GetResult());

            if (user is null)
                return BadRequest("User has not been found!");

            return Ok(user);
        }

        [HttpPost("UpdateUser")]
        public IActionResult UpdateUser(string username, PmUserDto user)
        {
            //TODO When updating the token should be enough to identify the user that is being updated!
            var userToUpdate = _userManager.FindByNameAsync(username).GetAwaiter()
                .GetResult();
            
            if (userToUpdate is null)
                return NotFound();

            //Map User data from and to the user to update
            _mapper.Map(user, userToUpdate);
            
            return _userManager.UpdateAsync(userToUpdate).GetAwaiter().GetResult()
                .Succeeded
                ? Ok("User has been updated!")
                : BadRequest("User has not been updated!");
        }

        [HttpPost("AddFriend")]
        public IActionResult AddFriend(string username, string targetUser)
        {
            var error = _userRepository.AddFriend(username, targetUser);
            if (error is ErrorType.USER_NOT_FOUND)
            {
                return BadRequest("User has not been added to the list!");
            }

            return Ok("Friend Successfully added!");
        }

        [HttpPost("RemoveFriend")]
        public IActionResult RemoveFriend(string username, string targetUser)
        {
            var error = _userRepository.RemoveFriend(username, targetUser);
            if (error is ErrorType.USER_NOT_FOUND)
            {
                return BadRequest("User has not been found to delete!");
            }

            return Ok("Friend Successfully removed!");
        }
    }
}