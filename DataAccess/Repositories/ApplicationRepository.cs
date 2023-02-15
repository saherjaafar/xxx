using AutoMapper;
using Common.ApiRequest;
using Common.ApiRequest.Dto;
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
    public class ApplicationRepository : BaseRepository<Application>, IApplicationRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public ApplicationRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }


        public TResponseVM<ApplicationDetailsDto> Get(long applicationId)
        {
            var application = (from a in _context.Applications
                              .Include(i => i.Country)
                              .Include(i => i.CountryDistrict).Include(i => i.DistrictState)
                              .Include(i => i.ApplicationFollowUps).Include(i => i.Service)
                              .Where(i => i.Id == applicationId)
                               select new ApplicationDetailsDto
                               {
                                   SupplierName = a.Name,
                                   Country = new SelectDto { label = a.Country.Name, value = a.Country.Id.ToString() },
                                   Category = new SelectDto { label = a.Category.Name, value = a.Category.Id.ToString() },
                                   CountryDistrict = new SelectDto { label = a.CountryDistrict.Name, value = a.CountryDistrict.Id.ToString() },
                                   DistrictState = new SelectDto { label = a.DistrictState.Name, value = a.DistrictState.Id.ToString() },
                                   HasMeeting = new SelectDto { label = a.HasMeeting.ToString(), value = a.HasMeeting.ToString() },
                                   IsApprovedForPublish = a.IsApprovedForPublish,
                                   IsCalled = a.IsCalled,
                                   MeetingDate = a.MeetingDate,
                                   Note = a.Note,
                                   PhoneNumber = a.Number,
                                   ServiceId = a.Service == null ? 0 : a.Service.Id,
                                   IsNeedFollowUp = a.IsNeedFollowUp,
                                   FollowUpDate = (DateTime)(a.FollowUpDate == null ? DateTime.Now : a.FollowUpDate),
                                   FollowUps = a.ApplicationFollowUps
                               }).FirstOrDefault();
            if (application is null)
                return new TResponseVM<ApplicationDetailsDto> { HasError = true, StatusCode = 404, Message = $"Application Id : {applicationId} not exist" };

            return new TResponseVM<ApplicationDetailsDto> { HasError = false, StatusCode = 200, obj = application };
        }

        public List<ApplicationsListDto> List(List<Expression<Func<Application, bool>>> criteria)
        {
            IQueryable<Application> query = _context.Applications.AsQueryable();

            if (criteria != null)
                foreach (var c in criteria)
                    query = query.Where(c);

            query = query.Include(i => i.Country)
                               .Include(i => i.CountryDistrict)
                               .Include(i => i.DistrictState);

            IQueryable<ApplicationsListDto> list = (from a in query
                                                    select new ApplicationsListDto
                                                    {
                                                        Id = a.Id,
                                                        Name = a.Name,
                                                        IsCalled = a.IsCalled,
                                                        HasMeeting = a.HasMeeting,
                                                        IsApprovedForPublish = a.IsApprovedForPublish,
                                                        Date = a.MeetingDate.Date.Day.ToString() + "-" + a.MeetingDate.Date.Month.ToString() + "-" + a.MeetingDate.Date.Year.ToString(),
                                                        Category = a.Category.Name,
                                                        Country = a.Country.Name,
                                                        District = a.CountryDistrict.Name,
                                                        State = a.DistrictState.Name,
                                                    });
            return list.ToList();
        }

        public List<Expression<Func<Application, bool>>> GenerateCriterias(long? categoryId, long? districtId, long? stateId, bool? isCalled, bool? NeedFollowUp)
        {
            List<Expression<Func<Application, bool>>> criteria = new List<Expression<Func<Application, bool>>>();
            if (categoryId.HasValue && categoryId != 0)
                criteria.Add(a => a.CategoryId == categoryId);
            if (districtId.HasValue && districtId != 0)
                criteria.Add(a => a.CountryDistrictId == districtId);
            if (stateId.HasValue && stateId != 0)
                criteria.Add(a => a.DistrictStateId == stateId);
            if (isCalled.HasValue)
                criteria.Add(a => a.IsCalled == isCalled);
            if (NeedFollowUp.HasValue)
                criteria.Add(a => a.IsNeedFollowUp == NeedFollowUp);
            return criteria;
        }

        public TResponseVM<ResponseVM> Add(AddNewApplicationDto body)
        {
            try
            {
                Application app = new Application
                {
                    Name = body.SupplierName,
                    CountryId = body.CountryId,
                    CountryDistrictId = body.CountryDistrictId,
                    DistrictStateId = body.DistrictStateId,
                    Number = body.PhoneNumber,
                    HasMeeting = body.HasMeeting,
                    MeetingDate = body.MeetingDate,
                    Intrested = false,
                    Note = body.Note,
                    CategoryId = body.CategoryId,
                    IsCalled = body.IsCalled,
                    IsApprovedForPublish = body.IsApprovedForPublish,
                    IsNeedFollowUp = body.IsNeedFollowUp,
                    FollowUpDate = body.IsNeedFollowUp ? body.FollowUpDate : null,
                };
                _context.Applications.Add(app);
                _unitOfWork.SaveChanges();
                return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Application Created Successfully" };
            }
            catch (Exception ex)
            {
                return new TResponseVM<ResponseVM> { StatusCode = 400, HasError = true, Message = ex.Message };
            }

        }

        public TResponseVM<ResponseVM> Update(UpdateApplicationDto body)
        {
            var application = _unitOfWork.Applications.GetById(body.ApplicationId);
            if (application is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 404, Message = $"Application Id : {body.ApplicationId} not exist" };

            bool needUpdateCache = false;
            long categoryToUpdate = application.CategoryId;

            if (application.Name != body.SupplierName || application.CategoryId != body.CategoryId)
            needUpdateCache= true;
            if(application.CategoryId != body.CategoryId)
                categoryToUpdate = application.CategoryId;

            application.Name = body.SupplierName;
            application.CountryId = body.CountryId;
            application.CountryDistrictId = body.CountryDistrictId;
            application.DistrictStateId = body.DistrictStateId;
            application.Number = body.PhoneNumber;
            application.HasMeeting = body.HasMeeting;
            application.MeetingDate = body.MeetingDate;
            application.Note = body.Note;
            application.CategoryId = body.CategoryId;
            application.IsCalled = body.IsCalled;
            application.IsApprovedForPublish = body.IsApprovedForPublish;
            application.IsNeedFollowUp= body.IsNeedFollowUp;
            application.FollowUpDate = body.IsNeedFollowUp ? body.FollowUpDate : null;

            _unitOfWork.SaveChanges();

            if (needUpdateCache)
            {
                _cacheManager.Remove(CacheKeysEnum.ServiceCategoryList + "-" + application.CategoryId);
                _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/service/UpdateServiceCategorySelectListCache/" + application.CategoryId });
                _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.GET, Url = "public/service/UpdateServiceCategorySelectListCache/" + categoryToUpdate });
            }

            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = "Application Updated Successfully" };
            }
    }
}
