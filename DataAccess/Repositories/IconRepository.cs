using AutoMapper;
using Common.ApiRequest;
using Common.ApiRequest.Dto;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using Core.Repositories;
using DataAccess.Services;
using System.Collections.Generic;
using System.Linq;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class IconRepository : BaseRepository<Icon>, IIconRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private readonly IconService _iconService;
        public IconRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest, IconService iconService) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _iconService = iconService;
        }



        public IEnumerable<IconsListDto> List()
        {
            var list = new List<IconsListDto>();
            if (!_cacheManager.Exist<List<IconsListDto>>(CacheKeysEnum.IconsList.ToString()))
            {
                list = (from i in _context.Icons
                        select new IconsListDto
                        {
                            IconId = i.Id,
                            IconName = i.Name,
                            Svg = i.Svg,
                        }).ToList();

                _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "icon/UpdateListCache" });
            }
            else
                list = _cacheManager.Get<List<IconsListDto>>(CacheKeysEnum.IconsList.ToString());

            return list;
                
        }
        public IEnumerable<SelectDto> ListSelect()
        {
            var list = new List<SelectDto>();
            if (!_cacheManager.Exist<List<SelectDto>>(CacheKeysEnum.IconsListSelect.ToString()))
            {
                list = (from i in _context.Icons
                        select new SelectDto
                        {
                            value = i.Id.ToString(),
                            label = i.Name,
                        }).ToList();
                _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "icon/UpdateListCache" });
            }
            else
                list = _cacheManager.Get<List<SelectDto>>(CacheKeysEnum.IconsListSelect.ToString());
            return list;
        }

        public TResponseVM<ResponseVM> AddIcon(AddIconDto body)
        {
            var dbIcon = _iconService.GetByName(body.IconName);
            if(dbIcon is not null)
                return new TResponseVM<ResponseVM> { HasError= true, StatusCode = 409, Message = $"Icon : {body.IconName} Already Exist" };

            Icon icon = new Icon { Name= body.IconName , Svg = body.Svg };
            _unitOfWork.Icons.Add(icon);
            _unitOfWork.SaveChanges();
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "icon/UpdateListCache" });
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Icon Added Successfully" };
        }

        public TResponseVM<ResponseVM> UpdateIcon(UpdateIconDto body)
        {
            var icon = _unitOfWork.Icons.GetById(body.IconId);
            if (icon is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Icon Id : {body.IconId} not exist" };

            icon.Name = body.IconName;
            icon.Svg = body.Svg;
            _unitOfWork.SaveChanges();
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "icon/UpdateListCache" });

            return new TResponseVM<ResponseVM> { StatusCode = 200, HasError = false, Message = "Icon Updated Successfully" };
        }

        public void UpdateListCache()
        {
            _iconService.UpdateListCache();
        }
    }
}
