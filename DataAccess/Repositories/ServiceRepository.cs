using AutoMapper;
using Common.ApiRequest;
using Common.ApiRequest.Dto;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Common.Enums.SD;
using static System.Net.Mime.MediaTypeNames;

namespace DataAccess.Repositories
{
    public class ServiceRepository : BaseRepository<Service>, IServiceRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private readonly IConfiguration _configuration;
        public ServiceRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest, IConfiguration configuration) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _configuration = configuration;
        }

        #region Admin

        public Service Get(long serviceId, params Expression<Func<Service, object>>[] includes)
        {
            var query = _context.Services.AsQueryable();
            return includes
                .Aggregate(
                    query.AsQueryable(),
                    (current, include) => current.Include(include)
                )
                .FirstOrDefault(e => e.Id == serviceId);
        }

        public List<ServicesListDto> List()
        {
            return (from s in _context.Services
                    .Include(i => i.Application)
                    select new ServicesListDto
                    {
                        ServiceId = s.Id,
                        ApplicationId = s.ApplicationId,
                        Category = s.Application.Category.Name,
                        Country = s.Application.Country.Name,
                        ServiceName = s.Application.Name,
                        DueDate = s.DueDate.Day + "-" + s.DueDate.Month + "-" + s.DueDate.Year,
                        IsPublished = s.IsPublished,
                    }).ToList();
        }

        public List<SelectDto> ListSelect()
        {
            var list = (from s in _context.Services
                        select new SelectDto
                        {
                            label = s.Application.Name,
                            value = s.Id.ToString(),
                        }).ToList();
            return list;
        }

        public TResponseVM<SelectDto> ListSelectByCategory(long categoryId)
        {
            var category = _unitOfWork.Categories.GetById(categoryId);
            if (category is null)
                return new TResponseVM<SelectDto>() { HasError = true, StatusCode = 404, Message = $"Categoy id : {categoryId} not exist" };

            var list = (from s in _context.Services
                        where s.Application.CategoryId == categoryId
                        select new SelectDto
                        {
                            label = s.Application.Name,
                            value = s.Id.ToString(),
                        }).ToList();
            return new TResponseVM<SelectDto>() { HasError = false, StatusCode = 200, ListObj = list };
        }

        public TResponseVM<ServiceDetailsDto> GetServiceDetails(long serviceId)
        {
            var service = _context.Services
                            .Include(i => i.Application)
                            .Include(i => i.Application.Country)
                            .Include(i => i.Application.CountryDistrict)
                            .Include(i => i.Application.Category)
                            .FirstOrDefault(i => i.Id == serviceId);

            if (service is null)
                return new TResponseVM<ServiceDetailsDto>() { HasError = true, StatusCode = 404, Message = $"Service id {serviceId} not exist" };

            var details = new ServiceDetailsDto
            {
                ServiceName = service.Application.Name,
                Country = new SelectDto
                {
                    label = service.Application.Country.Name,
                    value = service.Application.Country.Id.ToString()
                },
                CountryDistrict = new SelectDto
                {
                    label = service.Application.CountryDistrict.Name,
                    value = service.Application.CountryDistrict.Id.ToString()
                },
                Category = new SelectDto
                {
                    label = service.Application.Category.Name,
                    value = service.Application.Category.Id.ToString()
                },
                Description = service.Description,
                DueDate = service.DueDate,
                Email = service.Email,
                IsSubscribed = service.IsSubscribed,
                MainImage = service.MainImage,
                PhoneNumber = service.PhoneNumber,
                PhoneNumber2 = service.PhoneNumber2,
                Quote = service.Quote,
                SearchImage = service.SearchImage,
                YoutubeVideoId = service.YoutubeVideoId,
            };
            return new TResponseVM<ServiceDetailsDto>() { HasError = false, StatusCode = 200, obj = details };
        }

        public TResponseVM<ResponseVM> Add(AddServiceImagesDto images, AddServiceDto body)
        {
            if (!_unitOfWork.Upload.CheckIfImageValidity(images.MainImage) || !_unitOfWork.Upload.CheckIfImageValidity(images.SearchImage))
                return new TResponseVM<ResponseVM>() { HasError = true, StatusCode = 415, Message = "Unsupported media type" };

            var application = _unitOfWork.Applications.GetById(body.ApplicationId);

            if (application is null)
                return new TResponseVM<ResponseVM>() { HasError = true, StatusCode = 404, Message = $"Application id {body.ApplicationId} not exist" };

            Service service = new Service
            {
                ApplicationId = body.ApplicationId,
                CreatedAt = DateTime.Now,
                Description = body.Description,
                DueDate = body.DueDate,
                Email = body.Email,
                HasPackage = false,
                IsPublished = false,
                Lat = 0,
                Lng = 0,
                Logo = "",
                MainImage = "",
                PhoneNumber = body.PhoneNumber,
                PhoneNumber2 = body.PhoneNumber2,
                SearchImage = "",
                UpdatedAt = DateTime.Now,
                Quote = body.Quote,
                IsSubscribed = body.IsSubscribed,
                YoutubeVideoId = body.YoutubeVideoId,
            };
            _context.Services.Add(service);
            _unitOfWork.SaveChanges();

            service.MainImage = _unitOfWork.Upload.UploadDynamicImage(images.MainImage, "Services/" + application.Name.Replace(" ", "_") + service.Id);
            service.SearchImage = _unitOfWork.Upload.UploadDynamicImage(images.SearchImage, "Services/" + application.Name.Replace(" ", "_") + service.Id);
            _unitOfWork.SaveChanges();


            _cacheManager.Remove(CacheKeysEnum.ServiceCategoryList + "-" + application.CategoryId);
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/service/UpdateServiceCategorySelectListCache/"+application.CategoryId });

            _cacheManager.Remove(CacheKeysEnum.PublicCategoriesList.ToString());
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/Category/UpdateCache" });

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Service Added Successfully" };
        }

        public TResponseVM<ResponseVM> Update(UpdateServiceImagesDto images, UpdateServiceDetailsDto body)
        {
            var service = _context.Services.Include(i => i.Application).FirstOrDefault(i => i.Id == body.ServiceId);
            if (service is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Service Id {body.ServiceId} not exist" };

            service.Email = body.Email;
            service.PhoneNumber = body.PhoneNumber;
            service.PhoneNumber2 = body.PhoneNumber2;
            service.DueDate = body.DueDate;
            service.Description = body.Description;
            service.UpdatedAt = DateTime.Now;
            service.Quote = body.Quote;
            service.IsSubscribed = body.IsSubscribed;
            service.YoutubeVideoId = body.YoutubeVideoId;

            if (images.MainImage is not null)
            {
                if (!_unitOfWork.Upload.CheckIfImageValidity(images.MainImage))
                {
                    _unitOfWork.SaveChanges();
                    return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 415, Message = "Unsupported media type" };
                }
                else
                    service.MainImage = _unitOfWork.Upload.UploadDynamicImage(images.MainImage, "Services/" + service.Application.Name.Replace(" ", "_") + service.Id);
            }

            if (images.SearchImage is not null)
            {
                if (!_unitOfWork.Upload.CheckIfImageValidity(images.SearchImage))
                {
                    _unitOfWork.SaveChanges();
                    return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 415, Message = "Unsupported media type" };
                }
                else
                {
                    service.SearchImage = _unitOfWork.Upload.UploadDynamicImage(images.SearchImage, "Services/" + service.Application.Name.Replace(" ", "_") + service.Id);
                    _cacheManager.Remove(CacheKeysEnum.ServiceCategoryList + "-" + service.Application.CategoryId);
                    _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/service/UpdateServiceCategorySelectListCache/" + service.Application.CategoryId });
                }
            }
            _unitOfWork.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.serviceMainDetails + service.Id.ToString());

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Service Updated Successfully" };
        }

        public TResponseVM<ResponseVM> ManagePublish(long serviceId)
        {
            var service = Get(serviceId, c => c.Application);
            if (service is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Service id {serviceId} not exist" };

            service.IsPublished = !service.IsPublished;
            _unitOfWork.SaveChanges();

            _cacheManager.Remove(CacheKeysEnum.ServiceCategoryList + "-" + service.Application.CategoryId);
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/service/UpdateServiceCategorySelectListCache/" + service.Application.CategoryId });

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Service Updated Successfully" };
        }

        #endregion

        #region Public

        public List<SelectDto> ServiceCategorySelectList(long categoryId)
        {
            string cachKey = CacheKeysEnum.ServiceCategoryList + "-" + categoryId;
            if (_cacheManager.Exist<List<SelectDto>>(cachKey))
                return _cacheManager.Get<List<SelectDto>>(cachKey);
            else
            {
                var services = (from a in _context.Applications
                                join s in _context.Services on a.Id equals s.ApplicationId
                                into sa
                                from saJoin in sa.DefaultIfEmpty()
                                where a.CategoryId == categoryId && saJoin.IsPublished == true
                                select new SelectDto { label = a.Name, value = saJoin.Id.ToString() }).ToList();
                _cacheManager.Set(cachKey, services);
                return services;
            }
        }

        public void UpdateServiceCategorySelectListCache(long categoryId)
        {
            string cachKey = CacheKeysEnum.ServiceCategoryList + "-" + categoryId;
            var services = (from a in _context.Applications
                            join s in _context.Services on a.Id equals s.ApplicationId
                            into sa
                            from saJoin in sa.DefaultIfEmpty()
                            where a.CategoryId == categoryId && saJoin.IsPublished == true
                            select new SelectDto { label = a.Name, value = saJoin.Id.ToString() }).ToList();
            _cacheManager.Set(cachKey, services);
        }

        public SearchServiceDto PublicServicesList(int page, long categoryId, string seachKey)
        {
            List<PublicSearchServiceListDto> searchResult = new List<PublicSearchServiceListDto>();
            if (seachKey != "")
            {
                searchResult = (from a in _context.Applications
                                join s in _context.Services on a.Id equals s.ApplicationId
                                into sa
                                from saJoin in sa.DefaultIfEmpty()

                                join d in _context.CountryDistricts on a.CountryDistrictId equals d.Id
                                into da
                                from daJoin in da.DefaultIfEmpty()

                                join stat in _context.DistrictStates on a.DistrictStateId equals stat.Id
                                into astat
                                from astatJoin in astat.DefaultIfEmpty()

                                where a.CategoryId == categoryId && saJoin.IsPublished == true && a.Name.Contains(seachKey)
                                select new PublicSearchServiceListDto
                                {
                                    ServiceId = saJoin.Id,
                                    Name = a.Name,
                                    Location = daJoin.Name + " - " + astatJoin.Name,
                                    SearchImage = saJoin.SearchImage,
                                }).ToList();
            }
            var query = (from a in _context.Applications
                         join s in _context.Services on a.Id equals s.ApplicationId
                         into sa
                         from saJoin in sa.DefaultIfEmpty()

                         join d in _context.CountryDistricts on a.CountryDistrictId equals d.Id
                         into da
                         from daJoin in da.DefaultIfEmpty()

                         join stat in _context.DistrictStates on a.DistrictStateId equals stat.Id
                         into astat
                         from astatJoin in astat.DefaultIfEmpty()

                         where a.CategoryId == categoryId && saJoin.IsPublished == true
                         select new PublicSearchServiceListDto
                         {
                             ServiceId = saJoin.Id,
                             Name = a.Name,
                             Location = daJoin.Name + " - " + astatJoin.Name,
                             SearchImage = saJoin.SearchImage,
                         }).OrderBy(x => x.ServiceId).Skip((page - 1) * 15).Take(seachKey != "" ? 15 : (15 - searchResult.Count)).ToList();

            var servicesCount = (from a in _context.Applications
                                 join s in _context.Services on a.Id equals s.ApplicationId
                                 where a.CategoryId == categoryId && s.IsPublished == true
                                 select new { s.Id }).Count();

            foreach (var i in searchResult)
            {
                var existing = query.FirstOrDefault(x => x.ServiceId == i.ServiceId);
                query.Remove(existing);
            }

            decimal c = decimal.Parse(servicesCount.ToString()) / 15;

            SearchServiceDto res = new SearchServiceDto
            {
                Pages = int.Parse(Math.Ceiling(c).ToString()),
                Count = servicesCount,
                SearchResult = searchResult,
                Services = query,
            };

            return res;
        }

        public SearchServiceResultDto PublicServicesListFilter(long categoryId, long district = 0, long rating = 0, int page = 1)
        {
            var servicesCount = (from a in _context.Applications
                                 join s in _context.Services on a.Id equals s.ApplicationId
                                 into s2
                                 from s3 in s2.DefaultIfEmpty()

                                 join d in _context.CountryDistricts on a.CountryDistrictId equals d.Id
                                 into d2
                                 from d3 in d2.DefaultIfEmpty()

                                 where a.CategoryId == categoryId && d3.Id == district
                                 select new SelectDto { label = a.Name, value = s3.Id.ToString() }).ToList().Count();

            var services = (from a in _context.Applications
                            join s in _context.Services on a.Id equals s.ApplicationId
                            into sa
                            from saJoin in sa.DefaultIfEmpty()

                            join d in _context.CountryDistricts on a.CountryDistrictId equals d.Id
                            into da
                            from daJoin in da.DefaultIfEmpty()

                            join st in _context.DistrictStates on a.DistrictStateId equals st.Id
                            into ats
                            from atsJoin in ats.DefaultIfEmpty()

                            where a.CategoryId == categoryId && saJoin.IsPublished == true
                            where (district != 0 ? daJoin.Id == district : true == true)
                            select new PublicSearchServiceListDto
                            {
                                ServiceId = saJoin.Id,
                                Name = a.Name,
                                SearchImage = saJoin.SearchImage,
                                Location = daJoin.Name + " - " + atsJoin.Name
                            }).Skip(15 * (page - 1)).ToList();

            return new SearchServiceResultDto
            {
                Count = servicesCount,
                Pages = (int)Math.Ceiling(decimal.Parse(servicesCount.ToString("0.00")) / 15),
                Services = services
            };
        }

        public TResponseVM<PublicServiceDetailsDto> PublicServiceDetails(long serviceId)
        {
            var service = Get(serviceId);
            if (service == null)
                return new TResponseVM<PublicServiceDetailsDto> { HasError = true, StatusCode = 404, Message = $"Service Id : {serviceId} not exist" };

            PublicMainServiceDetailsDto mainDetails = new PublicMainServiceDetailsDto();
            if (!_cacheManager.Exist<PublicMainServiceDetailsDto>(CacheKeysEnum.serviceMainDetails + serviceId.ToString()))
            {
                mainDetails = (from a in _context.Applications
                               join s in _context.Services on a.Id equals s.ApplicationId
                               into sa
                               from saJoin in sa.DefaultIfEmpty()
                               where saJoin.Id == serviceId
                               select new PublicMainServiceDetailsDto
                               {
                                   Name = a.Name,
                                   ServiceId = saJoin.Id,
                                   MainImage = saJoin.MainImage,
                                   Description = saJoin.Description,
                                   CategoryId = a.CategoryId,
                                   Quote = saJoin.Quote,
                                   SearchImage = saJoin.SearchImage,
                                   YoutubeVideoId = saJoin.YoutubeVideoId,
                               }).FirstOrDefault();
                _cacheManager.Set<PublicMainServiceDetailsDto>(CacheKeysEnum.serviceMainDetails + serviceId.ToString(), mainDetails);
            }
            else
                mainDetails = _cacheManager.Get<PublicMainServiceDetailsDto>(CacheKeysEnum.serviceMainDetails + serviceId.ToString());

            // social media
            List<PublicServiceSocialMediaDto> socialMedia = new List<PublicServiceSocialMediaDto>();
            if (!_cacheManager.Exist<List<PublicServiceSocialMediaDto>>(CacheKeysEnum.serviceSocialMedia + serviceId.ToString()))
            {
                socialMedia = (from s in _context.ServiceSocialMedias
                               join i in _context.Icons on s.IconId equals i.Id
                               into si
                               from siJoin in si.DefaultIfEmpty()
                               where s.ServiceId == serviceId
                               select new PublicServiceSocialMediaDto
                               {
                                   Icon = siJoin.Name,
                                   Id = s.Id,
                                   Link = s.SocialMediaLink,
                                   Svg = siJoin.Svg,
                               }).ToList();
                _cacheManager.Set<List<PublicServiceSocialMediaDto>>(CacheKeysEnum.serviceSocialMedia + serviceId.ToString(), socialMedia);
            }
            else
                socialMedia = _cacheManager.Get<List<PublicServiceSocialMediaDto>>(CacheKeysEnum.serviceSocialMedia + serviceId.ToString());

            // Gallery
            List<PublicServiceGalleryDto> gallery = new List<PublicServiceGalleryDto>();
            if (!_cacheManager.Exist<List<PublicServiceGalleryDto>>(CacheKeysEnum.serviceGallery + serviceId.ToString()))
            {
                gallery = _context.ServiceGalleries
                    .Where(i => i.ServiceId == serviceId && i.IsDeleted == false)
                    .Select(x => new PublicServiceGalleryDto 
                    {
                        src = _configuration["ApiImagePath"] + x.Image,
                        height = x.Height,
                        width = x.Width
                    }).ToList();

                _cacheManager.Set<List<PublicServiceGalleryDto>>(CacheKeysEnum.serviceGallery + serviceId.ToString(), gallery);
            }
            else
                gallery = _cacheManager.Get<List<PublicServiceGalleryDto>>(CacheKeysEnum.serviceGallery + serviceId.ToString());

            // Packages
            List<PublicServicePackageDto> packages = new List<PublicServicePackageDto>();
            if (!_cacheManager.Exist<List<PublicServicePackageDto>>(CacheKeysEnum.servicePackage + serviceId.ToString()))
            {
                var dbPackages = _context.ServicePackages.Where(i => i.ServiceId == serviceId && i.IsPublished == true).ToList();
                foreach (var i in dbPackages)
                {
                    List<PublicServicePackageVariationDto> variationsList = new List<PublicServicePackageVariationDto>();
                    var variations = _context.ServicePackageAttributes.Where(x => x.ServicePackageId == i.Id).ToList();
                    foreach (var x in variations)
                    {
                        variationsList.Add(new PublicServicePackageVariationDto { Id = x.Id, Variation = x.Text });
                    }
                    packages.Add(new PublicServicePackageDto
                    {
                        Name = i.PackageName,
                        HasPrice = i.HasPrice,
                        Price = i.Price,
                        Variations = variationsList
                    });
                }
                _cacheManager.Set<List<PublicServicePackageDto>>(CacheKeysEnum.servicePackage + serviceId.ToString(), packages);
            }
            else
                packages = _cacheManager.Get<List<PublicServicePackageDto>>(CacheKeysEnum.servicePackage + serviceId.ToString());

            PublicServiceDetailsDto details = new PublicServiceDetailsDto
            {
                Details = mainDetails,
                Gallery = gallery,
                Packages = packages,
                SocialMedia = socialMedia
            };

            return new TResponseVM<PublicServiceDetailsDto> { HasError = false, StatusCode = 200, obj = details };
        }

        #endregion

        #region Private

        #endregion

    }
}
