using Core.Models;
using System;
using System.Collections.Generic;

namespace Core.Dto
{
    public class ApplicationDto
    {
    }

    public class ApplicationDetailsDto
    {
        public string SupplierName { get; set; }
        public SelectDto Category { get; set; }
        public SelectDto Country { get; set; }
        public SelectDto CountryDistrict { get; set; }
        public SelectDto DistrictState { get; set; }
        public string PhoneNumber { get; set; }
        public SelectDto HasMeeting { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Note { get; set; }
        public long ServiceId { get; set; }
        public bool IsCalled { get; set; }
        public bool IsApprovedForPublish { get; set; }
        public bool IsNeedFollowUp { get; set; }
        public DateTime FollowUpDate { get; set; }
        public ICollection<ApplicationFollowUp> FollowUps{ get; set; }
    }

    public class ApplicationsListDto
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string District { get; set; }
        public string State { get; set; }
        public bool HasMeeting { get; set; }
        public string Date { get; set; }
        public string Category { get; set; }
        public bool IsCalled { get; set; }
        public bool IsApprovedForPublish { get; set; }
    }

    public class AddNewApplicationDto
    {
        public string SupplierName { get; set; }
        public long CategoryId { get; set; }
        public long CountryId { get; set; }
        public long CountryDistrictId { get; set; }
        public long DistrictStateId { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasMeeting { get; set; }
        public DateTime MeetingDate { get; set; }
        public long AminId { get; set; }
        public string Note { get; set; }
        public bool IsCalled { get; set; }
        public bool IsApprovedForPublish { get; set; }
        public bool IsNeedFollowUp { get; set; }
        public DateTime FollowUpDate { get; set; }
    }

    public class UpdateApplicationDto
    {
        public long ApplicationId { get; set; }
        public string SupplierName { get; set; }
        public long CategoryId { get; set; }
        public long CountryId { get; set; }
        public long CountryDistrictId { get; set; }
        public long DistrictStateId { get; set; }
        public string PhoneNumber { get; set; }
        public bool HasMeeting { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Note { get; set; }
        public bool IsCalled { get; set; }
        public bool IsApprovedForPublish { get; set; }
        public bool IsNeedFollowUp { get; set; }
        public DateTime FollowUpDate { get; set; }
    }
}
