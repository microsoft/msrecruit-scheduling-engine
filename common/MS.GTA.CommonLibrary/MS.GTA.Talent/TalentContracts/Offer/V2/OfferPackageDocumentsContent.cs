//----------------------------------------------------------------------------
// <copyright file="OfferPackageDocumentsContent.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.OfferManagement.Contracts.V2
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using Microsoft.AspNetCore.Http;

    //TODO
    /// <summary>
    /// Specifies the Data Contract for saving offer package documents
    /// </summary>
    [DataContract]
    public class OfferPackageDocumentsContent
    {
        /// <summary>
        /// Gets or sets file to be saved
        /// </summary>
        [DataMember(Name = "files", IsRequired = false)]
        public IList<IFormFile> Files { get; set; }

        /// <summary>
        /// Gets or sets id of the file to be saved.
        /// </summary>
        [DataMember(Name ="offerPackageDocumentIds", IsRequired = false)]
        public IList<string> OfferPackageDocumentIds { get; set; }
    }
}
