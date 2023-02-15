using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IServicePaymentRepository : IBaseRepository<ServicePayment>
    {
        List<SelectDto> Statuses();
        TResponseVM<ServicePaymentListDto> List(long serviceId);
        TResponseVM<ResponseVM> Add(AddServicePaymentDto body);
        TResponseVM<ResponseVM> Update(UpdateServicePaymentDto body);
    }
}
