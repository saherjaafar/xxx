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
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class CategoryRepository : BaseRepository<Category>, ICategoryRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public CategoryRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        #region Admin

        public IEnumerable<ServiceCategoryListDTO> List()
        {
            return from c in _context.Categories
                   select new ServiceCategoryListDTO
                   {
                       ServiceCategoryId = c.Id,
                       ServiceCategoryName = c.Name,
                       IsPublished = c.IsPublished,
                       CreatedAt = c.CreatedAt.Day + "-" + c.CreatedAt.Month + "-" + c.CreatedAt.Year,
                   };
        }

        public IEnumerable<SelectDto> ListSelect()
        {
            return from c in _context.Categories
                   select new SelectDto
                   {
                       label = c.Name,
                       value = c.Id.ToString(),
                   };
        }

        public ServiceCategoryDetailsDto Details(long categoryId)
        {
            var category = (from c in _context.Categories.Include(i => i.CategoryDistricts).Include(i => i.Icon)
                            where c.Id == categoryId
                            select new ServiceCategoryDetailsDto
                            {
                                ServiceCategoryId = c.Id,
                                ServiceCategoryName = c.Name,
                                HomeImage = c.HomeImagePath,
                                Image = c.ImagePath,
                                Districts = c.CategoryDistricts.Select(i => new SelectDto { label = i.CountryDistrict.Name, value = i.CountryDistrict.Id.ToString() }).ToList(),
                                Filters = new List<SelectDto>(),
                                Icon = new SelectDto { label = c.Icon.Name, value = c.Icon.Id.ToString() }
                            }).FirstOrDefault();
            var filers = GetCategoryFiltersSelect(category.ServiceCategoryId);
            category.Filters = filers;
            return category;
        }


        public Category GetByName(string name)
        {
            return _context.Categories.FirstOrDefault(i => i.Name == name);
        }

        public List<CategoryFilter> GetCategoryFilters(long categoryId)
        {
            return _context.CategoryFilters.Where(i => i.CategoryId == categoryId).ToList();
        }

        public List<CategoryDistrict> GetCategoryDistricts(long categoryId)
        {
            return _context.CategoryDistricts.Where(i => i.CategoryId == categoryId).ToList();
        }

        public List<SelectDto> GetCategoryFiltersSelect(long categoryId)
        {
            return (from cf in _context.CategoryFilters
                    where cf.CategoryId == categoryId
                    select new SelectDto
                    {
                        label = cf.EnumKey,
                        value = cf.EnumKey
                    }).ToList();
        }

        public TResponseVM<ResponseVM> Add(UploadServiceCategoryImageDTO images, AddServiceCategoryDto body)
        {
            if (!_unitOfWork.Upload.CheckIfImageValidity(images.HomeImage) || !_unitOfWork.Upload.CheckIfImageValidity(images.Image))
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 415, Message = "Unsupported Media Type or Files damaged" };

            var existingCategory = GetByName(body.ServiceCategoryName);
            if (existingCategory is not null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 409, Message = $"Category : {body.ServiceCategoryName} already exist" };

            Category category = new Category
            {
                Name = body.ServiceCategoryName,
                IconId = body.IconId,
                IsPublished = false,
                ImagePath = "",
                HomeImagePath = "",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Categories.Add(category);
            _unitOfWork.SaveChanges();

            foreach (var i in body.Filters)
            {
                _context.CategoryFilters.Add(new CategoryFilter
                {
                    CategoryId = category.Id,
                    EnumKey = i.value
                });
            }
            _unitOfWork.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.AllowedFilters + category.Id.ToString());
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/Filter/updateCache" });

            foreach (var i in body.Districts)
            {
                _context.CategoryDistricts.Add(new CategoryDistrict
                {
                    CategoryId = category.Id,
                    CountryDistrictId = long.Parse(i.value)
                });
            }


            var imagePath = _unitOfWork.Upload.UploadDynamicImage(images.Image, "ServiceCategory");
            var homeImagePath = _unitOfWork.Upload.UploadDynamicImage(images.HomeImage, "ServiceCategory");
            category.ImagePath = imagePath;
            category.HomeImagePath = homeImagePath;

            _unitOfWork.SaveChanges();

            _cacheManager.Remove(CacheKeysEnum.PublicCategoriesList.ToString());
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/Category/UpdateCache" });

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Category Added Successfully" };
        }

        public TResponseVM<ResponseVM> Update(UploadServiceCategoryImageDTO images, UpdateServiceCategoryDto body)
        {
            var category = _unitOfWork.Categories.GetById(body.ServiceCategoryId);
            if (category is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Category id : {body.ServiceCategoryId} not exist" };

            if (images.Image is not null)
            {
                if (!_unitOfWork.Upload.CheckFileExtention(images.Image))
                    return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 415, Message = "Unsupported Media Type" };
            }
            if (images.HomeImage is not null)
            {
                if (!_unitOfWork.Upload.CheckFileExtention(images.HomeImage))
                    return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 415, Message = "Unsupported Media Type" };
            }

            category.Name = body.ServiceCategoryName;
            category.IconId = body.IconId;

            // Filters
            var dbFilters = GetCategoryFilters(category.Id);
            foreach (var i in dbFilters)
            {
                if (body.Filters.FirstOrDefault(x => x.value == i.EnumKey) == null)
                    _context.CategoryFilters.Remove(i);
            }
            foreach (var i in body.Filters)
            {
                if (dbFilters.FirstOrDefault(x => x.EnumKey == i.value) == null)
                    _context.CategoryFilters.Add(new CategoryFilter
                    {
                        CategoryId = category.Id,
                        EnumKey = i.value
                    });
            }
            _unitOfWork.SaveChanges();
            // update cache
            _cacheManager.Remove(CacheKeysEnum.AllowedFilters + category.Id.ToString());
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/Filter/updateCache" });

            // Districts
            var dbDistricts = GetCategoryDistricts(category.Id);
            foreach (var i in dbDistricts)
            {
                if (body.Districts.FirstOrDefault(x => x.value == i.CountryDistrictId.ToString()) == null)
                    _context.CategoryDistricts.Remove(i);
            }
            foreach (var i in body.Districts)
            {
                if (dbDistricts.FirstOrDefault(x => x.CountryDistrictId == long.Parse(i.value)) == null)
                    _context.CategoryDistricts.Add(new CategoryDistrict
                    {
                        CategoryId = category.Id,
                        CountryDistrictId = long.Parse(i.value)
                    });
            }

            // images
            if (images.Image is not null)
            {
                var imagePath = _unitOfWork.Upload.UploadDynamicImage(images.Image, "ServiceCategory");
                category.ImagePath = imagePath;
            }
            if (images.HomeImage is not null)
            {
                var imagePath = _unitOfWork.Upload.UploadDynamicImage(images.HomeImage, "ServiceCategory");
                category.HomeImagePath = imagePath;
            }
            _unitOfWork.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.CountryDistrictSelect.ToString() + "-cat-" + category.Id);
            _cacheManager.Remove(CacheKeysEnum.PublicCategoriesList.ToString());

            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/Category/UpdateCache" });

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Category Updated Successfully" };
        }

        public TResponseVM<ResponseVM> ManagePublish(long categoryId)
        {
            var category = _unitOfWork.Categories.GetById(categoryId);
            if (category is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Category id : {categoryId} not exist" };

            category.IsPublished = !category.IsPublished;
            _unitOfWork.SaveChanges();

            // update cache
            _cacheManager.Remove(CacheKeysEnum.PublicCategoriesList.ToString());
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/Category/UpdateCache" });

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Category Updated Successfully" };
        }

        #endregion

        #region Public

        public void UpdateCache()
        {
            UpdatePublicCategoryListCache();
        }

        public List<PublicCategoryListDto> PublicCategoriesList()
        {
            List<PublicCategoryListDto> list = new List<PublicCategoryListDto>();
            if (!_cacheManager.Exist<List<PublicCategoryListDto>>(CacheKeysEnum.PublicCategoriesList.ToString()))
            {
                var categories = _context.Categories.Where(i => i.IsPublished == true).ToList();
                foreach (var category in categories)
                {
                    var servicesCount = (from a in _context.Applications
                                         join s in _context.Services on a.Id equals s.ApplicationId
                                         into sa
                                         from asJoin in sa.DefaultIfEmpty()
                                         where a.CategoryId == category.Id && asJoin.IsPublished == true
                                         select new { a.Id }).ToList().Count();

                    list.Add(new PublicCategoryListDto
                    {
                        ServiceCategoryId = category.Id,
                        ServiceCategoryName = category.Name,
                        SuppliersCount = servicesCount,
                        SvgIcon = "",
                        Image360x360 = category.ImagePath,
                        Image620x350 = category.HomeImagePath
                    });
                }
                _cacheManager.Set<List<PublicCategoryListDto>>(CacheKeysEnum.PublicCategoriesList.ToString(), list);
            }
            else
                list = _cacheManager.Get<List<PublicCategoryListDto>>(CacheKeysEnum.PublicCategoriesList.ToString());
            return list;
        }

        #endregion

        #region Private

        private void UpdatePublicCategoryListCache()
        {
            List<PublicCategoryListDto> list = new List<PublicCategoryListDto>();
            var categories = _context.Categories.Where(i => i.IsPublished == true).ToList();
            foreach (var category in categories)
            {
                var servicesCount = (from a in _context.Applications
                                     join s in _context.Services on a.Id equals s.ApplicationId
                                     into sa
                                     from asJoin in sa.DefaultIfEmpty()
                                     where a.CategoryId == category.Id && asJoin.IsPublished == true
                                     select new { a.Id }).ToList().Count();

                list.Add(new PublicCategoryListDto
                {
                    ServiceCategoryId = category.Id,
                    ServiceCategoryName = category.Name,
                    SuppliersCount = servicesCount,
                    SvgIcon = "",
                    Image360x360 = category.ImagePath,
                    Image620x350 = category.HomeImagePath
                });
            }
            _cacheManager.Set<List<PublicCategoryListDto>>(CacheKeysEnum.PublicCategoriesList.ToString(), list);
        }


        #endregion
    }
}
