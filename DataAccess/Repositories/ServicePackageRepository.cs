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
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class ServicePackageRepository : BaseRepository<ServicePackage>, IServicePackageRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public ServicePackageRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        public ServicePackage Get(long servicePackageId, params Expression<Func<ServicePackage, object>>[] includes)
        {
            var query = _context.ServicePackages.AsQueryable();
            return includes
                .Aggregate(
                    query.AsQueryable(),
                    (current, include) => current.Include(include)
                )
                .FirstOrDefault(e => e.Id == servicePackageId);
        }

        public TResponseVM<ServicePackageDto> Get(long serviceId)
        {
            var service = _unitOfWork.Services.Get(serviceId, c => c.ServicePackage);
            if (service is null)
                return new TResponseVM<ServicePackageDto> { HasError = true, StatusCode = 404, Message = $"Service id {serviceId} not exist" };

            var packages = new List<ServicePackageDto>();
            
            foreach (var i in service.ServicePackage)
            {
                var attributes = (from pa in _context.ServicePackageAttributes
                                  where pa.ServicePackageId == i.Id
                                  select new PackageVariationDto { ItemId = pa.Id, Name = pa.Text }).ToList();

                packages.Add(new ServicePackageDto
                {
                    HasPrice = i.HasPrice,
                    IsPublished = i.IsPublished,
                    PackageId = i.Id,
                    PackageName = i.PackageName,
                    Price = i.Price,
                    Items = attributes
                });
            }
            return new TResponseVM<ServicePackageDto> { HasError = false, StatusCode = 200, ListObj = packages };
        }

        public TResponseVM<ServicePackageDto> Add(ServicePackageDto body, long serviceId)
        {
            var service = _unitOfWork.Services.Get(serviceId);
            if (service is null)
                return new TResponseVM<ServicePackageDto> { HasError = true, StatusCode = 404, Message = $"Service id = {serviceId} not exist" };

            var package = new ServicePackage
            {
                HasPrice = body.HasPrice,
                IsPublished = body.IsPublished,
                PackageName = body.PackageName,
                Price = body.Price,
                ServiceId = serviceId,
            };
            _context.ServicePackages.Add(package);
            _unitOfWork.SaveChanges();

            foreach (var i in body.Items)
            {
                var variation = _context.ServicePackageAttributes.Add(new ServicePackageAttribute
                {
                    ServicePackageId = package.Id,
                    Text = i.Name
                });
            }

            _unitOfWork.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.servicePackage + serviceId.ToString());
            return new TResponseVM<ServicePackageDto> { HasError = false, StatusCode = 200, Message = "Package Added Successfully" };
        }

        public TResponseVM<ServicePackageDto> Update(ServicePackageDto body)
        {
            var package = Get(body.PackageId, p => p.PackageAttributes);
            if (package is null)
                return new TResponseVM<ServicePackageDto> { HasError = true, StatusCode = 404, Message = $"Package id : {body.PackageId} not exist" };

            package.IsPublished = body.IsPublished;
            package.HasPrice = body.HasPrice;
            package.Price = body.Price;
            package.PackageName = body.PackageName;

            var items = package.PackageAttributes.ToList();
            foreach (var i in body.Items)
            {
                if (i.ItemId == 0)
                    _context.Add(new ServicePackageAttribute
                    {
                        ServicePackageId = body.PackageId,
                        Text = i.Name,
                    });
                else
                {
                    var attribute = _context.ServicePackageAttributes.FirstOrDefault(i => i.Id == i.Id);
                    attribute.Text = i.Name;
                }
            }
            foreach (var i in items)
            {
                var item = body.Items.FirstOrDefault(x => x.ItemId == i.Id);
                if (item == null)
                    _context.ServicePackageAttributes.Remove(i);
            }
            _unitOfWork.SaveChanges();
            _cacheManager.Remove(CacheKeysEnum.servicePackage + package.ServiceId.ToString());
            return new TResponseVM<ServicePackageDto> { HasError = false, StatusCode = 200, Message = "Package Updated Successfully" };
        }

        public TResponseVM<ResponseVM> Delete(long PackageId)
        {
            var package = Get(PackageId, p => p.PackageAttributes);

            if (package is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Package id : {PackageId} not exist" };

            _context.ServicePackageAttributes.RemoveRange(package.PackageAttributes);
            _context.ServicePackages.Remove(package);
            _unitOfWork.SaveChanges();

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Package Delete Successfully" };
        }
    }
}
