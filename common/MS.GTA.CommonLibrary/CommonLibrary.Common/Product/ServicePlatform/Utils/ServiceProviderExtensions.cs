//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;

namespace CommonLibrary.ServicePlatform.Utils
{
    /// <summary>
    /// Extension methods for <see cref="IServiceProvider"/>.
    /// </summary>
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// Strongly-typed version of vanilla <c>GetService</c> method over <see cref="IServiceProvider"/>.
        /// </summary>
        public static TService GetService<TService>(this IServiceProvider serviceProvider)
        {
            return (TService)serviceProvider.GetService(typeof(TService));
        }
    }
}
