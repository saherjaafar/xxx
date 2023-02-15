using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ServiceGalleryDto
    {
    }

    public class UploadImageDetailsDto
    {
        public long ServiceId { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
    }

    public class GalleryImagesListDto
    {
        public long ImageId { get; set; }
        public string src { get; set; }
        public int width { get; set; }
        public int height { get; set; }
    }

    public class GalleryImageDto
    {
        public IFormFile Image { get; set; }
    }
}
