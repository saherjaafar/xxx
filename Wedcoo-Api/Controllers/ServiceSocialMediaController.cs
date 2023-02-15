using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceSocialMediaController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceSocialMediaController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("List/{serviceId}")]
        public IActionResult GetList(long serviceId)
        {
            var res = _unitOfWork.ServiceSocialMedias.List(serviceId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("Manage/{serviceId}")]
        public IActionResult ManageSocialMediaLinks([FromBody] ManageServiceSocialMediaBodyDto body, long serviceId)
        {
            var res = _unitOfWork.ServiceSocialMedias.Manage(serviceId,body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
