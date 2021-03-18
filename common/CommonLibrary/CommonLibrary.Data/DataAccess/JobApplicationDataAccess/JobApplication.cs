//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Data.DataAccess
{
    using CommonLibrary.TalentEntities.Enum;
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// JOb Application Class
    /// </summary>
    public class JobApplication
    {
        /// <summary>
        /// The data access
        /// </summary>
        private readonly IDataAccess dataAccess;

        public JobApplication(
            IDataAccess dataAccess)
        {
            this.dataAccess = dataAccess;
        }

        public async Task<bool> UpdateOfferedJobApplicationStatusReason(string invitationId, string jobApplicationId, JobApplicationStatusReason statusReason)
        {
            return await this.dataAccess.UpdateOfferedJobApplicationStatusReason(jobApplicationId, statusReason, invitationId);
        }
    }
}
