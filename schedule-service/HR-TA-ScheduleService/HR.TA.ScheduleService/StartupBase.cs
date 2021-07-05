//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text;
    using Microsoft.ApplicationInsights.Extensibility;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.DependencyInjection.Extensions;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using HR.TA.Common.Base.Configuration;
    using HR.TA.Common.Base.KeyVault;
    using HR.TA.Common.Base.Security.V2;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.Base.Utilities;
    using HR.TA.Common.Common.Common.Base.Configuration;
    using HR.TA.Common.Common.Common.Web.Extensions;
    using HR.TA.Common.Common.WebNotifications.Configurations;
    using HR.TA.Common.DocumentDB.V2;
    using HR.TA.Common.Email;
    using HR.TA.Common.MSGraph;
    using HR.TA.Common.Product.ServicePlatform.AspNetCore.Builder;
    using HR.TA.Common.Product.ServicePlatform.Flighting.AppConfiguration;
    using HR.TA.Common.Product.ServicePlatform.Instrumentation.ApplicationInsights;
    using HR.TA.Common.Product.ServicePlatform.Utils;
    using HR.TA.Common.Routing.DocumentDb;
    using HR.TA.Common.Routing.Extensions;
    using HR.TA.Common.Web.Extensions;
    using HR.TA.Common.Web.MiddleWare;
    using HR.TA.Data.DataAccess;
    using HR.TA.ScheduleService.BusinessLibrary.Business;
    using HR.TA.ScheduleService.BusinessLibrary.Business.V1;
    using HR.TA.ScheduleService.BusinessLibrary.Configurations;
    using HR.TA.ScheduleService.BusinessLibrary.Helpers;
    using HR.TA.ScheduleService.BusinessLibrary.Interface;
    using HR.TA.ScheduleService.BusinessLibrary.Notification;
    using HR.TA.ScheduleService.BusinessLibrary.Providers;
    using HR.TA.ScheduleService.BusinessLibrary.Providers.User;
    using HR.TA.ScheduleService.BusinessLibrary.WebNotifications.Providers;
    using HR.TA.ScheduleService.Data.DataProviders;
    using HR.TA.ScheduleService.FalconData.Query;
    using HR.TA.ServicePlatform.AspNetCore.Builder;
    using HR.TA.ServicePlatform.AspNetCore.Security;
    using HR.TA.ServicePlatform.Azure.AAD;
    using HR.TA.ServicePlatform.Azure.Extensions;
    using HR.TA.ServicePlatform.Azure.Security;
    using HR.TA.ServicePlatform.Communication.Http;
    using HR.TA.ServicePlatform.Configuration;
    using HR.TA.ServicePlatform.Flighting;
    using HR.TA.ServicePlatform.Security;
    using HR.TA.ServicePlatform.Tracing;
    using CommonFalconQuery = HR.TA.Common.Routing.Client;
    using FalconQuery = HR.TA.ScheduleService.FalconData.Query;

    /// <summary>
    /// The base class for starting up the application.
    /// </summary>
    public abstract class StartupBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StartupBase"/> class.
        /// </summary>
        /// <param name="configuration">Instance of <see cref="Microsoft.Extensions.Configuration.IConfiguration"/></param>
        public StartupBase(Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            this.Configuration = configuration;
            this.ServiceDetailsToPublish = new ApplicationPublishDetails()
            {
                ApplicationName = "Schedule Service",
                ApplicationDescription = "API Interface to perform background interview schedule setup, track user responses, send emails and automated alerts in Microsoft Recruit.",
                Contact = "cfehrgtaivftes@microsoft.com",
                ContactName = "Microsoft Recruit Engineering Team",
                IsAuthorizedByBearer = true,
                Version = "v1",
                DocumentationRoute = "/swagger/v1/swagger.json"
            };
        }

        private Microsoft.Extensions.Configuration.IConfiguration Configuration { get; }

        /// <summary>
        /// Gets the details related to this service that shall be published to the consumers.
        /// </summary>
        private ApplicationPublishDetails ServiceDetailsToPublish { get; }

        /// <summary>
        /// Configures the dependency injection services used.
        /// </summary>
        /// <param name="services">The services collection to populate.</param>
        public virtual void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient(typeof(ITraceSource), p => ScheduleServiceTracer.Instance)
            .AddSingleton<IFeatureSwitchManager, FeatureSwitchManager>()
            .AddTransient<IHttpContextAccessor, HttpContextAccessor>()
            .AddScoped<IHCMPrincipalRetriever, HCMPrincipalRetriever>()
            .AddScoped<IHCMServiceContext, HCMServiceContext>()
            .AddHttpClient()
            .AddSingleton<IMsGraphProvider, MsGraphProvider>()
            .AddTransient<IHttpCommunicationClientFactory, HttpCommunicationClientFactory>()
            .AddSingleton(typeof(IDocumentDBProvider<>), typeof(DocumentDBProvider<>));
            if (Convert.ToBoolean(this.Configuration["AzureFeatureFlagManagement:IsEnabled"]))
            {
                services.AddSingleton<IFlightsProvider<IDictionary<string, string>>, AppConfigurationFlightProvider>();
            }
            else
            {
                // services.AddSingleton<IFlightsProvider<IDictionary<string, string>>, ECSConfigurationProvider>();
            }

            services.AddSingleton<IRoleManager, RoleManager>()
            .AddSingleton<IScheduleManager, ScheduleManager>()
            .AddSingleton<IRoomResourceProvider, RoomResourceProvider>()
            .AddSingleton<IOutlookProvider, OutlookProvider>()
            .AddSingleton<IUserDetailsProvider, UserDetailsProvider>()
            .AddSingleton<ISecretManagerProvider, SecretManagerProvider>()
            .AddSingleton<ISecretManager, SecretManager>()
            .AddSingleton<ILockManager, LockManager>()
            .AddSingleton<IEmailHelper, EmailHelper>()
            .AddSingleton<ICertificateManager, CertificateManager>()
            .AddSingleton<IConfigurationManager, ConfigurationManager>()
            .AddSingleton<IDocDbDataAccess, DocDbDataAccess>()
            .AddSingleton<IGraphSubscriptionManager, GraphSubscriptionManager>()
#pragma warning disable CS0618

            // DocumentDb client is obselete, Need to upgrade with DocumentClientGenerator when provisioning upgraded package is available
            .AddSingleton<IDocumentDB, DocumentDB>()
#pragma warning restore CS0618
            .AddSingleton<IEmailClient, EmailClient>()
            .AddSingleton<IAccessTokenCache, AccessTokenCache>()
            .AddSingleton<INotificationClient, NotificationClient>()
            .AddSingleton<IEmailService, EmailService>()
            .AddSingleton<IDocumentClientGenerator, DocumentClientGenerator>()
            .AddSingleton<IDocumentClientStore>(new DocumentClientStore(new MemoryCache(new MemoryCacheOptions())))
            .AddSingleton<FalconQuery.IFalconQueryClient, FalconQuery.FalconQueryClient>()
            .AddSingleton<FalconQuery.FalconQuery>()
            .AddSingleton<IServiceBusHelper, ServiceBusHelper>()
            .AddSingleton<IScheduleQuery, ScheduleQuery>()
            .AddSingleton<INotificationManager, NotificationManager>()
            .AddSingleton<IEmailManager, EmailManager>()
            .AddSingleton<IEmailTemplateDataAccess, EmailTemplateDataAccess>()
            .AddSingleton<CommonFalconQuery.IFalconQueryClient, CommonFalconQuery.FalconQueryClient>()
            .AddScoped<IInternalsProvider, InternalsProvider>()
            .AddScoped<ITokenCacheService, TokenCacheService>()
            .AddTenantResourceManager()
            .AddMemoryCache()
            .AddPrincipalRetriever()
            .AddHttpCommunicationClientFactory();

            services.AddHcmDocumentClient();
            services.AddMvcWithCustomExceptionHandler();
            services.AddCertificateManager();
            services.AddAzureActiveDirectoryClient();
            services.AddSecretManager();
            services.AddHealthEndpoints();
            services.AddPrincipalRetriever();
            services.SetupEnvironmentAuthorization();
            services.Replace(ServiceDescriptor.Singleton<ILoggerFactory, LoggerFactoryWithoutDispose>());
            services.AddAuthentication("Bearer").AddJwtBearer(options =>
                SetJwtBearerOptions(options, FabricXmlConfigurationHelper.Instance.ConfigurationManager));
            services.AddSwagger(this.ServiceDetailsToPublish);
            services.AddSwaggerGen(c =>
            {
                c.DocumentFilter<SwaggerDocumentHostFilter>();

                // Include XML Documentation in Swagger definition.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath, true);
            });
            services.AddCors();
            services.AddScoped(typeof(ValidateModelAttribute));
            _ = services.AddSingleton<IWebNotificationInternalsProvider, DefaultWebNotificationInternalsProvider>();
            _ = services.AddWebNotifications((sp, client) =>
            {
                var configurationManager = sp.GetRequiredService<IConfigurationManager>();
                var webNotificationServiceConfiguration = configurationManager?.Get<WebNotificationServiceConfiguration>();
                client.BaseAddress = new Uri(webNotificationServiceConfiguration.BaseServiceUrl);
                client.DefaultRequestHeaders.Add("User-Agent", "IVScheduleService");
            });

            var appInsightsOptions = new Microsoft.ApplicationInsights.AspNetCore.Extensions.ApplicationInsightsServiceOptions();
            var tc = TelemetryConfiguration.Active;
            var channel = tc.TelemetryChannel;

            appInsightsOptions.EnableAdaptiveSampling = false;
            services.AddApplicationInsightsTelemetry(appInsightsOptions);

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
        }

        /// <summary>
        /// Configures the application request pipeline.
        /// </summary>
        /// <param name="app">The application builder to configure.</param>
        /// <param name="configManager">The configuration manager instance to use.</param>
        /// <param name="loggerFactory">The instance for <see cref="ILoggerFactory"/>.</param>
        /// <param name="secretManger"> The secret manager.</param>
        /// <param name="env"> The hosting environment.</param>
        public void Configure(IApplicationBuilder app, IConfigurationManager configManager, ILoggerFactory loggerFactory, ISecretManager secretManger, IHostingEnvironment env)
        {
            var corsConfig = configManager.Get<CorsSetting>();
            var loggingConfig = configManager.Get<LoggingConfiguration>();
            loggerFactory.AddAppInsightsLoggerProvider(loggingConfig.InstrumentationKey);
            TraceSourceMeta.LoggerFactory = loggerFactory;
            var logger = loggerFactory.CreateLogger<StartupBase>();

            app.UseHealthEndpoints()
               .UseAuthentication()
               .UseCors(builder =>
                    builder.WithOrigins(corsConfig.AccessControlAllowOrigin.Split(','))
                           .WithMethods(corsConfig.AccessControlAllowMethods.Split(','))
                           .WithHeaders(corsConfig.AccessControlAllowHeaders.Split(',')));

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            try
            {
                app.UseFlightsProvider<IFlightsProvider<IDictionary<string, string>>, IDictionary<string, string>>(httpContext =>
                {
                    var context = new Dictionary<string, string>();

                    var hcmPrincipalRetriever = httpContext.RequestServices.GetService<IHCMPrincipalRetriever>();
                    var userPrincipal = hcmPrincipalRetriever?.Principal as IHCMUserPrincipal;

                    if (userPrincipal != null && !string.IsNullOrWhiteSpace(userPrincipal.ObjectId))
                    {
                        context.Add("userid", userPrincipal.ObjectId);
                        context.Add("token", hcmPrincipalRetriever.Principal.Token);
                        context.Add("Userupn", userPrincipal.UserPrincipalName);
                    }
                    else
                    {
                        // This makes the flighting service to return all the flights which are 100% enabled for all users.
                        // Our systems caches these flights to avoid multiple calls to flight service.
                        context.Add("enabled", "1");
                    }
                    return context;
                });
            }
            catch (Exception e)
            {
                logger.LogError(string.Format("Flights provider initialization failed: {0}", e.ToString()));
            }

            app.Use(
                async (httpContext, func) =>
                {
                    var hcmServiceContext = httpContext.RequestServices.GetService(typeof(IHCMServiceContext)) as IHCMServiceContext;
                    var principalRetriever = httpContext.RequestServices.GetService<IHCMPrincipalRetriever>();
                    var appPrincipal = principalRetriever.Principal as IHCMUserPrincipal;
                    var environmentInfo = configManager?.Get<Common.TalentAttract.Configuration.EnvironmentConfiguration>();

                    if (hcmServiceContext != null)
                    {
                        if (!string.IsNullOrEmpty(appPrincipal?.TenantId))
                        {
                            hcmServiceContext.TenantId = appPrincipal.TenantId;
                        }

                        hcmServiceContext.XRMInstanceApiUri = environmentInfo?.XRMInstanceApiUrl;
                        hcmServiceContext.EnvironmentId = environmentInfo?.XRMEnvironment;
                        hcmServiceContext.FalconDatabaseId = environmentInfo?.FalconDatabaseId;
                        hcmServiceContext.FalconOfferContainerId = environmentInfo?.FalconOfferContainerId;
                        hcmServiceContext.RootActivityId = httpContext.Request.Headers["x-ms-root-activity-id"];
                    }

                    await func.Invoke();
                });
            app.UseSwaggerInGTA(this.ServiceDetailsToPublish);
            app.SetTelemetryConfiguration(configManager, this.Configuration);
            app.UseMvc();
        }

        /// <summary>
        /// Configure the authentication options.
        /// </summary>
        /// <param name="options">Jwt Bearer options</param>
        /// <param name="configManager">The configuration manager instance to use.</param>
        private static void SetJwtBearerOptions(JwtBearerOptions options, IConfigurationManager configManager)
        {
            var authenticationConfig = configManager.Get<BearerTokenAuthentication>();

            options.Audience = authenticationConfig.Audience;
            options.Authority = authenticationConfig.Authority;
            options.TokenValidationParameters = new TokenValidationParameters()
            {
                ValidateAudience = true,
                ValidateIssuer = false,
                ValidAudiences = authenticationConfig.ValidAudiencesList
            };
        }
    }
}
