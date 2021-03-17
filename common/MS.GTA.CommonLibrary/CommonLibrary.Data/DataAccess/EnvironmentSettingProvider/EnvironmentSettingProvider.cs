//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.Data.DataAccess
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using CommonLibrary.Common.Attract.Contract;
    using CommonLibrary.Common.Base.Security;
    using CommonLibrary.Common.TalentAttract.Contract;
    using CommonLibrary.CommonDataService.Common.Internal;
    using CommonLibrary.ServicePlatform.Tracing;

    /// <summary>
    /// Environment Setting Provider Class
    /// </summary>
    public class EnvironmentSettingProvider : IEnvironmentSettingProvider
    {
        /// <summary>
        /// The environment setting helper
        /// </summary>
        private readonly IEnvironmentSettingHelper environmentSettingHelper;

        /// <summary> The trace source</summary>
        private readonly ITraceSource traceSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnvironmentSettingProvider"/> class.
        /// </summary>
        public EnvironmentSettingProvider(
            IEnvironmentSettingHelper environmentSettingHelper,
            ITraceSource traceSource)
        {
            this.environmentSettingHelper = environmentSettingHelper;
            this.traceSource = traceSource;
        }

        public async Task<OfferSettings> GetOfferSettings(IHCMApplicationPrincipal principal)
        {
            Contract.CheckValue(principal, nameof(principal));

            var environmentSetting = await this.environmentSettingHelper.GetEnvironmentSettings();
            var offerFeature = new List<OfferFeature>();
            var offerSettings = new OfferSettings();
            var offerExpiry = new OfferExpirySettings();
            if (environmentSetting != null && environmentSetting.OfferSettings != null && environmentSetting.OfferSettings.OfferFeature != null && environmentSetting.OfferSettings.OfferExpiry != null)
            {
                var customRedirectUrlfeature = environmentSetting.OfferSettings.OfferFeature.Where(c => c.OfferFeatureName == OfferFeatureName.CustomRedirectUrl).FirstOrDefault();
                if (customRedirectUrlfeature == null)
                {
                    offerFeature.Add(new OfferFeature()
                    {
                        OfferFeatureName = OfferFeatureName.CustomRedirectUrl,
                        IsEnabled = false
                    });
                }

                var onboardingRedirectfeature = environmentSetting.OfferSettings.OfferFeature.Where(c => c.OfferFeatureName == OfferFeatureName.OnboardingRedirectRequired).FirstOrDefault();
                if (onboardingRedirectfeature == null)
                {
                    offerFeature.Add(new OfferFeature()
                    {
                        OfferFeatureName = OfferFeatureName.OnboardingRedirectRequired,
                        IsEnabled = false
                    });
                }
                environmentSetting.OfferSettings.OfferFeature = environmentSetting.OfferSettings.OfferFeature.Concat(offerFeature);

                return environmentSetting.OfferSettings;
            }

            this.traceSource.TraceInformation($"Sending default offer settings");

            foreach (var value in Enum.GetValues(typeof(OfferFeatureName)))
            {

                offerFeature.Add(new OfferFeature()
                {
                    OfferFeatureName = (OfferFeatureName)value,
                    IsEnabled = false
                });
            }
            offerSettings.OfferFeature = offerFeature;
            offerExpiry.ExpiryDays = 14;
            offerExpiry.IsCustomDate = false;
            offerSettings.OfferExpiry = offerExpiry;

            return offerSettings;
        }

        public async Task<EmailTemplateSettings> GetEmailTemplateSettings()
        {
            var environmentSetting = await this.environmentSettingHelper.GetEnvironmentSettings();

            if (environmentSetting != null && environmentSetting.EmailTemplateSettings != null)
            {

                return environmentSetting.EmailTemplateSettings;
            }

            this.traceSource.TraceWarning($"Environment settings not found");
            return null;
        }

        /// <summary>
        /// Get candidate offer settings
        /// </summary>
        /// <param name="principal">Current user principal</param>
        /// <returns>Candidate Offer settings</returns>
        public async Task<CandidateOfferSettings> GetCandidateOfferSettings(IHCMApplicationPrincipal principal)
        {
            Contract.CheckValue(principal, nameof(principal));

            var environmentSetting = await this.environmentSettingHelper.GetEnvironmentSettings();
            var offerFeature = new List<OfferFeature>();
            var candidateOfferSettings = new CandidateOfferSettings();
            candidateOfferSettings.OfferFeature = new List<OfferFeature>();
            if (environmentSetting != null && environmentSetting.OfferSettings != null && environmentSetting.OfferSettings.OfferFeature != null)
            {
                if (environmentSetting.OfferSettings.OfferExpiry != null)
                {
                    offerFeature.Add(new OfferFeature()
                    {
                        OfferFeatureName = OfferFeatureName.DeclineOffer,
                        IsEnabled = environmentSetting.OfferSettings.OfferFeature.Where(of => of.OfferFeatureName == OfferFeatureName.DeclineOffer).Select(f => f.IsEnabled).FirstOrDefault(),
                    });
                }
                else
                {
                    offerFeature.Add(new OfferFeature()
                    {
                        OfferFeatureName = OfferFeatureName.DeclineOffer,
                        IsEnabled = false,
                    });

                    this.traceSource.TraceInformation($"Sending default candidate decline offer settings");
                }

                if (environmentSetting.OfferSettings.OfferAcceptanceRedirectionSettings != null)
                {
                    offerFeature.Add(new OfferFeature()
                    {
                        OfferFeatureName = OfferFeatureName.CustomRedirectUrl,
                        IsEnabled = environmentSetting.OfferSettings.OfferFeature.Where(of => of.OfferFeatureName == OfferFeatureName.CustomRedirectUrl).Select(f => f.IsEnabled).FirstOrDefault(),
                    });

                    candidateOfferSettings.OfferAcceptanceRedirectionSettings = environmentSetting.OfferSettings.OfferAcceptanceRedirectionSettings;
                }
                else
                {
                    offerFeature.Add(new OfferFeature()
                    {
                        OfferFeatureName = OfferFeatureName.CustomRedirectUrl,
                        IsEnabled = false,
                    });

                    this.traceSource.TraceInformation($"Sending default candidate post redirect url offer settings");
                }

                candidateOfferSettings.OfferFeature = offerFeature;
                return candidateOfferSettings;
            }

            this.traceSource.TraceInformation($"Sending default candidate offer settings");

            offerFeature.Add(new OfferFeature()
            {
                OfferFeatureName = OfferFeatureName.DeclineOffer,
                IsEnabled = false,
            });

            offerFeature.Add(new OfferFeature()
            {
                OfferFeatureName = OfferFeatureName.CustomRedirectUrl,
                IsEnabled = false,
            });

            candidateOfferSettings.OfferFeature = offerFeature;
            return candidateOfferSettings;
        }
    }
}
