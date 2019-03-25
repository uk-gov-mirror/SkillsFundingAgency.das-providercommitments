using System.Threading;
using System.Threading.Tasks;
using FluentValidation;
using MediatR;
using SFA.DAS.CommitmentsV2.Api.Client;

namespace SFA.DAS.ProviderCommitments.Application.Commands.CreateCohort
{
    public class CreateCohortHandler : IRequestHandler<CreateCohortRequest, CreateCohortResponse>
    {
        private readonly ICommitmentsApiClient _apiClient;

        public CreateCohortHandler(ICommitmentsApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        public async Task<CreateCohortResponse> Handle(CreateCohortRequest request, CancellationToken cancellationToken)
        {
            var apiResult = await _apiClient.CreateCohort(request, cancellationToken);

            return new CreateCohortResponse
            {
                CohortId = apiResult.CohortId,
                CohortReference = apiResult.CohortReference
            };
        }
    }
}
