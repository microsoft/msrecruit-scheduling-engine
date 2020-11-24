//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum
{
    using System.Runtime.Serialization;

    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum JobPostSupplier
    {
        [EnumMember(Value = "notSpecified")]
        NotSpecified = 0,
        [EnumMember(Value = "linkedIn")]
        LinkedIn = 1,
        [EnumMember(Value = "monster")]
        Monster = 2,
        [EnumMember(Value = "indeed")]
        Indeed = 3
    }
}
