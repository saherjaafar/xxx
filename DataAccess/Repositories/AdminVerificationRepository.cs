using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Models;
using Core.Repositories;

namespace DataAccess.Repositories
{
    public class AdminVerificationRepository : BaseRepository<AdminVerification>, IAdminVerification
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public AdminVerificationRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }
    }
}
