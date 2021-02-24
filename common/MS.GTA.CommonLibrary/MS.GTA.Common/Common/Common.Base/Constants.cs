//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace MS.GTA.Common.Base
{
    /// <summary>
    /// Class to encapsulate all the constants.
    /// </summary>
    public static class Constants
    {
        /// <summary>The Microsoft tenant ID</summary>
        public const string MicrosoftTenantId = "72f988bf-86f1-41af-91ab-2d7cd011db47";

        /// <summary>The dev environment name</summary>
        public const string LocalFabricEnvironmentName = "LOCALFABRIC";

        /// <summary>The dev environment name</summary>
        public const string DevEnvironmentName = "DEV";

        /// <summary>The dev environment name</summary>
        public const string LocalEnvironmentName = "LOCAL";

        /// <summary>The int environment name</summary>
        public const string IntEnvironmentName = "INT";

        /// <summary>The prod environment name</summary>
        public const string ProdEnvironmentName = "PROD";

        /// <summary>The exception errorType field name for serialization</summary>
        public const string ExceptionErrorTypeName = "ErrorType";

        /// <summary>The exception ErrorMessage field name for serialization</summary>
        public const string ExceptionMessageTypeName = "ErrorMessage";

        /// <summary>The "application/json" for headers</summary>
        public const string ApplicationJsonMediaType = "application/json";

        /// <summary>The bearer Authentication scheme </summary>
        public const string BearerAuthenticationScheme = "Bearer";

        /// <summary>The dev master key vault uri</summary>
        public const string DevMasterKeyVaultUri = "https://d365-akv-hcm-master-dev.vault.azure.net/";

        /// <summary>The int master key vault uri</summary>
        public const string IntMasterKeyVaultUri = "https://d365-akv-hcm-master-int.vault.azure.net/";

        /// <summary>The prod master key vault uri</summary>
        public const string ProdMasterKeyVaultUri = "https://d365-akv-hcm-master-prod.vault.azure.net/";

        /// <summary> The environment collection id </summary>
        public const string EnvironmentCollectionId = "EnvironmentsCollection";

        /// <summary> The character that seperates values in a string </summary>
        public const char SplitCharacter = ',';
    }

    public enum ErrorType
    {
        Transient = 0,
        Fatal = 1,
        Benign = 2
    }
}
