using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.ProviderCommitments.Application.Commands.CreateCohort;

namespace SFA.DAS.ProviderCommitments.UnitTests.Commands.CreateCohort
{
    [TestFixture]
    public class WhenCreateCohortRequestIsHandled
    {
        private CreateCohortHandlerFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new CreateCohortHandlerFixture();
        }

        [Test]
        public async Task ThenTheCohortIsCreated()
        {
            await _fixture.Act();
            _fixture.VerifyCohortWasCreated();
        }

        [Test]
        public async Task ThenTheCohortIdIsReturned()
        {
            await _fixture.Act();
            _fixture.VerifyCohortIdWasReturned();
        }

        [Test]
        public async Task ThenTheCohortReferenceIsReturned()
        {
            await _fixture.Act();
            _fixture.VerifyCohortReferenceWasReturned();
        }

        private class CreateCohortHandlerFixture
        {
            private readonly CreateCohortHandler _handler;
            private readonly CreateCohortRequest _request;
            private readonly CreateCohortRequest _requestClone;
            private CreateCohortResponse _result;
            private readonly CommitmentsV2.Api.Types.Responses.CreateCohortResponse _apiResponse;
            private readonly Mock<ICommitmentsApiClient> _apiClient;

            public CreateCohortHandlerFixture()
            {
                var autoFixture = new Fixture();

                _request = autoFixture.Create<CreateCohortRequest>();
                _requestClone = TestHelper.Clone(_request);

                _apiResponse = autoFixture.Create<CommitmentsV2.Api.Types.Responses.CreateCohortResponse>();
                _apiClient = new Mock<ICommitmentsApiClient>();
                _apiClient.Setup(x => x.CreateCohort(It.IsAny<CommitmentsV2.Api.Types.Requests.CreateCohortRequest>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(_apiResponse);

                _handler = new CreateCohortHandler(_apiClient.Object);
            }

            public async Task Act()
            {
                _result = await _handler.Handle(_requestClone, CancellationToken.None);
            }


            public CreateCohortHandlerFixture VerifyCohortWasCreated()
            {
                _apiClient.Verify(x =>
                    x.CreateCohort(It.Is<CommitmentsV2.Api.Types.Requests.CreateCohortRequest>(r =>
                        r.ProviderId == _request.ProviderId
                        && r.AccountLegalEntityId == _request.AccountLegalEntityId
                        && r.ReservationId == _request.ReservationId
                        && r.FirstName == _request.FirstName
                        && r.LastName == _request.LastName
                        && r.BirthDay == _request.BirthDay
                        && r.BirthMonth == _request.BirthMonth
                        && r.BirthYear == _request.BirthYear
                        && r.Uln == _request.Uln
                        && r.CourseCode == _request.CourseCode
                        && r.Cost == _request.Cost
                        && r.CourseStartMonth == _request.CourseStartMonth
                        && r.CourseStartYear == _request.CourseStartYear
                        && r.CourseEndMonth == _request.CourseEndMonth
                        && r.CourseEndYear == _request.CourseEndYear
                        && r.OriginatorReference == _request.OriginatorReference
                    ), It.IsAny<CancellationToken>()));
                return this;
            }

            public CreateCohortHandlerFixture VerifyCohortIdWasReturned()
            {
                Assert.AreEqual(_apiResponse.CohortId, _result.CohortId);
                return this;
            }

            public CreateCohortHandlerFixture VerifyCohortReferenceWasReturned()
            {
                Assert.AreEqual(_apiResponse.CohortReference, _result.CohortReference);
                return this;
            }
        }
    }
}
