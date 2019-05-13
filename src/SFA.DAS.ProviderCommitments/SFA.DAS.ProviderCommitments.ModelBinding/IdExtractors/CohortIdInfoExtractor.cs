
using SFA.DAS.HashingService;

namespace SFA.DAS.ProviderCommitments.ModelBinding.IdExtractors
{
    /// <summary>
    ///     Extracts the hashed cohort id from the request and makes it available to the model binder.
    /// </summary>
    public class CohortIdInfoExtractor : HashedPropertyModelBinder
    {
        public CohortIdInfoExtractor(IHashingService cohortIdHashingService) : 
            base(cohortIdHashingService, RouteValueKeys.CohortId)
        {
            // just call base   
        }
    }
}