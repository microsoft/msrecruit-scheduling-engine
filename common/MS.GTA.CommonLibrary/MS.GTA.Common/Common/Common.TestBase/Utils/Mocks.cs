//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Mocks.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TestBase.Utils
{
    using System.IO;
    using System.Security.Cryptography.X509Certificates;
    using System.Text;

    using Microsoft.AspNetCore.Http;
    using Moq;
    using ServicePlatform.Azure.AAD;
    using ServicePlatform.Azure.Security;
    using ServicePlatform.Configuration;
    using ServicePlatform.Tracing;

    /// <summary>
    /// Utilities for mocking common types
    /// </summary>
    public static class Mocks
    {
        /// <summary>Power App Cert Thumbprint.</summary>
        public const string PowerAppCertThumbprint = "80cea40a62ffa0116fcb40b7b5985adcd3e5ac39";

        /// <summary>Primary Access Key Secret Name.</summary>
        private const string StorageAccountPrimaryAccessKeySecretName = "AppTeamStorageAccountKey1";

        /// <summary>Secondary Access Key Secret Name.</summary>
        private const string StorageAccountSecondaryAccessKeySecretName = "AppTeamStorageAccountKey2";

        /// <summary>Blob Store secret Name.</summary>
        private const string BlobStoreNameSecretName = "AppTeamStorageAccountName";

        /// <summary>BlobStore Container Name.</summary>
        private const string BlobStoreContainerName = "hcm-common-unittests";

        /// <summary>Get mock Trace source.</summary>
        /// <returns>The trace source</returns>
        public static ITraceSource GetMockTraceSource()
        {
            return new Mock<ITraceSource>().Object;
        }

        /// <summary>
        /// Creates the mock file object
        /// </summary>
        /// <param name="fileName">name of the file</param>
        /// <param name="contentType">content type</param>
        /// <param name="fileText">file text</param>
        /// <returns>form file</returns>
        public static IFormFile CreateMockIFormTextFile(string fileName, string contentType, string fileText)
        {
            var stream = GenerateStreamFromString(fileText);
            var formFileMock = new Mock<IFormFile>();
            formFileMock.SetupGet(t => t.ContentType).Returns(contentType);
            formFileMock.SetupGet(t => t.Length).Returns(fileText.Length);
            formFileMock.SetupGet(t => t.FileName).Returns(fileName);
            formFileMock.Setup(t => t.OpenReadStream()).Returns(stream);

            return formFileMock.Object;
        }

        /// <summary>
        /// Generates the stream from string.
        /// </summary>
        /// <param name="inputString">input string</param>
        /// <returns>the stream object.</returns>
        public static Stream GenerateStreamFromString(string inputString)
        {
            var byteArray = Encoding.UTF8.GetBytes(inputString);
            var stream = new MemoryStream(byteArray);
            return stream;
        }

        /// <summary> Gets the ConfigManager for blob storage.</summary>
        /// <param name="usePrimaryKeyDummy">true if the first primary key should be dummy.</param>
        /// <param name="useSecondaryKeyDummy">true if the first secondary key should be dummy.</param>
        /// <param name="keyVaultUri">The key vault uri to use</param>
        /// <returns>IConfigurationManager object</returns>
        public static IConfigurationManager CreateMockConfigManager(bool usePrimaryKeyDummy = false, bool useSecondaryKeyDummy = false, string keyVaultUri = "")
        {
            return CreateConfigManagerMock(usePrimaryKeyDummy, useSecondaryKeyDummy, keyVaultUri).Object;
        }

        private static AzureActiveDirectoryClientConfiguration CreateActiveDirectoryClientConfigurationForDev()
        {
            var cloudTestCertThumbprint = "7FEB08E4D199D2682CA6FBA98D765A784D8979B0";
            var aadClientCertThumbprint = "DCF867B4C16BEAEA860F73124AA3296BC16B60DD";
            var aadIntClientCertThumbprint = "C1C4F0B3E6BCF87BD0D0DB747CEF8E99A1A84996";

            return new AzureActiveDirectoryClientConfiguration
            {
                Authority = @"https://login.windows.net/common",
                ClientId = "66832c40-f4dc-426a-8b91-d70cb1e587d0",
                ClientCertificateThumbprints = $"{aadClientCertThumbprint},{cloudTestCertThumbprint},{aadIntClientCertThumbprint}",
                ClientCertificateStoreLocation = StoreLocation.LocalMachine,
            };
        }

        /// <summary> Gets the ConfigManager for blob storage.</summary>
        /// <param name="usePrimaryKeyDummy">true if the first primary key should be dummy.</param>
        /// <param name="useSecondaryKeyDummy">true if the first secondary key should be dummy.</param>
        /// <param name="keyVaultUri">The key vault uri to use</param>
        /// <returns>IConfigurationManager object</returns>
        public static Mock<IConfigurationManager> CreateConfigManagerMock(bool usePrimaryKeyDummy = false, bool useSecondaryKeyDummy = false, string keyVaultUri = "")
        {
            var configMgr = new Mock<IConfigurationManager>();
            var aadClientConfigSettings = CreateActiveDirectoryClientConfigurationForDev();
            configMgr.Setup(configManager => configManager.Get<AzureActiveDirectoryClientConfiguration>()).Returns(aadClientConfigSettings);

            var targetKeyVaultUri = !string.IsNullOrEmpty(keyVaultUri) ?
                keyVaultUri :
                "https://hcm-akv-j4u-dev-westus2.vault.azure.net/";

            var keyVaultConfiguration = new KeyVaultConfiguration
            {
                KeyVaultUri = targetKeyVaultUri
            };

            configMgr.Setup(configManager => configManager.Get<KeyVaultConfiguration>()).Returns(keyVaultConfiguration);

            return configMgr;
        }

        /// <summary> Gets the ConfigManager for blob storage.</summary>
        /// <returns>IConfigurationManager object</returns>
        internal static IConfigurationManager CreateMockConfigManagerForMasterKeyVault()
        {
            var configMgr = new Mock<IConfigurationManager>();

            var aadClientConfigSettings = new AzureActiveDirectoryClientConfiguration
            {
                Authority = @"https://login.windows.net/common",
                ClientId = "8011eca4-96a3-4aef-9016-8d3e60678449",
                ClientCertificateThumbprints = PowerAppCertThumbprint,
                ClientCertificateStoreLocation = StoreLocation.LocalMachine,
            };

            configMgr.Setup(configManager => configManager.Get<AzureActiveDirectoryClientConfiguration>()).Returns(aadClientConfigSettings);

            var keyVaultConfiguration = new KeyVaultConfiguration
            {
                KeyVaultUri = "https://pa-rts-akv-deploy-int.vault.azure.net/",
            };

            configMgr.Setup(configManager => configManager.Get<KeyVaultConfiguration>()).Returns(keyVaultConfiguration);

            return configMgr.Object;
        }
    }
}
