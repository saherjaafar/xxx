using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using Core.Repositories;
using DataAccess.Services;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class CountryRepository : BaseRepository<Country>, ICountryRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        private readonly CountryService _countryService;
        public CountryRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest, CountryService countryService) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
            _countryService = countryService;
        }

        #region Admin

        public Country GetEntity(long countryId, params Expression<Func<Country, object>>[] includes)
        {
            var query = _context.Countries.AsQueryable();
            return includes
                .Aggregate(
                    query.AsQueryable(),
                    (current, include) => current.Include(include)
                )
                .FirstOrDefault(e => e.Id == countryId);
        }

        public IEnumerable<CountriesListDto> GetList()
        {
            return (from c in _context.Countries
                    select new CountriesListDto
                    {
                        CountryId = c.Id,
                        Name = c.Name,
                        Suffix = c.Suffix,
                        IsPublished = c.IsPublished,
                    }).ToList();
        }

        public IEnumerable<SelectDto> GetSelectList()
        {
            return (from c in _context.Countries
                    select new SelectDto
                    {
                        label = c.Name,
                        value = c.Id.ToString(),
                    }).ToList();
        }

        public TResponseVM<CountryDetailsDto> GetDetails(long id)
        {
            var country = _countryService.GetById(id);
            if (country is null)
                return new TResponseVM<CountryDetailsDto> { HasError = true, StatusCode = 404, Message = $"Country id : {id} not exist" };

            return new TResponseVM<CountryDetailsDto>
            {
                HasError = false,
                StatusCode = 200,
                obj = new CountryDetailsDto
                {
                    CountryId = country.Id,
                    Name = country.Name,
                    Suffix = country.Suffix,
                    Currency = country.Currency,
                    Icon = new SelectDto { label = country.Icon.Name, value = country.Icon.Id.ToString() },
                }
            };
        }

        public TResponseVM<SelectDto> GetDistrictsSelect(long countryId)
        {
            var country = _unitOfWork.Countries.GetById(countryId);
            if (country is null)
                return new TResponseVM<SelectDto> { HasError = true, StatusCode = 404, Message = $"Country id : {countryId} not exist" };

            var districts = (from d in _context.CountryDistricts
                             where d.CountryId == countryId
                             select new SelectDto
                             {
                                 label = d.Name,
                                 value = d.Id.ToString()
                             }).ToList();

            return new TResponseVM<SelectDto>
            {
                HasError = false,
                StatusCode = 200,
                ListObj = districts
            };
        }

        public TResponseVM<SelectDto> GetDistrictStatesSelect(long districtId)
        {
            var district = GetCountryDistrict(districtId);
            if (district is null)
                return new TResponseVM<SelectDto> { HasError = true, StatusCode = 404, Message = $"District id : {districtId} not exist" };

            var states = (from s in _context.DistrictStates
                          where s.CountryDistrictId == districtId
                          select new SelectDto
                          {
                              label = s.Name,
                              value = s.Id.ToString()
                          }).ToList();
            return new TResponseVM<SelectDto>
            {
                HasError = false,
                StatusCode = 200,
                ListObj = states
            };
        }

        public CountryDistrict GetCountryDistrict(long districtId)
        {
            return _context.CountryDistricts.FirstOrDefault(i => i.Id == districtId);
        }

        public TResponseVM<AddCountryDto> AddCountry(CountryDto country) => throw new System.NotImplementedException();

        public TResponseVM<UpdateCountryDto> UpdateCountry(CountryDto country) => throw new System.NotImplementedException();

        public TResponseVM<ResponseVM> ManagePublish(long countryId)
        {
            var country = _unitOfWork.Countries.GetById(countryId);
            if (country is null)
                return new TResponseVM<ResponseVM>() { HasError = true, StatusCode = 404, Message = $"Country id {countryId} not exist" };

            country.IsPublished = !country.IsPublished;
            _unitOfWork.SaveChanges();

            return new TResponseVM<ResponseVM>() { HasError = false, StatusCode = 200, Message = "Country Updated Successfully" };
        }

        public TResponseVM<CountryDistrictListDto> GetCountryDistrictsList(long countryId)
        {
            var country = GetEntity(countryId, c => c.CountryDistricts);
            if (country is null)
                return new TResponseVM<CountryDistrictListDto> { HasError = true, StatusCode = 404, Message = $"Country id {countryId} not exist" };

            var list = new List<CountryDistrictListDto>();
            foreach (var i in country.CountryDistricts)
            {
                list.Add(new CountryDistrictListDto
                {
                    Id = i.Id,
                    Name = i.Name,
                });
            }

            return new TResponseVM<CountryDistrictListDto> { HasError = false, StatusCode = 200, ListObj = list };
        }

        public TResponseVM<CountryDistrictDetailsDto> GetDistrict(long districtId)
        {
            var district = (from d in _context.CountryDistricts
                            where d.Id == districtId
                            select new CountryDistrictDetailsDto { Id = d.Id, Name = d.Name })
                           .FirstOrDefault();

            if (district is null)
                return new TResponseVM<CountryDistrictDetailsDto> { HasError = true, StatusCode = 404, Message = $"District Id : {districtId} not exist" };

            return new TResponseVM<CountryDistrictDetailsDto> { HasError = false, StatusCode = 200, obj = district };
        }

        public TResponseVM<DistrictStatesListDto> GetDistrictStatesList(long districtId)
        {
            var district = GetDistrict(districtId);
            if (district.obj is null)
                return new TResponseVM<DistrictStatesListDto> { HasError = true, StatusCode = 404, Message = $"District Id : {districtId} not exist" };

            var states = (from s in _context.DistrictStates
                          where s.CountryDistrictId == districtId
                          select new DistrictStatesListDto
                          {
                              Id = s.Id,
                              Name = s.Name,
                          }).ToList();

            return new TResponseVM<DistrictStatesListDto> { HasError = false, StatusCode = 200, ListObj = states };
        }

        public TResponseVM<AddDistrictStateDto> AddState(AddDistrictStateDto body)
        {
            var district = GetDistrict(body.DistrictId);
            if (district.obj is null)
                return new TResponseVM<AddDistrictStateDto> { HasError = true, StatusCode = 404, Message = $"District Id {body.DistrictId} not exist" };

            var state = new DistrictState
            {
                CountryDistrictId = body.DistrictId,
                Name = body.Name,
            };
            _context.DistrictStates.Add(state);
            _unitOfWork.SaveChanges();
            return new TResponseVM<AddDistrictStateDto> { HasError = false, StatusCode = 200, Message = "State Added Successfully" };

        }

        public TResponseVM<UpdateDistrictStateDto> UpdateState(UpdateDistrictStateDto body)
        {
            var state = _context.DistrictStates.FirstOrDefault(i => i.Id == body.StateId);
            if (state is null)
                return new TResponseVM<UpdateDistrictStateDto> { HasError = true, StatusCode = 404, Message = $"State Id : {state.Id} not exist" };

            state.Name = body.Name;
            _unitOfWork.SaveChanges();

            return new TResponseVM<UpdateDistrictStateDto> { HasError = false, StatusCode = 200, Message = $"State Updated Successfully" };
        }


        #endregion

        #region Public

        public List<SelectDto> PublicDistrictsCategoryListSelect(long categoryId)
        {
            List<SelectDto> districts = new List<SelectDto>();
            if (!_cacheManager.Exist<List<SelectDto>>(CacheKeysEnum.CountryDistrictSelect.ToString() + "-cat-" + categoryId))
            {
                districts = (from cd in _context.CategoryDistricts
                             join d in _context.CountryDistricts on cd.CountryDistrictId equals d.Id
                             into cdd
                             from cddJoin in cdd.DefaultIfEmpty()
                             where cd.CategoryId == categoryId
                             select new SelectDto
                             {
                                 label = cddJoin.Name,
                                 value = cddJoin.Id.ToString()
                             }).ToList();
                _cacheManager.Set<List<SelectDto>>(CacheKeysEnum.CountryDistrictSelect.ToString() + "-cat-" + categoryId, districts);
            }
            else
                districts = _cacheManager.Get<List<SelectDto>>(CacheKeysEnum.CountryDistrictSelect.ToString() + "-cat-" + categoryId);
            return districts;
        }

        #endregion
    }
}
