using Core.Dto;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
        #region Admin

        IEnumerable<ServiceCategoryListDTO> List();
        IEnumerable<SelectDto> ListSelect();
        ServiceCategoryDetailsDto Details(long categoryId);
        Category GetByName(string name);
        List<SelectDto> GetCategoryFiltersSelect(long categoryId);
        List<CategoryFilter> GetCategoryFilters(long categoryId);
        List<CategoryDistrict> GetCategoryDistricts(long categoryId);
        TResponseVM<ResponseVM> Add(UploadServiceCategoryImageDTO images, AddServiceCategoryDto body);
        TResponseVM<ResponseVM> Update(UploadServiceCategoryImageDTO images, UpdateServiceCategoryDto body);
        TResponseVM<ResponseVM> ManagePublish(long categoryId);

        #endregion

        #region Public

        void UpdateCache();
        List<PublicCategoryListDto> PublicCategoriesList();

        #endregion
    }
}
