//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Providers
{
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.DataProtection;
    using Microsoft.Extensions.Caching.Distributed;
    using Microsoft.Extensions.Logging;
    using Microsoft.IdentityModel.Clients.ActiveDirectory;
    using HR.TA.CommonDataService.Common.Internal;

    /// <summary>
    /// Implementation of Token service class.
    /// </summary>
    public class TokenCacheService : ITokenCacheService
    {
        private readonly ILogger<TokenCacheService> logger;
        private readonly IDataProtectionProvider dataProtectionProvider;
        private readonly IDistributedCache distributedCache;
        private TokenCache cache = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="TokenCacheService"/> class.
        /// </summary>
        /// <param name="distributedCache">An implementation of <see cref="Microsoft.Extensions.Caching.Distributed.IDistributedCache"/> in which to store the access tokens.</param>
        /// <param name="logger">The instance for <see cref="ILogger{TokenCacheService}"/>.</param>
        /// <param name="dataProtectionProvider">An <see cref="Microsoft.AspNetCore.DataProtection.IDataProtectionProvider"/> for creating a data protector.</param>
        public TokenCacheService(IDistributedCache distributedCache, IDataProtectionProvider dataProtectionProvider, ILogger<TokenCacheService> logger)
        {
            Contract.CheckValue(distributedCache, nameof(distributedCache));
            Contract.CheckValue(dataProtectionProvider, nameof(dataProtectionProvider));
            Contract.CheckValue(logger, nameof(logger));

            this.distributedCache = distributedCache;
            this.dataProtectionProvider = dataProtectionProvider;
            this.logger = logger;
        }

        /// <summary>
        /// Clears the token cache.
        /// </summary>
        /// <param name="cacheKey">Cache key to clear.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task ClearCacheAsync(string cacheKey)
        {
            var cache = await this.GetCacheAsync(cacheKey);
            cache.Clear();
        }

        /// <summary>
        /// Returns an instance of <see cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache"/>.
        /// </summary>
        /// <param name="cacheKey">Cache key to Get</param>
        /// <returns>An instance of <see cref="Microsoft.IdentityModel.Clients.ActiveDirectory.TokenCache"/>.</returns>
        public Task<TokenCache> GetCacheAsync(string cacheKey)
        {
            this.logger.LogTrace($"Invoking method {nameof(this.GetCacheAsync)} for cache key: {cacheKey}");
            if (this.cache == null)
            {
                this.logger.LogTrace($"Cache is found to be null.");
            }
            else
            {
                this.logger.LogTrace($"Cache is not null. {this.cache}. Cache key count: {this.cache.Count}");
            }

            this.cache = new DistributedTokenCache(cacheKey, this.distributedCache, this.dataProtectionProvider, this.logger);

            return Task.FromResult(this.cache);
        }
    }
}
