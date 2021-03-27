//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace HR.TA.ScheduleService.BusinessLibrary.Utils
{
    using System.Collections.Generic;

    /// <summary>
    /// Extensions for Collections
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Adds the elements of the specified collection to the end with null checks.
        /// </summary>
        /// <typeparam name="T">Type of the entities in collection</typeparam>
        /// <param name="self">Source collection</param>
        /// <param name="elementsCollection">Elements to be added</param>
        public static void AddSafeRange<T>(this List<T> self, IList<T> elementsCollection)
        {
            if (elementsCollection != null)
            {
                self.AddRange(elementsCollection);
            }
        }
    }
}
