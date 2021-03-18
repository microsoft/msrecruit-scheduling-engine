//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The environment metadata to display application count in candidate experience tenant hub page.
    /// </summary>
    [DataContract]
    public class EnvironmentMetadata
    {
        /// <summary>
        /// Gets or sets the environemnt id
        /// </summary>
        [DataMember(Name = "id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the application count
        /// </summary>
        [DataMember(Name = "applicationCount")]
        public int ApplicationCount { get; set; }
    }
}
