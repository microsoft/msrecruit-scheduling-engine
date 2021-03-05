//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using ServicePlatform.Context;

namespace ServicePlatform.Caching.Activities
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
