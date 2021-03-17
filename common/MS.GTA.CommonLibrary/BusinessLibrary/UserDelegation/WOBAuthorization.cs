//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n


namespace Common.BusinessLibrary.UserDelegation
{
    using Microsoft.AspNetCore.Http;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;
    using System.Linq;
    using Microsoft.Extensions.Caching.Distributed;
    using Common.Common.Routing.Client;
    using Common.Common.Base.Configuration;
    using Common.Common.Base.ServiceContext;
    using Common.Common.Provisioning.Entities.FalconEntities.Attract;
    using Common.TalentEntities.Enum;
    using Common.ServicePlatform.Tracing;
    using Common.ServicePlatform.Exceptions;
    using Common.Common.TalentEntities.Common;

    /// <summary>
    /// This is for the authorization of WOB request
    /// </summary>
    public class WOBAuthorization
    {
        /// <summary>The logger.</summary>
        /// <summary>
        /// Request delegate
        /// </summary>
        private readonly RequestDelegate _next;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="next"></param>
        public WOBAuthorization(RequestDelegate next)
        {
            _next = next;
        }
        /// <summary>
        /// This is for authorizing the request 
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext httpContext)
        {
            var trace = httpContext.RequestServices.GetService(typeof(ITraceSource)) as ITraceSource;
            var hcmServiceContext = httpContext.RequestServices.GetService(typeof(IHCMServiceContext)) as IHCMServiceContext;
            var onBehalfUserId = hcmServiceContext.WorkOnBehalfUserId;
            if (string.IsNullOrEmpty(onBehalfUserId))
            {
                hcmServiceContext.UserId = hcmServiceContext.ObjectId;
            }
            else
            {
                if (await this.ValidateUserAsync(httpContext, onBehalfUserId, hcmServiceContext.ObjectId))
                {
                    hcmServiceContext.UserId = onBehalfUserId;
                    var client = await GetFalconQueryClientAsync(httpContext).ConfigureAwait(false);
                    hcmServiceContext.Email = (await client.Get<Worker> (worker => worker.OfficeGraphIdentifier == onBehalfUserId).ConfigureAwait(false))
                        .Select(worker => worker.EmailPrimary).FirstOrDefault();
                } 
                else
                {
                    // Log that the authorization check does not allow caller to perform delegation. 
                    trace.TraceInformation($"WOB Auth middleware: No active delegation exists for User {hcmServiceContext.ObjectId} on {onBehalfUserId}. Hence using the caller context itself.");
                    
                    // Proceed with the caller context itself.
                    hcmServiceContext.UserId = hcmServiceContext.ObjectId;
                }
            }
            await _next(httpContext);
        }

        /// <summary>
        /// This method checks whether the caller is allowed to perform delegation on given user id.
        /// </summary>
        /// <param name="httpContext"></param>
        /// <param name="onBehalfUserId"></param>
        /// <param name="currentUserId"></param>
        /// <returns>True if the caller is authorized to delegate on behalf of given user id, False otherwise.</returns>
        private async Task<bool> ValidateUserAsync(HttpContext httpContext, string onBehalfUserId, string currentUserId)
        {
            var redisCache = httpContext.RequestServices.GetService(typeof(IDistributedCache)) as IDistributedCache;
            var cacheKey = currentUserId + "-wob";
            var authorizedWobUserIdsAsBytes = await redisCache.GetAsync(cacheKey).ConfigureAwait(false);
            List<string> authorizedOnBehalfUserIds;

            if (authorizedWobUserIdsAsBytes != null)
            {
                var authorizedWobUserIdsAsString = Encoding.UTF8.GetString(authorizedWobUserIdsAsBytes);
                authorizedOnBehalfUserIds = authorizedWobUserIdsAsString.Split(new char[] { ',' }).ToList();
            }
            else
            {
                var client = await GetFalconQueryClientAsync(httpContext).ConfigureAwait(false);
                authorizedOnBehalfUserIds = (await client.Get<Delegation>(
                   dg => dg.To.OfficeGraphIdentifier == currentUserId
                   && dg.DelegationStatus == DelegationStatus.Active
                   && dg.RequestStatus == RequestStatus.Active))
               ?.ToList().Select(dg => dg.From.OfficeGraphIdentifier)?.ToList();
                if (authorizedOnBehalfUserIds.Count > 0)
                {
                    var authorizedOnBehalfUserIdsString = string.Join(",", authorizedOnBehalfUserIds);
                    var wobEnvInfo = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<WOBEnvironmentConfiguration>();
                    var options = new DistributedCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromMinutes(Convert.ToDouble(wobEnvInfo?.CacheRefreshTimeInMins)));
                    byte[] encodedAuthorizedOnBehalfUserIdsString = Encoding.UTF8.GetBytes(authorizedOnBehalfUserIdsString);
                    await redisCache.SetAsync(cacheKey, encodedAuthorizedOnBehalfUserIdsString, options);
                }
            }
            if (authorizedOnBehalfUserIds.Contains(onBehalfUserId))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Gets falcon query client
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns>Client</returns>
        private async Task<Common.DocumentDB.IHcmDocumentClient> GetFalconQueryClientAsync(HttpContext httpContext)
        {
            var falconClient = httpContext.RequestServices.GetService(typeof(IFalconQueryClient)) as IFalconQueryClient;
            var environmentInfo = FabricXmlConfigurationHelper.Instance.ConfigurationManager.Get<Common.TalentAttract.Configuration.EnvironmentConfiguration>();
            var client = await falconClient.GetFalconClient(environmentInfo?.FalconCommonContainerId, environmentInfo?.FalconDatabaseId).ConfigureAwait(false);
            return client;
        }
    }
}
