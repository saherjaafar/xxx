namespace Core.Models
{
    public class CategoryFilter
    {
        public long Id { get; set; }
        public Category Category { get; set; }
        public long CategoryId { get; set; }
        public string EnumKey { get; set; }
    }
}
