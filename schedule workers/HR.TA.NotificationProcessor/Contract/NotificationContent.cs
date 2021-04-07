//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
namespace HR.TA.NotificationProcessor.Contract
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    /// <summary>
    /// Notification Content
    /// </summary>
    public class NotificationContent
    {
        /// <summary>
        /// Gets or sets notification list from graph.
        /// </summary>
        [JsonProperty(PropertyName = "value")]
        public List<Notification> Value { get; set; }
    }
}