// <copyright file="OfferTemplateRole.cs" company="Microsoft Corporation">
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
// </copyright>

namespace MS.GTA.Common.OfferManagement.Contracts.Enums.V1
{
    using System.Runtime.Serialization;

    /// <summary>
    /// Enum for Offer Template Role
    /// </summary>
    [DataContract]
    public enum OfferTemplateRole
    {
        /// <summary>
        /// TemplateManager
        /// </summary>
        TemplateManager,

        /// <summary>
        /// TemplateViewer
        /// </summary>
        TemplateViewer
    }
}