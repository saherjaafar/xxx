using System;

namespace Core.Models
{
    public class ServiceGallery
    {
        public long Id { get; set; }
        public string Image { get; set; }
        public bool IsPublished { get; set; }
        public DateTime UploadedAt { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public bool IsDeleted { get; set; }

        public Service Service { get; set; }
        public long ServiceId { get; set; }
    }
}
