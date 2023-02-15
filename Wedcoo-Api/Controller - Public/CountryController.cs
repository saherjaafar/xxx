using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CountryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("list/districts/select")]
        public IActionResult PublicCategoryListSelect(long categoryId = 0)
        {
            var res = _unitOfWork.Countries.PublicDistrictsCategoryListSelect(categoryId);
            return Ok(res);
        }
    }
}
