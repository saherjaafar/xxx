using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("List")]
        public IActionResult List()
        {
            var res = _unitOfWork.Categories.List();
            return Ok(res);
        }

        [HttpGet("List/Select")]
        public IActionResult ListSelect()
        {
            var res = _unitOfWork.Categories.ListSelect();
            return Ok(res);
        }

        [HttpGet("Get/{categoryId}")]
        public IActionResult GetServiceCategoryDetails(long categoryId)
        {
            var res = _unitOfWork.Categories.Details(categoryId);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("Add")]
        public IActionResult Add([FromForm] UploadServiceCategoryImageDTO ImageBody, [FromForm] string Details)
        {
            try
            {
                var details = JsonConvert.DeserializeObject<AddServiceCategoryDto>(Details);
                var res = _unitOfWork.Categories.Add(ImageBody, details);
                return StatusCode(res.StatusCode,res.Message);
            }
            catch
            {
                return BadRequest("Invalid form data");
            }
        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateServiceCategory([FromForm] UploadServiceCategoryImageDTO ImageBody, [FromForm] string Details)
        {
            try
            {
                var details = JsonConvert.DeserializeObject<UpdateServiceCategoryDto>(Details);
                var res = _unitOfWork.Categories.Update(ImageBody, details);
                return StatusCode(res.StatusCode, res.Message);
            }
            catch
            {
                return BadRequest("Invalid form data");
            }
        }

        [Authorize]
        [HttpPut("Publish/{categoryId}")]
        public IActionResult ManagePublish(long categoryId)
        {
            var res = _unitOfWork.Categories.ManagePublish(categoryId);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
