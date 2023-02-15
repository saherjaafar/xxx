using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Models
{
    public class CategoryDistrict
    {
        public long Id { get; set; }
        public Category Category { get; set; }
        public long CategoryId { get; set; }
        [ForeignKey("CountryDistrictId")]
        public CountryDistrict CountryDistrict { get; set; }
        public long CountryDistrictId { get; set; }
    }
}
