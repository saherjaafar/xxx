using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using Core.Repositories;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class UserFavoriteRepository : BaseRepository<UserFavorite>, IUserFavoriteRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private readonly IConfiguration _configuration;
        public UserFavoriteRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest, IConfiguration configuration) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _configuration = configuration;
        }

        public TResponseVM<FavoriteListDto> List(long userId)
        {
            var user = _unitOfWork.Users.Get(i => i.Id == userId);
            if (user is null)
                return new TResponseVM<FavoriteListDto> { HasError = true, StatusCode = 404, Message = $"User id : {userId} not exist" };

            var favorites = (from f in _context.UserFavorites
                             join s in _context.Services on f.ServiceId equals s.Id
                             into fs
                             from fsJoin in fs.DefaultIfEmpty()

                             join a in _context.Applications on fsJoin.ApplicationId equals a.Id
                             into fa
                             from faJoin in fa.DefaultIfEmpty()

                             join d in _context.CountryDistricts on faJoin.CountryDistrictId equals d.Id
                             into fd
                             from fdJoin in fd.DefaultIfEmpty()

                             join st in _context.DistrictStates on faJoin.DistrictStateId equals st.Id
                             into fst
                             from fstJoin in fst.DefaultIfEmpty()

                             where f.UserId == userId && fsJoin.IsPublished == true

                             select new FavoriteListDto
                             {
                                 FavoriteId = f.Id,
                                 ServiceId = f.ServiceId,
                                 Name = faJoin.Name,
                                 Image = fsJoin.SearchImage,
                                 Location = fdJoin.Name + " - " + fstJoin.Name
                             }).ToList();

            return new TResponseVM<FavoriteListDto> { HasError = false, StatusCode = 200, ListObj = favorites };
        }

        public TResponseVM<ResponseVM> ManageFavorites(ManageFavoritesDto body)
        {
            var user = _unitOfWork.Users.Get(i => i.Id == body.UserId);
            if (user is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"User id : {body.UserId} not exist" };

            var service = _unitOfWork.Services.Get(body.ServiceId);
            if (service is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Service id : {body.UserId} not exist" };

            var favorite = (from f in _context.UserFavorites
                            join s in _context.Services on f.ServiceId equals s.Id
                            into sf
                            from sfJoin in sf.DefaultIfEmpty()

                            join a in _context.Applications on sfJoin.ApplicationId equals a.Id
                            into fa
                            from faJoin in fa.DefaultIfEmpty()

                            where f.ServiceId == body.ServiceId && f.UserId == body.UserId
                            select new UserFavorite { ServiceId = f.ServiceId, CategoryId = f.CategoryId, UserId = f.UserId, Id = f.Id }).FirstOrDefault();

            if (favorite is null)
            {
                var x = (from s in _context.Services
                         join a in _context.Applications on s.ApplicationId equals a.Id
                         into sa
                         from saJoin in sa.DefaultIfEmpty()
                         where s.Id == body.ServiceId
                         select new { saJoin.CategoryId }).FirstOrDefault();
                _context.UserFavorites.Add(new UserFavorite { ServiceId = service.Id, CategoryId = x.CategoryId, UserId = body.UserId });
            }
            else
                _context.Remove(favorite);
            _unitOfWork.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Favorites updated successfully" };
        }
    }
}
