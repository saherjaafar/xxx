using System.ComponentModel.DataAnnotations;

namespace Core.Dto
{
    public class CountryDto
    {
        public long Id { get; set; }

        [Required, MaxLength(20)]
        public string Name { get; set; }

        [Required, MaxLength(5)]
        public string Suffix { get; set; }
        public long IconId { get; set; }
        public bool IsPublished { get; set; }
    }

    public class CountriesListDto
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string Suffix { get; set; }
        public bool IsPublished { get; set; }
    }

    public class CountryDetailsDto
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string Suffix { get; set; }
        public SelectDto Icon { get; set; }
        public string Currency { get; set; }
    }

    public class AddCountryDto
    {
        public string Name { get; set; }
        public string Suffix { get; set; }
        public long IconId { get; set; }
        public string Currency { get; set; }
    }

    public class UpdateCountryDto
    {
        public long CountryId { get; set; }
        public string Name { get; set; }
        public string Suffix { get; set; }
        public long IconId { get; set; }
        public string Currency { get; set; }
    }
}
