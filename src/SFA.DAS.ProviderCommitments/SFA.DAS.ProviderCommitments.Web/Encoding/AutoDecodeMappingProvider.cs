using System.Collections.Generic;
using SFA.DAS.Encoding;
using SFA.DAS.Encoding.Mvc;

namespace SFA.DAS.ProviderCommitments.Web.Encoding
{
    public class AutoDecodeMappingProvider : IAutoDecodeMappingProvider
    {
        public Dictionary<string, AutoDecodeMapping> AutoDecodeMappings { get; }

        public AutoDecodeMappingProvider()
        {
            AutoDecodeMappings = new Dictionary<string, AutoDecodeMapping>
                {
                    { "ApprenticeshipId", new AutoDecodeMapping("apprenticeshipHashedId", EncodingType.ApprenticeshipId) },
                    { "AccountLegalEntityId", new AutoDecodeMapping("employerAccountLegalEntityPublicHashedId", EncodingType.PublicAccountLegalEntityId) },
                    { "AccountId", new AutoDecodeMapping("AccountHashedId", EncodingType.AccountId) },
                    { "CohortId", new AutoDecodeMapping("cohortReference", EncodingType.CohortReference) },
                    { "DraftApprenticeshipId", new AutoDecodeMapping("draftApprenticeshipHashedId", EncodingType.ApprenticeshipId)}
                };
        }
    }
}
