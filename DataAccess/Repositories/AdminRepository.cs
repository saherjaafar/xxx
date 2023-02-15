using AutoMapper;
using Common.ApiRequest;
using Common.Utilities;
using Core.Cache;
using Core.Dto;
using Core.Dto.Admin;
using Core.Dto.LoginDto;
using Core.Dto.RegisterDto;
using Core.Dto.TResponse;
using Core.EmailManager;
using Core.Models;
using Core.Repositories;
using DataAccess.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Security.Policy;
using System.Threading.Tasks;
using wedcoo_api.Authentication;

namespace DataAccess.Repositories
{
    public class AdminRepository : BaseRepository<Admin>, IAdminRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private IConfiguration _configuration;
        private readonly AdminService _adminService;
        private readonly IJwtAuthentication _jwt;
        public AdminRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper,
            IApiRequest apiRequest, IConfiguration configuration, AdminService adminService, IJwtAuthentication jwt
            ) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _configuration = configuration;
            _adminService = adminService;
            _jwt = jwt;
        }

        public IEnumerable<AdminsListDto> List()
        {
            return (from a in _context.Admins.AsNoTracking().Include(i => i.AdminRole)
                    select new AdminsListDto
                    {
                        AdminId = a.ID,
                        Email = UtilClass.DecryptBackByAES(a.Email),
                        IsLocked = a.IsLocked,
                        PhoneNumber = UtilClass.DecryptBackByAES(a.PhoneNumber),
                        FullName = UtilClass.DecryptBackByAES(a.FirstName) + " " + UtilClass.DecryptBackByAES(a.LastName),
                        Role = a.AdminRole.Name
                    }).ToList();
        }

        public IEnumerable<SelectDto> ListSelect()
        {
            return (from a in _context.Admins
                    select new SelectDto
                    {
                        label = UtilClass.DecryptBackByAES(a.FirstName) + " " + UtilClass.DecryptBackByAES(a.LastName),
                        value = a.ID.ToString(),
                    }).ToList();
        }

        public TResponseVM<AdminDetailsDto> GetDetails(long adminId)
        {
            var admin = _context.Admins.Include(i => i.AdminRole).FirstOrDefault(i => i.ID == adminId);
            if (admin is null)
                return new TResponseVM<AdminDetailsDto> { HasError = true, StatusCode = 404, Message = $"Id : {adminId} not exist" };

            var details = new AdminDetailsDto
            {
                Email = UtilClass.DecryptBackByAES(admin.Email),
                FirstName = UtilClass.DecryptBackByAES(admin.FirstName),
                LastName = UtilClass.DecryptBackByAES(admin.LastName),
                PhoneNumber = UtilClass.DecryptBackByAES(admin.PhoneNumber),
                Role = new SelectDto { label = admin.AdminRole?.Name, value = admin.AdminRole?.Id.ToString() }
            };

            return new TResponseVM<AdminDetailsDto>
            {
                HasError = false,
                StatusCode = 200,
                obj = details,
            };
        }

        public TResponseVM<LoginResponseDto> Login(LoginRequestDto body)
        {
            var admin = _adminService.GetByEmail(UtilClass.DecryptFrontByAES(body.Email));

            if (admin is null)
                return new TResponseVM<LoginResponseDto> { HasError = true, StatusCode = 404, Message = "Invalid Credientials" };
            if (admin.IsLocked)
                return new TResponseVM<LoginResponseDto> { HasError = true, StatusCode = 403, Message = "Your Account Is Locked, For Any Details Please Contact Administrator" };

            var verification = _adminService.GetVerification(admin.ID);

            if (!verification.IsEmailVerified)
            {
                _adminService.UpdateVerificationTokenExpiry(admin.ID);
                string verificationLink = _configuration["AdminEnvironment"] + "Verification/" + UtilClass.DecryptBackByAES(admin.Email) + "/" + verification.EmailVerificationToken.ToString();
                EmailManager.AdminVerifyEmail(UtilClass.DecryptFrontByAES(admin.Email), verificationLink, _configuration["EmailTemplatePath"]);
                //EmailManager.AdminVerifyEmail(admin.Email, verificationLink, _configuration["EmailTemplatePath"]);
                return new TResponseVM<LoginResponseDto> { HasError = true, StatusCode = 405, Message = "You need to verify your email. An Email has sent to you ! Please Check Your Email." };
            }
            var role = _unitOfWork.AdminRoles.GetById(admin.AdminRoleId);

            admin.LoginToken = Guid.NewGuid();
            admin.LoginTokenExpiry = DateTime.Now.AddHours(3);
            _context.SaveChanges();

            var response = new LoginResponseDto
            {
                AdminId = admin.ID,
                Email = UtilClass.EncryptFrontByAES(UtilClass.DecryptBackByAES(admin.Email)),
                LoginToken = UtilClass.EncryptFrontByAES(admin.LoginToken.ToString()),
                Permissions = role,
                Token = UtilClass.EncryptFrontByAES(_jwt.AdminAuthentication(admin.Email, role.Name)),
            };
            _unitOfWork.SaveChanges();
            return new TResponseVM<LoginResponseDto>
            {
                HasError = false,
                StatusCode = 200,
                obj = response
            };
        }

        public TResponseVM<CheckLoginTokenDto> CheckEmailToken(CheckLoginTokenDto body)
        {

            var admin = _context.Admins.FirstOrDefault(i => i.Email == UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.Email)));
            if (admin is null)
                return new TResponseVM<CheckLoginTokenDto> { HasError = true, StatusCode = 404, Message = "Email Not Exist" };

            if (admin.LoginToken != Guid.Parse(UtilClass.DecryptFrontByAES(body.LoginToken)))
                return new TResponseVM<CheckLoginTokenDto> { HasError = true, StatusCode = 404, Message = "Invalid Token" };

            if (admin.LoginTokenExpiry < DateTime.Now)
                return new TResponseVM<CheckLoginTokenDto> { HasError = true, StatusCode = 401, Message = "Expired Token" };

            return new TResponseVM<CheckLoginTokenDto> { HasError = true, StatusCode = 200, Message = "Authorized" };
        }

        public TResponseVM<ResponseVM> Register(AdminRegisterDto body)
        {
            if (_adminService.CheckIfEmailExist(body.Email))
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 409, Message = "Email Already Registered" };

            Admin admin = new Admin
            {
                Email = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.Email)),
                //Email = UtilClass.EncryptBackByAES(body.Email.ToLower()),
                AdminRoleId = body.RoleId,
                FirstName = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.FirstName)),
                //FirstName = UtilClass.EncryptBackByAES(body.FirstName),
                LastName = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.LastName)),
                //LastName = UtilClass.EncryptBackByAES(body.LastName),
                IsLocked = false,
                LoginToken = Guid.NewGuid(),
                LoginTokenExpiry = DateTime.Now,
                Password = UtilClass.HashText(UtilClass.MakeRandomPassword(12)),
                //Password = UtilClass.HashText("S@her111"),
                PhoneNumber = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.PhoneNumber)),
                //PhoneNumber = UtilClass.EncryptBackByAES(body.PhoneNumber),
                IsFirstLogin = true
            };
            _unitOfWork.Admins.Add(admin);
            _unitOfWork.SaveChanges();
            AdminVerification verification = new AdminVerification
            {
                AdminId = admin.ID,
                EmailVerificationToken = Guid.NewGuid(),
                EmailVerificationTokenExpiry = DateTime.Now.AddMinutes(30),
                ResetPasswordToken = Guid.NewGuid(),
                ResetPasswordTokenExpiry = DateTime.Now,
                IsEmailVerified = false
            };
            _unitOfWork.AdminVerifications.Add(verification);
            _unitOfWork.SaveChanges();
            string verificationLink1 = _configuration["AdminEnvironment"] + "Verification/" + UtilClass.DecryptBackByAES(admin.Email) + "/" + verification.EmailVerificationToken.ToString();
            EmailManager.AdminVerifyEmail(UtilClass.DecryptBackByAES(admin.Email), verificationLink1, _configuration["EmailTemplatePath"]);
            return new TResponseVM<ResponseVM>
            {
                HasError = false,
                StatusCode = 200,
                Message = "Admin Registered Successfully"
            };
        }

        public TResponseVM<ResponseVM> VerifyResetPassword(VerifyResetPasswordDto body)
        {
            var adminExist = _adminService.CheckIfEmailExist(UtilClass.DecryptFrontByAES(body.Email));
            if (!adminExist)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = "Invalid Data Request" };

            var verifications = (from a in _context.Admins
                                 join v in _context.AdminVerification on a.ID equals v.AdminId
                                 into av
                                 from avJoin in av.DefaultIfEmpty()
                                 where a.Email == body.Email && avJoin.EmailVerificationToken == body.Token
                                 select new { avJoin.EmailVerificationTokenExpiry }).FirstOrDefault();
            if (verifications?.EmailVerificationTokenExpiry < DateTime.Now)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 403, Message = "Request Expired" };
            else
            {
                var admin = _context.Admins.FirstOrDefault(i => i.Email == body.Email);
                admin.Password = UtilClass.HashText(UtilClass.DecryptFrontByAES(body.Password));
                admin.IsFirstLogin = false;
                _context.SaveChanges();
                return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Password Changed Successfully" };
            }
        }

        public TResponseVM<ResponseVM> ManageLock(long adminId)
        {
            var admin = _unitOfWork.Admins.GetById(adminId);
            if (admin is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Id : {adminId} not exist" };

            admin.IsLocked = !admin.IsLocked;
            _context.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = admin.IsLocked ? "Account Locked Successfully" : "Account Unlocked Successfully" };
        }

        public TResponseVM<ResponseVM> Update(UpdateAdminDto body, long adminId)
        {
            var admin = _unitOfWork.Admins.GetById(adminId);
            if (admin is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Id {body.Id} not exist" };

            bool emailUpdated = admin.Email != UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.Email));
            admin.Email = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.Email));
            admin.FirstName = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.FirstName));
            admin.LastName = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.LastName));
            admin.PhoneNumber = UtilClass.EncryptBackByAES(UtilClass.DecryptFrontByAES(body.PhoneNumber));
            admin.AdminRoleId = body.RoleId;
            _context.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Account Updated Successfully" };
        }

        public TResponseVM<ResponseVM> CheckEmailVerification(CheckEmailVerificationDto body)
        {
            var admin = _adminService.GetByEmail(body.Email);
            if (admin is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 401, Message = "Email Not Exist" };

            var verifications = _adminService.GetVerification(admin.ID);
            if (verifications.EmailVerificationToken != body.Token)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 401, Message = "Invalid Token" };
            if (verifications.EmailVerificationTokenExpiry < DateTime.Now)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 401, Message = "Request Expired" };

            return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 200, Message = "Authorized" };
        }

        public TResponseVM<ResponseVM> RequestForgetPassword(ForgetPasswordDto body)
        {
            var admin = _adminService.GetByEmail(UtilClass.DecryptFrontByAES(body.Email));
            if (admin is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = "Email Not Exist" };

            var newToken = Guid.NewGuid();
            var verification = _adminService.GetVerification(admin.ID);
            verification.ResetPasswordToken = newToken;
            verification.EmailVerificationTokenExpiry = DateTime.Now.AddMinutes(15);
            _context.SaveChanges();

            string link = _configuration["AdminEnvironment"] + "resetpassword/" + UtilClass.DecryptBackByAES(admin.Email) + "/" + verification.ResetPasswordToken.ToString();
            EmailManager.ForgetPassword(UtilClass.DecryptBackByAES(admin.Email), link, _configuration["EmailTemplatePath"]);

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Please Check your email" };
        }

        public TResponseVM<ResponseVM> ResetPassword(ResetPasswordDto body)
        {
            var admin = _adminService.GetByEmail(UtilClass.DecryptFrontByAES(body.Email));
            if (admin is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = "Email Not Exist" };

            var verification = _adminService.GetVerification(admin.ID);

            if (verification.ResetPasswordToken != body.Token)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 401, Message = "Invalid Token" };

            if (verification.ResetPasswordTokenExpiry > DateTime.Now)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 401, Message = "Expired Request" };

            admin.Password = UtilClass.HashText(UtilClass.DecryptFrontByAES(body.Password));
            verification.ResetPasswordTokenExpiry = DateTime.Now;
            _context.SaveChanges();

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Password Reset Successully" };
        }

        public TResponseVM<ResponseVM> VerifyAccount(AdminVerificationDto body)
        {
            var admin = _adminService.GetByEmail(UtilClass.DecryptFrontByAES(body.Email));
            if (admin is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = "Email Not Exist" };

            var verification = _adminService.GetVerification(admin.ID);

            if (verification.EmailVerificationToken != body.Token)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 401, Message = "Invalid Request" };

            admin.Password = UtilClass.HashText(UtilClass.DecryptFrontByAES(body.Password));
            verification.EmailVerificationTokenExpiry = DateTime.Now;
            admin.IsFirstLogin = false;
            verification.IsEmailVerified = true;
            _context.SaveChanges();

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Account Verified Successfully" };
        }

        public async void GetX()
        {

            var sss = _apiRequest.GetX<GetXDto>("https://localhost:5005/api/Admin/getx");
            List<GetXDto> datas = new List<GetXDto>();
            var wc = new System.Net.WebClient();
            using var httpClient = new HttpClient();
            foreach (var data in sss)
            {

                // Add Application
                var xApplication = data.XApplicationDto;

                //check if category exist
                var applicationCategory = _context.Categories.FirstOrDefault(i => i.Name == xApplication.Category.Name);
                if (applicationCategory == null)
                {
                    // check for icon
                    Icon icon = _context.Icons.FirstOrDefault(x => x.Name == xApplication.Category.IconName);
                    if (icon == null)
                    {
                        icon = new Icon
                        {
                            Name = xApplication.Category.IconName,
                            Svg = xApplication.Category.IconSvg
                        };
                        _context.Icons.Add(icon);
                        _context.SaveChanges();
                    }
                    applicationCategory = new Category
                    {
                        Name = xApplication.Category.Name,
                        IconId = icon.Id,
                        ImagePath = xApplication.Category.ImagePath,
                        HomeImagePath = xApplication.Category.HomeImagePath,
                        IsPublished = xApplication.Category.IsPublished,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now,
                    };
                    _context.Categories.Add(applicationCategory);
                    _context.SaveChanges();

                    var cdnUri = "https://cdn.wedcoo.com/Uploads/";
                    var firstPath = "C:\\Wedcoo\\Wedcoo-Api-v2\\Wedcoo-Api\\wwwroot\\Uploads\\";

                    var folderHome = xApplication.Category.HomeImagePath.Split('/')[0];
                    var nameHome = xApplication.Category.HomeImagePath.Split('/')[1].Split(".")[0];
                    var extentionHome = xApplication.Category.HomeImagePath.Split('/')[1].Split(".")[1];
                    _unitOfWork.Upload.DownloadImage(cdnUri + xApplication.Category.HomeImagePath, firstPath, folderHome, nameHome, extentionHome);

                    var folder = xApplication.Category.ImagePath.Split('/')[0];
                    var name = xApplication.Category.ImagePath.Split('/')[1].Split(".")[0];
                    var extention = xApplication.Category.ImagePath.Split('/')[1].Split(".")[1];
                    _unitOfWork.Upload.DownloadImage(cdnUri + xApplication.Category.ImagePath, firstPath, folder, name, extention);
                }

                var country = _context.Countries.FirstOrDefault(i => i.Name == xApplication.CountryName);
                var district = _context.CountryDistricts.FirstOrDefault(i => i.Name == xApplication.DistrictName);
                var state = _context.DistrictStates.FirstOrDefault(i => i.Name == xApplication.StateName);

                var app = new Application
                {
                    CategoryId = applicationCategory.Id,
                    CountryDistrictId = district.Id,
                    CountryId = country.Id,
                    DistrictStateId = state.Id,
                    HasMeeting = xApplication.HasMeeting,
                    Intrested = xApplication.Intrested,
                    IsApprovedForPublish = xApplication.IsApprovedForPublish,
                    IsCalled = xApplication.IsCalled,
                    MeetingDate = xApplication.MeetingDate,
                    Name = xApplication.Name,
                    Note = xApplication.Note,
                    Number = xApplication.Number,
                    IsNeedFollowUp = false,
                    FollowUpDate = null,
                };
                _context.Applications.Add(app);
                _context.SaveChanges();

                var xService = data.XServiceDto;
                var xGallery = data.XGalleryDto;
                var xSocialMedia = data.XSocialMediaLinkDto;
                if (xService.ApplicationId != 0)
                {
                    //Add Service
                    var service = new Service
                    {
                        Id = xService.ServiceId,
                        ApplicationId = app.Id,
                        DueDate = xService.DueDate,
                        Description = xService.Description,
                        Email = xService.Email,
                        HasPackage = xService.HasPackage,
                        CreatedAt = DateTime.Now,
                        IsPublished = xService.IsPublished,
                        IsSubscribed = xService.IsSubscribed,
                        Lat = 0,
                        Lng = 0,
                        Logo = xService.Logo,
                        MainImage = xService.MainImage,
                        PhoneNumber = xService.PhoneNumber,
                        PhoneNumber2 = xService.PhoneNumber2,
                        Quote = xService.Quote,
                        UpdatedAt = DateTime.Now,
                        YoutubeVideoId = xService.YoutubeVideoId,
                        SearchImage = xService.SearchImage,
                    };
                    _context.Services.Add(service);
                    _context.SaveChanges();

                    var cdnUri = "https://cdn.wedcoo.com/Uploads/";
                    var firstPath = "C:\\Wedcoo\\Wedcoo-Api-v2\\Wedcoo-Api\\wwwroot\\Uploads\\";

                    var folderHome = "Services/" + app.Name.Replace(" ", "_") + service.Id;
                    var name = xService.MainImage.Split('/')[2].Split(".")[0];
                    var extentionHome = xService.MainImage.Split('/')[2].Split(".")[1];
                    var finalUri = cdnUri + xService.MainImage;
                    _unitOfWork.Upload.DownloadImage1(finalUri, firstPath, folderHome, name, extentionHome);

                    var nameSeach = xService.SearchImage.Split('/')[2].Split(".")[0];
                    var extentionSearch = xService.SearchImage.Split('/')[2].Split(".")[1];
                    var finalUriSearch = cdnUri + xService.SearchImage;
                    _unitOfWork.Upload.DownloadImage1(finalUriSearch, firstPath, folderHome, nameSeach, extentionSearch);


                    // Add Social media
                    if (xSocialMedia != null)
                    {
                        foreach (var i in xSocialMedia)
                        {
                            Icon icon = _context.Icons.FirstOrDefault(x => x.Name == i.IconName);
                            if (icon == null)
                            {
                                icon = new Icon
                                {
                                    Name = i.IconName,
                                    Svg = i.IconSvg
                                };
                                _context.Icons.Add(icon);
                                _context.SaveChanges();
                            }

                            _context.ServiceSocialMedias.Add(new ServiceSocialMedia
                            {
                                IconId = icon.Id,
                                ServiceId = service.Id,
                                SocialMediaLink = i.SocialMediaLink,
                            });
                        }
                        _context.SaveChanges(true);
                    }

                    // add Gallery
                    if (xGallery != null)
                    {
                        foreach (var x in xGallery)
                        {
                            _context.ServiceGalleries.Add(new ServiceGallery
                            {
                                Height = x.Height,
                                Image = x.Image,
                                IsDeleted = x.IsDeleted,
                                IsPublished = x.IsPublished,
                                ServiceId = service.Id,
                                UploadedAt = DateTime.Now,
                                Width = x.Width,
                            });

                            var cdnUri1 = "https://cdn.wedcoo.com/Uploads/";
                            var firstPath1 = "C:\\Wedcoo\\Wedcoo-Api-v2\\Wedcoo-Api\\wwwroot\\Uploads\\";

                            var folderHome1 = "Services/" + app.Name.Replace(" ", "_") + service.Id + "/Gallery";
                            var name1 = x.Image.Split('/')[3].Split(".")[0];
                            var extentionHome1 = x.Image.Split('/')[3].Split(".")[1];
                            var finalUri1 = cdnUri + x.Image;
                            _unitOfWork.Upload.DownloadImage1(finalUri1, firstPath1, folderHome1, name1, extentionHome1);

                        }
                        _context.SaveChanges();
                    }
                }
            }
        }

        public void GetXCountries()
        {
            var countries = _apiRequest.GetX<XCountry>("https://localhost:5005/api/Admin/getCountries");
            foreach (var c in countries)
            {
                Icon icon = _context.Icons.FirstOrDefault(x => x.Name == c.IconName);
                if (icon == null)
                {
                    icon = new Icon
                    {
                        Name = c.IconName,
                        Svg = c.IconSvg
                    };
                    _context.Icons.Add(icon);
                    _context.SaveChanges();
                }

                var country = new Country
                {
                    Currency = c.Currency,
                    IconId = icon.Id,
                    IsPublished = c.IsPublished,
                    Name = c.CountryName,
                    Suffix = c.Suffix,
                };
                _context.Countries.Add(country);
                _context.SaveChanges();

                foreach (var d in c.Districts)
                {
                    var district = new CountryDistrict
                    {
                        CountryId = country.Id,
                        Name = d.DistrictName
                    };
                    _context.CountryDistricts.Add(district);
                    _context.SaveChanges();

                    foreach (var s in d.States)
                    {
                        _context.DistrictStates.Add(new DistrictState
                        {
                            CountryDistrictId = district.Id,
                            Name = s.Name,
                        });
                        _context.SaveChanges();
                    }
                }
            }
        }
    }

}
