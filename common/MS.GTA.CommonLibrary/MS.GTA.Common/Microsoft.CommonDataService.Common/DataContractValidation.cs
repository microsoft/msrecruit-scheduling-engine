//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MS.GTA.CommonDataService.Common
{
    /// <summary>
    /// Provides validation semantics for data contract objects.
    /// </summary>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    internal interface IDataContractValidatable
    {
        bool IsValid();
    }

    /// <summary>
    /// Defines helper methods for implementing <see cref="IDataContractValidatable.IsValid"/>.
    /// </summary>
    [Obsolete("These APIs are obsolete and will be removed in a future release. Use an implementation under the MS.GTA.CommonDataService.Common.Internal namespace instead.")]
    internal static class DataContractValidation
    {
        /// <summary>
        /// Determines whether the specified enumeration value is valid according to the definition of the enumeration.
        /// </summary>
        internal static bool IsValid(this Enum enumValue)
        {
            return enumValue != null && Enum.IsDefined(enumValue.GetType(), enumValue);
        }

        /// <summary>
        /// Determines whether all the items in the specified list are valid according to their implementation of 
        /// <see cref="IDataContractValidatable"/>.
        /// </summary>
        internal static bool AreAllValid<T>(this IList<T> list) where T : IDataContractValidatable
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i] == null || !list[i].IsValid())
                    return false;
            }
            return true;
        }
    }
}
