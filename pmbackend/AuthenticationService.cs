﻿using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using pmbackend.Models;
using pmbackend.Models.Dto;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using pmbackend.ErrorTypes;

namespace pmbackend
{
    /// <summary>
    /// Service created to implement the basic functions to authenticate a user.
    /// </summary>
    public class AuthenticationService : IAuthService
    {


        private readonly UserManager<PmUser> _userManager;
        private readonly IConfiguration _configuration;
        public AuthenticationService(UserManager<PmUser> userManager, IConfiguration configuration) {  _userManager = userManager; _configuration = configuration; }

        /// <summary>
        /// This function registers the user through morphing it into an identitUser data class
        /// The identityUser class is used by Entityframework to make code-first implementations for database interaction.
        /// </summary>
        /// <param name="pmUser">The user that is registering</param>
        /// <returns>A result depending on if the user is added or not</returns>
        public async Task<bool> RegisterUser(PmUserDto pmUser)
        {
            var hashedPW = BCrypt.Net.BCrypt.EnhancedHashPassword(pmUser.Password);

            var identityUser = new PmUser    
            {
                UserName = pmUser.Username,
                Email = pmUser.Username
            };

            var result = await _userManager.CreateAsync(identityUser, pmUser.Password);

            return result.Succeeded;
        }

        /// <summary>
        /// Function used for logging a user in.
        /// If a user has logged in, then a token gets created to go along with him.
        /// </summary>
        /// <param name="pmUser">User that is logging in</param>
        /// <returns>A result to check if the credentials are correct.</returns>
        public async Task<bool> Login(PmUserDto pmUser)
        {
            var user = await _userManager.FindByNameAsync(pmUser.Username);
           
            if (user is null)
            {
                return false;
            }

            return await _userManager.CheckPasswordAsync(user, pmUser.Password);
        }

        /// <summary>
        /// Generates a JSON Web Token to pass to the user.
        /// A JWT consist of multiple parts to properly be used for authentication.
        /// First off a claim gets setup so that the token has the username and corresponding role as data to be retrieved 
        /// when using a controller for instance.
        /// Then a security key gets created through using the pre-existing libraries for this.
        /// The Credentials are signed by the combination of a security key and algorithm to encrypt it.
        /// Finally the token gets setup with the data and returned once finished.
        /// </summary>
        /// <param name="pmUser">The user to generate the token for</param>
        /// <returns>A JWT to use for authentication within the API</returns>
        public string GenerateTokenString(PmUserDto pmUser)
        {
            IEnumerable<Claim> customClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, pmUser.Username),
                new Claim(ClaimTypes.Role, "User"),
                // new Claim(ClaimTypes.)
            };

            
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("Jwt:Key").Value));

            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityToken = new JwtSecurityToken(
                claims: customClaims,
                expires: DateTime.Now.AddDays(2),
                issuer: _configuration.GetSection("Jwt:Issuer").Value,
                audience: _configuration.GetSection("Jwt:Audience").Value,
                signingCredentials: signingCredentials);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(securityToken);

            return tokenString; 
        }

        /// <summary>
        /// Deletes a specified user, only with the proper 
        /// It uses an enum ErrorType to determine what kind of error is occuring inside of the function.
        /// Used for easier backtracing when debugging this part of the code.
        /// </summary>
        /// <param name="claim">Specifically a role that the user has</param>
        /// <param name="username">The user to delete</param>
        /// <returns>An ErrorType corresponding to the error that has occured</returns>
        public async Task<ErrorType> DeleteUser(string claim, string username)
        {
            var user = await _userManager.FindByNameAsync(username);

            if(user == null)
            {
                return ErrorType.USER_NOT_FOUND;
            }

            if(claim == "User")
            {
                await _userManager.DeleteAsync(user);
                return ErrorType.USER_DELETED;
            }

            return ErrorType.UNPRIVILEGED_ATTEMPT;
        }
    }
}
