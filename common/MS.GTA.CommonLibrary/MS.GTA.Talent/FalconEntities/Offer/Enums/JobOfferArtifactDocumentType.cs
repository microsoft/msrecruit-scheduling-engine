//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace MS.GTA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferArtifactDocumentType
    {
        /// <summary> doc </summary>
        Doc = 0,

        /// <summary> docX </summary>
        DocX = 1,

        /// <summary> pdf </summary>
        Pdf = 2,

        /// <summary> html </summary>
        Html = 3,

        /// <summary> jpg </summary>
        Jpg = 4
    }
}
