using System;
using System.ComponentModel.DataAnnotations;
using SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse;
using SFA.DAS.ProviderCommitments.Models;

namespace SFA.DAS.ProviderCommitments.Web.Models
{
    public class DraftApprenticeshipViewModel
    {

        public DraftApprenticeshipViewModel(DateTime? dateOfBirth, DateTime? startDate, DateTime? endDate) : base()
        {
            DateOfBirth = dateOfBirth == null ? new DateModel() : new DateModel(dateOfBirth.Value);
            StartDate = startDate == null ? new MonthYearModel("") : new MonthYearModel($"{startDate.Value.Month}{startDate.Value.Year}");
            EndDate = endDate == null ? new MonthYearModel("") : new MonthYearModel($"{endDate.Value.Month}{endDate.Value.Year}");
        }

        public DraftApprenticeshipViewModel()
        {
            DateOfBirth = new DateModel();
            StartDate = new MonthYearModel("");
            EndDate = new MonthYearModel("");
        }


        public int ProviderId { get; set; }

        public string CohortReference { get; set; }
        public long? CohortId { get; set; }

        public Guid? ReservationId { get; set; }

        [Display(Name = "Employer")]
        [MaxLength(100)]
        public string Employer { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of birth")]
        [DataType(DataType.Date)]
        public DateModel DateOfBirth { get; }

        [Display(Name = "Day")]
        public int? BirthDay { get => DateOfBirth.Day ; set => DateOfBirth.Day = value; }

        [Display(Name = "Month")]
        public int? BirthMonth { get => DateOfBirth.Month; set => DateOfBirth.Month = value; }

        [Display(Name = "Year")]
        public int? BirthYear { get => DateOfBirth.Year; set => DateOfBirth.Year = value; }

        [Display(Name = "Unique Learner Number (ULN)")]
        public string Uln { get; set; }

        public string CourseCode { get; set; }

        [Display(Name = "Planned training start date")]
        public MonthYearModel StartDate { get; set; }

        [Display(Name = "Month")]
        public int? StartMonth { get => StartDate.Month; set => StartDate.Month = value; }

        [Display(Name = "Year")]
        public int? StartYear { get => StartDate.Year; set => StartDate.Year = value; }

        [Display(Name = "Projected finish date")]
        public MonthYearModel EndDate { get; }

        [Display(Name = "Month")]
        public int? EndMonth { get => EndDate.Month; set => EndDate.Month = value; }

        [Display(Name = "Year")]
        public int? EndYear { get => EndDate.Year; set => EndDate.Year = value; }

        [Display(Name = "Total agreed apprenticeship price (excluding VAT)")]
        public int? Cost { get; set; }

        [Display(Name = "Reference (optional)")]
        public string Reference { get; set; }

        public ICourse[] Courses { get; set; }
    }
}