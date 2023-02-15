using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Sponsor
    {
        public long Id { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedDate { get; set; }
        public long Views { get; set; }

        public virtual Service Service { get; set; }
        public long ServiceId { get; set; }
        public ICollection<SponsorType> SponsorTypes { get; set; }
    }
}
