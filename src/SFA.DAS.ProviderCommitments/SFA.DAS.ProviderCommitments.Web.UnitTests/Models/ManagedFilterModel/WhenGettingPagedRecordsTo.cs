﻿using AutoFixture.NUnit3;
using FluentAssertions;
using NUnit.Framework;
using SFA.DAS.ProviderCommitments.Web.Models;

namespace SFA.DAS.ProviderCommitments.Web.UnitTests.Models.ManagedFilterModel
{
    public class WhenGettingPagedRecordsTo
    {
        [Test, AutoData]
        public void And_PageNumber_1_Then_Should_Be_PageSize(ManageApprenticesFilterModel filterModel)
        {
            filterModel.PageNumber = 1;
            filterModel.NumberOfRecordsFound = 20 * ManageApprenticesFilterModel.PageSize;

            filterModel.PagedRecordsTo.Should().Be(ManageApprenticesFilterModel.PageSize);
        }

        [Test, AutoData]
        public void And_PageNumber_2_Then_Should_Be_Double_PageSize(ManageApprenticesFilterModel filterModel)
        {
            filterModel.PageNumber = 2;
            filterModel.NumberOfRecordsFound = 20 * ManageApprenticesFilterModel.PageSize;

            filterModel.PagedRecordsTo.Should().Be(2 * ManageApprenticesFilterModel.PageSize);
        }

        [Test, AutoData]
        public void And_PageNumber_3_Then_Should_Be_Triple_PageSize(ManageApprenticesFilterModel filterModel)
        {
            filterModel.PageNumber = 3;
            filterModel.NumberOfRecordsFound = 20 * ManageApprenticesFilterModel.PageSize;

            filterModel.PagedRecordsTo.Should().Be(3 * ManageApprenticesFilterModel.PageSize);
        }

        [Test, AutoData]
        public void And_TotalRecords_Less_Than_Calculated_PagedRecordsTo_Then_Is_TotalRecords(ManageApprenticesFilterModel filterModel)
        {
            filterModel.PageNumber = 3;
            filterModel.NumberOfRecordsFound = 3 * ManageApprenticesFilterModel.PageSize - 20;

            filterModel.PagedRecordsTo.Should().Be(filterModel.NumberOfRecordsFound);
        }
    }
}