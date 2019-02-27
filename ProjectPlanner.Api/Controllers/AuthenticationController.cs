using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectPlanner.Api.Common.Authentication;
using ProjectPlanner.Api.Common.Helpers;
using ProjectPlanner.Api.Contexts;
using ProjectPlanner.Api.Models;
using ProjectPlanner.Api.Validator;

namespace ProjectPlanner.Api.Controllers
{
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly IConfiguration _config;

        public AuthenticationController(UserContext userContext, IConfiguration config)
        {
            _context = userContext;
            _config = config;
        }

        [HttpPost, AllowAnonymous]
        [Route("api/login")]
        public async Task<ActionResult> Login(AuthenticationBody auth)
        {
            if (!ModelState.IsValid)
                return BadRequest();

            var user = await _context.Users.Where(u => u.Username == auth.Username).FirstOrDefaultAsync();

            if (user == null)
                return NotFound();

            var requestHash = AuthenticationHelper.GenerateSaltedHash(AuthenticationHelper.StringToByteArray(auth.Password), user.PasswordSalt);

            if (!AuthenticationHelper.CompareByteArrays(requestHash, user.PasswordHash))
                return Unauthorized();

            var token = BuildToken(user);

            return Ok(new { token });
        }

        private string BuildToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                new Claim(JwtRegisteredClaimNames.Email, user.EmailAddress),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim("username", user.Username)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.UtcNow.AddDays(7),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}