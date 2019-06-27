﻿using System.Threading;
using System.Threading.Tasks;
using Moq;
using NUnit.Framework;

namespace SFA.DAS.ProviderCommitments.UnitTests.Services.ProviderCommitmentsServiceTests
{
    [TestFixture]
    public class WhenGettingADraftApprenticeship
    {
        private ProviderCommitmentsServiceTestFixtures _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new ProviderCommitmentsServiceTestFixtures();
            _fixture.SetupGetDraftApprenticeshipReturnValue(_fixture.GetDraftApprenticeshipResponse)
                .SetupHashingToEncodeInput();
        }

        [Test]
        public async Task ShouldMapValuesFromApiCallAndAddHashValues()
        {
            var providerId = 1;
            var cohortId = 2;
            var apprenticeshipId = 123;

            var result = await _fixture.Sut.GetDraftApprenticeshipForCohort(providerId, cohortId, apprenticeshipId);

            Assert.AreEqual(providerId, result.ProviderId);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.Id, result.DraftApprenticeshipId);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.FirstName, result.FirstName);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.LastName, result.LastName);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.Uln, result.UniqueLearnerNumber);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.CourseCode, result.CourseCode);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.Cost, result.Cost);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.StartDate, result.StartDate);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.EndDate, result.EndDate);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.DateOfBirth, result.DateOfBirth);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.Reference, result.OriginatorReference);
            Assert.AreEqual(_fixture.GetDraftApprenticeshipResponse.ReservationId, result.ReservationId);

            Assert.AreEqual($"CRX{cohortId}X", result.CohortReference);
            Assert.AreEqual($"AX{_fixture.GetDraftApprenticeshipResponse.Id}X", result.DraftApprenticeshipHashedId);
        }

        [Test]
        public async Task ShouldCallClientApiWithCorrectParameters()
        {
            await _fixture.Sut.GetDraftApprenticeshipForCohort(1, 2, 123);

            _fixture.CommitmentsApiClientMock.Verify(x => x.GetDraftApprenticeship(2, 123, It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}