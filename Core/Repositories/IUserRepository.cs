using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUserRepository : IBaseRepository<User>
    {
        public User GetEntity(long userId, params Expression<Func<User, object>>[] includes);
        public User Get(params Expression<Func<User, bool>>[] conditions);
        TResponseVM<ResponseVM> UserRegistration(PublicUserRegistrationDto body);
        TResponseVM<PublicUserLoginResponseDto> Login(PublicUserLoginDto body);
        TResponseVM<PublicUserEmailVerificationDto> EmailVerification(PublicUserEmailVerificationDto body);
    }
}
