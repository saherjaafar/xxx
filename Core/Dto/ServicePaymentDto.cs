using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Dto
{
    public class ServicePaymentDto
    {

    }

    public class ServicePaymentListDto
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public SelectDto StatusSelect { get; set; }
        public string StrDate { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }

    public class AddServicePaymentDto
    {
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
        public long ServiceId { get; set; }
    }

    public class UpdateServicePaymentDto
    {
        public long Id { get; set; }
        public string Status { get; set; }
        public DateTime Date { get; set; }
        public decimal Amount { get; set; }
    }
}
