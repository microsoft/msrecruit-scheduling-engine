//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA..Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferStatusReason
    {

        /// <summary> new </summary>
        New = 0,

        /// <summary> draft </summary>
        Draft = 1,

        /// <summary> review </summary>
        Review = 2,

        /// <summary> approved </summary>
        Approved = 3,

        /// <summary> published </summary>
        Published = 4,

        /// <summary> accepted </summary>
        Accepted = 5,

        /// <summary> withdrawnCandidateDispositioned </summary>
        WithdrawnCandidateDispositioned = 6,

        /// <summary> needRevision </summary>
        NeedRevision = 7,

        /// <summary> withdrawnOther </summary>
        WithdrawnOther = 8,

        /// <summary> declined </summary>
        Declined = 9,

        /// <summary> closed </summary>
        Closed = 10,

        /// <summary> expired </summary>
        Expired = 11,
    }
}