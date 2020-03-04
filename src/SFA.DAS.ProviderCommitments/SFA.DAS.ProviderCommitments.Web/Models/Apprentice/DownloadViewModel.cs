﻿using System;
using System.IO;
using System.Threading.Tasks;
using SFA.DAS.CommitmentsV2.Api.Types.Requests;

namespace SFA.DAS.ProviderCommitments.Web.Models.Apprentice
{
    public class DownloadViewModel
    {
        public string Name { get; set; }
        public string ContentType => "text/csv";
        public Func<GetApprenticeshipsRequest, Task<MemoryStream>> GetAndCreateContent { get; set; }
        public GetApprenticeshipsRequest Request { get; set; }
        public Func<bool> Dispose { get; set; }
    }
}
