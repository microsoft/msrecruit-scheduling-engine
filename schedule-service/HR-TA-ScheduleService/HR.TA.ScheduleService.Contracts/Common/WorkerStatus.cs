//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentEntities.Enum.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum WorkerStatus
    {
        Active = 0,
        Inactive = 1
    }
}