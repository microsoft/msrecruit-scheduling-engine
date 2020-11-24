// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="EntityType.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

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
