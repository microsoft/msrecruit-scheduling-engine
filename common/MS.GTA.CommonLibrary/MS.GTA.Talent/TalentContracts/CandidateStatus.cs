//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateStatus.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum CandidateStatus
    {
        Available = 0,
        NotAvailable = 1
    }
}
