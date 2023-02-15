using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{

    public class DealListDto
    {
        public long DealId { get; set; }
        public string Title { get; set; }
        public string CreationDate { get; set; }
        public bool ShowPrice { get; set; }
        public bool IsPublished { get; set; }
    }
    public class DealDto
    {
        public long DealId { get; set; }
        public long ServiceId { get; set; }
        public string Title { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public bool Publish { get; set; }
        public string Description { get; set; }
        public List<DealVariationDto> Variations { get; set; }
        public List<SelectDto> Districts { get; set; }
    }

    public class DealVariationDto
    {
        public long VariationId { get; set; }
        public string Text { get; set; }
    }
}
