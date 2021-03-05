//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace Common.Provisioning.Entities.FalconEntities.Offer
{
    using System.Runtime.Serialization;

    [DataContract]
    public enum JobOfferRole
    {
        /// <summary> approver </summary>
        Approver = 0,

        /// <summary> fyi </summary>
        Fyi = 1,

        /// <summary> owner </summary>
        Owner = 2,

        /// <summary> coauthor </summary>
        CoAuthor = 3
    }
}
