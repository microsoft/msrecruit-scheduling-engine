//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="CandidateStatus.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
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
