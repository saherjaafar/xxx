using Core.Dto;
using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IFilterRepository : IBaseRepository<CategoryFilter>
    {
        #region Admin

        List<SelectDto> GetFiltersSelect();

        #endregion

        #region Public

        void UpdateCache(long? categoryId);
        List<SelectDto> GetCategoryFiltersListSelect(long categoryId);

        #endregion
    }
}
