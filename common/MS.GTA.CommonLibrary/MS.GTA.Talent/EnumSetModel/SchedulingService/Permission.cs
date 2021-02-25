//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Talent.EnumSetModel.SchedulingService
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.Serialization;
    using System.Text;

    /// <summary>
    /// User permission in scheduling service
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1402:FileMayOnlyContainASingleClass", Justification = "Small closely related classes may be combined.")]
    [DataContract]
    public enum Permission
    {
        /// <summary>
        /// No permission
        /// </summary>
        [DataMember(Name = "none")]
        None,

        /// <summary>
        /// Read only permission
        /// </summary>
        [DataMember(Name = "read")]
        Read,

        /// <summary>
        /// Write permission
        /// </summary>
        [DataMember(Name = "write")]
        Write,
    }
}
