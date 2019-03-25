using SFA.DAS.ProviderCommitments.Application.Commands.CreateCohort;
using SFA.DAS.ProviderCommitments.Web.Models;

namespace SFA.DAS.ProviderCommitments.Web.Mappers
{
    public class CreateCohortRequestMapper : ICreateCohortRequestMapper
    {
        public CreateCohortRequest Map(AddDraftApprenticeshipViewModel source)
        {
            return new CreateCohortRequest
            {
                AccountLegalEntityId = source.AccountLegalEntity.AccountLegalEntityId ?? 0,
                ProviderId = source.ProviderId,
                ReservationId = source.ReservationId,
                FirstName = source.FirstName,
                LastName = source.LastName,
                BirthDay = source.BirthDay,
                BirthMonth = source.BirthMonth,
                BirthYear = source.BirthYear,
                Uln = source.UniqueLearnerNumber,
                CourseCode = source.CourseCode,
                Cost = source.Cost,
                CourseStartMonth = source.StartMonth,
                CourseStartYear = source.StartYear,
                CourseEndMonth = source.FinishMonth,
                CourseEndYear = source.FinishYear,
                OriginatorReference = source.Reference
            };
        }
    }
}
