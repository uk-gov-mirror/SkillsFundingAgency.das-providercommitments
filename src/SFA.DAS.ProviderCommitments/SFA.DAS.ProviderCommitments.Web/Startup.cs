﻿using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SFA.DAS.Authorization.Mvc;
using SFA.DAS.ProviderCommitments.Web.Authentication;
using SFA.DAS.ProviderCommitments.Web.DependencyResolution;
using SFA.DAS.ProviderCommitments.Web.Extensions;
using SFA.DAS.ProviderCommitments.Web.Validators;
using StructureMap;

namespace SFA.DAS.ProviderCommitments.Web
{

    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .Configure<CookiePolicyOptions>(options =>
                {
                    // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                    options.CheckConsentNeeded = context => true;
                    options.MinimumSameSitePolicy = SameSiteMode.None;
                })
                .AddProviderIdamsAuthentication(Configuration)
                .AddDasAuthorization()
                .AddMvc(options =>
                {
                    ConfigureAuthorization(options);
                })
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddControllersAsServices()
                .AddSessionStateTempDataProvider()
                .AddFluentValidation(fv=>fv.RegisterValidatorsFromAssemblyContaining<AddDraftApprenticeshipViewModelValidator>())
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            services.AddHealthChecks();
        }

        /// <summary>
        ///     Override in integration tests to override authorization behaviour.
        /// </summary>
        protected virtual void ConfigureAuthorization(MvcOptions options)
        {
            var policy = new AuthorizationPolicyBuilder()
                .RequireProviderInRouteMatchesProviderInClaims()
                .Build();

            options.Filters.Add(new AuthorizeFilter(policy));
            options.AddAuthorization();
        }

        public void ConfigureContainer(Registry registry)
        {
            IoC.Initialize(registry);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseStatusCodePagesWithReExecute("/error", "?statuscode={0}")
                .UseUnauthorizedAccessExceptionHandler()
                .UseHttpsRedirection()
                .UseFeatureToggles()
                .UseStaticFiles()
                .UseCookiePolicy()
                .UseAuthentication()
                .UseMvc(routes =>
                {
                    routes.MapRoute(
                        name: "default",
                        template: "{controller=Home}/{action=Index}/{id?}");
                })
                .UseHealthChecks("/health-check"); 

            var logger = loggerFactory.CreateLogger(nameof(Startup));
            logger.Log(LogLevel.Information, "Application start up configure is complete");

        }
    }
}
