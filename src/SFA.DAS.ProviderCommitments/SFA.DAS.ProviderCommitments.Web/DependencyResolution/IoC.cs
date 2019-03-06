﻿using SFA.DAS.ProviderCommitments.DependencyResolution;
using StructureMap;

namespace SFA.DAS.ProviderCommitments.Web.DependencyResolution
{
    public static class IoC
    {
        public static void Initialize(Registry registry)
        {
            registry.IncludeRegistry<CommitmentsApiRegistry>();
            registry.IncludeRegistry<MediatorRegistry>();
            registry.IncludeRegistry<HashingRegistry>();

            registry.IncludeRegistry<DefaultRegistry>();
        }
    }
}
