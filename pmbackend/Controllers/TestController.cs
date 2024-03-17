using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using pmbackend.Models.Dto;
using System.Drawing;
using pmbackend.ErrorTypes;
using System.Security.Claims;

namespace pmbackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController: ControllerBase
    {
        private readonly IAuthService _authService;  

        public TestController(IAuthService authService) { _authService = authService; }

        /// <summary>
        /// Simple get function to test out wether or not the controller can extract data from the JSON Web Token (JWT).
        /// </summary>
        /// <returns></returns>
        [HttpGet]        
        public dynamic Get()
        {
            var userId = User.FindFirst(ClaimTypes.Name)?.Value;

            if(userId == null)
            {
                return "BRO THIS IS NOT WORKING";
            }

            return new { UserId = userId};
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string username)
        { 
            //Searches for the role of the user that is trying to delete a target user from username
            var role = User.FindFirst(ClaimTypes.Role)?.Value;
            if (role == null)
            {
                return BadRequest("Username not found!");
            }

            var status = await _authService.DeleteUser(role, username);
            

            //Checks status, and based on previous action returns a corresponding HTTP response.
            switch(status)
            {
                case ErrorType.USER_NOT_FOUND:
                    return BadRequest("User Not found!");
                case ErrorType.USER_DELETED:
                    return Ok("User has been deleted succesfully!");
                case ErrorType.UNPRIVILEGED_ATTEMPT:
                    return BadRequest("Unprivileged attempt to delete account");
                default:
                    return BadRequest($"Unable to delete user: {username}");
            }
        }
        
    }
}
