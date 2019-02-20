﻿using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ProviderCommitments.Domain_Models;
using SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse;
using SFA.DAS.ProviderCommitments.Interfaces;
using SFA.DAS.Apprenticeships.Api.Client;
using SFA.DAS.Apprenticeships.Api.Types;
using Framework = SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse.Framework;
using Provider = SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse.Provider;
using Standard = SFA.DAS.ProviderCommitments.Domain_Models.ApprenticeshipCourse.Standard;

namespace SFA.DAS.ProviderCommitments.Services
{
        public class ApprenticeshipInfoService : IApprenticeshipInfoService
        {
            private const string StandardsKey = "Standards";
            private const string FrameworksKey = "Frameworks";

            private readonly ICache _cache;
            private readonly IApprenticeshipInfoServiceConfiguration _configuration;
            private readonly IApprenticeshipInfoServiceMapper _mapper;

            public ApprenticeshipInfoService(ICache cache, IApprenticeshipInfoServiceConfiguration configuration, IApprenticeshipInfoServiceMapper mapper)
            {
                _cache = cache;
                _configuration = configuration;
                _mapper = mapper;
            }

            public async Task<StandardsView> GetStandardsAsync(bool refreshCache = false)
            {
                if (!await _cache.ExistsAsync(StandardsKey) || refreshCache)
                {
                    var api = new StandardApiClient(_configuration.BaseUrl);

                    var standards = (await api.GetAllAsync()).OrderBy(x => x.Title).ToList();

                    await _cache.SetCustomValueAsync(StandardsKey, _mapper.MapFrom(standards));
                }

                return await _cache.GetCustomValueAsync<StandardsView>(StandardsKey);
            }

            public async Task<FrameworksView> GetFrameworksAsync(bool refreshCache = false)
            {
                if (!await _cache.ExistsAsync(FrameworksKey) || refreshCache)
                {
                    var api = new FrameworkApiClient(_configuration.BaseUrl);

                    var frameworks = (await api.GetAllAsync()).OrderBy(x => x.Title).ToList();

                    await _cache.SetCustomValueAsync(FrameworksKey, _mapper.MapFrom(frameworks));
                }

                return await _cache.GetCustomValueAsync<FrameworksView>(FrameworksKey);
            }

            public ProvidersView GetProvider(long ukPrn)
            {
                var api = new Providers.Api.Client.ProviderApiClient(_configuration.BaseUrl);
                var providers = api.Get(ukPrn);
                return _mapper.MapFrom(providers);
            }
        }
    }