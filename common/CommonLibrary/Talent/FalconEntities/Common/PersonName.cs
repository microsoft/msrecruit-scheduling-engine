//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.TalentEntities.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public class PersonName
    {
        [DataMember(Name = "GivenName", IsRequired = false, EmitDefaultValue = false, Order = 0)]
        public string GivenName { get; set; }

        [DataMember(Name = "MiddleName", IsRequired = false, EmitDefaultValue = false, Order = 1)]
        public string MiddleName { get; set; }

        [DataMember(Name = "Surname", IsRequired = false, EmitDefaultValue = false, Order = 2)]
        public string Surname { get; set; }
    }
}
