// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="BapException.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.BapClient.Contracts
{
    using System.Runtime.Serialization;

    [DataContract]
    public class BapException
    {
        [DataMember]
        public BapError Error { get; set; }
    }

    [DataContract]
    public class BapError
    {
        [DataMember]
        public string Code { get; set; }
        
        [DataMember]
        public string Message { get; set; }
    }
}
