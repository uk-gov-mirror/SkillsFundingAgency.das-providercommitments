﻿using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Commitments.Shared.Interfaces;
using SFA.DAS.ProviderCommitments.Services;
using SFA.DAS.ProviderCommitments.Web.Models;

namespace SFA.DAS.ProviderCommitments.Web.Controllers
{
    [Route("v2/{providerId}/apprentices")]
    public class ManageApprenticesController : Controller
    {
        private readonly ICommitmentsService _commitmentsService;
        private readonly ICreateCsvService _createCsvService;

        public ManageApprenticesController(ICommitmentsService commitmentsService, ICreateCsvService createCsvService)
        {
            _commitmentsService = commitmentsService;
            _createCsvService = createCsvService;
        }

        public async Task<IActionResult> Index(uint providerId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var model = new ManageApprenticesViewModel
            {
                ProviderId = providerId,
                Apprenticeships = await _commitmentsService.GetApprenticeships(providerId)
            };

            return View(model);
        }

        [HttpGet]
        [Route("download",Name = "Download")]
        public async Task<IActionResult> Download(uint providerId)
        {
            var result = await _commitmentsService.GetApprenticeships(providerId);

            var csvContent = result.Select(c => (ApprenticeshipDetailsCsvModel)c).ToList();
            
            var csvFileContent = _createCsvService.GenerateCsvContent(csvContent);
            return File(csvFileContent, "text/csv", $"{"Manageyourapprentices"}_{DateTime.Now:yyyyMMddhhmmss}.csv");
        }

        [Route("{apprenticeshipId}", Name = "ApprenticeshipDetails")]
        public IActionResult Details(uint providerId, long apprenticeshipId)
        {
            return Content($"Details of apprenticeship Id:[{apprenticeshipId}].");
        }
    }
}