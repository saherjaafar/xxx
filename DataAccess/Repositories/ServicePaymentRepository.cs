using AutoMapper;
using Common.ApiRequest;
using Common.Utilities;
using Core.Cache;
using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using Core.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Common.Enums.SD;

namespace DataAccess.Repositories
{
    public class ServicePaymentRepository : BaseRepository<ServicePayment>, IServicePaymentRepository
    {
        private readonly CacheManager _cacheManager;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IApiRequest _apiRequest;
        public ServicePaymentRepository(ApplicationDbContext context, CacheManager cacheManager, UnitOfWork unitOfWork, IMapper mapper, IApiRequest apiRequest) : base(context, cacheManager, unitOfWork, mapper, apiRequest)
        {
            _cacheManager = cacheManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _apiRequest = apiRequest;
        }

        public TResponseVM<ResponseVM> Add(AddServicePaymentDto body)
        {
            var service = _unitOfWork.Services.GetById(body.ServiceId);
            if (service is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 200, Message = $"Service Id {body.ServiceId} not exist" };

            var payment = new ServicePayment
            {
                ServiceId = body.ServiceId,
                Amount = body.Amount,
                Date = body.Date,
                Status = body.Status,
            };
            _context.ServicePayments.Add(payment);
            _unitOfWork.SaveChanges();

            return new TResponseVM<ResponseVM>{ HasError = false, StatusCode = 200, Message = "Payment Added Successfully"};
        }

        public TResponseVM<ServicePaymentListDto> List(long serviceId)
        {
            var service = _unitOfWork.Services.Get(serviceId, c => c.Payments);
            if (service is null)
                return new TResponseVM<ServicePaymentListDto> { HasError = true, StatusCode = 404, Message = $"Service Id {serviceId} not exist" };

            var payments = (from s in service.Payments
                            select new ServicePaymentListDto
                            {
                                Id = s.Id,
                                Amount = s.Amount,
                                Date = s.Date,
                                Status = s.Status,
                                StatusSelect = new SelectDto { label = s.Status, value = s.Status },
                                StrDate = s.Date.Day + " - " + s.Date.Month + " - " + s.Date.Year,
                            }).ToList();

            return new TResponseVM<ServicePaymentListDto> { HasError = false, StatusCode = 200, ListObj= payments };
        }

        public List<SelectDto> Statuses()
        {
            List<SelectDto> paymentTypes = new List<SelectDto>();
            foreach (var i in UtilClass.GetEnumValues<PaymentsStatusEnum>())
            {
                paymentTypes.Add(new SelectDto { label = i.ToString(), value = i.ToString() });
            }
            return paymentTypes;
        }

        public TResponseVM<ResponseVM> Update(UpdateServicePaymentDto body)
        {
            var payment = _context.ServicePayments.FirstOrDefault(i => i.Id == body.Id);
            if (payment is null)
                return new TResponseVM<ResponseVM> { HasError = true, StatusCode = 200, Message = $"Payment Id {body.Id} not exist" };

            payment.Status = body.Status;
            payment.Amount= body.Amount;
            payment.Date = body.Date;

            _unitOfWork.SaveChanges();
            return new TResponseVM<ResponseVM> { HasError = false, StatusCode = 200, Message = $"Payment Updated Successfully" };
        }
    }
}
