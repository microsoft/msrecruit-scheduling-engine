//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using CommonLibrary.Common.OfferManagement.Contracts.Enums.V1;

    /// <summary>
    /// An activity in a job application
    /// </summary>
    public class ApplicationActivity
    {
        /// <summary>
        /// Gets or sets the identifier of the application activity.
        /// </summary>
        public string ApplicationActivityID { get; set; }

        /// <summary>
        /// Gets or sets the type of the job application activity.
        /// </summary>
        public ApplicationActivityType? ActivityType { get; set; }

        /// <summary>
        /// Gets or sets the type of the job application activity.
        /// </summary>
        public List<ApplicationActivityParticipant> ApplicationActivityParticipants { get; set; }

        /// <summary>
        /// Gets or sets the ordinal of the job opening stage.
        /// </summary>
        public long? JobOpeningStageOrdinal { get; set; }
    }
}
