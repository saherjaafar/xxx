using System;
using System.Collections.Generic;

namespace Core.Models
{
    public class Application
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Number { get; set; }
        public bool HasMeeting { get; set; }
        public DateTime MeetingDate { get; set; }
        public bool Intrested { get; set; }
        public string Note { get; set; }
        public bool IsCalled { get; set; }
        public bool IsApprovedForPublish { get; set; }
        public bool IsNeedFollowUp { get; set; }
        public DateTime? FollowUpDate { get; set; }

        public Country Country { get; set; }
        public long CountryId { get; set; }

        public CountryDistrict CountryDistrict { get; set; }
        public long CountryDistrictId { get; set; }

        public DistrictState DistrictState { get; set; }
        public long DistrictStateId { get; set; }

        public Category Category { get; set; }
        public long CategoryId { get; set; }

        public ICollection<ApplicationFollowUp> ApplicationFollowUps { get; set; }
        public virtual Service Service { get; set; }
    }
}
