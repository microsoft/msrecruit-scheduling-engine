//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TestBase.Contracts
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The runner service
    /// </summary>
    [DataContract]
    public class RunnerService
    {
        /// <summary>
        /// Gets or sets the runner service name
        /// </summary>
        [DataMember(Name = "runnerServiceName")]
        public string RunnerServiceName { get; set; }

        /// <summary>
        /// Gets or sets retry time in minutes.
        /// </summary>
        [DataMember(Name = "retryIntervalInMinutes")]
        public int RetryIntervalInMinutes { get; set; }

        /// <summary>
        /// Gets or sets the runner service
        /// </summary>
        [DataMember(Name = "isRunnerEnabled")]
        public bool IsRunnerEnabled { get; set; }
    }
}
