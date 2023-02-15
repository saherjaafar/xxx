using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;

namespace Core.Repositories
{
    public interface IServiceSocialMediaRepository : IBaseRepository<ServiceSocialMedia>
    {
        TResponseVM<ServiceSocialMediaListDto> List(long serviceId);
        TResponseVM<ManageServiceSocialMediaBodyDto> Manage(long serviceId, ManageServiceSocialMediaBodyDto body);
    }
}
