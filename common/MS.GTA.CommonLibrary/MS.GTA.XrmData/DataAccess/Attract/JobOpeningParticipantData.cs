//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOpeningParticipantData.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.XrmData.DataAccess.Attract
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using MS.GTA.Common.TalentAttract.Contract;
    using MS.GTA.CommonDataService.Common.Internal;
    using MS.GTA.Data;
    using MS.GTA.XrmData.EntityExtensions;
    
    public partial class DataAccess : IDataAccess
    {
        /// <summary>
        /// Get Hiring team members.
        /// </summary>
        /// <param name="jobOpeningId">Job Opening Id</param>
        /// <returns>List of hiring team members.</returns>
        public async Task<IEnumerable<TeamMember>> GetHiringTeamMembers(string jobOpeningId)
        {
            Contract.CheckNonEmpty(jobOpeningId, nameof(jobOpeningId));

            return (await this.query.GetJobOpeningParticipantByJobId(jobOpeningId))?.Where(p => p != null).Select(p => p?.ToTeamMemberViewModel());
        }
    }
}
