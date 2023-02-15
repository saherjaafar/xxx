using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Core.Dto
{
    public class CategoryDto
    {
    }

    public class ServiceCategoryListDTO
    {
        public long ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public string CreatedAt { get; set; }
        public bool IsPublished { get; set; }
    }

    public class ServiceCategoryDetailsDto
    {
        public long ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public SelectDto Icon { get; set; }
        public string Image { get; set; }
        public string HomeImage { get; set; }
        public List<SelectDto> Filters { get; set; }
        public List<SelectDto> Districts { get; set; }
    }

    public class UploadServiceCategoryImageDTO
    {
        public IFormFile Image { get; set; }
        public IFormFile HomeImage { get; set; }
        public string Path { get; set; }
    }

    public class AddServiceCategoryDto
    {
        public long ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public long IconId { get; set; }
        public string Image { get; set; }
        public List<SelectDto> Filters { get; set; }
        public List<SelectDto> Districts { get; set; }
    }

    public class UpdateServiceCategoryDto
    {
        public long ServiceCategoryId { get; set; }
        public string ServiceCategoryName { get; set; }
        public long IconId { get; set; }
        public string Image { get; set; }
        public List<SelectDto> Filters { get; set; }
        public List<SelectDto> Districts { get; set; }

    }
}
