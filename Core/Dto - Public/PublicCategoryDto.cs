using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto___Public
{
    public class PublicCategoryDto
    {
    }

    public class PublicCategoryListDto
    {
        public long ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public string Image360x360 { get; set; }
        public string Image620x350 { get; set; }
        public string SvgIcon { get; set; }
        public int SuppliersCount { get; set; }
    }
}
