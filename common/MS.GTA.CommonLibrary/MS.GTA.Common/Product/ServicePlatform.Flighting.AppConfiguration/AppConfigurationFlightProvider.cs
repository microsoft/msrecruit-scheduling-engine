//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="AppConfigurationFlightProvider.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.Rest;
using MS.GTA.Common.Base.Configuration;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Azure.Security;
using MS.GTA.ServicePlatform.Configuration;
using MS.GTA.ServicePlatform.Flighting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MS.GTA.Common.Product.ServicePlatform.Flighting.AppConfiguration
{
    /// <summary>
    /// A flights provider implementation of the Azure App Configuration.
    /// </summary>
    /// <remarks>
    /// The class is meant to be consumed as a singleton service.
    public class AppConfigurationFlightProvider : FlightsProvider<IDictionary<string, string>>
    {
        private readonly AppConfigurationFlightConfiguration configuration;
        private readonly AADClientConfiguration aadConfig;
        private readonly HttpClient httpClient;
        private readonly ILogger<AppConfigurationFlightProvider> logger;
        private readonly ISecretManager secretManager;

        /// <summary>
        /// Creates a new instance of <see cref="AppConfigurationFlightProvider"/> given an implementation of
        /// <see cref="IConfigurationManager"/>. The provided instance needs
        /// to be able to supply the <see cref="AppConfigurationFlightConfiguration"/> when requested during
        /// initialization.
        /// </summary>
        public AppConfigurationFlightProvider(IConfigurationManager configurationManager, ILogger<AppConfigurationFlightProvider> logger, ISecretManager secManager)
        {
            Contract.CheckValue(configurationManager, nameof(configurationManager));
            configuration = configurationManager.Get<AppConfigurationFlightConfiguration>();
            aadConfig = configurationManager.Get<AADClientConfiguration>();
            secretManager = secManager;
            this.logger = logger;
            this.httpClient = new HttpClient();
            System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;

        }

        /// <summary>
        /// Determines whether the provided flight is universally enabled or not.
        /// </summary>
        /// <returns>Returns true if the provided flight is enabled, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flight"/> argument is null.</exception>
        public override bool IsEnabled(Flight flight)
        {
            return IsEnabled(flight, null);
        }

        /// <summary>
        /// Determines whether the provided flight is universally enabled or not.
        /// </summary>
        /// <returns>Returns true if the provided flight is enabled, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flight"/> argument is null.</exception>
        public override bool IsEnabled(Flight flight, IDictionary<string, string> evaluationContext)
        {
            var url = $"evaluate?{flight.Name}";
            bool isEnabled = false;
            if (evaluationContext == null || evaluationContext.Count == 0) return isEnabled;
            var featureData = GetFeatureData(url, evaluationContext);
            if (string.IsNullOrWhiteSpace(featureData)) return isEnabled;
            var dict = JsonConvert.DeserializeObject(featureData, typeof(Dictionary<string, bool>)) as Dictionary<string, bool>;
            dict.TryGetValue(flight.Name, out isEnabled);
            return isEnabled;
        }

        /// <summary>
        /// Gives all universally enabled flights.
        /// </summary>
        /// <returns>Returns a collection of flights that are universally enabled.</returns>
        protected override IEnumerable<string> GetEnabledNames()
        {
            return this.GetEnabledNames(null);
        }

        /// <summary>
        /// Gives all enabled flights for the provided context.
        /// </summary>
        /// <returns>Returns a collection of flights that are enabled.</returns>
        protected override IEnumerable<string> GetEnabledNames(IDictionary<string, string> evaluationContext)
        {
            var url = "Evaluate";
            var lstFeatures = new List<string>();
            if (evaluationContext == null || evaluationContext.Count == 0) return lstFeatures;
            var featureData = GetFeatureData(url, evaluationContext);
            if (string.IsNullOrWhiteSpace(featureData)) return lstFeatures;
            var features = JsonConvert.DeserializeObject(featureData, typeof(Dictionary<string,bool>)) as Dictionary<string,bool>;
            foreach (var feature in features)
            {
                if (feature.Value) lstFeatures.Add(feature.Key);
            }

            return lstFeatures;
        }

        [Obsolete("This method is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        protected override IEnumerable<string> GetEnabledNames<TContext>(TContext context)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Determines whether the provided flight is enabled or not for provided context.
        /// </summary>
        /// <returns>Returns true if the provided flight is enabled, otherwise returns false.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="flight"/> argument is null.</exception>
        /// /// <exception cref="ArgumentNullException">Thrown when the <paramref name="evaluationContext"/> argument is null.</exception>
        [Obsolete("This method is obsolete and will be removed in a future release. Use IFlightsProvider<TEvaluationContext> members instead.")]
        public override bool IsEnabled<TContext>(Flight flight, TContext context)
        {
            throw new NotImplementedException();
        }

        private string GetFeatureData(string url, IDictionary<string,string> evaluationContext)
        {
            using (var requestMessage = GetRequestMessage(url, evaluationContext))
            {
                var result = httpClient.SendAsync(requestMessage).Result;

                if (result.IsSuccessStatusCode)
                {
                    var content = result.Content.ReadAsStringAsync().Result;
                    return content;
                }
                else
                {
                    logger.LogError("Request to fetch universal flights failed with status code:{0} and reason pharase {1}", result.StatusCode, result.ReasonPhrase);
                }

            }

            return string.Empty;
        }

        private HttpRequestMessage GetRequestMessage(string uri, IDictionary<string, string> evaluationContext)
        {
            var url = $"{configuration.ApiUrl}/{uri}";
            var requestMessage = new HttpRequestMessage(HttpMethod.Get, url);
            var token = string.Empty;
            if (evaluationContext != null || evaluationContext.Count !=0)
            {
                evaluationContext.TryGetValue("token",out token);
                if(!string.IsNullOrWhiteSpace(token))
                    evaluationContext.Remove("token");
                var context = ContextToJson(evaluationContext);
                requestMessage.Headers.Add("X-flightcontext", context);
            }

            if(string.IsNullOrWhiteSpace(token)) token = GetAADToken().GetAwaiter().GetResult();
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
            requestMessage.Headers.Add("X-Environment", configuration.Environment);
            requestMessage.Headers.Add("X-Application", configuration.AppName);

            
            return requestMessage;
        }

        private async Task<string> GetAADToken()
        {
            string token =string.Empty;
            try
            {
                string authority = string.Concat(aadConfig.AADInstance, aadConfig.TenantID);
                var clientSecret = await this.secretManager.GetSecretAsync(configuration.AppKey, logger);
                ClientCredential cd = new ClientCredential(configuration.AppClientId, clientSecret);
                var authContext = new AuthenticationContext(authority);
                AuthenticationResult result;

                result = await authContext.AcquireTokenAsync(configuration.AppClientId, cd);
                if (result != null && result.AccessToken != null)
                {
                    token = result.AccessToken;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format("AADInstance: {0} Exception :{1}", aadConfig.AADInstance, ex.Message), ex);
            }
            return token;
        }

        private string ContextToJson(IDictionary<string, string> dict)
        {
            var entries = dict.Select(d =>
                string.Format("\"{0}\": \"{1}\"", d.Key, d.Value));
            return "{" + string.Join(",", entries) + "}";
        }

    }
}
