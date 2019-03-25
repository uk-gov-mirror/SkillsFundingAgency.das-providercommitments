using MediatR;

namespace SFA.DAS.ProviderCommitments.Application.Commands.CreateCohort
{
    public class CreateCohortRequest : CommitmentsV2.Api.Types.Requests.CreateCohortRequest, IRequest<CreateCohortResponse>
    {
    }
}
