using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse;
using SFA.DAS.ProviderCommitments.ModelBinding.Models;

namespace SFA.DAS.ProviderCommitments.Web.Models
{
    public class AddDraftApprenticeshipViewModel
    {
        public Guid ReservationId { get; set; }
        public int ProviderId { get; set; }
        public AccountLegalEntity AccountLegalEntity { get; set; }

        [Display(Name = "Employer")]
        [MaxLength(100)]
        public string Employer { get; set; }

        [Display(Name = "First Name")]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Display(Name = "Day")]
        public int? BirthDay { get; set; }

        [Display(Name = "Month")]
        public int? BirthMonth { get; set; }

        [Display(Name = "Year")]
        public int? BirthYear { get; set; }

        [Display(Name = "Unique Learner Number (ULN)")]
        public string UniqueLearnerNumber { get; set; }

        public string CourseCode { get; set; }

        [Display(Name = "Apprenticeship course")]
        public string CourseName { get; set; }

        [Display(Name = "Month")]
        public int? StartMonth { get; set; }

        [Display(Name = "Year")]
        public int? StartYear { get; set; }

        [Display(Name = "Month")]
        public int? FinishMonth { get; set; }

        [Display(Name = "Year")]
        public int? FinishYear { get; set; }

        [Display(Name = "Total agreed apprenticeship price (excluding VAT)")]
        public int? Cost { get; set; }

        [Display(Name = "Reference (optional)")]
        public string Reference { get; set; }

        public ICourse[] Courses { get; set; }
    }
}