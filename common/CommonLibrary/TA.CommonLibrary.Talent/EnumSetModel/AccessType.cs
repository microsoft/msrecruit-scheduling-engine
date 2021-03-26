//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TA.CommonLibrary.TalentEngagementService.Data
{
    [DataContract(Namespace = "TA.CommonLibrary.TalentEngagement")]
    public enum AccessType
    {
        [EnumMember(Value = "exportData")]
        ExportData = 0,
    }
}
