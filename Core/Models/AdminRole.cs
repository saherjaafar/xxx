
namespace Core.Models
{
    public class AdminRole
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool ReadIcon { get; set; }
        public bool WriteIcon { get; set; }
        public bool ReadUsers { get; set; }
        public bool ManageUsers { get; set; }
        public bool ReadServiceCategory { get; set; }
        public bool ManageServiceCategory { get; set; }
        public bool ReadCountry { get; set; }
        public bool ManageCountry { get; set; }
        public bool ReadIcons { get; set; }
        public bool ManageIcons { get; set; }
        public bool ReadApplication { get; set; }
        public bool ManageApplication { get; set; }
        public bool ReadService { get; set; }
        public bool ManageService { get; set; }
        public bool ReadSponsor { get; set; }
        public bool ManageSponsor { get; set; }
    }
}
