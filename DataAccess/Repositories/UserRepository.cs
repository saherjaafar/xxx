using AutoMapper;
using Common.ApiRequest;
using Common.Utilities;
using Core.Cache;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.EmailManager;
using Core.Models;
using Core.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using wedcoo_api.Authentication;

namespace DataAccess.Repositories
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private readonly IConfiguration _configuration;
        private readonly IJwtAuthentication _jwt;
        public UserRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest, IConfiguration configuration, IJwtAuthentication jwt) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _configuration = configuration;
            _jwt = jwt;
        }

        public User Get(params Expression<Func<User, bool>>[] conditions)
        {
            var query = _context.Users.AsQueryable();
            foreach (var condition in conditions)
                query.Where(condition);
            return query.FirstOrDefault();
        }

        public User GetEntity(long userId, params Expression<Func<User, object>>[] includes)
        {
            var query = _context.Users.AsQueryable();
            return includes
                .Aggregate(
                    query.AsQueryable(),
                    (current, include) => current.Include(include)
                )
                .FirstOrDefault(e => e.Id == userId);
        }

        public TResponseVM<PublicUserLoginResponseDto> Login(PublicUserLoginDto body)
        {
            var user = Get(i => i.Email == body.Email);
            if (user is null)
                return new TResponseVM<PublicUserLoginResponseDto> { HasError = true, StatusCode = 404, Message = "Invalid Email or Password" };

            if (!UtilClass.ValidateHash(UtilClass.DecryptFrontByAES(body.Password), user.Password))
                return new TResponseVM<PublicUserLoginResponseDto> { HasError = true, StatusCode = 404, Message = "Invalid Email or Password" };

            if (!user.IsConfirmed)
            {
                user.EmailConfirmationToken = Guid.NewGuid();
                _unitOfWork.SaveChanges();
                var link = _configuration["Environment"] + "Verification/"  + user.Email + "/" + user.EmailConfirmationToken.ToString();
                user.EmailConfirmationTokenExpriry = DateTime.Now.AddMinutes(15);
                _unitOfWork.SaveChanges();
                EmailManager.SendUserVerificationEmail(user.Email, link, _configuration["EmailTemplatePath"],"Email Verification");
                return new TResponseVM<PublicUserLoginResponseDto> { HasError = true, Message = "Verification needed, an email was send to your email", StatusCode = 401 };
            }

            var token = _jwt.UserAuthentication(user.Email);
            PublicUserLoginResponseDto res = new PublicUserLoginResponseDto { UserId = user.Id, Token = token };
            return new TResponseVM<PublicUserLoginResponseDto> { HasError = false, obj = res, StatusCode = 200 };
        }

        public TResponseVM<ResponseVM> UserRegistration(PublicUserRegistrationDto body)
        {
            var checkUser = Get(i => i.Email == body.Email.Trim() || i.PhoneNumber == body.PhoneNumber);
            if(checkUser is not null)
                return new TResponseVM<ResponseVM> { HasError= true, StatusCode = 409, Message = "Email or phone number already register for another account" };

            User user = new User
            {
                Email = body.Email.Trim(),
                FirstName = body.FirstName,
                LastName = body.LastName,
                Password = UtilClass.HashText(UtilClass.DecryptFrontByAES(body.Password)),
                PhoneNumber = body.PhoneNumber,
                RegistrationDate = DateTime.Now,
                IsConfirmed = false,
                IsFacebookUser = false,
                IsGoogleUser = false,
                EmailConfirmationToken = Guid.NewGuid(),
                EmailConfirmationTokenExpriry = DateTime.Now.AddHours(1),
                ResetPasswordToken = Guid.NewGuid(),
                ResetPasswordTokenExpiry = DateTime.Now,
            };
            _context.Users.Add(user);
            _context.SaveChanges();

            var link = _configuration["Environment"] + "Verification/" + body.Email + "/" + user.EmailConfirmationToken.ToString();
            user.EmailConfirmationTokenExpriry = DateTime.Now.AddMinutes(15);
            _unitOfWork.SaveChanges();
            var res = EmailManager.SendUserVerificationEmail(body.Email, link, _configuration["EmailTemplatePath"], "Wedcoo Account Created !");
            user.Note = res;
            _unitOfWork.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Account Create Successfyll" };
        }

        public TResponseVM<PublicUserEmailVerificationDto> EmailVerification(PublicUserEmailVerificationDto body)
        {
            var user = Get(i => i.Email == body.Email,i => i.EmailConfirmationToken == body.Token);
            if (user is null)
                return new TResponseVM<PublicUserEmailVerificationDto> { HasError = false, StatusCode = 404, Message = "Invalid Request" };

            if(user.EmailConfirmationTokenExpriry < DateTime.Now)
            {
                user.EmailConfirmationToken = Guid.NewGuid();
                _unitOfWork.SaveChanges();
                var link = _configuration["Environment"] + "Verification/" + user.Email + "/" + user.EmailConfirmationToken.ToString();
                user.EmailConfirmationTokenExpriry = DateTime.Now.AddMinutes(15);
                _unitOfWork.SaveChanges();
                EmailManager.SendUserVerificationEmail(user.Email, link, _configuration["EmailTemplatePath"], "Email Verification");
                return new TResponseVM<PublicUserEmailVerificationDto> { HasError = true, StatusCode = 401, Message = "Expired Token !! New verification email was sent to your email" };
            }

            user.IsConfirmed= true;
            user.EmailConfirmationTokenExpriry = DateTime.Now;
            _unitOfWork.SaveChanges();
            return new TResponseVM<PublicUserEmailVerificationDto> { HasError = false, StatusCode = 200, Message = "Account Activated Successfully" };
        }
    }
}
