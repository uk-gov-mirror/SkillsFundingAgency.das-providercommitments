﻿using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Apprenticeships.Api.Types;
using SFA.DAS.Authorization.Mvc.Attributes;
using SFA.DAS.Authorization.ProviderPermissions.Options;
using SFA.DAS.CommitmentsV2.Shared.Extensions;
using SFA.DAS.CommitmentsV2.Shared.Interfaces;
using SFA.DAS.CommitmentsV2.Shared.Models;
using SFA.DAS.CommitmentsV2.Api.Types.Validation;
using SFA.DAS.ProviderCommitments.Application.Commands.CreateCohort;
using SFA.DAS.ProviderCommitments.Queries.GetAccountLegalEntity;
using SFA.DAS.ProviderCommitments.Queries.GetTrainingCourses;
using SFA.DAS.ProviderCommitments.Web.Models;
using SFA.DAS.ProviderCommitments.Web.Requests;
using SFA.DAS.ProviderUrlHelper;

namespace SFA.DAS.ProviderCommitments.Web.Controllers
{
    [Route("{providerId}/unapproved")]
//    [DasAuthorize(ProviderOperation.CreateCohort)]
    public class CreateCohortWithDraftApprenticeshipController : Controller
    {
        private readonly IMediator _mediator;
        private readonly IMapper<AddDraftApprenticeshipViewModel, CreateCohortRequest> _createCohortRequestMapper;
        private readonly IMapper<SelectEmployerRequest, SelectEmployerViewModel> _selectEmployerViewModelMapper;
        private readonly ILinkGenerator _urlHelper;

        public CreateCohortWithDraftApprenticeshipController(IMediator mediator,
            IMapper<AddDraftApprenticeshipViewModel, CreateCohortRequest> createCohortRequestMapper,
            IMapper<SelectEmployerRequest, SelectEmployerViewModel> selectEmployerViewModelMapper,
            ILinkGenerator urlHelper)
        {
            _mediator = mediator;
            _createCohortRequestMapper = createCohortRequestMapper;
            _selectEmployerViewModelMapper = selectEmployerViewModelMapper;
            _urlHelper = urlHelper;
        }

        [HttpGet]
        [Route("add-apprentice")]
        [Route("add/apprentice")]
        public async Task<IActionResult> AddDraftApprenticeship(CreateCohortWithDraftApprenticeshipRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new AddDraftApprenticeshipViewModel
            {
                EmployerAccountLegalEntityPublicHashedId = request.EmployerAccountLegalEntityPublicHashedId,
                AccountLegalEntityId = request.AccountLegalEntityId,
                StartDate = new MonthYearModel(request.StartMonthYear),
                ReservationId = request.ReservationId.Value,
                CourseCode = request.CourseCode
            };

            await AddEmployerAndCoursesToModel(model);

            return View(model);
        }

        [HttpPost]
        [Route("add-apprentice")]
        [Route("add/apprentice")]
        public async Task<IActionResult> AddDraftApprenticeship(AddDraftApprenticeshipViewModel model)
        {
            // TODO this will probably need to be removed later (once validation is moved to API)
            if (!ModelState.IsValid)
            {
                await AddEmployerAndCoursesToModel(model);
                return View(model);
            }

            var request = await _createCohortRequestMapper.Map(model);

            try
            {
                var response = await _mediator.Send(request);

                var cohortDetailsUrl = $"{model.ProviderId}/apprentices/{response.CohortReference}/Details";
                var url = _urlHelper.ProviderApprenticeshipServiceLink(cohortDetailsUrl);
                return Redirect(url);
            }
            catch (CommitmentsApiModelException ex)
            {
                ModelState.AddModelExceptionErrors(ex);

                await AddEmployerAndCoursesToModel(model);
                return View(model);
            }
        }

        [HttpGet]
        [Route("add/select-employer")]
        public async Task<IActionResult> SelectEmployer(SelectEmployerRequest request)
        {
            var model = await _selectEmployerViewModelMapper.Map(request);

            return View(model);

        }

        private async Task AddEmployerAndCoursesToModel(AddDraftApprenticeshipViewModel model)
        {
            var getEmployerTask =
                GetEmployerIfRequired(model.AccountLegalEntityId);

            var getCoursesTask = GetCourses();

            await Task.WhenAll(getEmployerTask, getCoursesTask);

            model.Employer = getEmployerTask.Result?.LegalEntityName;
            model.Courses = getCoursesTask.Result;
        }

        private Task<GetAccountLegalEntityResponse>  GetEmployerIfRequired(long? accountLegalEntityId)
        {
            if (!accountLegalEntityId.HasValue)
            {
                return Task.FromResult((GetAccountLegalEntityResponse) null);
            }

            return _mediator.Send(new GetAccountLegalEntityRequest
            {
                EmployerAccountLegalEntityId = accountLegalEntityId.Value
            });
        }

        private async Task<ITrainingProgramme[]> GetCourses()
        {
            var result = await _mediator.Send(new GetTrainingCoursesQueryRequest { IncludeFrameworks = true });

            return result.TrainingCourses;
        }
    }
}