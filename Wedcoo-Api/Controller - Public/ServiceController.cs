using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("UpdateServiceCategorySelectListCache/{categoryId}")]
        public IActionResult UpdateServiceCategorySelectListCache(long categoryId)
        {
            _unitOfWork.Services.UpdateServiceCategorySelectListCache(categoryId);
            return Ok();
        }

        [HttpGet("category/services/{categoryId}")]
        public IActionResult ServiceCategorySelectList(long categoryId)
        {
            var res = _unitOfWork.Services.ServiceCategorySelectList(categoryId);
            return Ok(res);
        }

        [HttpGet("List/Search/{categoryId}")]
        public IActionResult PublicServicesList(long categoryId, int page = 1, string searchKey = "")
        {
            var res = _unitOfWork.Services.PublicServicesList(page,categoryId,searchKey);
            return Ok(res);
        }

        [HttpGet("category/{categoryId}/filter")]
        public IActionResult PublicServicesListFilter(long categoryId, long district = 0, long rating = 0, int page = 1)
        {
            var res = _unitOfWork.Services.PublicServicesListFilter(categoryId, district,rating,page);
            return Ok(res);
        }

        [HttpGet("Get/{serviceId}")]
        public IActionResult GetPublicServiceDetails(long serviceId)
        {
            var res = _unitOfWork.Services.PublicServiceDetails(serviceId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.obj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }
    }
}
