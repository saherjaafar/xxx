using Core.Dto;
using Core.Dto.Admin;
using Core.Dto.LoginDto;
using Core.Dto.RegisterDto;
using Core.Dto.TResponse;
using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IAdminRepository : IBaseRepository<Admin>
    {
        public IEnumerable<AdminsListDto> List();
        public IEnumerable<SelectDto> ListSelect();
        public TResponseVM<AdminDetailsDto> GetDetails(long adminId);
        public TResponseVM<LoginResponseDto> Login(LoginRequestDto body);
        public TResponseVM<CheckLoginTokenDto> CheckEmailToken(CheckLoginTokenDto body);
        public TResponseVM<ResponseVM> CheckEmailVerification(CheckEmailVerificationDto body);
        public TResponseVM<ResponseVM> Register(AdminRegisterDto body);
        public TResponseVM<ResponseVM> Update(UpdateAdminDto body, long adminId);
        public TResponseVM<ResponseVM> VerifyAccount(AdminVerificationDto body);
        public TResponseVM<ResponseVM> ResetPassword(ResetPasswordDto body);
        public TResponseVM<ResponseVM> VerifyResetPassword(VerifyResetPasswordDto body);
        public TResponseVM<ResponseVM> RequestForgetPassword(ForgetPasswordDto body);
        public TResponseVM<ResponseVM> ManageLock(long AdminId);
        public void GetX();
        public void GetXCountries();
    }
}
