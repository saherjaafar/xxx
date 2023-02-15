
namespace Core.Models
{
    public class UserFavorite
    {
        public long Id { get; set; }

        public User User { get; set; }
        public long UserId { get; set; }

        public Service Service { get; set; }
        public long ServiceId { get; set; }

        public Category Category { get; set; }
        public long CategoryId { get; set; }
    }
}
