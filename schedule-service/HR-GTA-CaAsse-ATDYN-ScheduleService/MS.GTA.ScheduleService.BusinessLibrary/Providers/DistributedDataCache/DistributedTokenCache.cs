// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="DistributedTokenCache.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------
namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using MS.GTA.CommonDataService.Common.Internal;

    /// <summary>
    /// Implementation of Distributed cache
    /// </summary>
    public class DistributedTokenCache : TokenCache
    {
        private const string ServiceName = "Scheduler";
        private readonly ILogger<TokenCacheService> logger;
        private readonly IDistributedCache distributedCache;
        private readonly IDataProtector protector;
        private readonly string cacheKey;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedTokenCache"/> class.
        /// </summary>
        /// <param name="cacheKey">Key to cache</param>
        /// <param name="distributedCache">An implementation of <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/> in which to store the access tokens.</param>
        /// <param name="logger">The instance for <see cref="ILogger{TokenCacheService}"/>.</param>
        /// <param name="dataProtectionProvider">An <see cref="Microsoft.AspNetCore.DataProtection.IDataProtectionProvider"/> for creating a data protector.</param>
        public DistributedTokenCache(
            string cacheKey,
            IDistributedCache distributedCache,
            IDataProtectionProvider dataProtectionProvider,
            ILogger<TokenCacheService> logger)
            : base()
        {
            Contract.CheckValue(cacheKey, nameof(cacheKey));
            Contract.CheckValue(distributedCache, nameof(distributedCache));
            Contract.CheckValue(dataProtectionProvider, nameof(dataProtectionProvider));
            Contract.CheckValue(logger, nameof(logger));

            this.cacheKey = $"{ServiceName}-{cacheKey}";
            this.distributedCache = distributedCache;
            this.logger = logger;
            this.protector = dataProtectionProvider.CreateProtector(typeof(DistributedTokenCache).FullName);
            this.AfterAccess = this.AfterAccessNotification;
            this.LoadFromCache();
        }

        /// <summary>
        /// Handles the AfterAccessNotification event, which is triggered right after ADAL accesses the cache.
        /// </summary>
        /// <param name="args">An instance of <see cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCacheNotificationArgs"/> containing information for this event.</param>
        public void AfterAccessNotification(TokenCacheNotificationArgs args)
        {
            if (this.HasStateChanged)
            {
                if (this.Count > 0)
                {
                    this.distributedCache.Set(
                        this.cacheKey,
                        this.protector.Protect(this.SerializeAdalV3()),
                        new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1) });

                    this.logger.LogTrace($"{nameof(this.AfterAccessNotification)} - Token states changed for Client: {args.ClientId} User: {args.UniqueId}  Resource: {args.Resource} writing all tokens back to store for cache key {this.cacheKey}");
                }
                else
                {
                    // There are no tokens for this user/client, so remove them from the cache.
                    // This was previously handled in an overridden Clear() method, but the built-in Clear() calls this
                    // after the dictionary is cleared.
                    this.distributedCache.Remove(this.cacheKey);
                    this.logger.LogInformation("Cleared token cache for: {0}", this.cacheKey ?? "<none>");
                }

                this.HasStateChanged = false;
            }
        }

        /// <summary>
        /// Attempts to load tokens from distributed cache.
        /// </summary>
        private void LoadFromCache()
        {
            this.logger.LogTrace($"{nameof(this.LoadFromCache)} - Retrieve token for key: {this.cacheKey}");
            byte[] cacheData = this.distributedCache.Get(this.cacheKey);
            if (cacheData != null)
            {
                try
                {
                    this.DeserializeAdalV3(this.protector.Unprotect(cacheData));
                    this.logger.LogTrace("Retrieved all tokens from store for Key: {0}", this.cacheKey);
                }
                catch (Exception e)
                {
                    this.logger.LogError(e, $"Exception occured while reading from redis for key {this.cacheKey}");
                    this.DeserializeAdalV3(null);
                }
            }
        }
    }
}
