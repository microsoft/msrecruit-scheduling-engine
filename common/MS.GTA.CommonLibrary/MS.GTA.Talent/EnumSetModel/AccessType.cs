//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace TalentEngagementService.Data
{
    [DataContract(Namespace = "TalentEngagement")]
    public enum AccessType
    {
        [EnumMember(Value = "exportData")]
        ExportData = 0,
    }
}
