//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="Gender.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentEntities.Enum.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum Gender
    {
        Male = 0,
        Female = 1,
        NotSpecified = 2,
        Nonspecific = 3
    }
}
