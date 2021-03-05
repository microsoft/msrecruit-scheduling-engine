//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Integration.Contracts
{
    /// <summary>
    /// MSIntArtifact
    /// </summary>
    public class MSIntArtifact
    {
        public int? ArtifactOrdinal { get; set; }

        public string ArtifactName { get; set; }

        public string ArtifactFileLabel { get; set; }

        public string ArtifactType { get; set; }

        public string ArtifactDocumentType { get; set; }

        public string UploadedByPersona { get; set; }

        public string ArtifactDownloadUri { get; set; }
    }
}
