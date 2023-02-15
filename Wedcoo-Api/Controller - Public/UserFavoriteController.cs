using Core;
using Core.Dto___Public;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Wedcoo_Api.Controller___Public
{
    [Route("api/public/[controller]")]
    [ApiController]
    public class UserFavoriteController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UserFavoriteController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet("list/{userId}")]
        public IActionResult FavoritesList(long userId)
        {
            var res = _unitOfWork.UserFavorites.List(userId);
            if (!res.HasError)
                return StatusCode(res.StatusCode, res.ListObj);
            else
                return StatusCode(res.StatusCode, res.Message);
        }

        [HttpPut("manage")]
        public IActionResult ManageFavorites([FromBody] ManageFavoritesDto body)
        {
            var res = _unitOfWork.UserFavorites.ManageFavorites(body);
            return StatusCode(res.StatusCode, res.Message);
        }
    }
}
