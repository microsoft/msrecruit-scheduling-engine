//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="WorkerType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.TalentEntities.Enum.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum WorkerType
    {
        Employee = 0,
        Contractor = 1,
        Volunteer = 2,
        Unspecified = 3
    }
}