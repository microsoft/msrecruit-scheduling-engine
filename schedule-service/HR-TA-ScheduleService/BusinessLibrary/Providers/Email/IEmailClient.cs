//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Providers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using HR.TA.Common.Email.GraphContracts;
    using HR.TA.Talent.EnumSetModel.SchedulingService;

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
