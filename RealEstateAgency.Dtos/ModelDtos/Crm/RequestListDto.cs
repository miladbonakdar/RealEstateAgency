﻿using System;
using System.Collections.Generic;
using RealEstateAgency.DAL.DtoContracts;
using RealEstateAgency.DAL.Models;

namespace RealEstateAgency.Dtos.ModelDtos.Crm
{
    public class RequestListDto : IDto
    {
        public int Id { get; set; }
        public int RequestTypeId { get; set; }
        public bool CanAddProperty { get; set; }
        public int UserAccountIdRequester { get; set; }
        public string TrackingNumber { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string MarketingAssistantTrackingCode { get; set; }
        public UserAccount User { get; set; }
        public int AgentId { get; set; }
        public Property Property { get; set; }
        public IEnumerable<RequestAction> Actions { get; set; }
        public IEnumerable<RequestState> States { get; set; }
        public bool IsAssigned { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
