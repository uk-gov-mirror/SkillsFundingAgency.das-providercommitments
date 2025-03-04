﻿using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using MediatR;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.ProviderCommitments.Queries.GetTrainingCourses;
using SFA.DAS.ProviderCommitments.Web.Mappers.Cohort;
using SFA.DAS.ProviderCommitments.Web.Models;

namespace SFA.DAS.ProviderCommitments.Web.UnitTests.Mappers.Cohort
{
    [TestFixture]
    public class WhenIMapCreateCohortWithDraftApprenticeshipRequestToAddDraftApprenticeshipViewModel
    {
        private AddDraftApprenticeshipViewModelMapper _mapper;
        private CreateCohortWithDraftApprenticeshipRequest _source;
        private Mock<ICommitmentsApiClient> _commitmentsApiClient;
        private Mock<IMediator> _mediator;
        private AccountLegalEntityResponse _accountLegalEntityResponse;
        private GetTrainingCoursesQueryResponse _trainingCoursesQueryResponse;

        [SetUp]
        public void Arrange()
        {
            var fixture = new Fixture();

            _source = fixture.Build<CreateCohortWithDraftApprenticeshipRequest>().With(x => x.StartMonthYear, "042020").Create();
            _accountLegalEntityResponse = fixture.Create<AccountLegalEntityResponse>();

            _trainingCoursesQueryResponse = fixture.Create<GetTrainingCoursesQueryResponse>();

            _commitmentsApiClient = new Mock<ICommitmentsApiClient>();
            _commitmentsApiClient.Setup(x => x.GetAccountLegalEntity(_source.AccountLegalEntityId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(_accountLegalEntityResponse);
            
            _mediator = new Mock<IMediator>();
            _mediator.Setup(x => x.Send(It.IsAny<GetTrainingCoursesQueryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(_trainingCoursesQueryResponse);

            _mapper = new AddDraftApprenticeshipViewModelMapper(_commitmentsApiClient.Object, _mediator.Object);
        }

        [Test]
        public async Task ThenAccountLegalEntityIdIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_source.AccountLegalEntityId, result.AccountLegalEntityId);
        }

        [Test]
        public async Task ThenEmployerAccountLegalEntityPublicHashedIdIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_source.EmployerAccountLegalEntityPublicHashedId, result.EmployerAccountLegalEntityPublicHashedId);
        }

        [Test]
        public async Task ThenEmployerIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_accountLegalEntityResponse.LegalEntityName, result.Employer);
        }

        [Test]
        public async Task ThenCourseCodeIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_source.CourseCode, result.CourseCode);
        }

        [Test]
        public async Task ThenCoursesAreMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_trainingCoursesQueryResponse.TrainingCourses, result.Courses);
        }

        [TestCase(ApprenticeshipEmployerType.Levy, true)]
        [TestCase(ApprenticeshipEmployerType.NonLevy, false)]
        public async Task ThenFrameworkCoursesAreIncludeOrNotInMediatorRequest(ApprenticeshipEmployerType levyStatus, bool frameworksAreIncluded)
        {
            _accountLegalEntityResponse.LevyStatus = levyStatus;
            await _mapper.Map(_source);
            _mediator.Verify(x=>x.Send(It.Is<GetTrainingCoursesQueryRequest>(p=>p.IncludeFrameworks == frameworksAreIncluded), It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ThenStartMonthYearIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_source.StartMonthYear, result.StartDate.MonthYear);
        }

        [Test]
        public async Task ThenReservationIdIsMappedCorrectly()
        {
            var result = await _mapper.Map(_source);
            Assert.AreEqual(_source.ReservationId, result.ReservationId);
        }
    }
}