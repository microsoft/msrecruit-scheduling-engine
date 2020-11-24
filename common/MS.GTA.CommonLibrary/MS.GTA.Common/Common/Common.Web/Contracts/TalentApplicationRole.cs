//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="TalentApplicationRole.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------
namespace MS.GTA.Common.Web.Contracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum TalentApplicationRole
    {
        AttractAdmin = 0,
        OnboardAdmin = 1,
        LearnAdmin = 2,
        AttractRecruiter = 3,
        AttractHiringManager = 4,
        AttractReader = 5,
    }
}
