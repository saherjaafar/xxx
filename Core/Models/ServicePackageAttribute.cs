
namespace Core.Models
{
    public class ServicePackageAttribute
    {
        public long Id { get; set; }
        public string Text { get; set; }

        public ServicePackage ServicePackage { get; set; }
        public long ServicePackageId { get; set; }
    }
}
