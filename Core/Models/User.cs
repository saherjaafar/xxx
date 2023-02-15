using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class User
    {
        public long Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool IsGoogleUser { get; set; }
        public bool IsFacebookUser { get; set; }
        public DateTime RegistrationDate { get; set; }
        public Guid ResetPasswordToken { get; set; }
        public DateTime ResetPasswordTokenExpiry { get; set; }
        public Guid EmailConfirmationToken { get; set; }
        public DateTime EmailConfirmationTokenExpriry { get; set; }
        public bool IsConfirmed { get; set; }
        public string Note { get; set; }

        public virtual ICollection<UserFavorite> Favorites { get; set; }
    }
}
