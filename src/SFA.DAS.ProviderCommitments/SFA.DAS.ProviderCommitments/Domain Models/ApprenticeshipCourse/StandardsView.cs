﻿using System;

namespace SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse
{
    public class StandardsView
    {
        public DateTime CreationDate { get; set; }
        public Standard[] Standards { get; set; }
    }
}