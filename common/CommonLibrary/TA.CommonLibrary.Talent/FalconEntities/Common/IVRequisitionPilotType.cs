//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TA.CommonLibrary.Talent.FalconEntities.Common
{
    [DataContract]
    public enum IVRequisitionPilotType
    {
        PrePilot = 0,
        FirstRingPilot = 1,
        SecondRingPilot = 2,
        ThirdRingPilot = 3,
        FourthRingPilot = 4,
        FifthRingPilot = 5,
        SixthRingPilot = 6,

    }
}
