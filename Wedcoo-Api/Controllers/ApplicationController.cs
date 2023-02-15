using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Core.Dto;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //[Authorize]
        [HttpGet("Get/{appId}")]
        public IActionResult GetApplication(long appId)
        {
            var res = _unitOfWork.Applications.Get(appId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.obj);
        }

        [HttpGet("list")]
        public IActionResult List(long? categoryId, long? districtId, long? stateId, bool? isCalled, bool? NeedFollowUp)
        {
            var criteria = _unitOfWork.Applications.GenerateCriterias(categoryId, districtId, stateId, isCalled, NeedFollowUp);
            var res = _unitOfWork.Applications.List(criteria);
            return Ok(res);
        }

        [Authorize]
        [HttpPost("Add")]
        public IActionResult Add([FromBody] AddNewApplicationDto body)
        {
            var res = _unitOfWork.Applications.Add(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateApplication([FromBody] UpdateApplicationDto body)
        {
            var res = _unitOfWork.Applications.Update(body);
            return StatusCode(res.StatusCode,res.Message);  
        }
    }
}
