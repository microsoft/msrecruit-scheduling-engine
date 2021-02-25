//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace MS.GTA.TalentEngagementService.Data
{
    [DataContract(Namespace = "MS.GTA.TalentEngagement")]
    public enum AccessType
    {
        [EnumMember(Value = "exportData")]
        ExportData = 0,
    }
}
