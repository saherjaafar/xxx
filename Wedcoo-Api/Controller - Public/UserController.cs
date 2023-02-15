using Core;
using Core.Dto___Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("register")]
        public IActionResult UserRegistration([FromBody] PublicUserRegistrationDto body)
        {
            var res = _unitOfWork.Users.UserRegistration(body);
            return StatusCode(res.StatusCode,res.Message);
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] PublicUserLoginDto body)
        {
            var res = _unitOfWork.Users.Login(body);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.obj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPost("verification")]
        public IActionResult Verification([FromBody] PublicUserEmailVerificationDto body)
        {
            var res = _unitOfWork.Users.EmailVerification(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
