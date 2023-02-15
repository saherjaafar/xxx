using Core;
using Core.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicePaymentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ServicePaymentController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("status")]
        public IActionResult StatusesList()
        {
            var res = _unitOfWork.ServicePayments.Statuses();
            return Ok(res);
        }

        [HttpGet("list/{serviceId}")]
        public IActionResult List(long serviceId)
        {
            var res = _unitOfWork.ServicePayments.List(serviceId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPost("Add")]
        public IActionResult Add([FromBody] AddServicePaymentDto body)
        {
            var res = _unitOfWork.ServicePayments.Add(body);
            return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("Update")]
        public IActionResult Update([FromBody] UpdateServicePaymentDto body)
        {
            var res = _unitOfWork.ServicePayments.Update(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
