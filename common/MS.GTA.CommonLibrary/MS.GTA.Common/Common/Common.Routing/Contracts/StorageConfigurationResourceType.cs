//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="StorageConfigurationResourceType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.Contracts
{
    /// <summary>
    /// A storage configuration resource type.
    /// </summary>
    public static class StorageConfigurationResourceType
    {
        /// <summary>
        /// Gets the document DB resource.
        /// </summary>
        public static string DocumentDb => nameof(DocumentDb);

        /// <summary>
        /// Gets the REDIS cache resource.
        /// </summary>
        public static string RedisCache => nameof(RedisCache);

        /// <summary>
        /// Gets the Blob resource.
        /// </summary>
        public static string BlobStorage => nameof(BlobStorage);

        /// <summary>
        /// Gets the sql DB resource.
        /// </summary>
        public static string SqlDb => nameof(SqlDb);

        /// <summary>
        /// Gets the Storage Account resource.
        /// </summary>
        public static string StorageAccount => nameof(StorageAccount);
    }
}