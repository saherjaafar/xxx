using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class DistrictStateDto
    {
    }

    public class DistrictStatesListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }

    public class AddDistrictStateDto
    {
        public long DistrictId { get; set; }
        public string Name { get; set; }
    }

    public class UpdateDistrictStateDto
    {
        public long StateId { get; set; }
        public string Name { get; set; }
    }
}
