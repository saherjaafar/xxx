using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto;
using Core.Models;
using Core.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class FilterRepository : BaseRepository<CategoryFilter>, IFilterRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public FilterRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        #region Admin

        public List<SelectDto> GetFiltersSelect()
        {
            List<SelectDto> filters = new List<SelectDto>();
            foreach (var i in Enum.GetValues(typeof(FiltersEnum)))
            {
                filters.Add(new SelectDto
                {
                    label = i.ToString(),
                    value = i.ToString(),
                });
            }
            return filters;
        }

        #endregion

        #region Public

        public List<SelectDto> GetCategoryFiltersListSelect(long categoryId)
        {
            var cachKey = CacheKeysEnum.AllowedFilters + categoryId.ToString();
            if (!_cacheManager.Exist<List<SelectDto>>(cachKey))
            {
                var filters = (from cf in _context.CategoryFilters
                               where cf.CategoryId == categoryId
                               select new SelectDto { label = cf.EnumKey, value = cf.EnumKey }).ToList();

                _cacheManager.Set<List<SelectDto>>(cachKey, filters);
                return filters;
            }
            else
                return _cacheManager.Get<List<SelectDto>>(cachKey);
        }

        public void UpdateCache(long? categoryId)
        {
            if(categoryId.HasValue)
            {
                var filters = (from cf in _context.CategoryFilters
                               where cf.CategoryId == categoryId
                               select new SelectDto { label = cf.EnumKey, value = cf.EnumKey }).ToList();

                _cacheManager.Set<List<SelectDto>>(CacheKeysEnum.AllowedFilters + categoryId.ToString(), filters);
            }
        }

        #endregion

    }
}
