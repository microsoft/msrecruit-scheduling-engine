//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Data.DataAccess
{
    using System.Threading.Tasks;
    using HR.TA.Common.Attract.Contract;
    using HR.TA.Common.Base.Security;
    using HR.TA.Common.TalentAttract.Contract;

    /// <summary>
    /// Environment Setting Provider Interface
    /// </summary>
    public interface IEnvironmentSettingProvider
    {
        /// <summary>
        /// Gets the email template settings.
        /// </summary>
        /// <returns>Email template Settings</returns>
        Task<EmailTemplateSettings> GetEmailTemplateSettings();

        /// <summary>
        /// Gets the offer settings.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Offer Settings</returns>
        Task<OfferSettings> GetOfferSettings(IHCMApplicationPrincipal principal);

        /// <summary>
        /// Gets the candidate offer settings.
        /// </summary>
        /// <param name="principal">The principal.</param>
        /// <returns>Candidate Offer Settings</returns>
        Task<CandidateOfferSettings> GetCandidateOfferSettings(IHCMApplicationPrincipal principal);
    }
}