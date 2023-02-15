using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Core.Repositories
{
    public interface ICountryRepository : IBaseRepository<Country>
    {
        #region Admin

        Country GetEntity(long countryId, params Expression<Func<Country, object>>[] includes);
        IEnumerable<CountriesListDto> GetList();
        IEnumerable<SelectDto> GetSelectList();
        TResponseVM<CountryDetailsDto> GetDetails(long id);
        TResponseVM<SelectDto> GetDistrictsSelect(long countryId);
        TResponseVM<SelectDto> GetDistrictStatesSelect(long districtId);
        TResponseVM<DistrictStatesListDto> GetDistrictStatesList(long districtId);
        CountryDistrict GetCountryDistrict(long districtId);
        TResponseVM<AddCountryDto> AddCountry(CountryDto country);
        TResponseVM<UpdateCountryDto> UpdateCountry(CountryDto country);
        TResponseVM<ResponseVM> ManagePublish(long countryId);
        TResponseVM<CountryDistrictListDto> GetCountryDistrictsList(long countryId);
        TResponseVM<CountryDistrictDetailsDto> GetDistrict(long districtId);
        TResponseVM<AddDistrictStateDto> AddState(AddDistrictStateDto body);
        TResponseVM<UpdateDistrictStateDto> UpdateState(UpdateDistrictStateDto body);

        #endregion

        #region Public

        List<SelectDto> PublicDistrictsCategoryListSelect(long categoryId);

        #endregion

    }
}
