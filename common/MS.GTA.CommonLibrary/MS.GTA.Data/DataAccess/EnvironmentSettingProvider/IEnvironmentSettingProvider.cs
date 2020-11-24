//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IEnvironmentSettingProvider.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Data.DataAccess
{
    using System.Threading.Tasks;
    using MS.GTA.Common.Attract.Contract;
    using MS.GTA.Common.Base.Security;
    using MS.GTA.Common.TalentAttract.Contract;

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