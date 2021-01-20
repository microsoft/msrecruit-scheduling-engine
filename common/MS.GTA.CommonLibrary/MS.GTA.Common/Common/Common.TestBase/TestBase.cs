// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TestBaseV2.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace MS.GTA.Common.TestBase
{
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Net.Http;
    using System.Reflection;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Base.Configuration;
    using Base.Security.V2;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Tokens;
    using MS.GTA.Common.Web.Configuration;
    using Moq;
    using Newtonsoft.Json;
    using ServicePlatform.AspNetCore.Security;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Azure.Security;
    using ServicePlatform.Communication.Http;
    using ServicePlatform.Communication.Http.Extensions;
    using ServicePlatform.Configuration;
    using ServicePlatform.Security;
    using ServicePlatform.Tracing;
    using Utils;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Web.S2SHandler.V2;
    using AuthenticationResult = Microsoft.IdentityModel.Clients.ActiveDirectory.AuthenticationResult;

    /// <summary>The base class for Integration Tests.</summary>
    public abstract class TestBase
    {
        /// <summary>Gets or sets the test context.</summary>
        public TestContext TestContext { get; set; }

        /// <summary>Gets or sets the cluster URL.</summary>
        public static string ClusterUri { get; set; }

        /// <summary>Trace source.</summary>
        protected ITraceSource TraceSource => Utils.Mocks.GetMockTraceSource();

        /// <summary>The test run settings.</summary>
        protected TestRunSettings TestRunSettings => new TestRunSettings(this.TestContext);

        /// <summary>The audience.</summary>
        private string Audience => "bd613935-9d03-4e7f-a037-0cdd256a0155";

        /// <summary>The authority.</summary>
        private string Authority => @"https://talent.dynamics.com/authority";

        private readonly bool useMocks;

        public TestBase(bool useMocks = true)
        {
            this.useMocks = useMocks;
        }

        public TalentMocks TalentMocks { get; set; } = new TalentMocks();

        public IConfigurationManager ConfigurationManager { get; set; }

        public ILogger<TestBase> Logger { get; set; }

        public ILoggerFactory LoggerFactory { get; set; }

        public ServiceCollection ServiceCollection { get; set; } = new ServiceCollection();

        public ServiceProvider ServiceProvider { get; set; }

        public HttpContext HttpContext { get; set; } = new DefaultHttpContext();

        [TestInitialize]
        public void BaseSetup()
        {
            if (this.useMocks)
            {
                this.TalentMocks.ConfigurationManager
                    .Setup(m => m.Get<AzureActiveDirectoryClientConfiguration>())
                    .Returns(new AzureActiveDirectoryClientConfiguration
                    {
                        Authority = "authority",
                        ClientId = "clientId",
                        ClientCertificateThumbprints = "thumbprints"
                    });
                this.TalentMocks.ConfigurationManager
                    .Setup(m => m.Get<AADClientConfiguration>())
                    .Returns(new AADClientConfiguration
                    {
                        AADInstance = "authority",
                        ClientId = "clientId",
                        ClientCertificateThumbprints = "thumbprints",
                    });
                this.TalentMocks.ConfigurationManager
                    .Setup(m => m.Get<EnvironmentNameConfiguration>())
                    .Returns(new EnvironmentNameConfiguration() { EnvironmentName = "Dev" });
                this.TalentMocks.ConfigurationManager
                    .Setup(m => m.Get<S2SHandlerConfiguration>())
                    .Returns(new S2SHandlerConfiguration() { });

                this.ConfigurationManager = this.TalentMocks.ConfigurationManager.Object;
            }
            else
            {
                if (this.TestContext != null && !ConfigurationHelper.IsInitialized)
                {
                    var configurationSource = new TestContextConfigurationSource(this.TestContext);
                    ConfigurationHelper.InitalizeConfigurationHelperWithOverride(null, new [] { configurationSource });
                }
                
                this.ConfigurationManager = ConfigurationHelper.Instance.ConfigurationManager;
            }
            
            var httpContextAccessorMock = new Moq.Mock<IHttpContextAccessor>();

            this.ServiceCollection
                .AddSingleton<IConfigurationManager>(s => this.ConfigurationManager)
                .AddSingleton<IHttpContextAccessor>(s => httpContextAccessorMock.Object);

            if (this.useMocks)
            {
                this.TalentMocks.mockHttpCommunicationClientFactory
                    .Setup(m => m.Create())
                    .Returns(this.TalentMocks.mockHttpCommunicationClient.Object);
                this.TalentMocks.mockHttpCommunicationClientFactory
                    .Setup(m => m.Create(It.IsAny<HttpCommunicationClientOptions>(), It.IsAny<HttpMessageHandler>()))
                    .Returns(this.TalentMocks.mockHttpCommunicationClient.Object);
                
                this.ServiceCollection
                    .AddSingleton<IHttpCommunicationClientFactory>(sp => this.TalentMocks.mockHttpCommunicationClientFactory.Object)
                    // TODO
                    // .AddSingleton<IS2SHandler>(sp => this.TalentMocks.S2SHandler.Object)
                    .AddSingleton<IAzureActiveDirectoryClient>(sp => this.TalentMocks.AzureActiveDirectoryClient.Object);
            }
            else
            {
                this.ServiceCollection
                    .AddSingleton<ISecretManager, SecretManager>()
                    // TODO
                    // .AddSingleton<IS2SHandler, S2SHandler>()
                    .AddSingleton<IAzureActiveDirectoryClient, AzureActiveDirectoryClient>()
                    .AddHttpCommunicationClientFactory();
            }

            this.ServiceCollection
                .AddPrincipalRetriever()
                .AddCertificateManager();

            this.ServiceCollection
                .AddLogging(lb =>
                {
                    lb.AddConsole();
                    lb.AddProvider(new TestContextLogProvider(this.TestContext));
                });
            this.ServiceCollection.AddSingleton<ILogger>(s => s.GetService<ILogger<TestBase>>());

            this.ServiceProvider = this.ServiceCollection.BuildServiceProvider();

            this.HttpContext.RequestServices = this.ServiceProvider;
            httpContextAccessorMock
                .Setup(m => m.HttpContext)
                .Returns(this.HttpContext);
            
            this.LoggerFactory = this.ServiceProvider.GetRequiredService<ILoggerFactory>();
            this.Logger = this.ServiceProvider.GetRequiredService<ILogger<TestBase>>();
            TraceSourceMeta.LoggerFactory = this.LoggerFactory;
        }
        
        /// <summary>The get token.</summary>
        /// <param name="testUserEmail">The test user email.</param>
        /// <param name="testUserKeyVaultName">The test user key vault name.</param>
        /// <returns>The <see cref="string"/>.</returns>
        public string GetToken(string testUserEmail, string testUserKeyVaultName)
        {
            var certificateManager = new CertificateManager();
            var mockConfigManager = this.CreateMockConfigurationManager().Object;
            var aadClient = new AzureActiveDirectoryClient(mockConfigManager, certificateManager);
            var secretManager = new SecretManager(aadClient, mockConfigManager);

            var username = testUserEmail;
            var password = secretManager.ReadSecretAsync(testUserKeyVaultName).Result.Value;

            var accessToken = KeyVault.GetUserToken(
                username,
                password,
                KeyVault.IntResourceId,
                KeyVault.IntClientId,
                KeyVault.Authority);
            Assert.IsNotNull(accessToken, "Token cannot be null. it is required to make service call");
            return accessToken;
        }

        /// <summary>Gets a B2C test token.</summary>
        /// <param name="testUserEmail">The email of the test user.</param>
        /// <param name="testUserObjectId">The object ID of the test user.</param>
        /// <param name="testUserPasswordSecretName">The secret name for the password of the test user.</param>
        /// <returns>The B2C test token.</returns>
        public string GetB2CTestToken(string testUserEmail, string testUserObjectId, string testUserPasswordSecretName)
        {
            var certificateManager = new CertificateManager();
            var mockConfigManager = this.CreateMockConfigurationManager().Object;
            var aadClient = new AzureActiveDirectoryClient(mockConfigManager, certificateManager);
            var secretManager = new SecretManager(aadClient, mockConfigManager);

            var username = testUserEmail;
            var password = secretManager.ReadSecretAsync(testUserPasswordSecretName).Result.Value;
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(password));

            var now = DateTime.UtcNow;

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, username),
                new Claim("emails", testUserEmail),
                new Claim("oid", testUserObjectId),
                new Claim("idp", "testIdentityProvider"),
                new Claim("tfp", "testIdentityProvider"),
                new Claim(JwtRegisteredClaimNames.Iat, new DateTimeOffset(now).ToUniversalTime().ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
            };

            var jwt = new JwtSecurityToken(
                issuer: this.Authority,
                audience: this.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(TimeSpan.FromMinutes(5)),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        /// <summary>The get email and access token tuple.</summary>
        /// <param name="testUserSecretNames">The test user secret names.</param>
        /// <param name="testUserPasswordSecretNames">The test user password secret names.</param>
        /// <returns>The email and acess toke tuple.</returns>
        public async Task<Tuple<string, string>> GetEmailAndAccessTokenTupleAsync(string testUserSecretNames, string testUserPasswordSecretNames)
        {
            var certificateManager = new CertificateManager();
            var mockConfigManager = this.CreateMockConfigurationManager().Object;
            var aadClient = new AzureActiveDirectoryClient(mockConfigManager, certificateManager);
            var secretManager = new SecretManager(aadClient, mockConfigManager);

            var userEmailAndAccessTokenTuple = await KeyVault.GetUserCredentials(
                testUserSecretNames,
                testUserPasswordSecretNames,
                mockConfigManager,
                KeyVault.IntResourceId,
                KeyVault.IntClientId,
                KeyVault.Authority);
            Assert.IsNotNull(userEmailAndAccessTokenTuple, "Email and Access Token are not null. they are required to make service call");
            return userEmailAndAccessTokenTuple;
        }

        /// <summary>The create mock config manager.</summary>
        /// <returns>The <see cref="IConfigurationManager"/>.</returns>
        public Mock<IConfigurationManager> CreateMockConfigurationManager()
        {
            var mock = new Mock<IConfigurationManager>();
            var clientConfiguration1 = new AzureActiveDirectoryClientConfiguration
            {
                Authority = "https://login.windows.net/common",
                ClientId = "bd613935-9d03-4e7f-a037-0cdd256a0155",
                ClientCertificateThumbprints = "5CA62DE9DAF34E51896885285941D885BAD4766E",
                ClientCertificateStoreLocation = StoreLocation.CurrentUser
            };
            mock.Setup(configManager => configManager.Get<AzureActiveDirectoryClientConfiguration>()).Returns(clientConfiguration1);
            var vaultConfiguration = new KeyVaultConfiguration { KeyVaultUri = "https://hrgtakeyvault-dev.vault.azure.net/" };
            mock.Setup(configManager => configManager.Get<KeyVaultConfiguration>()).Returns(vaultConfiguration);
            return mock;
        }

        protected async Task<string> GetToken(string testUserUsernameSecretName, string testUserPasswordSecretName, string resource = null)
        {
            var clientId = this.ConfigurationManager.Get<AzureActiveDirectoryClientConfiguration>().ClientId;
            
            if (resource == null)
            {
                resource = clientId;
            }
            
            var secretManager = this.ServiceProvider.GetRequiredService<ISecretManager>();

            var usernameResult = await secretManager.TryGetSecretAsync(testUserUsernameSecretName);
            var passwordResult = await secretManager.TryGetSecretAsync(testUserPasswordSecretName);

            if (!usernameResult.Succeeded)
            {
                throw new Exception($"Unable to get username secret: {usernameResult.Exception}");
            }
            
            if (!passwordResult.Succeeded)
            {
                throw new Exception($"Unable to get username secret: {passwordResult.Exception}");
            }

            var username = usernameResult?.Result?.Value;
            var password = passwordResult?.Result?.Value;

            var token = await AuthenticationContextExtensions.AcquireTokenAsync(
                this.LoggerFactory,
                this.ServiceProvider.GetRequiredService<IHttpCommunicationClientFactory>(),
                "https://login.windows.net/common",
                username,
                password,
                resource,
                KeyVault.IntClientId);

            return token;
        }
    }
    
    public class TalentMocks
    {
        public Mock<IHttpCommunicationClient> mockHttpCommunicationClient { get; set;} = new Mock<IHttpCommunicationClient>();
        
        public Mock<IHttpCommunicationClientFactory> mockHttpCommunicationClientFactory { get; set;} = new Mock<IHttpCommunicationClientFactory>();
        
        public Mock<IConfigurationManager> ConfigurationManager { get; set; } = new Mock<IConfigurationManager>();

        // TODO
        // public Mock<IS2SHandler> S2SHandler { get; set; } = new Mock<IS2SHandler>();

        public Mock<IAzureActiveDirectoryClient> AzureActiveDirectoryClient { get; set; } =
            new Mock<IAzureActiveDirectoryClient>();
    }

    public static class TestHelperMethods
    {
        public static AuthenticationResult GetAuthenticationResult(string token)
        {
            return Activator.CreateInstance(
                typeof(AuthenticationResult), 
                BindingFlags.NonPublic | BindingFlags.Instance, 
                null, 
                new object[] { "Bearer", token, DateTimeOffset.MaxValue }, null) as AuthenticationResult;
        }
        
        public static HttpContent ToJsonStringContent(this object obj)
        {
            var json = JsonConvert.SerializeObject(obj);
            return new StringContent(json, Encoding.UTF8, "application/json");
        }
    }
}