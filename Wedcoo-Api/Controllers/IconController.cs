using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IconController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public IconController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("UpdateListCache")]
        public IActionResult UpdateListCache()
        {
            _unitOfWork.Icons.UpdateListCache();
            return Ok();
        }

        [Authorize]
        [HttpGet("List")]
        public IActionResult GetIconsList()
        {
            var res = _unitOfWork.Icons.List();
            return Ok(res);
        }

        [HttpGet("All/Select")]
        public IActionResult GetListSelect()
        {
            var res = _unitOfWork.Icons.ListSelect();
            return Ok(res);
        }

        [Authorize]
        [HttpPost("AddIcon")]
        public IActionResult AddIcon([FromBody] AddIconDto body) 
        {
            var res = _unitOfWork.Icons.AddIcon(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("UpdateIcon")]
        public IActionResult UpdateIcon([FromBody] UpdateIconDto body)
        {
            var res = _unitOfWork.Icons.UpdateIcon(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
