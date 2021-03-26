//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace HR.TA.TalentEngagementService.Data
{
    [DataContract(Namespace = "HR.TA.TalentEngagement")]
    public enum AccessType
    {
        [EnumMember(Value = "exportData")]
        ExportData = 0,
    }
}
