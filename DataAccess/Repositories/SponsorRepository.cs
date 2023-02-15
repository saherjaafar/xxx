using AutoMapper;
using Common.ApiRequest;
using Common.ApiRequest.Dto;
using Common.Utilities;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using Core.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class SponsorRepository : BaseRepository<Sponsor>, ISponsorRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public SponsorRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        #region Admin

        public Sponsor GetEntity(long sponsorId, params Expression<Func<Sponsor, object>>[] includes)
        {
            var query = _context.Sponsors.AsQueryable();
            return includes
                .Aggregate(
                    query.AsQueryable(),
                    (current, include) => current.Include(include)
                )
                .FirstOrDefault(e => e.Id == sponsorId);
        }

        public List<SelectDto> GetSponsorTypesSelect()
        {
            List<SelectDto> sponsorTypes = new List<SelectDto>();
            foreach (var i in UtilClass.GetEnumValues<SponsorTypeEnum>())
            {
                sponsorTypes.Add(new SelectDto { label = i.ToString(), value = i.ToString() });
            }
            return sponsorTypes;
        }

        public List<SponsorsListDto> List()
        {
            var list = (from s in _context.Sponsors
                       .Include(i => i.Service).ThenInclude(x => x.Application)
                        select new SponsorsListDto
                        {
                            id = s.Id,
                            serviceId = s.ServiceId,
                            title = s.Service.Application.Name,
                            start = s.StartDate.Year + "-" + s.StartDate.Month.ToString("00") + "-" + s.StartDate.Day.ToString("00"),
                            end = s.EndDate.Year + "-" + s.EndDate.Month.ToString("00") + "-" + s.EndDate.Day.ToString("00"),
                        }).ToList();
            return list;
        }

        public TResponseVM<SponsorToUpdateDto> GetSponsorToUpdate(long sponsorId)
        {
            var sponsor = GetEntity(sponsorId, c => c.SponsorTypes, c => c.Service.Application);
            if (sponsor is null)
                return new TResponseVM<SponsorToUpdateDto> { HasError = true, StatusCode = 404, Message = $"Sponsor id : {sponsorId} not exist" };

            List<SelectDto> typesList = new List<SelectDto>();
            foreach (var i in sponsor.SponsorTypes)
            {
                typesList.Add(new SelectDto { label = i.Type, value = i.Type });
            }

            var res = new SponsorToUpdateDto
            {
                id = sponsor.Id,
                service = new SelectDto { label = sponsor.Service.Application.Name, value = sponsor.ServiceId.ToString() },
                start = sponsor.StartDate.Year + "-" + sponsor.StartDate.Month + "-" + sponsor.StartDate.Day,
                end = sponsor.EndDate.Year + "-" + sponsor.EndDate.Month + "-" + sponsor.EndDate.Day,
                types = typesList
            };

            return new TResponseVM<SponsorToUpdateDto> { HasError = false, StatusCode = 200, obj = res };

        }

        public TResponseVM<ManageSponsorDto> Add(ManageSponsorDto body)
        {
            var checkSponsor = GetEntity(body.SponsorId, c => c.SponsorTypes);
            if (checkSponsor is not null)
                return new TResponseVM<ManageSponsorDto> { HasError = true, StatusCode = 404, Message = $"Sponsor id : {body.SponsorId} not exist" };

            var isServiceSponsored = false;
            foreach (var i in body.Types)
            {
                isServiceSponsored = IsServiceSponsored(body.ServiceId, body.StartDate, body.EndDate, i.value);
                if (isServiceSponsored)
                    break;
            }
            if (isServiceSponsored)
                return new TResponseVM<ManageSponsorDto> { HasError = true, StatusCode = 409, Message = "Profile Already Sponsored in a selected type" };

            var sponsor = new Sponsor
            {
                CreatedDate = DateTime.Now,
                ServiceId = body.ServiceId,
                StartDate = body.StartDate,
                EndDate = body.EndDate,
                Views = 0
            };
            _context.Sponsors.Add(sponsor);
            _unitOfWork.SaveChanges();

            foreach (var i in body.Types)
            {
                var type = new SponsorType
                {
                    SponsorId = sponsor.Id,
                    Type = i.value
                };
                _context.SponsorTypes.Add(type);
            }
            _unitOfWork.SaveChanges();

            return new TResponseVM<ManageSponsorDto> { HasError = false, StatusCode = 200, Message = "Action Submitted Successfully" };
        }

        public TResponseVM<ManageSponsorDto> Update(ManageSponsorDto body)
        {
            var sponsor = GetEntity(body.SponsorId, c => c.SponsorTypes);
            if (sponsor is null)
                return new TResponseVM<ManageSponsorDto> { HasError = true, StatusCode = 404, Message = $"Sponsor id : {body.SponsorId} not exist" };

            bool isSponsored = false;
            string type = "";
            foreach (var i in body.Types)
            {
                var sponsored = IsServiceSponsored(body.ServiceId, body.StartDate, body.EndDate, i.value, sponsor.Id);
                if (sponsored)
                {
                    isSponsored = true;
                    type = i.value;
                    break;
                }
            }
            if (isSponsored)
                return new TResponseVM<ManageSponsorDto> { HasError = true, StatusCode = 409, Message = "Profile Already Sponsored in a selected type" };

            sponsor.StartDate = body.StartDate;
            sponsor.EndDate = body.EndDate.ToUniversalTime();
            sponsor.ServiceId = body.ServiceId;

            foreach (var i in sponsor.SponsorTypes)
            {
                if (body.Types.FirstOrDefault(x => x.value == i.Type) == null)
                    _context.SponsorTypes.Remove(i);
            }
            foreach (var i in body.Types)
            {
                if (sponsor.SponsorTypes.FirstOrDefault(x => x.Type == i.value) == null)
                    _context.SponsorTypes.Add(new SponsorType { SponsorId = body.SponsorId, Type = i.value });
            }

            _unitOfWork.SaveChanges();
            return new TResponseVM<ManageSponsorDto> { HasError = false, StatusCode = 200, Message = "Action Submited Successfully" };
        }

        #endregion

        #region Public

        public List<PublicSponsorListDto> PublicList(long categoryId, string type, long? serviceId)
        {
            int count = (from s in _context.Sponsors
                         join t in _context.SponsorTypes on s.Id equals t.SponsorId
                         into st
                         from stJoin in st.DefaultIfEmpty()

                         join se in _context.Services on s.ServiceId equals se.Id
                         into sse
                         from sseJoin in sse.DefaultIfEmpty()

                         where stJoin.Type == type && s.ServiceId != serviceId && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now && sseJoin.IsPublished == true

                         select new { s.Id }).ToList().Count();

            Random rand = new Random();
            int countSkip = rand.Next(0, (count) > 8 ? count : 0);

            List<PublicSponsorListDto> res = (from s in _context.Sponsors
                                              join ser in _context.Services on s.ServiceId equals ser.Id
                                              into ss
                                              from ssJoin in ss.DefaultIfEmpty()

                                              join t in _context.SponsorTypes on s.Id equals t.SponsorId
                                              into t2
                                              from ttsJoin in t2.DefaultIfEmpty()

                                              join a in _context.Applications on ssJoin.ApplicationId equals a.Id
                                              into sa
                                              from saJoin in sa.DefaultIfEmpty()

                                              join d in _context.CountryDistricts on saJoin.CountryDistrictId equals d.Id
                                              into da
                                              from daJoin in da.DefaultIfEmpty()

                                              join st in _context.DistrictStates on saJoin.DistrictStateId equals st.Id
                                              into sst
                                              from sstJoin in sst.DefaultIfEmpty()

                                              where saJoin.CategoryId == categoryId && ttsJoin.Type == type && s.StartDate <= DateTime.Now && s.EndDate >= DateTime.Now && ssJoin.IsPublished == true
                                              where (serviceId != 0 ? (ssJoin.Id != serviceId) : (true == true))

                                              select new PublicSponsorListDto
                                              {
                                                  SponsorId = s.Id,
                                                  ServiceId = ssJoin.Id,
                                                  Name = saJoin.Name,
                                                  Image = ssJoin.SearchImage,
                                                  Location = daJoin.Name + " - " + sstJoin.Name
                                              }).Skip(countSkip).Take(countSkip > 8 ? 8 : (8 - countSkip)).ToList();

            res = UtilClass.ShuffleList<PublicSponsorListDto>(res);
            _apiRequest.SendAsync<ApiResponseModel>(new ApiRequestModel { ApiType = ApiType.POST, Url = "public/sponsor/UpdateSponsorView", Data = res });

            if (res.Count < 8)
            {
                int servicesCount = (from a in _context.Applications
                                     join s in _context.Services on a.Id equals s.ApplicationId
                                     where a.CategoryId == categoryId
                                     select new { a.Id }).Count();

                int skipper = rand.Next(0, (servicesCount - res.Count() - 8));

                var services = (from s in _context.Services
                                join a in _context.Applications on s.ApplicationId equals a.Id
                                into sa
                                from saJoin in sa.DefaultIfEmpty()

                                join d in _context.CountryDistricts on saJoin.CountryDistrictId equals d.Id
                                into da
                                from daJoin in da.DefaultIfEmpty()

                                join st in _context.DistrictStates on saJoin.DistrictStateId equals st.Id
                                into sst
                                from sstJoin in sst.DefaultIfEmpty()

                                where saJoin.CategoryId == categoryId && s.IsPublished == true
                                where (serviceId != 0 ? (s.Id != serviceId) : (true == true))

                                select new PublicSponsorListDto
                                {
                                    ServiceId = s.Id,
                                    Name = saJoin.Name,
                                    Location = daJoin.Name + " - " + sstJoin.Name,
                                    Image = s.SearchImage
                                }).Skip(skipper).Take(8 - res.Count).ToList();
                res.AddRange(services);
            }
            res = res.GroupBy(i => i.ServiceId).Select(i => i.First()).ToList();
            return res;
        }

        public void UpdateSponsorView(ListPublicSponsorListDto sponsors)
        {
            foreach (var i in sponsors.Sponsors)
            {
                var s = _context.Sponsors.FirstOrDefault(x => x.Id == i.SponsorId);
                s.Views = s.Views + 1;
                _context.SaveChanges();
            }
        }

        #endregion

        #region Private

        private bool IsServiceSponsored(long serviceId, DateTime startDate, DateTime endDate, string type, long sponsorId = 0)
        {
            var res = (from s in _context.Sponsors
                       join t in _context.SponsorTypes on s.Id equals t.SponsorId
                       into st
                       from stJoin in st.DefaultIfEmpty()
                       where s.ServiceId == serviceId && s.StartDate >= startDate && s.StartDate <= endDate && stJoin.Type == type
                       where sponsorId != 0 ? s.Id != sponsorId : true == true
                       select new { s.ServiceId }).FirstOrDefault();
            if (res is not null)
                return true;
            return false;
        }

        #endregion


    }
}
