using Core;
using Core.Dto;
using Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DealController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public DealController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("get/{dealId}")]
        public IActionResult GetDeal(long dealId)
        {
            var res = _unitOfWork.Deals.Get(dealId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.obj);
        }

        [Authorize]
        [HttpGet("list/{serviceId}")]
        public IActionResult GetDeals(long serviceId)
        {
            var res = _unitOfWork.Deals.List(serviceId);
            if (res.HasError)
                return StatusCode(res.StatusCode, res.Message);
            else
                return StatusCode(res.StatusCode, res.ListObj);
        }

        [Authorize]
        [HttpPost("add")]
        public IActionResult AddDeal([FromBody] DealDto body)
        {
            var res = _unitOfWork.Deals.Add(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [Authorize]
        [HttpPut("Update")]
        public IActionResult UpdateDeal([FromBody] DealDto body)
        {
            var res = _unitOfWork.Deals.UpdateDeal(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
