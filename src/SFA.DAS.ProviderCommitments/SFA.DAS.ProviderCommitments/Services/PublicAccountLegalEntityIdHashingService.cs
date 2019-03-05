﻿using SFA.DAS.ProviderCommitments.Configuration;
using SFA.DAS.ProviderCommitments.Services.Temp;

namespace SFA.DAS.ProviderCommitments.Services
{
    public class PublicAccountLegalEntityIdHashingService : HashingService, IPublicAccountLegalEntityIdHashingService
    {
        public PublicAccountLegalEntityIdHashingService(PublicAccountLegalEntityIdHashingConfiguration configuration) :
            base(configuration.Alphabet, configuration.Salt)
        {
            // just call base    
        }
    }
}