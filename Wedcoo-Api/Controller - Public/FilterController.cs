using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("updateCache")]
        public IActionResult UpdateCache(long? categoryId)
        {
            _unitOfWork.Filters.UpdateCache(categoryId);
            return Ok();
        }

        [HttpGet("allowedFilters/{categoryId}")]
        public IActionResult GetCategoryFiltersListSelect(long categoryId)
        {
            var res = _unitOfWork.Filters.GetCategoryFiltersListSelect(categoryId);
            return Ok(res);
        }

    }
}
