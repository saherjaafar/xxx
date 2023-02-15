using System.Collections.Generic;

namespace Core.Dto
{
    public class ServiceSocialMediaDto
    {
    }

    public class ServiceSocialMediaListDto
    {
        public long ServiceSocialMediaId { get; set; }
        public SelectDto Icon { get; set; }
        public string Link { get; set; }
    }

    public class ManageServiceSocialMediaDto
    {
        public long ServiceSocialMediaId { get; set; }
        public SelectDto Icon { get; set; }
        public string Link { get; set; }
    }

    public class ManageServiceSocialMediaBodyDto
    {
        public List<ManageServiceSocialMediaDto> Links { get; set; }
    }
}
