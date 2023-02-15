using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork= unitOfWork;
        }

        #region Country

        [Authorize]
        [HttpGet("List")]
        public IActionResult GetCountriesList()
        {
            var res = _unitOfWork.Countries.GetList();
            return Ok(res);
        }

        [Authorize]
        [HttpGet("List/Select")]
        public IActionResult GetCountriesSelectList()
        {
            var res = _unitOfWork.Countries.GetSelectList();
            return Ok(res);
        }

        [Authorize]
        [HttpGet("Details/{countryId}")]
        public IActionResult GetCountryDetails(long countryId)
        {
            var res = _unitOfWork.Countries.GetDetails(countryId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.obj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("Publish/{countryId}")]
        public IActionResult PublishUnpublishCountry(long countryId)
        {
            var res = _unitOfWork.Countries.ManagePublish(countryId);
            return StatusCode(res.StatusCode, res.Message);
        }

        #endregion

        #region Districts

        [Authorize]
        [HttpGet("{countryId}/Districts")]
        public IActionResult GetCountryDistricts(long countryId)
        {
            var res = _unitOfWork.Countries.GetDistrictsSelect(countryId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpGet("{countryId}/Districts/List")]
        public IActionResult GetCountryDistrictsList(long countryId)
        {
            var res = _unitOfWork.Countries.GetCountryDistrictsList(countryId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpGet("{countryId}/Districts/{districtId}")]
        public IActionResult GetDistrict(long districtId)
        {
            var res = _unitOfWork.Countries.GetDistrict(districtId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.obj);
        }

        #endregion

        #region States

        [Authorize]
        [HttpGet("GetDistrictStates/{districtId}")]
        public IActionResult GetDistrictStates(long districtId)
        {
            var res = _unitOfWork.Countries.GetDistrictStatesList(districtId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpGet("GetDistrictStates/{districtId}/select")]
        public IActionResult GetDistrictStatesSelect(long districtId)
        {
            var res = _unitOfWork.Countries.GetDistrictStatesSelect(districtId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPost("State/Add")]
        public IActionResult AddState([FromBody] AddDistrictStateDto body)
        {
            var res = _unitOfWork.Countries.AddState(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("State/Update")]
        public IActionResult UpdateState([FromBody] UpdateDistrictStateDto body)
        {
            var res = _unitOfWork.Countries.UpdateState(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        #endregion

    }
}
