//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.TalentEntities.Enum.Common
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum WorkerStatus
    {
        Active = 0,
        Inactive = 1
    }
}
