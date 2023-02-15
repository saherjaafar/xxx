using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicePackageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePackageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("List/{serviceId}")]
        public IActionResult GetPackages(long serviceId)
        {
            var res = _unitOfWork.ServicePackages.Get(serviceId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.ListObj);
        }

        [Authorize]
        [HttpPost("Add/{serviceId}")]
        public IActionResult AddPackage([FromBody] ServicePackageDto body, long serviceId)
        {
            var res = _unitOfWork.ServicePackages.Add(body, serviceId);
            return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdatePackage([FromBody] ServicePackageDto body)
        {
            var res = _unitOfWork.ServicePackages.Update(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
