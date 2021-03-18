//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using CommonLibrary.ServicePlatform.Context;

namespace CommonLibrary.ServicePlatform.Caching.Activities
{
    /// <summary>
    /// Represents an activity for a get command on an extended <c>redis</c> cache.
    /// </summary>
    internal sealed class ExtendedMemoryCacheGetActivityType : SingletonActivityType<ExtendedMemoryCacheGetActivityType>
    {
        public ExtendedMemoryCacheGetActivityType()
            : base("SP.ExtMemCache.Get")
        {
        }
    }
}
