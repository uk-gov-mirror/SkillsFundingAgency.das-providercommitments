using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.Encoding.Mvc;
using SFA.DAS.ProviderCommitments.Web.Attributes;

namespace SFA.DAS.ProviderCommitments.Web.Models.Apprentice
{
    public class SelectEmployerRequest
    {
        [AutoDecode]
        public long ApprenticeshipId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        public long ProviderId { get; set; }
    }
}
