//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V1
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Worker contract.
    /// </summary>
    [DataContract]
    public class Worker
    {
        /// <summary>Gets or sets the Office Graph Identifier.</summary>
        [DataMember(Name = "officeGraphIdentifier")]
        public string OfficeGraphIdentifier { get; set; }

        /// <summary>Gets or sets the value of full name </summary>
        [DataMember(Name = "fullName")]
        public string FullName { get; set; }

        /// <summary> Gets or sets the value of given name </summary>
        [DataMember(Name = "givenName")]
        public string GivenName { get; set; }

        /// <summary>Gets or sets the value of middle name </summary>
        [DataMember(Name = "middleName")]
        public string MiddleName { get; set; }

        /// <summary>Gets or sets the value of surname </summary>
        [DataMember(Name = "surName")]
        public string SurName { get; set; }

        /// <summary>Gets or sets the value of primary email </summary>
        [DataMember(Name = "emailPrimary")]
        public string EmailPrimary { get; set; }

        /// <summary>Gets or sets the value of Phone Primary </summary>
        [DataMember(Name = "phonePrimary")]
        public string PhonePrimary { get; set; }

        /// <summary>Gets or sets the value of Description </summary>
        [DataMember(Name = "description")]
        public string Description { get; set; }

        /// <summary>Gets or sets the value of Profession </summary>
        [DataMember(Name = "profession")]
        public string Profession { get; set; }
    }
}