using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class DealVariation
    {
        public long Id { get; set; }
        public string Text { get; set; }

        public Deal Deal { get; set; }
        public long DealId { get; set; }
    }
}
