using Core.Dto;
using Core.Dto.TResponse;
using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IIconRepository : IBaseRepository<Icon>
    {
        public IEnumerable<IconsListDto> List();
        public IEnumerable<SelectDto> ListSelect();
        public TResponseVM<ResponseVM> AddIcon(AddIconDto body);
        public TResponseVM<ResponseVM> UpdateIcon(UpdateIconDto body);
        public void UpdateListCache();
    }
}
