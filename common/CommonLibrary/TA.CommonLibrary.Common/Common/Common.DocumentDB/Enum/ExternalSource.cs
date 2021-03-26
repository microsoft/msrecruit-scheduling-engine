//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TA.CommonLibrary.Common.DocumentDB.Enums
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum ExternalSource
    {

        [EnumMember(Value = "internal")]
        Internal = 0,
        [EnumMember(Value = "mSDataMall")]
        MSDataMall = 1,
        [EnumMember(Value = "linkedIn")]
        LinkedIn = 2,
        [EnumMember(Value = "greenhouse")]
        Greenhouse = 3,
        [EnumMember(Value = "iCIMS")]
        ICIMS = 4
    }
}
