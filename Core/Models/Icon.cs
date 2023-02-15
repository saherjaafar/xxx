
using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Icon
    {
        public long Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; }
        [Required]
        public string Svg { get; set; }

    }
}
