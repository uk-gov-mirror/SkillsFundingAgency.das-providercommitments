﻿using SFA.DAS.Authorization.ModelBinding;

namespace SFA.DAS.ProviderCommitments.Web.Requests
{
    public class NonReservationsAddDraftApprenticeshipRequest : IAuthorizationContextModel
    {
        public long? CohortId { get; set; }

        public string CohortReference { get; set; }
    }
}
