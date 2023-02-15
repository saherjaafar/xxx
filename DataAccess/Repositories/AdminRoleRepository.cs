using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto;
using Core.Models;
using Core.Repositories;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.Repositories
{
    public class AdminRoleRepository : BaseRepository<AdminRole>, IAdminRoleRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public AdminRoleRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        public IEnumerable<SelectDto> ListSelect()
        {
            return (from a in _context.AdminRoles
                    select new SelectDto { label = a.Name, value = a.Id.ToString() }).ToList();
        }
    }
}
