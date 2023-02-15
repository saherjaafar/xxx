using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class Deal
    {
        public long Id { get; set; }
        public decimal Price { get; set; }
        public bool ShowPrice { get; set; }
        public bool IsPublished { get; set; }
        public string Description { get; set; }
        public DateTime CreationDate { get; set; }
        public string Title { get; set; }
        public string ImageUrl { get; set; }

        public virtual Service Service { get; set; }
        public long ServiceId { get; set; }
        public virtual ICollection<DealDistrict> DealDistricts { get; set; }
        public virtual ICollection<DealVariation> DealVariations { get; set; }
    }
}
