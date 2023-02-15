using Core.Models;

namespace Core.Dto.LoginDto
{
    public class LoginResponseDto
    {
        public long AdminId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public string LoginToken { get; set; }
        public AdminRole Permissions { get; set; }
    }
}
