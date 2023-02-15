
namespace Core.Models
{
    public class DistrictState
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public CountryDistrict CountryDistrict { get; set; }
        public long CountryDistrictId { get; set; }
    }
}
