//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace Common.Contracts
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
