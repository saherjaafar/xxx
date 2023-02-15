using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class SponsorDto
    {
    }

    public class SponsorsListDto
    {
        public long id { get; set; }
        public string title { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public long serviceId { get; set; }
    }

    public class SponsorToUpdateDto
    {
        public long id { get; set; }
        public string start { get; set; }
        public string end { get; set; }
        public SelectDto service { get; set; }
        public List<SelectDto> types { get; set; }
    }

    public class ManageSponsorDto
    {
        public long SponsorId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public long ServiceId { get; set; }
        public List<SelectDto> Types { get; set; }
    }
}
