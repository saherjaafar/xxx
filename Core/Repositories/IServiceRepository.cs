using Core.Dto;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface IServiceRepository : IBaseRepository<Service>
    {
        #region Admin

        Service Get(long serviceId, params Expression<Func<Service, object>>[] includes);
        List<ServicesListDto> List();
        TResponseVM<SelectDto> ListSelectByCategory(long categoryId);
        List<SelectDto> ListSelect();
        TResponseVM<ServiceDetailsDto> GetServiceDetails(long serviceId);
        TResponseVM<ResponseVM> Add(AddServiceImagesDto images, AddServiceDto body);
        TResponseVM<ResponseVM> Update(UpdateServiceImagesDto images, UpdateServiceDetailsDto body);
        public TResponseVM<ResponseVM> ManagePublish(long ServiceId);

        #endregion

        #region Public

        List<SelectDto> ServiceCategorySelectList(long categoryId);
        void UpdateServiceCategorySelectListCache(long categoryId);
        SearchServiceDto PublicServicesList(int page, long categoryId, string seachKey);
        SearchServiceResultDto PublicServicesListFilter(long categoryId, long district = 0, long rating = 0, int page = 1);
        TResponseVM<PublicServiceDetailsDto> PublicServiceDetails(long serviceId);

        #endregion

    }
}
