using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Linq;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class ServiceGalleryRepository : BaseRepository<ServiceGallery>, IServiceGalleryRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private readonly IConfiguration _configuration;
        public ServiceGalleryRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest, IConfiguration configuration) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _configuration = configuration;
        }

        public TResponseVM<GalleryImagesListDto> GetGallery(long serviceId)
        {
            var service = _unitOfWork.Services.Get(serviceId);
            if (service is null)
                return new TResponseVM<GalleryImagesListDto> { HasError = true, StatusCode = 404, Message = $"Service id : {serviceId} not exist" };

            var gallery = (from g in _context.ServiceGalleries
                           where g.ServiceId == serviceId
                           select new GalleryImagesListDto
                           {
                               height = g.Height,
                               ImageId = g.Id,
                               src = _configuration["ApiImagePath"] + g.Image,
                               width = g.Width,
                           }).ToList();
            return new TResponseVM<GalleryImagesListDto> { HasError = false, StatusCode = 200, ListObj = gallery };

        }

        public TResponseVM<ResponseVM> UploadGalleryImage(UploadImageDetailsDto details, GalleryImageDto image)
        {
            if (!_unitOfWork.Upload.CheckIfImageValidity(image.Image))
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 415, Message = "Insupported Media Type" };

            var service = _unitOfWork.Services.Get(details.ServiceId, s => s.Application);
            if (service is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Service id : {details.ServiceId} not exist" };

            var imageUrl = _unitOfWork.Upload.UploadDynamicImage(image.Image, "Services/" + service.Application.Name.Replace(" ", "_") + service.Id + "/Gallery");
            _context.ServiceGalleries.Add(new ServiceGallery
            {
                Height = details.Height,
                Image = imageUrl,
                IsPublished = true,
                ServiceId = service.Id,
                UploadedAt = DateTime.Now,
                Width = details.Width,
                IsDeleted = false
            });
            _context.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.serviceGallery + service.Id.ToString());
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = imageUrl };
        }

        public TResponseVM<ResponseVM> Delete(long imageId)
        {
            var image = _context.ServiceGalleries.Include(i => i.Service).ThenInclude(i => i.Application).FirstOrDefault(i => i.Id == imageId);
            var path = Path.Combine(_configuration["ApiRootFolder"], "Services\\" + image.Service.Application.Name.Replace(" ", "_") + image.Service.Id + "\\Gallery\\" + image.Image);
            if (File.Exists(Path.Combine(_configuration["ApiRootFolder"], image.Image)))
            {
                File.Delete(Path.Combine(_configuration["ApiRootFolder"], image.Image));
            }
            _context.ServiceGalleries.Remove(image);
            _unitOfWork.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Image Deleted Successfully" };
        }
    }
}
