//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.ScheduleService.Contracts.V1
{
    /// <summary>
    /// Open authentication settings
    /// </summary>
    public class SkypeOpenAuthSetting
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SkypeOpenAuthSetting"/> class.
        /// </summary>
        /// <param name="issuer">Skype for business issuer</param>
        /// <param name="audienceUri">Skype for business audience</param>
        /// <param name="tenantId">Skype for business tenant</param>
        /// <param name="apiUrl">Skype for business API URL</param>
        /// <param name="deleteMeetingUrl">Skype for business delete API URL</param>
        public SkypeOpenAuthSetting(string issuer, string audienceUri, string tenantId, string apiUrl, string deleteMeetingUrl)
        {
            this.Issuer = issuer;
            this.AudienceUri = audienceUri;
            this.TenantId = tenantId;
            this.ApiUrl = apiUrl;
            this.DeleteMeetingUrl = deleteMeetingUrl;
        }

        /// <summary>
        /// Gets or sets the issuer
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience
        /// </summary>
        public string AudienceUri { get; set; }

        /// <summary>
        /// Gets or sets the tenant
        /// </summary>
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets the API URL
        /// </summary>
        public string ApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the delete Meeting URL
        /// </summary>
        public string DeleteMeetingUrl { get; set; }
    }
}
