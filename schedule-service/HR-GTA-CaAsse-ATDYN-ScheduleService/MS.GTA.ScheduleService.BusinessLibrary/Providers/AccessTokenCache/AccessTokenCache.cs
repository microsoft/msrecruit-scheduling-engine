// <copyright file="AccessTokenCache.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Caching.Memory;
    using Microsoft.Extensions.Logging;
    using MS.GTA.ScheduleService.BusinessLibrary.Utils;

    /// <summary>
    /// Access token cache. It only saves the non empty token
    /// </summary>
    public class AccessTokenCache : IAccessTokenCache
    {
        private const int ExpirationBufferInMinutes = 10;
        private const int MaxCacheTimeInMinutes = 50;

        /// <summary>
        /// access token cache
        /// </summary>
        private readonly MemoryCache accessTokenCache;

        /// <summary>
        /// The instance for <see cref="ILogger{AccessTokenCache}"/>
        /// </summary>
        private readonly ILogger<AccessTokenCache> logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccessTokenCache"/> class
        /// </summary>
        /// <param name="logger">The instance for <see cref="ILogger{AccessTokenCache}"/>.</param>
        public AccessTokenCache(ILogger<AccessTokenCache> logger)
        {
            this.logger = logger;
            var memoryCacheOptions = new MemoryCacheOptions();
            this.accessTokenCache = new MemoryCache(memoryCacheOptions);
        }

        /// <inheritdoc />
        public async Task<string> GetOrAddAccessTokenAsync(string userEmail, Func<Task<string>> populatingFunction, string environmentId = null)
        {
            var cachekey = FormattableString.Invariant($"{environmentId}-{userEmail}");
            var cachedToken = (string)this.accessTokenCache.Get(cachekey);

           this.logger.LogInformation($"AccessTokenCache: current cache size is {this.accessTokenCache.Count}");

            if (!string.IsNullOrEmpty(cachedToken))
            {
               this.logger.LogInformation("AccessTokenCache: access token is found in the cache. Return it.");
                return cachedToken;
            }

           this.logger.LogInformation("AccessTokenCache: access token is not found in the cache. Execute function to generate token.");
            var token = await populatingFunction();

            if (!string.IsNullOrEmpty(token))
            {
               this.logger.LogInformation("AccessTokenCache: access token is generated");
                var expirationDateTime = AccessToken.GetExpirationDateTimeFromToken(token);
               this.logger.LogInformation($"AccessTokenCache: absolute expiration date time is {expirationDateTime}");

                var absoluteExpiration = expirationDateTime.AddMinutes(-ExpirationBufferInMinutes);
                if (absoluteExpiration > DateTime.UtcNow)
                {
                    var validTimeSpan = absoluteExpiration - DateTime.UtcNow;
                    var maxCacheTimeSpan = TimeSpan.FromMinutes(MaxCacheTimeInMinutes);
                    var cacheTimeSpan = validTimeSpan > maxCacheTimeSpan ? maxCacheTimeSpan : validTimeSpan;

                   this.logger.LogInformation($"AccessTokenCache: access token is cached for {cacheTimeSpan}");

                    this.accessTokenCache.Set(cachekey, token, cacheTimeSpan);
                }
            }
            else
            {
               this.logger.LogInformation("AccessTokenCache: generated access token is empty");
            }

            return token;
        }
    }
}
