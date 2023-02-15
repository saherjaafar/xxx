using Core;
using Core.Dto.Admin;
using Core.Dto.LoginDto;
using Core.Dto.RegisterDto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("GetX")]
        public IActionResult GetX()
        {
            _unitOfWork.Admins.GetX();
            return Ok();
        }

        //[HttpGet("GetXCountries")]
        //public IActionResult GetXCountries()
        //{
        //    _unitOfWork.Admins.GetXCountries();
        //    return Ok();
        //}

        [HttpGet("List")]
        public IActionResult GetAdmins()
        {
            return Ok(_unitOfWork.Admins.List());
        }

        [HttpGet("All/Select")]
        public IActionResult GetAdminsSelect()
        {
            return Ok(_unitOfWork.Admins.ListSelect());
        }

        [HttpGet("Details/{adminId}")]
        public IActionResult GetAdminDetails(long adminId)
        {
            var res = _unitOfWork.Admins.GetDetails(adminId);
            if(res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            return Ok(res.obj);
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] LoginRequestDto body)
        {
            var res = _unitOfWork.Admins.Login(body);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return Ok(res.obj);
        }

        [HttpPost("CheckLoginToken")]
        public IActionResult CheckLoginToken([FromBody] CheckLoginTokenDto body)
        {
            var res = _unitOfWork.Admins.CheckEmailToken(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPost("CheckEmailVerification")]
        public IActionResult CheckEmailVerification(CheckEmailVerificationDto body)
        {
            var res = _unitOfWork.Admins.CheckEmailVerification(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPost("AdminRegistration")]
        public IActionResult Register([FromBody] AdminRegisterDto body)
        {
            var res = _unitOfWork.Admins.Register(body);

            return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("Update/{adminId}")]
        public IActionResult Update([FromBody] UpdateAdminDto body,long adminId)
        {
            var res = _unitOfWork.Admins.Update(body,adminId);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("VerifyResetPassword")]
        public IActionResult VerifyResetPassword([FromBody] VerifyResetPasswordDto body)
        {
            var res = _unitOfWork.Admins.VerifyResetPassword(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("Forgetpassword")]
        public IActionResult RequestForgetPassword([FromBody] ForgetPasswordDto body)
        {
            var res = _unitOfWork.Admins.RequestForgetPassword(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("ResetPassword")]
        public IActionResult ResetPassword([FromBody] ResetPasswordDto body)
        {
            var res = _unitOfWork.Admins.ResetPassword(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("Account/Verification")]
        public IActionResult AdminAccountVerification([FromBody] AdminVerificationDto body)
        {
            var res = _unitOfWork.Admins.VerifyAccount(body);
            return StatusCode(res.StatusCode,res.Message);
        }

        [Authorize]
        [HttpPut("Lock/{adminId}")]
        public IActionResult ManageLock(long adminId)
        {
            var res = _unitOfWork.Admins.ManageLock(adminId);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
