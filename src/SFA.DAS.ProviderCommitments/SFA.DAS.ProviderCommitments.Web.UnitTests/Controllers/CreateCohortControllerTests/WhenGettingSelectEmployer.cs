﻿using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.ProviderCommitments.Web.Controllers;
using SFA.DAS.ProviderCommitments.Web.Models;
using SFA.DAS.ProviderCommitments.Web.Requests;
using SFA.DAS.ProviderUrlHelper;

namespace SFA.DAS.ProviderCommitments.Web.UnitTests.Controllers.CreateCohortControllerTests
{
    [TestFixture]
    public class WhenGettingSelectEmployer 
    {

        [Test]
        public async Task AndModelStateInvalid_ThenReturnsBadRequest()
        {
            var fixture = new SelectEmployerFixture()
                .WithModelStateErrors();

            var result = await fixture.Act();
            
            Assert.AreEqual(typeof(BadRequestObjectResult), result.GetType());
        }

        [Test]
        public async Task ThenCallsModelMapper()
        {
            var fixture = new SelectEmployerFixture();

            await fixture.Act();

            fixture.VerifyMapperWasCalled();
        }

        [Test]
        public async Task ThenReturnsView()
        {
            var fixture = new SelectEmployerFixture();

            var result = await fixture.Act() as ViewResult;

            Assert.NotNull(result);
            Assert.AreEqual(typeof(SelectEmployerViewModel), result.Model.GetType());
        }
    }

    public class SelectEmployerFixture
    {
        public CohortController Sut { get; set; }
        
        private readonly Mock<IModelMapper> _modelMapperMock;
        private readonly SelectEmployerViewModel _viewModel;
        private readonly SelectEmployerRequest _request;
        private readonly long _providerId;

        public SelectEmployerFixture()
        {
            _request = new SelectEmployerRequest { ProviderId = _providerId };
            _modelMapperMock = new Mock<IModelMapper>();
            _viewModel = new SelectEmployerViewModel
            {
                AccountProviderLegalEntities = new List<AccountProviderLegalEntityViewModel>(),
                BackLink = "Test.com"
            };
            _providerId = 123;

            _modelMapperMock
                .Setup(x => x.Map<SelectEmployerViewModel>(_request))
                .ReturnsAsync(_viewModel);

            Sut = new CreateCohortWithDraftApprenticeshipController(Mock.Of<IMediator>(), _modelMapperMock.Object, Mock.Of<ILinkGenerator>());
        }

        public SelectEmployerFixture WithModelStateErrors()
        {
            Sut.ControllerContext.ModelState.AddModelError("TestError","Test Error");
            return this;
        }

        public void VerifyMapperWasCalled()
        {
            _modelMapperMock.Verify(x => x.Map<SelectEmployerViewModel>(_request));
        }

        public async Task<IActionResult> Act() => await Sut.SelectEmployer(_request);
    }
}
