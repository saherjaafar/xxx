using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class ServiceSocialMediaRepository : BaseRepository<ServiceSocialMedia>, IServiceSocialMediaRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public ServiceSocialMediaRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        public TResponseVM<ServiceSocialMediaListDto> List(long serviceId)
        {
            var service = _unitOfWork.Services.GetById(serviceId);
            if (service is null)
                return new TResponseVM<ServiceSocialMediaListDto> { HasError = true, StatusCode = 404, Message = $"Service id : {serviceId} not exist" };

            var icons = (from ss in _context.ServiceSocialMedias.Include(i => i.Icon)
                         where ss.ServiceId == serviceId
                         select new ServiceSocialMediaListDto
                         {
                             ServiceSocialMediaId = ss.Id,
                             Link = ss.SocialMediaLink,
                             Icon = new SelectDto
                             {
                                 label = ss.Icon.Name,
                                 value = ss.Icon.Id.ToString(),
                             }
                         }).ToList();
            return new TResponseVM<ServiceSocialMediaListDto> { HasError = false, StatusCode = 200, ListObj = icons };
        }

        public TResponseVM<ManageServiceSocialMediaBodyDto> Manage(long serviceId, ManageServiceSocialMediaBodyDto body)
        {
            var service = _unitOfWork.Services.GetById(serviceId);
            if (service is null)
                return new TResponseVM<ManageServiceSocialMediaBodyDto> { HasError = true, StatusCode = 404, Message = $"Service id {serviceId} not exist" };

            var dbLinks = _context.ServiceSocialMedias.Where(i => i.ServiceId == serviceId).ToList();
            foreach (var i in dbLinks)
            {
                if (body.Links.FirstOrDefault(x => x.ServiceSocialMediaId == i.Id) == null)
                    _context.ServiceSocialMedias.Remove(i);
            }

            foreach (var i in body.Links)
            {
                if (dbLinks.FirstOrDefault(x => x.Id == i.ServiceSocialMediaId) == null)
                    _context.ServiceSocialMedias.Add(new ServiceSocialMedia
                    {
                        ServiceId = serviceId,
                        IconId = long.Parse(i.Icon.value),
                        SocialMediaLink = i.Link
                    });
            }
            _unitOfWork.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.serviceSocialMedia + serviceId.ToString());

            return new TResponseVM<ManageServiceSocialMediaBodyDto> { HasError = false, StatusCode = 200, Message = "Links updated successfully" };
        }
    }
}
