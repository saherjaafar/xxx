using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Models
{
    public class ServicePayment
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount{ get; set; }
        public string Status { get; set; }
        public virtual Service Service { get; set; }
        public long ServiceId { get; set; }
    }
}
