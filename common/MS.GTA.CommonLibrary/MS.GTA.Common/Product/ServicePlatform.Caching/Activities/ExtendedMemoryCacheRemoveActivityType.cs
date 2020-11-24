//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation. All rights reserved.
//----------------------------------------------------------------------------

using MS.GTA.ServicePlatform.Context;

namespace MS.GTA.ServicePlatform.Caching.Activities
{
    /// <summary>
    /// Represents an activity for a remove command on an extended <c>redis</c> cache.
    /// </summary>
    internal sealed class ExtendedMemoryCacheRemoveActivityType : SingletonActivityType<ExtendedMemoryCacheRemoveActivityType>
    {
        public ExtendedMemoryCacheRemoveActivityType()
            : base("SP.ExtMemCache.Rmv")
        {
        }
    }
}
