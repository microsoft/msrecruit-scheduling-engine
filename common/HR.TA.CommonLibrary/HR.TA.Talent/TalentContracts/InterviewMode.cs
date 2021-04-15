//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum that tells about Candidate Interview Mode.
    /// </summary>
    [DataContract]
    public enum InterviewMode
    {
        /// <summary>
        /// No Interview
        /// </summary>
        None,

        /// <summary>
        /// Face to Face interview
        /// </summary>
        FaceToFace,

       /// <summary>
       /// PhoneSkype Call interview
       /// </summary>
        OnlineCall,
    }
}
