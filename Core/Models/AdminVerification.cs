using System;

namespace Core.Models
{
    public class AdminVerification
    {
        public long Id { get; set; }
        public Guid EmailVerificationToken { get; set; }
        public DateTime EmailVerificationTokenExpiry { get; set; }
        public bool IsEmailVerified { get; set; }
        public Guid ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiry { get; set; }
        public Admin Admin { get; set; }
        public long AdminId { get; set; }
    }
}
