//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using ServicePlatform.Context;

namespace ServicePlatform.Caching.Activities
{
    /// <summary>
    /// Represents an activity for a set command on an extended <c>redis</c> cache.
    /// </summary>
    internal sealed class ExtendedMemoryCacheSetActivityType : SingletonActivityType<ExtendedMemoryCacheSetActivityType>
    {
        public ExtendedMemoryCacheSetActivityType()
            : base("SP.ExtMemCache.Set")
        {
        }
    }
}
