//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IEmailClient.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.ScheduleService.BusinessLibrary.Providers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using MS.GTA.Common.Email.GraphContracts;
    using MS.GTA.Talent.EnumSetModel.SchedulingService;

    /// <summary>The EmailClient interface.</summary>
    public interface IEmailClient
    {
        /// <summary>
        /// Acquire scheduler service authentication token
        /// </summary>
        /// <param name="userEmail">User Email</param>
        /// <returns>The scheduler service token</returns>
        Task<string> GetServiceAccountTokenByEmail(string userEmail);
    }
}
