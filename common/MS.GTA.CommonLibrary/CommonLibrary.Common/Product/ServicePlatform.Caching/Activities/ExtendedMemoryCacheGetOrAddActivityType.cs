//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Caching.Activities
{
    /// <summary>
    /// Represents an activity for a get or set command on an extended <c>redis</c> cache.
    /// </summary>
    internal sealed class ExtendedMemoryCacheGetOrAddActivityType : SingletonActivityType<ExtendedMemoryCacheGetOrAddActivityType>
    {
        public ExtendedMemoryCacheGetOrAddActivityType()
            : base("SP.ExtMemCache.GoS")
        {
        }
    }
}
