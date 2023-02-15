using Core.Dto;
using Core.Models;
using System.Collections.Generic;

namespace Core.Repositories
{
    public interface IAdminRoleRepository : IBaseRepository<AdminRole>
    {
        public IEnumerable<SelectDto> ListSelect();
    }
}
