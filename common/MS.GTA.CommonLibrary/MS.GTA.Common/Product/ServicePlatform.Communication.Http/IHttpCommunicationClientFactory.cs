//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using System;
using System.Net.Http;

namespace MS.GTA.ServicePlatform.Communication.Http
{
    /// <summary>
    /// A factory for <see cref="IHttpCommunicationClient"/>. The interface declares <see cref="IDisposable"/> to allow
    /// use within a using statement for factories holding onto critical resources (e.g. connection pools).
    /// </summary>
    public interface IHttpCommunicationClientFactory : IDisposable
    {
        /// <summary>
        /// Creates a new instance of <see cref="IHttpCommunicationClient"/> with default options.
        /// </summary>
        IHttpCommunicationClient Create();

        /// <summary>
        /// Creates a new instance of <see cref="IHttpCommunicationClient"/> with the 
        /// given <see cref="HttpCommunicationClientOptions"/> and <see cref="HttpMessageHandler"/>.
        /// </summary>
        IHttpCommunicationClient Create(HttpCommunicationClientOptions options, HttpMessageHandler messageHandler = null);
    }
}
