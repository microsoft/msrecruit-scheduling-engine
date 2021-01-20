//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="InterviewMode.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
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
