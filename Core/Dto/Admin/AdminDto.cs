using System;

namespace Core.Dto.Admin
{
    public class AdminDto
    {
    }

    public class UpdateAdminDto
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public long RoleId { get; set; }
    }

    public class AdminsListDto
    {
        public long AdminId { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsLocked { get; set; }
        public string Role { get; set; }
    }

    public class AdminDetailsDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public SelectDto Role { get; set; }
    }

    public class VerifyResetPasswordDto
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string Password { get; set; }
    }

    public class CheckLoginTokenDto
    {
        public string Email { get; set; }
        public string LoginToken { get; set; }
    }

    public class CheckEmailVerificationDto
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
    }

    public class ForgetPasswordDto
    {
        public string Email { get; set; }
    }

    public class ResetPasswordDto
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string Password { get; set; }
    }

    public class AdminVerificationDto
    {
        public string Email { get; set; }
        public Guid Token { get; set; }
        public string Password { get; set; }
    }
}
