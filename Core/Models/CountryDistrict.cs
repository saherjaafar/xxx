using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CountryDistrict
    {
        public long Id { get; set; }
        public string Name { get; set; }

        [ForeignKey("CountryId")]
        public Country Country { get; set; }
        public long CountryId { get; set; }
        public ICollection<CategoryDistrict> CategoryDistricts { get; set; }
    }
}
