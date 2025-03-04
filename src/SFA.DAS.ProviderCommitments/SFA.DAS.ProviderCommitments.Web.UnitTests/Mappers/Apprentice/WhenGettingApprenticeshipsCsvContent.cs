﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Api.Client;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;
using SFA.DAS.CommitmentsV2.Api.Types.Responses;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.Encoding;
using SFA.DAS.ProviderCommitments.Web.Mappers.Apprentice;
using SFA.DAS.ProviderCommitments.Web.Models.Apprentice;
using SFA.DAS.Testing.AutoFixture;

namespace SFA.DAS.ProviderCommitments.Web.UnitTests.Mappers.Apprentice
{
    public class WhenGettingApprenticeshipsCsvContent
    {
        [Test, MoqAutoData]
        public async Task Then_Passes_Filter_Args_To_Api(
            DownloadRequest csvRequest,
            [Frozen] Mock<ICommitmentsApiClient> mockApiClient,
            DownloadApprenticesRequestMapper mapper)
        {
            await mapper.Map(csvRequest);

            mockApiClient.Verify(client => client.GetApprenticeships(
                It.Is<GetApprenticeshipsRequest>(apiRequest =>
                    apiRequest.ProviderId == csvRequest.ProviderId &&
                    apiRequest.SearchTerm == csvRequest.SearchTerm && 
                    apiRequest.EmployerName == csvRequest.SelectedEmployer &&
                    apiRequest.CourseName == csvRequest.SelectedCourse &&
                    apiRequest.Status == csvRequest.SelectedStatus &&
                    apiRequest.StartDate == csvRequest.SelectedStartDate &&
                    apiRequest.EndDate == csvRequest.SelectedEndDate),
                It.IsAny<CancellationToken>()));
        }

        [Test]
        public async Task ShouldMapValues()
        {
            //Arrange
            var fixture = new Fixture();
            var clientResponse = fixture.Create<GetApprenticeshipsResponse>();
            var request = fixture.Create<DownloadRequest>();
            var client = new Mock<ICommitmentsApiClient>();
            var csvService = new Mock<ICreateCsvService>();
            var currentDateTime = new Mock<ICurrentDateTime>();
            var encodingService = new Mock<IEncodingService>();
            var expectedCsvContent = new byte[] {1, 2, 3, 4};
            var expectedMemoryStream = new MemoryStream(expectedCsvContent);
            currentDateTime.Setup(x => x.UtcNow).Returns(new DateTime(2020, 12, 30));
            var expectedFileName = $"{"Manageyourapprentices"}_{currentDateTime.Object.UtcNow:yyyyMMddhhmmss}.csv";

            var mapper = new DownloadApprenticesRequestMapper(client.Object, csvService.Object, currentDateTime.Object, encodingService.Object);

            client.Setup(x => x.GetApprenticeships(It.Is<GetApprenticeshipsRequest>(r => 
                    r.ProviderId.Equals(request.ProviderId)), It.IsAny<CancellationToken>()))
                .ReturnsAsync(clientResponse);
            csvService.Setup(x => x.GenerateCsvContent(It.IsAny<IEnumerable<ApprenticeshipDetailsCsvModel>>(), true))
                .Returns(expectedMemoryStream);

            //Act
            var content = await mapper.Map(request);

            //Assert
            Assert.AreEqual(expectedFileName, content.Name);
            Assert.AreEqual(expectedMemoryStream, content.Content);
        }
    }
}
