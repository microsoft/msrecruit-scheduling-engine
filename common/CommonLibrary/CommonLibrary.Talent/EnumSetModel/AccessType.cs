//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;

namespace CommonLibrary.TalentEngagementService.Data
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum AccessType
    {
        [EnumMember(Value = "exportData")]
        ExportData = 0,
    }
}
