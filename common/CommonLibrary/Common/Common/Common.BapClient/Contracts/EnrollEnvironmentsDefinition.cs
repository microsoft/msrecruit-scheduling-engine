//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.BapClient.Contracts
{
    /// <summary>The enroll environments definition.</summary>
    public class EnrollEnvironmentsDefinition
    {
        /// <summary>Gets or sets the back fill environment.</summary>
        public EnvironmentDefinition BackFillEnvironment { get; set; }

        /// <summary>Gets or sets the default environment.</summary>
        public EnvironmentDefinition DefaultEnvironment { get; set; }
    }
}
