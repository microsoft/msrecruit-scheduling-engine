//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using System;
using MS.GTA.CommonDataService.Common.Internal;

namespace MS.GTA.ServicePlatform.Communication.Http.Internal
{
    internal sealed class HttpCommunicationClientInternalOptions
    {
        internal HttpCommunicationClientInternalOptions(HttpCommunicationClientOptions original)
        {
            Contract.AssertValue(original, nameof(original));

            ThrowOnNonSuccessResponse = original.ThrowOnNonSuccessResponse;
            Timeout = original.Timeout;
        }

        internal bool ThrowOnNonSuccessResponse { get; }

        internal TimeSpan Timeout { get; }
    }
}
