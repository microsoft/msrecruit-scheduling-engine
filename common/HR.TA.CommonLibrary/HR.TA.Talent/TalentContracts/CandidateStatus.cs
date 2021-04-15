//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum CandidateStatus
    {
        Available = 0,
        NotAvailable = 1
    }
}
