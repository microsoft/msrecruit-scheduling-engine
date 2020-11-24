//----------------------------------------------------------------------------
// <copyright company="Microsoft Corporation" file="IGlobalServiceProvider.cs">
// Copyright (c) Microsoft Corporation. All rights reserved.
// </copyright>
//----------------------------------------------------------------------------

namespace MS.GTA.Common.Routing.GlobalService
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
