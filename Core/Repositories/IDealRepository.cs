using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IDealRepository : IBaseRepository<Deal>
    {
        Deal GetEntity(long dealId, params Expression<Func<Deal, object>>[] includes);
        TResponseVM<DealDto> Get(long dealId);
        TResponseVM<DealListDto> List(long serviceId);
        TResponseVM<ResponseVM> Add(DealDto body);
        TResponseVM<ResponseVM> UpdateDeal(DealDto body);
    }
}
