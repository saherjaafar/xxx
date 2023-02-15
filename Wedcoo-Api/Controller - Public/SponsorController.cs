using Core;
using Core.Dto;
using Core.Dto___Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class SponsorController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public SponsorController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpPost("UpdateSponsorView")]
        public IActionResult UpdateSponsorView([FromBody] string body)
        {
            var details = JsonConvert.DeserializeObject<ListPublicSponsorListDto>(body);
            _unitOfWork.Sponsors.UpdateSponsorView(details);
            return Ok();
        }

        [HttpGet("list/category/{categoryId}/type/{type}")]
        public IActionResult PublicList(long categoryId, string type, long serviceId = 0)
        {
            return Ok(_unitOfWork.Sponsors.PublicList(categoryId, type, serviceId));
        }
    }
}
