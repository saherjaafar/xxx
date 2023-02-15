using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("Details/{serviceId}")]
        public IActionResult GetServiceDetails(long serviceId)
        {
            var res = _unitOfWork.Services.GetServiceDetails(serviceId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.obj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpGet("List")]
        public IActionResult GetServicesList(long categoryId = 0)
        {
            var res = _unitOfWork.Services.List();
            return Ok(res);
        }

        [Authorize]
        [HttpGet("List/Select")]
        public IActionResult GetServicesSelect()
        {
            var res = _unitOfWork.Services.ListSelect();
            return Ok(res);
        }

        [Authorize]
        [HttpGet("GetServicesByCategoryForSelect/{categoryId}")]
        public IActionResult GetServicesByCategoryForSelect(long categoryId)
        {
            var res = _unitOfWork.Services.ListSelectByCategory(categoryId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.obj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPost("Details/Add")]
        public IActionResult AddService([FromForm] AddServiceImagesDto Images, [FromForm] string body)
        {
            try
            {
                var details = JsonConvert.DeserializeObject<AddServiceDto>(body);
                var res = _unitOfWork.Services.Add(Images, details);
                return StatusCode(res.StatusCode, res.Message);
            }
            catch
            {
                return BadRequest("Invalid Data Obj");
            }
        }

        [Authorize]
        [HttpPut("Details/Update")]
        public IActionResult UpdateDetails([FromForm] UpdateServiceImagesDto images, [FromForm] string Body)
        {
            try
            {
                var details = JsonConvert.DeserializeObject<UpdateServiceDetailsDto>(Body);
                var res = _unitOfWork.Services.Update(images, details);
                return StatusCode(res.StatusCode, res.Message);
            }
            catch
            {
                return BadRequest("Invalid Data Obj");
            }
        }

        [Authorize]
        [HttpPut("Publish/{serviceId}")]
        public IActionResult ManagePublish(long serviceId)
        {
            var res = _unitOfWork.Services.ManagePublish(serviceId);
            return StatusCode(res.StatusCode,res.Message);
        }
    }
}
