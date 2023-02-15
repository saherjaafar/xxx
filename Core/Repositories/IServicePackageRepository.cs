using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using System.Linq.Expressions;
using System;

namespace Core.Repositories
{
    public interface IServicePackageRepository : IBaseRepository<ServicePackage>
    {
        ServicePackage Get(long servicePackageId, params Expression<Func<ServicePackage, object>>[] includes);
        TResponseVM<ServicePackageDto> Get(long serviceId);
        TResponseVM<ServicePackageDto> Add(ServicePackageDto body,long serviceId);
        TResponseVM<ServicePackageDto> Update(ServicePackageDto body);
        TResponseVM<ResponseVM> Delete(long PackageId);
    }
}
