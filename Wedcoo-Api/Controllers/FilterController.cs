using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilterController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public FilterController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("list")]
        public IActionResult GetFilters()
        {
            var filters = _unitOfWork.Filters.GetFiltersSelect();
            return Ok(filters);
        }
    }
}
