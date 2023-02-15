using System;

namespace wedcoo_api.Authentication
{
    public class AuthenticationResponseVM
    {
        public long AdminId { get; set; }
        public string Email { get; set; }
        public string Token { get; set; }
        public Guid LoginToken { get; set; }
        //public AdminRole Permissions { get; set; }
    }
}
