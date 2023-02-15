using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto___Public
{
    public class PublicUserDto
    {
    }

    public class PublicUserRegistrationDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string Password { get; set; }
    }

    public class PublicUserLoginDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }

    public class PublicUserLoginResponseDto
    {
        public long UserId { get; set; }
        public string Token { get; set; }
    }

    public class PublicUserEmailVerificationDto
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
    }
}
