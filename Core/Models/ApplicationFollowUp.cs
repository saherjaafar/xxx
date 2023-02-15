using System;

namespace Core.Models
{
    public class ApplicationFollowUp
    {
        public long Id { get; set; }
        public DateTime Date { get; set; }
        public string Note { get; set; }
        public Application Application { get; set; }
        public long ApplicationId { get; set; }
    }
}
