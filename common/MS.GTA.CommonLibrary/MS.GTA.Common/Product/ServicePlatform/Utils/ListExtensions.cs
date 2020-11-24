//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System;
using System.Collections.Generic;

namespace MS.GTA.ServicePlatform.Utils
{
    /// <summary>
    /// List extension methods for <see cref="IList{T}"/>.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Randomizes a given list using <see cref="Random"/> by walking through the list
        /// and swapping the current element with a random element from within the remaining
        /// elements.
        /// </summary>
        /// <typeparam name="T">The type in the list.</typeparam>
        /// <param name="list">The list to randomize.</param>
        /// <param name="random">The randomizing algorithm to use.</param>
        public static void Randomize<T>(this IList<T> list, Random random)
        {
            for (var i = 0; i < list.Count; i++)
            {
                list.Swap(i, random.Next(i, list.Count));
            }
        }

        /// <summary>
        /// Swaps two values in a list.
        /// </summary>
        /// <typeparam name="T">The type in the list.</typeparam>
        /// <param name="list">The list to swap elements in.</param>
        /// <param name="i">The index of the first element to swap with the second element.</param>
        /// <param name="j">The index of the second element to swap with the first.</param>
        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }
    }
}
