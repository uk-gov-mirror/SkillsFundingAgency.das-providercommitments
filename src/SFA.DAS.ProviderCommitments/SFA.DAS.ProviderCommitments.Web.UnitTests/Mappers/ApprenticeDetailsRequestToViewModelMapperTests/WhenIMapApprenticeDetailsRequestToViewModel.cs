﻿using AutoFixture;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Authorization.Features.Services;
using SFA.DAS.Authorization.ProviderFeatures.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderCommitments.Features;
using SFA.DAS.ProviderCommitments.Web.Mappers.Apprentice;
using SFA.DAS.ProviderCommitments.Web.Models.Apprentice;

namespace SFA.DAS.ProviderCommitments.Web.UnitTests.Mappers.ApprenticeDetailsRequestToViewModelMapperTests
{
    [TestFixture]
    public class WhenIMapApprenticeDetailsRequestToViewModel
    {
        private WhenIMapApprenticeDetailsRequestToViewModelFixture _fixture;

        [SetUp]
        public void Arrange()
        {
            _fixture = new WhenIMapApprenticeDetailsRequestToViewModelFixture();
        }

        [Test]
        public async Task ThenApprenticeshipHashedIdIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.Source.ApprenticeshipHashedId, _fixture.Result.ApprenticeshipHashedId);
        }

        [Test]
        public async Task ThenFullNameIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.FirstName + " " + _fixture.ApiResponse.LastName, _fixture.Result.ApprenticeName);
        }

        [Test]
        public async Task ThenEmployerIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.EmployerName, _fixture.Result.Employer);
        }

        [Test]
        public async Task ThenReferenceIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.CohortReference, _fixture.Result.Reference);
        }

        [Test]
        public async Task ThenStatusIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.Status, _fixture.Result.Status);
        }

        [Test]
        public async Task ThenStopDateIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.StopDate, _fixture.Result.StopDate);
        }

        [Test]
        public async Task ThenAgreementIdIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.AgreementId, _fixture.Result.AgreementId);
        }

        [Test]
        public async Task ThenDateOfBirthIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.DateOfBirth, _fixture.Result.DateOfBirth);
        }

        [Test]
        public async Task ThenUlnIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.Uln, _fixture.Result.Uln);
        }

        [Test]
        public async Task ThenCourseNameIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.CourseName, _fixture.Result.CourseName);
        }

        [Test]
        public async Task ThenStartDateIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.StartDate, _fixture.Result.StartDate);
        }

        [Test]
        public async Task ThenEndDateIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.EndDate, _fixture.Result.EndDate);
        }

        [Test]
        public async Task ThenProviderRefIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.ApiResponse.Reference, _fixture.Result.ProviderRef);
        }

        [Test]
        public async Task ThenPriceIsMappedCorrectly()
        {
            await _fixture.Map();
            Assert.AreEqual(_fixture.PriceEpisodesApiResponse.PriceEpisodes.First().Cost, _fixture.Result.Cost);
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task ThenChangeOfEmployerEnabledIsMappedCorrectly(bool enabled)
        {
            _fixture.WithChangeOfEmployerToggle(enabled);

            await _fixture.Map();

            Assert.AreEqual(enabled, _fixture.Result.ChangeOfEmployerEnabled);
        }

        public class WhenIMapApprenticeDetailsRequestToViewModelFixture
        {
            private readonly DetailsViewModelMapper _mapper;
            public DetailsRequest Source { get; }
            public DetailsViewModel Result { get; private set; }
            public GetApprenticeshipResponse ApiResponse { get; }
            public GetPriceEpisodesResponse PriceEpisodesApiResponse { get; }
            
            private readonly Mock<IEncodingService> _encodingService;
            private readonly Mock<IFeatureTogglesService<ProviderFeatureToggle>> _featureToggleService;
            public string CohortReference { get; }
            public string AgreementId { get; }

            public WhenIMapApprenticeDetailsRequestToViewModelFixture()
            {
                var fixture = new Fixture();
                Source = fixture.Create<DetailsRequest>();
                ApiResponse = fixture.Create<GetApprenticeshipResponse>();
                CohortReference = fixture.Create<string>();
                AgreementId = fixture.Create<string>();
                PriceEpisodesApiResponse = new GetPriceEpisodesResponse
                {
                    PriceEpisodes = new List<GetPriceEpisodesResponse.PriceEpisode>
                    {
                        new GetPriceEpisodesResponse.PriceEpisode {Cost = 100, FromDate = DateTime.UtcNow}
                    }
                };

                _encodingService = new Mock<IEncodingService>();
                _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.CohortReference)).Returns(CohortReference);
                _encodingService.Setup(x => x.Encode(It.IsAny<long>(), EncodingType.PublicAccountLegalEntityId)).Returns(AgreementId);

                _featureToggleService = new Mock<IFeatureTogglesService<ProviderFeatureToggle>>();
                _featureToggleService
                    .Setup(x => x.GetFeatureToggle(It.IsAny<string>()))
                    .Returns(new ProviderFeatureToggle()
                    {
                        Feature = nameof(ProviderFeature.ChangeOfEmployer),
                        IsEnabled = false,
                        Whitelist = null
                    });

                var apiClient = new Mock<ICommitmentsApiClient>();
                apiClient.Setup(x => x.GetApprenticeship(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(ApiResponse);

                apiClient.Setup(x => x.GetPriceEpisodes(It.IsAny<long>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(PriceEpisodesApiResponse);

                _mapper = new DetailsViewModelMapper(apiClient.Object, _encodingService.Object, _featureToggleService.Object);
            }

            public async Task<WhenIMapApprenticeDetailsRequestToViewModelFixture> Map()
            {
                Result = await _mapper.Map(Source);
                return this;
            }

            public WhenIMapApprenticeDetailsRequestToViewModelFixture WithChangeOfEmployerToggle(bool enabled)
            {
                _featureToggleService
                    .Setup(x => x.GetFeatureToggle(nameof(ProviderFeature.ChangeOfEmployer)))
                    .Returns(new ProviderFeatureToggle()
                    {
                        Feature = nameof(ProviderFeature.ChangeOfEmployer),
                        IsEnabled = enabled,
                        Whitelist = null
                    });
                return this;
            }
        }
    }
}
