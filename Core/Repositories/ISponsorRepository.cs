using Core.Dto;
using Core.Dto.TResponse;
using Core.Dto___Public;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Core.Repositories
{
    public interface ISponsorRepository : IBaseRepository<Sponsor>
    {
        #region Admin

        Sponsor GetEntity(long sponsorId, params Expression<Func<Sponsor, object>>[] includes);
        List<SelectDto> GetSponsorTypesSelect();
        List<SponsorsListDto> List();
        TResponseVM<SponsorToUpdateDto> GetSponsorToUpdate(long sponsorId);
        TResponseVM<ManageSponsorDto> Add(ManageSponsorDto body);
        TResponseVM<ManageSponsorDto> Update(ManageSponsorDto body);

        #endregion

        #region Public

        void UpdateSponsorView(ListPublicSponsorListDto sponsors);
        List<PublicSponsorListDto> PublicList(long categoryId,string type,long? serviceId);

        #endregion

    }
}
