using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto.Admin
{

    public class GetXDto
    {
        public XApplication XApplicationDto { get; set; }
        public XService XServiceDto { get; set; }
        public List<XServiceSocialMedia> XSocialMediaLinkDto { get; set; }
        public List<XServiceGallery> XGalleryDto { get; set; }
    }

    public class XApplication
    {
        public long ApplicationId { get; set; }
        public string Name { get; set; }
        public string CountryName { get; set; }
        public string DistrictName { get; set; }
        public string StateName { get; set; }
        public string Number { get; set; }
        public bool HasMeeting { get; set; }
        public DateTime MeetingDate { get; set; }
        public bool Intrested { get; set; }
        public string Note { get; set; }
        public XCategory Category { get; set; }
        public bool IsCalled { get; set; }
        public bool IsApprovedForPublish { get; set; }
    }

    public class XService
    {
        public long ServiceId { get; set; }
        public bool IsPublished { get; set; }
        public string MainImage { get; set; }
        public string SearchImage { get; set; }
        public string Logo { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public bool HasPackage { get; set; }
        public long ApplicationId { get; set; }
        public string Quote { get; set; }
        public bool IsSubscribed { get; set; }
        public string YoutubeVideoId { get; set; }
    }

    public class XServiceSocialMedia
    {
        public long ServiceSocialMediaId { get; set; }
        public long ServiceId { get; set; }
        public long IconId { get; set; }
        public string IconName { get; set; }
        public string IconSvg { get; set; }
        public string SocialMediaLink { get; set; }
    }

    public class XServiceGallery
    {
        public long ServiceGalleryId { get; set; }
        public long ServiceId { get; set; }
        public string Image { get; set; }
        public bool IsPublished { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class XCategory
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string IconName { get; set; }
        public string IconSvg { get; set; }
        public bool IsPublished { get; set; }
        public string HomeImagePath { get; set; }
    }

    #region Country

    public class XCountry
    {
        public long CountryId { get; set; }
        public string CountryName { get; set; }
        public string Suffix { get; set; }
        public string IconName { get; set; }
        public string IconSvg { get; set; }
        public string Currency { get; set; }
        public bool IsPublished { get; set; }
        public List<XDistrict> Districts { get; set; }

    }

    public class XDistrict
    {
        public string DistrictName { get; set; }
        public List<XState> States { get; set; }
    }

    public class XState
    {
        public string Name { get; set; }
    }

    #endregion
}
