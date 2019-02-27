using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProjectPlanner.Api.Models
{
    public class User
    {
        public long Id { get; set; }
        public string Username { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }

        [NotMapped]
        public string Password { get; set; }
    }

    public class JwtBody
    {
        public string Token { get; set; }
    }
}
