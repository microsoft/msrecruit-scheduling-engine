//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace HR.TA..Talent.TalentContracts.TalentMatch
{
    /// <summary>
    /// Initializes a new instance of the <see cref="JobSkill" /> class.
    /// </summary>
    [DataContract]
    public class JobSkill
    {
        /// <summary>Gets or sets job opening skill.</summary>
        [DataMember(Name ="skill", EmitDefaultValue = false, IsRequired = false)]
        public string Skill { get; set; }

        /// <summary>Gets or sets skill score.</summary>
        [DataMember(Name = "score", EmitDefaultValue = false, IsRequired = false)]
        public double Score { get; set; }
    }
}
