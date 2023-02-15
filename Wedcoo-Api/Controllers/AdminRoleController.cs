using Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminRoleController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AdminRoleController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [Authorize]
        [HttpGet("ListSelect")]
        public IActionResult GetListSelect()
        {
            var res = _unitOfWork.AdminRoles.ListSelect();
            return Ok(res);
        }
    }
}
