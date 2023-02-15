using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface IUserFavoriteRepository : IBaseRepository<UserFavorite>
    {
        TResponseVM<FavoriteListDto> List(long userId);
        TResponseVM<ResponseVM> ManageFavorites(ManageFavoritesDto body);
    }
}
