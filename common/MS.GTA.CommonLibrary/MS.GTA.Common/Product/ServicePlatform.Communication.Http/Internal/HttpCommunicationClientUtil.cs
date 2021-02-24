//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Net.Http;
using MS.GTA.CommonDataService.Common.Internal;
using MS.GTA.ServicePlatform.Communication.Http.Handlers;

namespace MS.GTA.ServicePlatform.Communication.Http.Internal
{
    /// <summary>
    /// Internal communication utilities.
    /// </summary>
    internal static class HttpCommunicationClientUtil
    {
        /// <summary>
        /// Combines the provided <paramref name="customHandlers"/> and <paramref name="sharedTransportHandler"/>.
        /// </summary>
        internal static HttpMessageHandler CombineHandlers(HttpMessageHandler sharedTransportHandler, IReadOnlyList<DelegatingHandler> customHandlers)
        {
            Contract.AssertValue(sharedTransportHandler, nameof(sharedTransportHandler));
            Contract.AssertValueOrNull(customHandlers, nameof(customHandlers));

            if (customHandlers == null || customHandlers.Count == 0)
            {
                return sharedTransportHandler;
            }

            var pipeline = sharedTransportHandler;
            for (int i = customHandlers.Count - 1; i >= 0; --i)
            {
                if (customHandlers[i].InnerHandler != null)
                {
                    throw new DelegatingInnerHandlerPresentException(customHandlers[i]);
                }

                customHandlers[i].InnerHandler = pipeline;
                pipeline = customHandlers[i];
            }

            return pipeline;
        }
    }
}
