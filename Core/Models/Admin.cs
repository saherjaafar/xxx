using System;

namespace Core.Models
{
    public class Admin
    {
        public long ID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Guid LoginToken { get; set; }
        public DateTime LoginTokenExpiry { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLocked { get; set; }
        public AdminRole AdminRole { get; set; }
        public long AdminRoleId { get; set; }
        public bool IsFirstLogin { get; set; }
    }
}
