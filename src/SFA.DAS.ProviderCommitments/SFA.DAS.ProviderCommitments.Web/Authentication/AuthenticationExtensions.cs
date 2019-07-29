using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authentication.WsFederation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SFA.DAS.Provider.Idams.Stub.Extensions;
using SFA.DAS.ProviderCommitments.Configuration;

namespace SFA.DAS.ProviderCommitments.Web.Authentication
{
    public static class AuthenticationExtensions
    {
        public static IServiceCollection AddProviderIdamsAuthentication(this IServiceCollection services, IConfiguration config)
        {
            var authenticationSettings = config.GetSection(ProviderCommitmentsConfigurationKeys.AuthenticationSettings).Get<AuthenticationSettings>();
            
            var cookieOptions = new Action<CookieAuthenticationOptions>(options =>
            {
                options.CookieManager = new ChunkingCookieManager { ChunkSize = 3000 };
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
                options.ReturnUrlParameter = "/Home/Index";
            });

            if (authenticationSettings.UseStub)
            {
                services.AddProviderIdamsStubAuthentication(cookieOptions,
                    new OpenIdConnectEvents { OnTokenValidated = context => OnSecurityTokenValidated(context.HttpContext) });

                return services;
            }
            else
            {
                services.AddAuthentication(sharedOptions =>
                    {
                        sharedOptions.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                        sharedOptions.DefaultChallengeScheme = WsFederationDefaults.AuthenticationScheme;
                    })
                    .AddWsFederation(options =>
                    {
                        // See: https://docs.microsoft.com/en-us/aspnet/core/security/authentication/ws-federation?view=aspnetcore-2.2
                        // This is the AAD tenant's "Federation Metadata Document" found on the app registrations blade
                        options.MetadataAddress = authenticationSettings.MetadataAddress;
                        // This is the app's "App ID URI" found in the app registration's Settings > Properties blade.
                        options.Wtrealm = authenticationSettings.Wtrealm;
                        options.Events.OnSecurityTokenValidated = context => OnSecurityTokenValidated(context.HttpContext);
                    }).AddCookie(cookieOptions);
                return services;
            }
        }

        private static Task OnSecurityTokenValidated(HttpContext context)
        {
            //todo: capture the claims values
            var claims = context.User.Claims;
            return Task.CompletedTask;
        }
    }
}
