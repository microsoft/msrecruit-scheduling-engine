//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Runtime.Serialization;

namespace CommonLibrary.TalentEntities.Enum
{
    [DataContract(Namespace = "CommonLibrary.TalentEngagement")]
    public enum JobApplicationActivityType
    {
        [EnumMember(Value = "IVRequested")]
        IVRequested = 0,

        [EnumMember(Value = "Interview")]
        Interview = 1,

        [EnumMember(Value = "Feedback")]
        Feedback = 2,

        [EnumMember(Value = "Offer")]
        Offer = 3        
    }
}
