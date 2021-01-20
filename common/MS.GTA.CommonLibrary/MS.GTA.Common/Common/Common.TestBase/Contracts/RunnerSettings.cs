//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="RunnerSettings.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TestBase.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The runner settings.
    /// </summary>
    [DataContract]
    public class RunnerSettings
    {
        /// <summary>
        /// Gets or sets the environment id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the runner services
        /// </summary>
        [DataMember(Name = "runnerServices")]
        public List<RunnerService> RunnerServices { get; set; }
    }
}
