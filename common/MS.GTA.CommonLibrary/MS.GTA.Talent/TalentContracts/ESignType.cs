﻿//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ESignType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for E-Signature Types
    /// </summary>
    [DataContract]
    public enum ESignType
    {
        /// <summary>
        /// ESign
        /// </summary>
        ESign,

        /// <summary>
        /// DocuSign
        /// </summary>
        DocuSign,

        /// <summary>
        /// AdobeSign
        /// </summary>
        AdobeSign,        
    }
}
