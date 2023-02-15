using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Service
    {
        public long Id { get; set; }
        public bool IsPublished { get; set; }
        public string MainImage { get; set; }
        public string SearchImage { get; set; }
        public string Logo { get; set; }
        public DateTime DueDate { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneNumber2 { get; set; }
        public string Email { get; set; }
        public bool HasPackage { get; set; }
        public string Quote { get; set; }
        public bool IsSubscribed { get; set; }
        public string YoutubeVideoId { get; set; }

        public Application Application { get; set; }
        public long ApplicationId { get; set; }
        public ICollection<ServiceSocialMedia> ServiceSocialMedia { get; set; }
        public ICollection<ServicePackage> ServicePackage { get; set; }
        public ICollection<ServiceGallery> ServiceGallery { get; set; }
        public ICollection<Deal> Deals { get; set; }
        public ICollection<ServicePayment> Payments { get; set; }
    }
}
