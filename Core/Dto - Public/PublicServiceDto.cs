using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto___Public
{
    public class PublicServiceDto
    {
    }

    #region Search
    public class SearchServiceDto
    {
        public int Pages { get; set; }
        public int Count { get; set; }
        public List<PublicSearchServiceListDto> SearchResult { get; set; }

        public List<PublicSearchServiceListDto> Services { get; set; }
    }

    public class PublicSearchServiceListDto
    {
        public long ServiceId { get; set; }
        public string Name { get; set; }
        public string Location { get; set; }
        public string SearchImage { get; set; }
    }

    public class SearchServiceResultDto
    {
        public int Count { get; set; }
        public List<PublicSearchServiceListDto> Services { get; set; }
        public int Pages { get; set; }
    }

    #endregion

    #region Details

    public class PublicServiceDetailsDto
    {
        public PublicMainServiceDetailsDto Details { get; set; }
        public List<PublicServiceSocialMediaDto> SocialMedia { get; set; }
        public List<PublicServiceGalleryDto> Gallery { get; set; }
        public List<PublicServicePackageDto> Packages { get; set; }
    }

    public class PublicMainServiceDetailsDto
    {
        public long ServiceId { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public string Description { get; set; }
        public long CategoryId { get; set; }
        public string Quote { get; set; }
        public string SearchImage { get; set; }
        public string YoutubeVideoId { get; set; }
    }

    public class PublicServiceSocialMediaDto
    {
        public long Id { get; set; }
        public string Link { get; set; }
        public string Icon { get; set; }
        public string Svg { get; set; }
    }

    public class PublicServiceGalleryDto
    {
        public string src { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class PublicServicePackageDto
    {
        public string Name { get; set; }
        public bool HasPrice { get; set; }
        public decimal Price { get; set; }
        public List<PublicServicePackageVariationDto> Variations { get; set; }
    }

    public class PublicServicePackageVariationDto
    {
        public long Id { get; set; }
        public string Variation { get; set; }
    }

    #endregion
}
