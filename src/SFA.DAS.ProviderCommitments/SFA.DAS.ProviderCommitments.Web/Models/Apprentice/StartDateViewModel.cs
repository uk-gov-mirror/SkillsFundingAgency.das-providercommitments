using SFA.DAS.Authorization.ModelBinding;
using SFA.DAS.CommitmentsV2.Shared.Models;
using System;
using Microsoft.AspNetCore.Mvc;
using SFA.DAS.Encoding.Mvc;
using SFA.DAS.ProviderCommitments.Web.Attributes;
using SFA.DAS.ProviderCommitments.Web.Extensions;

namespace SFA.DAS.ProviderCommitments.Web.Models.Apprentice
{
    public class StartDateViewModel
    {
        public StartDateViewModel()
        {
            StartDate = new MonthYearModel("");
        }
        [AutoDecode]
        public long AccountLegalEntityId { get; set; }
        public string ApprenticeshipHashedId { get; set; }
        [AutoDecode]
        public long ApprenticeshipId { get; set; }
        public string EmployerAccountLegalEntityPublicHashedId { get; set; }
        public long ProviderId { get; set; }
        public string EndDate { get; set; }
        public int? Price { get; set; }
        public MonthYearModel StartDate { get; set; }
        public DateTime? StopDate { get; set; }
        [ModelBinder(typeof(SilentModelBinder2))]
        public int? StartMonth { get => StartDate.Month; set => StartDate.Month = value; }
        [ModelBinder(typeof(SilentModelBinder2))]
        public int? StartYear { get => StartDate.Year; set => StartDate.Year = value; }
        public bool InEditMode => Price.HasValue;
    }
}