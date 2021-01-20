﻿// ----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="JobOfferArtifactDocumentType.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
// ----------------------------------------------------------------------------

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