//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------'r'n

namespace CommonLibrary.Common.Base.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class EnumerableExtensions
    {
        public static IEnumerable<IList<T>> Chunk<T>(this IEnumerable<T> elements, int chunkSize)
        {
            if (elements != null)
            {
                var elementsArray = elements.ToArray();
                for (var minIndex = 0; minIndex < elementsArray.Length; minIndex += chunkSize)
                {
                    yield return new ArraySegment<T>(elementsArray, minIndex, Math.Min(chunkSize, elementsArray.Length - minIndex));
                }
            }
        }
    }
}
