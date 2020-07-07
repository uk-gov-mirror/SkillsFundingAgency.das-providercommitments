using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.Encoding.Mvc;
using SFA.DAS.ProviderCommitments.Web.Attributes;
using SFA.DAS.ProviderCommitments.Web.Extensions;

namespace SFA.DAS.ProviderCommitments.Web.Models.Apprentice
{
    public class StartDateRequest
    {
        public long ProviderId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        [AutoDecode]
        public long AccountLegalEntityId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        [AutoDecode]
        public long ApprenticeshipId { get; set; }
        public string StartDate { get; set; }
        public string EndDate { get; set; }
        public int? Price { get; set; }
    }
}
