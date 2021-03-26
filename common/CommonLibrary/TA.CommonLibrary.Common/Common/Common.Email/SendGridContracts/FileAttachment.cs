//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Email.SendGridContracts
{
    using System;
    using System.Drawing;
    using CommonDataService.Common.Internal;
    using Newtonsoft.Json;

    /// <summary>
    /// FileAttachment class for using Microsoft Graph
    /// </summary>
    public class FileAttachment
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileAttachment" /> class.
        /// </summary>
        public FileAttachment()
        {
            this.ContentId = Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Gets or sets the Content of attachment in base64 block.
        /// </summary>
        [JsonProperty("contentBytes")]
        public string Base64Content { get; set; }

        /// <summary>
        /// Gets the contentId.
        /// </summary>
        [JsonProperty("contentId")]
        public string ContentId { get; private set; }

        /// <summary>
        /// Gets the Filename.
        /// </summary>
        [JsonProperty("name")]
        public string Filename => $"{Name}.{Type}";

        /// <summary>
        /// Gets or sets the file name. It is the name portion of the file name without extension.
        /// </summary>
        [JsonIgnore]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type. It is an extension of the attached file.
        /// </summary>
        [JsonProperty(PropertyName = "contentType")]
        public string Type { get; set; }

        /// <summary>
        /// Gets the ODataType. fileAttachment Resource type to be used in Microsoft Graph API.
        /// </summary>
        [JsonProperty(PropertyName = "@odata.type")]
        public string ODataType => "#microsoft.graph.fileAttachment";
    }
}
