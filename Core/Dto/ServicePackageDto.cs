using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ServicePackageDto
    {
        public long PackageId { get; set; }
        public string PackageName { get; set; }
        public bool HasPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }
        public List<PackageVariationDto> Items { get; set; }
    }

    public class PackageVariationDto
    {
        public long ItemId { get; set; }
        public string Name { get; set; }
    }
}
