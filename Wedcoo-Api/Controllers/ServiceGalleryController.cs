using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceGalleryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServiceGalleryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("Get/Gallery/{serviceId}")]
        public IActionResult GetGallery(long serviceId)
        {
            var res = _unitOfWork.ServiceGalleries.GetGallery(serviceId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.ListObj);
        }

        [Authorize]
        [HttpPost("Upload")]
        public IActionResult Upload([FromForm] GalleryImageDto image, [FromForm] string detailsStr)
        {
            try
            {
                var details = JsonConvert.DeserializeObject<UploadImageDetailsDto>(detailsStr);
                var res = _unitOfWork.ServiceGalleries.UploadGalleryImage(details, image);
                return StatusCode(res.StatusCode, res.Message);
            }
            catch
            {
                return BadRequest("Invalid Data");
            }
        }

        [Authorize]
        [HttpPut("Delete/{imageId}")]
        public IActionResult DeleteImage(long imageId)
        {
            var res = _unitOfWork.ServiceGalleries.Delete(imageId);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
