//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System.Collections.Generic;
namespace HR.TA.NotificationProcessor.Contract
{
    public class ServiceBusMessages
    {
        /// <summary>Gets or sets Message</summary>
        public string Message { get; set; }
     
        /// <summary>Gets or sets UserProperties</summary>
        public IDictionary<string, string> UserProperties { get; set; }
        
        /// <summary>Message Id</summary>
        public string MessageId { get; set; }
    }
}