using Core;
using Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SponsorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("Type/List/Select")]
        public IActionResult GetSponsorTypesSelect()
        {
            var res = _unitOfWork.Sponsors.GetSponsorTypesSelect();
            return StatusCode(200,res);
        }

        [Authorize]
        [HttpGet("List")]
        public IActionResult GetSponsors()
        {
            var res = _unitOfWork.Sponsors.List();
            return StatusCode(200,res);
        }

        [Authorize]
        [HttpGet("Get/{sponsorId}")]
        public IActionResult GetSponsorToUpdate(long sponsorId)
        {
            var res = _unitOfWork.Sponsors.GetSponsorToUpdate(sponsorId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.obj);
        }

        [Authorize]
        [HttpPost("Add")]
        public IActionResult AddSponsor([FromBody] ManageSponsorDto body)
        {
            var res = _unitOfWork.Sponsors.Add(body);
            return StatusCode(res.StatusCode,res.Message);
        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateSponsor([FromBody] ManageSponsorDto body)
        {
            var res = _unitOfWork.Sponsors.Update(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
