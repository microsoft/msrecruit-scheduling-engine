//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------
using HR.TA.ServicePlatform.Context;

namespace HR.TA.ServicePlatform.Caching.Activities
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
