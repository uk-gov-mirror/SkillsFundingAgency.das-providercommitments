﻿namespace SFA.DAS.ProviderCommitments.Web.Requests
{
    public class GetApprenticeshipsRequest
    {
        public long ProviderId { get; set; }
        public int PageNumber { get; set; }
        public int PageItemCount { get; set; }
        public string SortField { get; set; }
        public bool ReverseSort { get; set; }
    }
}
