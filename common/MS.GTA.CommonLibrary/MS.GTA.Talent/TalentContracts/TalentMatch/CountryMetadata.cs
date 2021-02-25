//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace MS.GTA.Talent.TalentContracts.TalentMatch
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Metadata Search Response
    /// </summary>
    [DataContract]
    public class CountryMetadata
    {
        /// <summary>
        /// Gets or sets Country Code
        /// </summary>
        [DataMember(Name = "countryCode")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets Country
        /// </summary>
        [DataMember(Name = "country")]
        public string Country { get; set; }

        /// <summary>
        /// Gets or sets State
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets City
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }
    }
}
