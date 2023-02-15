using AutoMapper;
using Common.ApiRequest;
using Common.UploadService;
using Core;
using Core.Cache;
using Core.Repositories;
using DataAccess.Repositories;
using DataAccess.Services;
using Microsoft.Extensions.Configuration;
using wedcoo_api.Authentication;

namespace DataAccess
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        protected readonly CacheManager _cacheManager;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        IConfiguration _configuration;
        private readonly IJwtAuthentication _jwt;

        public ICountryRepository Countries { get; set; }
        private readonly CountryService _countryService;
        public IIconRepository Icons { get; set; }
        private readonly IconService _iconService;
        public IAdminRepository Admins { get; set; }
        private readonly AdminService _adminService;
        public IAdminVerification AdminVerifications { get; set; }
        public IAdminRoleRepository AdminRoles {get; set;}
        public ICategoryRepository Categories { get; set; }
        public IFilterRepository Filters { get; set; }
        public IUploadService Upload { get; set; }
        public IApplicationRepository Applications { get; set; }
        public IServiceRepository Services { get; set; }
        public IServiceGalleryRepository ServiceGalleries { get; set; }
        public IServicePackageRepository ServicePackages { get; set; }
        public IServicePackageAttributeRepository ServicePackageAttributes { get; set; }
        public IServiceSocialMediaRepository ServiceSocialMedias { get; set; }
        public IDealRepository Deals { get; set; }
        public ISponsorRepository Sponsors { get; set; }
        public IServicePaymentRepository ServicePayments { get; set; }
        public IUserRepository Users { get; set; }
        public IUserFavoriteRepository UserFavorites { get; set; }

        public UnitOfWork(ApplicationDbContext context, CacheManager cacheManager, IApiRequest apiRequest, IConfiguration configuration, IJwtAuthentication jwt,
            AdminService adminService, IconService iconService, CountryService countryService)
        {
            _context = context;
            _cacheManager = cacheManager;
            _apiRequest = apiRequest;
            _configuration = configuration;
            _jwt = jwt;
            _countryService = countryService;
            Countries = new CountryRepository(_context, _cacheManager, this,_mapper, _apiRequest,_countryService);
            _iconService = iconService;
            Icons = new IconRepository(_context, _cacheManager, this,_mapper, _apiRequest, _iconService);
            _adminService = adminService;
            Admins = new AdminRepository(_context, _cacheManager, this, _mapper, _apiRequest,_configuration, _adminService,_jwt);
            AdminRoles = new AdminRoleRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            AdminVerifications = new AdminVerificationRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Categories = new CategoryRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Filters = new FilterRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Upload = new UploadService(_configuration);
            Applications = new ApplicationRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Services = new ServiceRepository(_context, _cacheManager, this, _mapper, _apiRequest,_configuration);
            ServiceGalleries = new ServiceGalleryRepository(_context, _cacheManager, this, _mapper, _apiRequest,_configuration);
            ServicePackages = new ServicePackageRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            ServicePackageAttributes = new ServicePackageAttributeRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            ServiceSocialMedias = new ServiceSocialMediaRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Deals = new DealRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Sponsors = new SponsorRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            ServicePayments = new ServicePaymentRepository(_context, _cacheManager, this, _mapper, _apiRequest);
            Users = new UserRepository(_context, _cacheManager, this, _mapper, _apiRequest, _configuration,_jwt);
            UserFavorites = new UserFavoriteRepository(_context, _cacheManager, this, _mapper, _apiRequest, _configuration);
        }   

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
