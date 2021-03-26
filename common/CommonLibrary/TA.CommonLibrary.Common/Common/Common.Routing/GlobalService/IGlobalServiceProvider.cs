//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Routing.GlobalService
{
    using ServicePlatform.GlobalService.ClientLibrary;

    /// <summary> The global service client provider interface</summary>
    public interface IGlobalServiceClientProvider
    {
        /// <summary> Gets the singleton routing client </summary>
        /// <returns>The singleton <see cref="RoutingClient"/> instance </returns>
        RoutingClient GetSingletonRoutingClient();

        /// <summary> Gets the singleton global service management client </summary>
        /// <returns>The singleton <see cref="GlobalServiceManagementClient"/> instance </returns>
        GlobalServiceManagementClient GetSingletonManagementClient();

        /// <summary>The get global service client.</summary>
        /// <returns>The <see cref="IGlobalServiceClient"/>.</returns>
        IGlobalServiceClient GetGlobalServiceClient();
    }
}
