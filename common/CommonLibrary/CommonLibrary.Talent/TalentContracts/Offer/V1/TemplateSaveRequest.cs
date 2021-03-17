//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace CommonLibrary.Common.OfferManagement.Contracts.V1
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Http;

    //TODO
    /// <summary>
    /// Specifies the Data Contract for Template
    /// </summary>
    [DataContract]
    public class TemplateSaveRequest
    {
        /// <summary>
        /// Gets or sets the tokens.
        /// </summary>
        [DataMember(Name = "tokenIds")]
        public IEnumerable<string> TokenIds { get; set; }

        /// <summary>
        /// Gets or sets file which has template contents.
        /// </summary>
        [DataMember(Name = "file")]
        public IFormFile File { get; set; }

        /// <summary>
        /// Gets or sets name with which template should be saved.
        /// </summary>
        [DataMember(Name = "name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether offer text is editable or not.
        /// </summary>
        [DataMember(Name = "isOfferTextEditable")]
        public bool IsOfferTextEditable { get; set; }
    }
}
