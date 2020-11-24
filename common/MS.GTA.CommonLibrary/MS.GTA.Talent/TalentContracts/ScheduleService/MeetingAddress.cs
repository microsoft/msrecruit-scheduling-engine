// <copyright file="MeetingAddress.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>

namespace MS.GTA.ScheduleService.Contracts.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// The street address.
    /// </summary>
    [DataContract]
    public class MeetingAddress
    {
        /// <summary>
        /// Gets or sets the address type.
        /// </summary>
        [DataMember(Name = "type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the post office box number.
        /// </summary>
        [DataMember(Name = "postOfficeBox")]
        public string PostOfficeBox { get; set; }

        /// <summary>
        /// Gets or sets the street name.
        /// </summary>
        [DataMember(Name = "street")]
        public string Street { get; set; }

        /// <summary>
        /// Gets or sets the city name.
        /// </summary>
        [DataMember(Name = "city")]
        public string City { get; set; }

        /// <summary>
        /// Gets or sets the state name.
        /// </summary>
        [DataMember(Name = "state")]
        public string State { get; set; }

        /// <summary>
        /// Gets or sets the country or region.
        /// </summary>
        [DataMember(Name = "countryOrRegion")]
        public string CountryOrRegion { get; set; }

        /// <summary>
        /// Gets or sets the postal code.
        /// </summary>
        [DataMember(Name = "postalCode")]
        public string PostalCode { get; set; }
    }
}
