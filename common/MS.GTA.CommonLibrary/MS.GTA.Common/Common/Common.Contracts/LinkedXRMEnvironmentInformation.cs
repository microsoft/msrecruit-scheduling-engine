// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="LinkedXRMEnvironmentInformation.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

namespace MS.GTA.Common.Contracts
{
    using System;
    using System.Runtime.Serialization;

    [DataContract]
    public class LinkedXRMEnvironmentInformation
    {
        /// <summary>
        /// Gets or sets the XRM instance id.
        /// </summary>
        [DataMember(Name = "instanceId")]
        public string InstanceId { get; set; }

        /// <summary>
        /// Gets or sets the XRM instance URL
        /// </summary>
        [DataMember(Name = "instanceUrl")]
        public string InstanceUrl { get; set; }

        /// <summary>
        /// Gets or sets the XRM instance API URL
        /// </summary>
        [DataMember(Name = "instanceApiUrl")]
        public string InstanceApiUrl { get; set; }

        /// <summary>
        /// Gets or sets the XRM instance created time
        /// </summary>
        [DataMember(Name = "createdTime")]
        public DateTime? CreatedTime { get; set; }

        /// <summary>
        /// Gets or sets the XRM instance modified time.
        /// </summary>
        [DataMember(Name = "modifiedTime")]
        public DateTime? ModifiedTime { get; set; }

        /// <summary>
        /// Gets or sets the XRM instance host name suffix.
        /// </summary>
        [DataMember(Name = "hostNameSuffix")]
        public string HostNameSuffix { get; set; }

        /// <summary>
        /// Gets or sets the XRM instance base language id.
        /// </summary>
        [DataMember(Name = "localeId")]
        public int LocaleId { get; set; }

        /// <summary>Gets or sets the initial user object id.</summary>
        [DataMember(Name = "initialUserObjectId")]
        public string InitialUserObjectId { get; set; }

        /// <summary> Gets or sets CRM Friendly Name </summary>
        [DataMember(Name = "friendlyName")]
        public string FriendlyName { get; set; }

        /// <summary> Gets or sets CRM Unique Name </summary>
        [DataMember(Name = "uniqueName")]
        public string UniqueName { get; set; }

        /// <summary> Gets or sets CRM Domain Name </summary>
        [DataMember(Name = "domainName")]
        public string DomainName { get; set; }

        /// <summary> Gets or sets whether the Attract Service Endpoint has been configured</summary>
        [DataMember(Name = "hasAttractServiceEndpointConfigured")]
        public bool? HasAttractServiceEndpointConfigured { get; set; }
    }
}