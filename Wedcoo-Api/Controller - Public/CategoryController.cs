using Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("UpdateCache")]
        public IActionResult UpdateCache()
        {
            _unitOfWork.Categories.UpdateCache();
            return Ok();
        }

        [HttpGet("Public/List")]
        public IActionResult PublicCategoriesList()
        {
            var res = _unitOfWork.Categories.PublicCategoriesList();
            return Ok(res);
        }
    }
}
