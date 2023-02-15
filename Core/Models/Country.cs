using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Country
    {
        public long Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }

        [Required, MaxLength(5)]
        public string Suffix { get; set; }
        public Icon Icon { get; set; }
        public long IconId { get; set; }
        public string Currency { get; set; }
        public bool IsPublished { get; set; }
        public virtual ICollection<CountryDistrict> CountryDistricts{ get; set; }
    }
}
