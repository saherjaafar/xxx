using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class SponsorType
    {
        public long Id { get; set; }
        public string Type { get; set; }

        public virtual Sponsor Sponsor { get; set; }
        public long SponsorId { get; set; }
    }
}
