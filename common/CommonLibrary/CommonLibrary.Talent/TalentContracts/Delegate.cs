//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Contract class representing person details and who they are acting on behalf of
    /// </summary>
    [DataContract]
    public class Delegate : Person
    {
        /// <summary>
        /// Gets or sets the user object id of the principal user for the delegate
        /// </summary>
        [DataMember(Name = "onBehalfOfUserObjectId", IsRequired = false, EmitDefaultValue = false)]
        public string OnBehalfOfUserObjectId { get; set; }
    }
}
