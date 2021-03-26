//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using Microsoft.AspNetCore.Hosting;

namespace HR.TA.ServicePlatform.AspNetCore.Communication
{
    /// <summary>
    /// Options to configure communication listener
    /// </summary>
    public sealed class CommunicationListenerOptions
    {
        /// <summary>
        /// Custom URI segment to use in listening address. Similar to UriPathSuffix in ServiceManifest.xml
        /// </summary>
        public string AppRoot { get; set; }

        /// <summary>
        /// Custom builder to configure webhost before communication listener adds default middleware.
        /// This is commonly used to override web host to use WebListener or Kestrel
        /// </summary>
        public Func<IWebHostBuilder, IWebHostBuilder> WebHostBuilder { get; set; }

        /// <summary>
        /// Defines url segments to be included as part of unique service url.
        /// </summary>
        public UniqueServiceUriSegments UniqueUriSegments { get; set; } = UniqueServiceUriSegments.All;
    }
}
