using Microsoft.AspNetCore.Http;
using System;

namespace Core.Dto
{
    public class ServiceDto
    {
    }

    public class ServicesListDto
    {
        public long ServiceId { get; set; }
        public string ServiceName { get; set; }
        public string Country { get; set; }
        public string DueDate { get; set; }
        public string Category { get; set; }
        public bool IsPublished { get; set; }
        public long ApplicationId { get; set; }
    }

    public class ServiceDetailsDto
    {
        public string ServiceName { get; set; }
        public SelectDto Category { get; set; }
        public SelectDto Country { get; set; }
        public SelectDto CountryDistrict { get; set; }
        public string MainImage { get; set; }
        public string SearchImage { get; set; }
        public DateTime DueDate { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public string Description { get; set; }
        public string Quote { get; set; }
        public bool IsSubscribed { get; set; }
        public string YoutubeVideoId { get; set; }
    }

    public class AddServiceDto
    {
        public long ApplicationId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }
        public string Quote { get; set; }
        public bool IsSubscribed { get; set; }
        public string YoutubeVideoId { get; set; }
    }

    public class AddServiceImagesDto
    {
        public IFormFile MainImage { get; set; }
        public IFormFile SearchImage { get; set; }
    }

    public class UpdateServiceDetailsDto
    {
        public long ApplicationId { get; set; }
        public long ServiceId { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public DateTime DueDate { get; set; }
        public string Description { get; set; }
        public string Quote { get; set; }
        public bool IsSubscribed { get; set; }
        public string YoutubeVideoId { get; set; }
    }

    public class UpdateServiceImagesDto
    {
        public IFormFile MainImage { get; set; }
        public IFormFile SearchImage { get; set; }
    }
}
