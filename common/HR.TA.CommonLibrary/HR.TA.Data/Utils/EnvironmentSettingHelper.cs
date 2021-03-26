//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Data
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Http;
    using Microsoft.Azure.Documents;
    using Microsoft.Azure.Documents.Client;
    using HR.TA.Common.Base.ServiceContext;
    using HR.TA.Common.DocumentDB;
    using HR.TA.Common.Routing.DocumentDb;
    using HR.TA.Common.TalentAttract.Contract;

    /// <summary>
    /// Environemtn Settings Helper
    /// </summary>
    public class EnvironmentSettingHelper : IEnvironmentSettingHelper
    {
        /// <summary>
        /// Database name.
        /// </summary>
        private const string DataBaseName = "HCMDatabase";

        /// <summary> Http header key for app name </summary>
        private const string AppNameHeader = "x-ms-app-name";

        /// <summary> Default app name </summary>
        private const string DefaultAppName = "offer";

        /// <summary>
        /// Partition key used for environment setting.
        /// </summary>
        private const string PartitionKey = "/id";

        /// <summary>The hcm service context.</summary>
        private readonly IHCMServiceContext hcmServiceContext;

        /// <summary> Http context accessor </summary>
        private readonly IHttpContextAccessor httpContextAccessor;

        /// <summary>The document client generator.</summary>
        private readonly IDocumentClientGenerator documentClientGenerator;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentSettingHelper"/> class.
        /// </summary>
        /// <param name="hcmServiceContext">The HCM service context.</param>
        /// <param name="httpContextAccessor">The HTTP context accessor.</param>
        /// <param name="documentClientGenerator">The document client generator.</param>
        public EnvironmentSettingHelper(
            IHCMServiceContext hcmServiceContext,
            IHttpContextAccessor httpContextAccessor,
            IDocumentClientGenerator documentClientGenerator)
        {
            this.hcmServiceContext = hcmServiceContext;
            this.httpContextAccessor = httpContextAccessor;
            this.documentClientGenerator = documentClientGenerator;
        }

        public async Task<EnvironmentSettings> GetEnvironmentSettings()
        {
            var documentId = GetUserEnvironmentSettingsDocumentId();
            var client = await this.GetClient();
            return await client.GetFirstOrDefault<EnvironmentSettings>(f => f.Id == documentId, this.GetFeedOptions());
        }

        public string GetUserEnvironmentSettingsDocumentId()
        {
            var appName = this.httpContextAccessor?.HttpContext?.Request?.Headers?[AppNameHeader];
            if (string.IsNullOrEmpty(appName))
            {
                appName = DefaultAppName;
            }
            return string.Concat(appName, "-", this.hcmServiceContext.EnvironmentId);
        }

        public async Task<IHcmDocumentClient> GetClient()
        {
            return await this.documentClientGenerator.GetHcmRegionalHcmDocumentClient(this.hcmServiceContext.TenantId, this.hcmServiceContext.EnvironmentId, DataBaseName, typeof(EnvironmentSettings).ToString(), PartitionKey);
        }

        private FeedOptions GetFeedOptions()
        {
            var feedOptions = new FeedOptions
            {
                EnableCrossPartitionQuery = true,
                MaxDegreeOfParallelism = -1,
            };

            if (!string.IsNullOrEmpty(this.hcmServiceContext.TenantId))
            {
                feedOptions.PartitionKey = new PartitionKey(this.GetUserEnvironmentSettingsDocumentId());
            }

            return feedOptions;
        }
    }
}
