//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TestBase.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using System.Threading.Tasks;
    using Base.Exceptions;
    using Base.Utilities;
    using CommonDataService.Common.Internal;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using Moq;
    using Newtonsoft.Json;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Azure.Security;
    using ServicePlatform.Communication.Http;
    using ServicePlatform.Exceptions;
    using ServicePlatform.Security;
    using ServicePlatform.Configuration;
    using ServicePlatform.Communication.Http.Extensions;
    using ServicePlatform.Tracing;
    using Microsoft.Extensions.Logging;

    //using VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Key Vault Utilities
    /// </summary>
    public static class KeyVault
    {
        /// <summary>Text File Content Type.</summary>
        public const string TextFileContentType = "application/text";

        /// <summary>The authority</summary>
        public const string Authority = "https://login.windows.net/common";

        /// <summary>The int resource id</summary>
        public const string IntResourceId = "0418c9a8-4454-47e1-98fc-98a7d0e6a01a";
        
        /// <summary>The </summary>
        public const string IntClientId = "0418c9a8-4454-47e1-98fc-98a7d0e6a01a";

        /// <summary>The int resource id</summary>
        public const string ProdResourceId = "5712d3fd-8e22-4040-afbf-70fa18f63627";

        /// <summary>The </summary>
        public const string ProdClientId = "197e6baa-c9ed-4354-a561-e263b71cf20f";

        /// <summary>The test user information secret</summary>
        public const string TestUserInformationSecretKey = "TestUserInformation";

        private static ILogger logger = new Mock<ILogger>().Object;

        /// <summary> Gets the Mock instance of SecretManager for BlobStorage needs.</summary>
        /// <param name="keyVaultUri">The key vault to use, defaults to https://hcm-akv-j4u-dev-westus2.vault.azure.net  if empty</param>
        /// <returns>SecretManager object</returns>
        public static ISecretManager CreateMockSecretManager(string keyVaultUri = "")
        {
            var configManager = Mocks.CreateMockConfigManager(false, false, keyVaultUri);
            var certificateManager = new CertificateManager();
            var client = new AzureActiveDirectoryClient(configManager, certificateManager);
            return new SecretManager(client, configManager);
        }

        /// <summary>
        /// Generates the token using UserNameSecret and UserPasswordSecret
        /// </summary>
        /// <param name="userNameSecrets">UserName Secrets</param>
        /// <param name="passwordSecrets">Password Secrets</param>
        /// <param name="configManager">Configuration Manager</param>
        /// <param name="resourceId">The Resource Id</param>
        /// <param name="clientId">The client Id</param>
        /// <param name="authContextUri">The context URI</param>
        /// <param name="logger">The logger.</param>
        /// <returns>
        /// The tuple containing the username and token
        /// </returns>
        /// <exception cref="AggregateException">Could not successfully generate token using any of the usernamesecrets: {userNameSecrets} and password secrets: {passwordSecrets}</exception>
        public static async Task<Tuple<string, string>> GetUserCredentials(string userNameSecrets, string passwordSecrets, IConfigurationManager configManager, string resourceId, string clientId, string authContextUri, ILogger logger = null)
        {
            Contract.CheckValue(userNameSecrets, nameof(userNameSecrets));
            Contract.CheckNonEmpty(userNameSecrets, nameof(userNameSecrets));
            Contract.CheckValue(passwordSecrets, nameof(passwordSecrets));
            Contract.CheckNonEmpty(passwordSecrets, nameof(passwordSecrets));
            Contract.CheckValue(configManager, nameof(configManager));
            Contract.CheckValue(resourceId, nameof(resourceId));
            Contract.CheckNonEmpty(resourceId, nameof(resourceId));
            Contract.CheckValue(authContextUri, nameof(authContextUri));
            Contract.CheckNonEmpty(authContextUri, nameof(authContextUri));

            if (logger != null)
            {
                KeyVault.logger = logger;
            }

            var certificateManager = new CertificateManager();
            var client = new AzureActiveDirectoryClient(configManager, certificateManager);
            var secretManager = new SecretManager(client, configManager);
            var exceptions = new List<Exception>();

            var usernameSecrets = new List<string>();
            var userpasswordSecrets = new List<string>();

            usernameSecrets.AddRange(userNameSecrets.Split(';').Select(t => t.Trim()));
            userpasswordSecrets.AddRange(passwordSecrets.Split(';').Select(t => t.Trim()));

            for (var i = 0; i < userpasswordSecrets.Count(); i++)
            {
                var userNameSecret = usernameSecrets.Count() == 1 ? usernameSecrets[0] : usernameSecrets[i];
                var passwordSecret = userpasswordSecrets[i];

                KeyVault.logger.LogInformation($"Trying to get the token for {userNameSecret} and {passwordSecret}");
                try
                {
                    var userEmail = await secretManager.GetSecretAsync(userNameSecret, KeyVault.logger);
                    var password = await secretManager.GetSecretAsync(passwordSecret, KeyVault.logger);
                    var token = GetUserToken(userEmail, password, resourceId, clientId, authContextUri);
                    KeyVault.logger.LogInformation($"Acquired the token successfully for {userNameSecret} and {passwordSecret}");
                    return new Tuple<string, string>(userEmail, token);
                }
                catch (KeyVaultAccessException kve)
                {
                    KeyVault.logger.LogInformation($"Runner.GetUserCredentials: {kve}");
                    exceptions.Add(kve);
                }
                catch (AggregateException ae)
                {
                    KeyVault.logger.LogError($"Runner.GetUserCredentials: Error validating credentials {ae}");
                    exceptions.Add(ae);
                }
                catch (Exception e)
                {
                    KeyVault.logger.LogError($"Failed to generate the token for {userNameSecret}");
                    exceptions.Add(e);
                }
            }

            throw new AggregateException($"Could not successfully generate token using any of the usernamesecrets: {userNameSecrets} and password secrets: {passwordSecrets}", exceptions);
        }

        /// <summary>
        /// Generates the token using UserNameSecret and used only for runners
        /// </summary>
        /// <param name="userNameSecrets">UserName Secrets</param>
        /// <param name="configManager">Configuration Manager</param>
        /// <param name="resourceId">The Resource Id</param>
        /// <param name="clientId">The client Id</param>
        /// <param name="authContextUri">The context URI</param>
        /// <param name="logger">The instnce of <see cref="ILogger"/>.</param>
        /// <returns>The tuple containing the username and token</returns>
        public static async Task<Tuple<string, string>> GetCredentials(string userNameSecrets, IConfigurationManager configManager, string resourceId, string clientId, string authContextUri, ILogger logger = null)
        {
            Contract.CheckValue(userNameSecrets, nameof(userNameSecrets));
            Contract.CheckNonEmpty(userNameSecrets, nameof(userNameSecrets));
            Contract.CheckValue(configManager, nameof(configManager));
            Contract.CheckValue(resourceId, nameof(resourceId));
            Contract.CheckNonEmpty(resourceId, nameof(resourceId));
            Contract.CheckValue(authContextUri, nameof(authContextUri));
            Contract.CheckNonEmpty(authContextUri, nameof(authContextUri));

            if (logger != null)
            {
                KeyVault.logger = logger; 
            }

            var certificateManager = new CertificateManager();
            var client = new AzureActiveDirectoryClient(configManager, certificateManager);
            var secretManager = new SecretManager(client, configManager);
            var exceptions = new List<Exception>();

            var testUserNameSecrets = new List<string>();
            testUserNameSecrets.AddRange(userNameSecrets.Split(';').Select(t => t.Trim()));
            var testInformation = await secretManager.GetSecretAsync(TestUserInformationSecretKey, KeyVault.logger);
            var testUserSecrets = JsonConvert.DeserializeObject<TestUserSecrets>(testInformation);
            var testUserNames = new List<string>();

            testUserNames.AddRange(testUserSecrets.TestUsers.Split(' ').Select(t => t.Trim()));
            var testUserPasswords = new List<string>();
            testUserPasswords.AddRange(testUserSecrets.TestUserPasswords.Split(' ').Select(t => t.Trim()));

            foreach (var testUserNameSecret in testUserNameSecrets)
            {
                KeyVault.logger.LogInformation($"Trying to get the token for {testUserNameSecret}");
                try
                {
                    var userEmail = testUserNames.FirstOrDefault(s => s.Contains(testUserNameSecret));
                    var index = testUserNames.FindIndex(testusers => testusers.Contains(testUserNameSecret));
                    var password = string.Empty;
                    if (index != -1)
                    {
                        password = testUserPasswords[index];
                    }
                    else
                    {
                        KeyVault.logger.LogError("Expect user Name to be available in test user information secret");
                        throw new DataValidationException(testUserNameSecret, "Expect user Name to be available in test user information secret");
                    }
                    var token = GetUserToken(userEmail, password, resourceId, clientId, authContextUri);
                    KeyVault.logger.LogInformation($"Acquired the token successfully for {testUserNameSecret}");
                    return new Tuple<string, string>(userEmail, token);
                }
                catch (Exception e)
                {
                    KeyVault.logger.LogError($"Failed to generate the token for {testUserNameSecret} with exception {e}");
                    exceptions.Add(e);
                }

            }

            throw new AggregateException($"Could not successfully generate token using any of the usernamesecrets: {userNameSecrets}", exceptions);
        }

        /// <summary>
        /// Get token 
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>
        ////TODO: Will make Obsolete when we start using GetAuthToken method for acquiring token by using usernamesecretkey
        public static string GetToken(string userEmail, string userPasswordSecret)
        {
            return KeyVault.GetToken(
                userEmail,
                userPasswordSecret,
                ProdResourceId,
                ProdClientId,
                Authority);
        }

        /// <summary>
        /// Get token 
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>
        ////TODO: Will make Obsolete when we start using GetINTAuthToken method for acquiring token by using usernamesecretkey
        public static string GetINTToken(string userEmail, string userPasswordSecret)
        {
            return KeyVault.GetToken(
                userEmail,
                userPasswordSecret,
                IntResourceId,
                IntClientId,
                Authority);
        }

        /// <summary>
        /// Get token 
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>        
        public static string GetMsIntToken(string userEmail, string userPasswordSecret)
        {
            return KeyVault.GetToken(
                userEmail,
                userPasswordSecret,
                IntResourceId,
                IntClientId,
                Authority);
        }

        /// <summary>
        /// Get B2C token 
        /// </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>
        ////TODO: Will make Obsolete when we start using GetB2CAuthToken method for acquiring token by using usernamesecretkey
        public static string GetB2CToken(string userEmail, string userPasswordSecret)
        {
            return KeyVault.GetToken(
                userEmail,
                userPasswordSecret,
                "b19b192e-7e40-4478-902f-85bc18f7275e",
                "39d9623a-50e5-4138-ba76-90fde73a5353",
                "https://login.microsoftonline.com/b8668f07-c6f2-44d5-b4b1-cd6f7ca3f636/");
        }

        /// <summary>
        /// Get token 
        /// </summary>
        /// <param name="userNameSecret">User Name Secret Key</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>
        public static string GetAuthToken(string userNameSecret, string userPasswordSecret)
        {
            return KeyVault.GetAuthToken(
                userNameSecret,
                userPasswordSecret,
                ProdResourceId,
                ProdClientId,
                Authority);
        }

        /// <summary>
        /// Get token 
        /// </summary>
        /// <param name="userNameSecret">User Name Secret Key</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>
        public static string GetINTAuthToken(string userNameSecret, string userPasswordSecret)
        {
            return KeyVault.GetAuthToken(
                userNameSecret,
                userPasswordSecret,
                IntResourceId,
                IntClientId,
                Authority);
        }

        /// <summary>
        /// Get B2C token 
        /// </summary>
        /// <param name="userNameSecret">User Name Secret Key</param>
        /// <param name="userPasswordSecret"> User Password Secret key></param>
        /// <returns>User token</returns>
        public static string GetB2CAuthToken(string userNameSecret, string userPasswordSecret)
        {
            return KeyVault.GetAuthToken(
                userNameSecret,
                userPasswordSecret,
                "b19b192e-7e40-4478-902f-85bc18f7275e",
                "39d9623a-50e5-4138-ba76-90fde73a5353",
                "https://login.microsoftonline.com/b8668f07-c6f2-44d5-b4b1-cd6f7ca3f636/");
        }

        /// <summary>Get token </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="userPasswordSecret">User Password Secret key&gt;</param>
        /// <param name="resourceId">The resource Id.</param>
        /// <param name="clientId">The client Id.</param>
        /// <param name="authContextUri">The Context URI.</param>
        /// <returns>User token</returns>
        ////TODO: Will make Obsolete when we start using GetAuthToken method for acquiring token by using usernamesecretkey
        public static string GetToken(string userEmail, string userPasswordSecret, string resourceId, string clientId, string authContextUri)
        {
            var secretManager = CreateMockSecretManager();
            var password = secretManager.ReadSecretAsync(userPasswordSecret).Result.Value;
            return GetUserToken(userEmail, password, resourceId, clientId, authContextUri);
        }

        /// <summary>Get token </summary>
        /// <param name="userNameSecret">User Name Secret Key</param>
        /// <param name="userPasswordSecret">User Password Secret key&gt;</param>
        /// <param name="resourceId">The resource Id.</param>
        /// <param name="clientId">The client Id.</param>
        /// <param name="authContextUri">The Context URI.</param>
        /// <returns>User token</returns>
        public static string GetAuthToken(string userNameSecret, string userPasswordSecret, string resourceId, string clientId, string authContextUri)
        {
            var secretManager = CreateMockSecretManager();
            var userEmail = secretManager.ReadSecretAsync(userNameSecret).Result.Value;
            var password = secretManager.ReadSecretAsync(userPasswordSecret).Result.Value;
            return GetUserToken(userEmail, password, resourceId, clientId, authContextUri);
        }

        /// <summary>Get token </summary>
        /// <param name="userEmail">User email</param>
        /// <param name="password">user password</param>
        /// <param name="resourceId">The resource Id.</param>
        /// <param name="clientId">The client Id.</param>
        /// <param name="authContextUri">The Context URI.</param>
        /// <returns>User token</returns>
        public static string GetUserToken(string userEmail, string password, string resourceId, string clientId, string authContextUri)
        {
            // Since we have no SSL certs on our endpoints right now.
            System.Net.ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            var authContext = new AuthenticationContext(authContextUri);
            var token = authContext.AcquireTokenAsync(new HttpCommunicationClientFactory(), authContextUri, userEmail, password, resourceId, clientId).Result;
            // TODO validate
            // Assert.IsNotNull(token, "Token cannot be null. it is requred to make service call");
            return token;
        }

        /// <summary> Gets the Mock instance of SecretManager for BlobStorage needs.</summary>
        /// <returns>SecretManager object</returns>
        internal static ISecretManager CreateMockSecretManagerForMasterKeyVault()
        {
            var configManager = Mocks.CreateMockConfigManagerForMasterKeyVault();
            var certificateManager = new CertificateManager();
            var client = new AzureActiveDirectoryClient(configManager, certificateManager);
            return new SecretManager(client, configManager);
        }
    }
}
