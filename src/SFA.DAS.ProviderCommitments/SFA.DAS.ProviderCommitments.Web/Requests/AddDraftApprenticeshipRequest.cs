﻿using System;
using SFA.DAS.Authorization;

namespace SFA.DAS.ProviderCommitments.Web.Requests
{
    public class AddDraftApprenticeshipRequest : IAuthorizationContextModel
    {
        public Guid ReservationId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public long AccountLegalEntityId { get; set; }
        public string StartMonthYear { get; set; }
        public string CourseCode { get; set; }
    }
}