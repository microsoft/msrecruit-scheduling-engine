// <copyright file="JobApplicationParticipants.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.ScheduleService.Data.Models
{
    using System.Collections.Generic;
    using MS.GTA.Common.Provisioning.Entities.FalconEntities.Attract;
    using MS.GTA.Talent.TalentContracts.InterviewService;

    /// <summary>
    /// The <see cref="JobApplicationParticipants"/> class holds job application and it's participants infomration together.
    /// </summary>
    public class JobApplicationParticipants
    {
        /// <summary>
        /// Gets or sets the job application.
        /// </summary>
        /// <value>
        /// The instance of <see cref="JobApplication"/>.
        /// </value>
        public JobApplication Application { get; set; }

        /// <summary>
        /// Gets the job participants information.
        /// </summary>
        /// <value>
        /// The instance of <see cref="List{IVWorker}"/>.
        /// </value>
        public List<IVWorker> Participants { get; private set; } = new List<IVWorker>();
    }
}