﻿using System.Collections.Generic;

namespace SFA.DAS.ProviderCommitments.Web.Models.Apprentice
{
    public class IndexViewModel
    {
        public long? ProviderId { get; set; }

        public IEnumerable<ApprenticeshipDetailsViewModel> Apprenticeships { get; set; }
        public string SortedByHeaderClassName { get; set; }
        public const string HeaderClassName = "das-table__sort";
    
        public void SortedByHeader()
        {
            SortedByHeaderClassName += HeaderClassName;
            if (FilterModel.ReverseSort)
            {
                SortedByHeaderClassName += " das-table__sort--desc";
            }
            else
            {
                SortedByHeaderClassName += " das-table__sort--asc";
            }
        }

        public ApprenticesFilterModel FilterModel { get; set; }
        public bool ShowPageLinks  => FilterModel.TotalNumberOfApprenticeshipsFound > Constants.ApprenticesSearch.NumberOfApprenticesPerSearchPage;
    }
}