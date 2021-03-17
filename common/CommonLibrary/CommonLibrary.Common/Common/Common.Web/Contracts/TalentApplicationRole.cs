//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace CommonLibrary.Common.Web.Contracts
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
