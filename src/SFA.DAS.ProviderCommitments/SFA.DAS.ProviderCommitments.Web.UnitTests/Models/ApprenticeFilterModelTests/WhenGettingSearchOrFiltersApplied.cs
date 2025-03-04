﻿using System;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.CommitmentsV2.Types;
using SFA.DAS.ProviderCommitments.Web.Models;
using SFA.DAS.ProviderCommitments.Web.Models.Apprentice;

namespace SFA.DAS.ProviderCommitments.Web.UnitTests.Models.ApprenticeFilterModelTests
{
    public class WhenGettingSearchOrFiltersApplied
    {
        [Test]
        public void And_Has_SearchTerm_Then_True()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SearchTerm = "asedfas"
            };

            filterModel.SearchOrFiltersApplied.Should().BeTrue();
        }

        [Test]
        public void And_Has_SelectedEmployer_Then_True()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SelectedEmployer = "asedfas"
            };

            filterModel.SearchOrFiltersApplied.Should().BeTrue();
        }

        [Test]
        public void And_Has_SelectedCourse_Then_True()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SelectedCourse = "asedfas"
            };

            filterModel.SearchOrFiltersApplied.Should().BeTrue();
        }

        [Test]
        public void And_Has_SelectedStatus_Then_True()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SelectedStatus = ApprenticeshipStatus.WaitingToStart
            };

            filterModel.SearchOrFiltersApplied.Should().BeTrue();
        }

        [Test]
        public void And_Has_SelectedStartDate_Then_True()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SelectedStartDate = DateTime.Today
            };

            filterModel.SearchOrFiltersApplied.Should().BeTrue();
        }

        [Test]
        public void And_Has_SelectedEndDate_Then_True()
        {
            var filterModel = new ApprenticesFilterModel
            {
                SelectedEndDate = DateTime.Today
            };

            filterModel.SearchOrFiltersApplied.Should().BeTrue();
        }

        [Test]
        public void And_No_Search_Or_Filter_Then_False()
        {
            var filterModel = new ApprenticesFilterModel();

            filterModel.SearchOrFiltersApplied.Should().BeFalse();
        }
    }
}