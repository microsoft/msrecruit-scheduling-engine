//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EmailAddress.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Email address
    /// </summary>
    [DataContract]
    public class EmailAddress
    {
        /// <summary>
        /// Gets or sets template parameters
        /// </summary>
        [DataMember(Name = "email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets template parameters
        /// </summary>
        [DataMember(Name = "name", IsRequired = false, EmitDefaultValue = false)]
        public string Name { get; set; }
    }
}
