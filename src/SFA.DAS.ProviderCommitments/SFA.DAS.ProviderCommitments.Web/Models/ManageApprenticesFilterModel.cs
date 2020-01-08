﻿using System;
using System.Collections.Generic;

namespace SFA.DAS.ProviderCommitments.Web.Models
{
    public class ManageApprenticesFilterModelBase
    {
        public int PageNumber { get; set; } = 1;
    }

    public class ManageApprenticesFilterModel : ManageApprenticesFilterModelBase
    {
        public const int PageSize = ProviderCommitmentsWebConstants.NumberOfApprenticesPerSearchPage;
        public int PagedRecordsFrom => TotalNumberOfApprenticeshipsFound == 0 ? 0 : (PageNumber - 1) * PageSize + 1;
        public int PagedRecordsTo {
            get
            {
                var potentialValue = PageNumber * PageSize;
                return TotalNumberOfApprenticeshipsFound < potentialValue ? TotalNumberOfApprenticeshipsFound: potentialValue;
            }
        }

        public int TotalNumberOfApprenticeshipsFound { get; set; }
        public int TotalNumberOfApprenticeshipsWithAlertsFound { get; set; }
        
        public IEnumerable<PageLink> PageLinks {
            get
            {
                var links = new List<PageLink>();
                var totalPages = (int)Math.Ceiling((double)TotalNumberOfApprenticeshipsFound / PageSize);
                var totalPageLinks = totalPages < 5 ? totalPages : 5;

                //previous link
                if (totalPages > 1 && PageNumber > 1)
                {
                    links.Add(new PageLink
                    {
                        Label = "Previous",
                        AriaLabel = "Previous page",
                        RouteData = BuildRouteData(PageNumber - 1)
                    });
                }

                //numbered links
                var pageNumberSeed = 1;
                if (totalPages > 5 && PageNumber > 3)
                {
                    pageNumberSeed = PageNumber - 2;

                    if (PageNumber > totalPages - 2)
                        pageNumberSeed = totalPages - 4;
                }

                for (var i = 0; i < totalPageLinks; i++)
                {
                    var link = new PageLink
                    {
                        Label = (pageNumberSeed + i).ToString(),
                        AriaLabel = $"Page {pageNumberSeed + i}",
                        IsCurrent = pageNumberSeed + i == PageNumber? true : (bool?)null,
                        RouteData = BuildRouteData(pageNumberSeed + i)
                    };
                    links.Add(link);
                }

                //next link
                if (totalPages > 1 && PageNumber < totalPages)
                {
                    links.Add(new PageLink
                    {
                        Label = "Next",
                        AriaLabel = "Next page",
                        RouteData = BuildRouteData(PageNumber + 1)
                    });
                }

                return links;
            }
        }

        private Dictionary<string, string> BuildRouteData(int pageNumber)
        {
            var routeData = new Dictionary<string, string>
            {
                {"pageNumber", pageNumber.ToString()}
            };

            return routeData;
        }
    }

    public class PageLink
    {
        public string Label { get; set; }
        public string AriaLabel { get; set; }
        public bool? IsCurrent { get; set; }
        public Dictionary<string, string> RouteData { get; set; }
    }
}
