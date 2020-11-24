//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="ImportResult.cs">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.TalentAttract.Contract
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;

    /// <summary>
    /// The client data contract to show the results of data import.
    /// </summary>
    [DataContract]
    public class ImportResult
    {
        /// <summary>
        /// Gets or sets the index of record.
        /// </summary>
        [DataMember(Name = "index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the import is successful for this record
        /// </summary>
        [DataMember(Name = "isSuccessful")]
        public bool IsSuccessful { get; set; }

        /// <summary>
        /// Gets or sets the detailed information if the import is not successful
        /// </summary>
        [DataMember(Name = "exceptionCode")]
        public string ExceptionCode { get; set; }

        /// <summary>
        /// Gets or sets job application id
        /// </summary>
        [DataMember(Name = "jobOpeningId", IsRequired = false)]
        public string JobOpeningId { get; set; }
    }
}
