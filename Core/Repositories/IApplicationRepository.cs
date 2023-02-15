using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IApplicationRepository : IBaseRepository<Application>
    {
        List<Expression<Func<Application, bool>>> GenerateCriterias(long? categoryId, long? districtId, long? stateId, bool? isCalled, bool? isApprovedForPublish);
        TResponseVM<ApplicationDetailsDto> Get(long applicationId);
        List<ApplicationsListDto> List(List<Expression<Func<Application, bool>>> criteria);
        TResponseVM<ResponseVM> Add(AddNewApplicationDto body);
        TResponseVM<ResponseVM> Update(UpdateApplicationDto body);
    }
}
