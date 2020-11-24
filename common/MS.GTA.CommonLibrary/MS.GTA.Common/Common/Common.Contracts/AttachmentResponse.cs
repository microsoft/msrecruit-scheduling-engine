namespace MS.GTA.Common.Contracts
{
    using System;
    using System.IO;
    using System.Runtime.Serialization;


    /// <summary>
    /// Attachment Response
    /// </summary>

    [DataContract]
    public class AttachmentResponse
    {
        /// <summary>
        /// Gets or sets Attachment Metadata
        /// </summary>
        [DataMember(Name = "attachmentName", IsRequired = false, EmitDefaultValue = false)]
        public string AttachmentName { get; set; }

        /// <summary>
        /// Gets or sets Attachment Type
        /// </summary>
        [DataMember(Name = "type", IsRequired = false, EmitDefaultValue = false)]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets Attachment Content
        /// </summary>
        [DataMember(Name = "attachmentStream", IsRequired = false, EmitDefaultValue = false)]
        public Stream AttachmentStream { get; set; }

        /// <summary>
        /// Gets or sets the LastUpdatedDateTime
        /// </summary>
        [DataMember(Name = "lastUpdatedDateTime", IsRequired = false, EmitDefaultValue = false)]
        public DateTime? LastUpdatedDateTime { get; set; }
    }
}
