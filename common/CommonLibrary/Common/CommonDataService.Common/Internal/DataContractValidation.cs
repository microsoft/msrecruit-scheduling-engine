//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace CommonDataService.Common.Internal
{
    /// <summary>
    /// Provides validation semantics for data contract objects.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public interface IDataContractValidatable
    {
        bool IsValid();
    }

    /// <summary>
    /// Defines helper methods for implementing <see cref="IDataContractValidatable.IsValid"/>.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public static class DataContractValidation
    {
        /// <summary>
        /// Determines whether the specified enumeration value is valid according to the definition of the enumeration.
        /// </summary>
        public static bool IsValid(this Enum enumValue)
        {
            return enumValue != null && Enum.IsDefined(enumValue.GetType(), enumValue);
        }

        /// <summary>
        /// Determines whether all the items in the specified list are valid according to their implementation of 
        /// <see cref="IDataContractValidatable"/>.
        /// </summary>
        public static bool AreAllValid<T>(this IList<T> list) where T : IDataContractValidatable
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
