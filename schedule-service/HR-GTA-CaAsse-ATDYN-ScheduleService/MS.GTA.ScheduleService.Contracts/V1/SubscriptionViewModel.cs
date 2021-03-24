//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System.Runtime.Serialization;
using CommonLibrary.Common.DocumentDB.Contracts;
using Newtonsoft.Json;

/*
 *  Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license.
 *  See LICENSE in the source repository root for complete license information.
 */

namespace MS.GTA.ScheduleService.Contracts.V1
{
    /// <summary>
    /// Gets or sets the data that displays in the Subscription view.
    /// </summary>
    [DataContract]
    public class SubscriptionViewModel : DocDbEntity
    {
        /// <summary>
        /// Gets or sets the subscriber email
        /// </summary>
        [DataMember(Name = "ServiceAccountEmail")]
        public string ServiceAccountEmail { get; set; }

        /// <summary>
        /// Gets or sets the environment Id for the subscription
        /// </summary>
        [DataMember(Name = "EnvironmentId")]
        public string EnvironmentId { get; set; }

        /// <summary>
        /// Gets or sets the tenant Id for the subscription
        /// </summary>
        [DataMember(Name = "TenantId")]
        public string TenantId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the boolean flag indicates if it is a system service account
        /// </summary>
        [DataMember(Name = "IsSystemServiceAccount")]
        public bool IsSystemServiceAccount { get; set; }

        /// <summary>
        /// Gets or sets the notification forwarding urls, seperated by comma
        /// </summary>
        [DataMember(Name = "ForwardingUrls")]
        public string ForwardingUrls { get; set; }

        /// <summary>
        /// Gets or sets the subscription
        /// </summary>
        [DataMember(Name = "Subscription")]
        public Subscription Subscription { get; set; }
    }
}
