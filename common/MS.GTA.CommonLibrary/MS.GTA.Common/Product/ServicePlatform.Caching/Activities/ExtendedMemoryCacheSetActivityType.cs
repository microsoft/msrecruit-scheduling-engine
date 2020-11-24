//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Caching.Activities
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
