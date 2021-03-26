//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    /// <summary>
    /// The information required for a login hint.
    /// </summary>
    [DataContract]
    public class LoginHint
    {
        [DataMember(Name = "obfuscatedUsername", IsRequired = false, EmitDefaultValue = false)]
        public string ObfuscatedUsername { get; set; }

        [DataMember(Name = "identityProvider", IsRequired = false, EmitDefaultValue = false)]
        public string IdentityProvider { get; set; }
    }
}
