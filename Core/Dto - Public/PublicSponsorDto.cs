using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto___Public
{
    public class PublicSponsorDto
    {
    }

    public class PublicSponsorListDto
    {
        public long SponsorId { get; set; }
        public long ServiceId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public string Location { get; set; }
    }

    public class ListPublicSponsorListDto
    {
        public List<PublicSponsorListDto> Sponsors { get; set; }
    }
}
