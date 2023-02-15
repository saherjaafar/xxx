using AutoMapper;
using Common.ApiRequest;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repositories
{
    public class DealRepository : BaseRepository<Deal>, IDealRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public DealRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        public Deal GetEntity(long dealId, params Expression<Func<Deal, object>>[] includes)
        {
            var query = _context.Deals.AsQueryable();
            return includes
                .Aggregate(
                    query,
                    (current, include) => current.Include(include)
                )
                .FirstOrDefault(e => e.Id == dealId);
        }

        public TResponseVM<DealDto> Get(long dealId)
        {
            var deal = GetEntity(dealId, c => c.DealDistricts, c => c.DealVariations);
            if (deal is null)
                return new TResponseVM<DealDto> { HasError = true, StatusCode = 404, Message = $"Deal id : {dealId} not exist" };

            var dto = new DealDto
            {
                DealId = dealId,
                Description = deal.Description,
                Districts = GenerateDealDistrictsSelect(deal.DealDistricts),
                Price = deal.Price,
                Publish = deal.IsPublished,
                ServiceId = deal.ServiceId,
                ShowPrice = deal.ShowPrice,
                Title = deal.Title,
                Variations = GenerateDealVariationDto(deal.DealVariations)
            };

            return new TResponseVM<DealDto> { HasError = false, StatusCode = 200, obj = dto} ;
        }

        public TResponseVM<DealListDto> List(long serviceId)
        {
            var service = _unitOfWork.Services.Get(serviceId, c => c.Deals);
            if (service is null)
                return new TResponseVM<DealListDto> { HasError = true, StatusCode = 404, Message = $"Service Id : {serviceId} not exist" };

            var list = new List<DealListDto>();
            foreach(var i in service.Deals)
            {
                list.Add(new DealListDto
                {
                    CreationDate = i.CreationDate.Day + " - " + i.CreationDate.Month + " - " + i.CreationDate.Year,
                    DealId = i.Id,
                    IsPublished = i.IsPublished,
                    ShowPrice = i.ShowPrice,
                    Title = i.Title,
                });
            }
            return new TResponseVM<DealListDto> { HasError = false, StatusCode = 200, ListObj= list} ;
        }

        public TResponseVM<ResponseVM> Add(DealDto body)
        {
            var service = _unitOfWork.Services.Get(body.ServiceId);
            if (service is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Service Id : {body.ServiceId} not exist" };

            Deal deal = new Deal
            {
                IsPublished = body.Publish,
                ServiceId = body.ServiceId,
                CreationDate = DateTime.Now,
                ImageUrl = "",
                Description = body.Description,
                Price = body.Price,
                ShowPrice = body.ShowPrice,
                Title = body.Title,
            };
            _unitOfWork.Deals.Add(deal);
            _unitOfWork.SaveChanges();

            foreach(var i in body.Districts)
            {
                DealDistrict district = new DealDistrict
                {
                    DealId = deal.Id,
                    CountryDistrictId = long.Parse(i.value),
                };
                _context.DealDistricts.Add(district);
            }

            foreach(var i in body.Variations)
            {
                DealVariation variation = new DealVariation
                {
                    DealId = deal.Id,
                    Text = i.Text
                };
                _context.DealVariations.Add(variation);
            }


            _unitOfWork.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 200, Message = "Deal Added Successfully" };
        }

        public TResponseVM<ResponseVM> UpdateDeal(DealDto body)
        {
            var deal = GetEntity(body.DealId, c => c.DealDistricts, c => c.DealVariations);
            if (deal is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Deal id : {body.DealId} not exist" };

            deal.Title = body.Title;
            deal.Price = body.Price;
            deal.Description = body.Description;
            deal.ShowPrice = body.ShowPrice;
            deal.IsPublished = body.Publish;

            // Variations
            foreach (var i in deal.DealVariations)
            {
                if (body.Variations.FirstOrDefault(x => x.VariationId == i.Id) == null)
                    _context.DealVariations.Remove(i);
            }

            foreach (var i in body.Variations)
            {
                var variation = deal.DealVariations.FirstOrDefault(x => x.Id == i.VariationId);
                if ( variation == null)
                    _context.DealVariations.Add(new DealVariation
                    {
                        DealId = body.DealId,
                        Text = i.Text
                    });
                else
                    variation.Text = i.Text;
            }

            // Districts
            foreach (var i in deal.DealDistricts)
            {
                if (body.Districts.FirstOrDefault(x => x.value == i.Id.ToString()) == null)
                    _context.DealDistricts.Remove(i);
            }

            foreach (var i in body.Districts)
            {
                if (deal.DealDistricts.FirstOrDefault(x => x.Id == long.Parse(i.value)) == null)
                    _context.DealDistricts.Add(new DealDistrict
                    {
                        DealId = body.DealId,
                        CountryDistrictId = long.Parse(i.value),
                    });
            }
            _unitOfWork.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Deal Updated Successfully" };
        }

        #region Private Methods

        private List<SelectDto> GenerateDealDistrictsSelect(ICollection<DealDistrict> dealDistricts)
        {
            var districts = _unitOfWork.Countries.GetDistrictsSelect(1).ListObj;
            List<SelectDto> res = new List<SelectDto>();
            foreach (var i in dealDistricts)
            {
                res.Add(new SelectDto
                {
                    label = districts.FirstOrDefault(x => x.value == i.CountryDistrictId.ToString()).label,
                    value = districts.FirstOrDefault(x => x.value == i.CountryDistrictId.ToString()).value,
                });
            }
            return res;
        }

        private List<DealVariationDto> GenerateDealVariationDto(ICollection<DealVariation> dealVariations)
        {
            List<DealVariationDto> res = new List<DealVariationDto>();
            foreach (var i in dealVariations)
            {
                res.Add(new DealVariationDto
                {
                    Text = i.Text,
                    VariationId = i.Id
                });
            }
            return res;
        }
        #endregion

    }
}
