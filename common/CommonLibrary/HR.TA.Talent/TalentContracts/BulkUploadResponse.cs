//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.TalentAttract.Contract
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Bulk upload of jobs and candidates response.
    /// </summary>
    [DataContract]
    public class BulkUploadResponse
    {
        /// <summary>
        /// Gets or sets version.
        /// </summary>
        [DataMember(Name = "version", IsRequired = false, EmitDefaultValue = false)]
        public long Version { get; set; }
    }
}
