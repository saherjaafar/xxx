
using System.Collections.Generic;

namespace Core.Models
{
    public class ServicePackage
    {
        public long Id { get; set; }
        public string PackageName { get; set; }
        public bool HasPrice { get; set; }
        public decimal Price { get; set; }
        public bool IsPublished { get; set; }

        public Service Service { get; set; }
        public long ServiceId { get; set; }
        public ICollection<ServicePackageAttribute> PackageAttributes { get; set; }
    }
}
