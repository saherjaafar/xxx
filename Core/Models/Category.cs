using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Category
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public bool IsPublished { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string HomeImagePath { get; set; }
        public Icon Icon { get; set; }
        public long IconId { get; set; }
        public ICollection<CategoryDistrict> CategoryDistricts{ get; set;}
    }
}
