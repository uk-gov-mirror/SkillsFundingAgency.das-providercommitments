﻿using System.Collections.Generic;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse;

namespace SFA.DAS.ProviderCommitments.Interfaces
{
    public interface IApprenticeshipInfoServiceMapper
    {
        FrameworksView MapFrom(List<FrameworkSummary> frameworks);
        ProvidersView MapFrom(Apprenticeships.Api.Types.Providers.Provider provider);
        StandardsView MapFrom(List<StandardSummary> standards);
    }
}