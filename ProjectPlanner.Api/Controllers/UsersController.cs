using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
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
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserContext _context;
        private readonly IConfiguration _config;

        public UsersController(UserContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.Select(u => GetUserColumns(u)).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(long id)
        {
            return await _context.Users.Select(u => GetUserColumns(u)).Where(u => u.Id == id).SingleAsync();
        }

        private User GetUserColumns(User u)
        {
            return new User
            {
                Id = u.Id,
                EmailAddress = u.EmailAddress,
                Username = u.Username,
                FirstName = u.FirstName,
                LastName = u.LastName
            };
        }
    }
}