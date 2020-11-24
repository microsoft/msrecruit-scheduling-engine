// <copyright company="Microsoft Corporation" file="TalentCustomFields.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Newtonsoft.Json.Linq;

    [DataContract]
    public abstract class TalentBaseContract
    {
        [DataMember(Name = "talentCustomFields")]
        public Dictionary<string, JToken> CustomFields { get; set; }
    }
}
