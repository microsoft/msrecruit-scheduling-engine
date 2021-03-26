//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum CandidateStatus
    {
        Available = 0,
        NotAvailable = 1
    }
}
