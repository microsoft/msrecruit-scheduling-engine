//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace ScheduleService.Contracts.V1
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Text;
    using CommonLibrary.Talent.EnumSetModel.SchedulingService;

    /// <summary>
    /// User permission for scheduling service
    /// </summary>
    [DataContract]
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    public class UserPermission
    {
        /// <summary>
        /// Gets or sets the list of users
        /// </summary>
        [DataMember(Name = "user")]
        public GraphPerson User { get; set; }

        /// <summary>
        /// Gets or sets the list of users
        /// </summary>
        [DataMember(Name = "permission")]
        public Permission Permission { get; set; }
    }
}
