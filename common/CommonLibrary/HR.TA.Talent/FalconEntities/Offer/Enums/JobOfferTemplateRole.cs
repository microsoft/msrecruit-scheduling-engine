//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferTemplateRole
    {
        /// <summary> templatemanager </summary>
        TemplateManager = 0,

        /// <summary> templateviewer </summary>
        TemplateViewer = 1,
    }
}
