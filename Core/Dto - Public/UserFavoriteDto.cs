using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto___Public
{
    public class UserFavoriteDto
    {
    }

    public class ManageFavoritesDto
    {
        public long UserId { get; set; }
        public long ServiceId { get; set; }
    }

    public class FavoriteListDto
    {
        public long FavoriteId { get; set; }
        public long ServiceId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }

    }
}
