using System;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
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
    [ApiController]
    public class AccountsController : ControllerBase
    {
        private UserContext _userContext;
        private IConfiguration _config;

        public AccountsController(UserContext userContext, IConfiguration config)
        {
            _userContext = userContext;
            _config = config;
        }

        [HttpPost, AllowAnonymous]
        [Route("api/[controller]/register")]
        public async Task<ActionResult<User>> Register([FromBody] JwtBody jwt)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            TokenValidationParameters validationParameters = new TokenValidationParameters
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateLifetime = false,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]))
            };

            SecurityToken validatedToken;
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();

            try
            {
                ClaimsPrincipal userClaims = handler.ValidateToken(jwt.Token, validationParameters, out validatedToken);
                var user = TokenData.MapTokenDataToObject<User>(userClaims);

                var result = new UserValidator().Validate(user);

                if (!result.IsValid)
                {
                    return BadRequest(result.Errors);
                }

                user.PasswordSalt = AuthenticationHelper.GenerateRandomByteArray(user.Password.Length);
                user.PasswordHash = AuthenticationHelper.GenerateSaltedHash(AuthenticationHelper.StringToByteArray(user.Password), user.PasswordSalt);

                try
                {
                    _userContext.Users.Add(user);
                    await _userContext.SaveChangesAsync();

                    return Ok(GetUserColumns(user));
                }
                catch (DbUpdateException e)
                {
                    var innerException = e.InnerException as SqlException;
                    if (innerException != null && innerException.Number == 2601)
                    {
                        return BadRequest(ExceptionHelpers.HandleUniqueIndex(innerException.Message, "IX_Users"));
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            catch (SecurityTokenInvalidSignatureException)
            {
                return Unauthorized("Invalid Token");
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
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