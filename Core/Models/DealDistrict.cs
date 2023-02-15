using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DealDistrict
    {
        public long Id { get; set; }

        public Deal Deal { get; set; }
        public long DealId { get; set; }

        public CountryDistrict CountryDistrict { get; set; }
        public long CountryDistrictId { get; set; }

    }
}
