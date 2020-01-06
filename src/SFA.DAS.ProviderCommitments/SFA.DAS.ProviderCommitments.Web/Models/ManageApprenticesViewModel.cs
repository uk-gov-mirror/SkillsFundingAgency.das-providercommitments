﻿using System.Collections.Generic;
using System.Linq;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using ApprenticeshipDetails = SFA.DAS.CommitmentsV2.Api.Types.Responses.ApprenticeshipDetails;

namespace SFA.DAS.ProviderCommitments.Web.Models
{
    public class ManageApprenticesViewModel
    {
        public long ProviderId { get; set; }
        public bool AnyApprenticeships => Apprenticeships != null && Apprenticeships.Any();
        public IEnumerable<ApprenticeshipDetails> Apprenticeships { get; set; }
        public string SortField { get; set; }
        public bool IsReversed { get; set; }
    }
}