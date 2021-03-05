//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace Common.Base.Utilities
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Extended LINQ class exposes simplified LINQ operations on collections
    /// </summary>
    public static class ExtendedLinq
    {
        /// <summary>
        /// Checks if the collection is null or empty
        /// </summary>
        /// <remarks>
        /// Please note that Any is used on the collection to check for emptiness so depending on the
        /// implementation the collection may need to be partially expanded in which case this method
        /// may not be suitable for your use
        /// </remarks>
        /// <typeparam name="T">The type of the enumerable</typeparam>
        /// <param name="source">The source enumerable</param>
        /// <returns>True if the enumerable is null or empty otherwise false</returns>
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> source)
        {
            return source == null || !source.Any();
        }

        /// <summary>
        /// Ensures a non-null Enumerable is returned.
        /// </summary>
        /// <typeparam name="T">Generic type</typeparam>
        /// <param name="source">The source object</param>
        /// <returns>Returns the enumerable or Enumerable.Empty when enumerable is null.</returns>
        public static IEnumerable<T> EnsureNotNull<T>(this IEnumerable<T> source)
        {
            return source ?? Enumerable.Empty<T>();
        }

        /// <summary>The for each.</summary>
        /// <param name="collection">The collection.</param>
        /// <param name="action">The action.</param>
        /// <typeparam name="T">The type.</typeparam>
        public static void ForEach<T>(this IEnumerable<T> collection, Action<T> action)
        {
            if (collection == null || action == null)
            {
                return;
            }

            foreach (var obj in collection)
            {
                action(obj);
            }
        }

        public static IEnumerable<TR> SelectNonNull<T, TR>(this IEnumerable<T> collection, Func<T,TR> selector)
        {
            if (collection == null)
            {
                return Enumerable.Empty<TR>();
            }

            return collection.Where(each => each != null).Select(selector);
        }

        /// <summary>
        /// Selects a random entry in an enumerable collection
        /// </summary>
        /// <typeparam name="T">The enumerable type</typeparam>
        /// <param name="enumerable">the enumerable collection</param>
        /// <returns>A random entry</returns>
        public static T Random<T>(this IEnumerable<T> enumerable)
        {
            if (enumerable == null || enumerable.Count() == 0)
            {
                return default(T);
            }

            int randomIndex = new Random().Next(0, enumerable.Count());
            return enumerable.ElementAt(randomIndex);
        }
    }
}
