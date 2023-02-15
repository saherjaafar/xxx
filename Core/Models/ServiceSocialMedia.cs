
namespace Core.Models
{
    public class ServiceSocialMedia
    {
        public long Id { get; set; }
        public string SocialMediaLink { get; set; }

        public Service Service { get; set; }
        public long ServiceId { get; set; }

        public Icon Icon { get; set; }
        public long IconId { get; set; }
    }
}
