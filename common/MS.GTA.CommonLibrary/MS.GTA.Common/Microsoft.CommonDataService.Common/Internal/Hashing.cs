//----------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//----------------------------------------------------------------------------

using System.Collections.Generic;
using System.Reflection;

namespace MS.GTA.CommonDataService.Common.Internal
{
    /// <summary>
    /// Utility methods for generating and combining hash codes.
    /// </summary>
    /// <remarks>
    /// These APIs support the SDK infrastructure and are not intended to be used
    /// directly from your code. The APIs may change or be removed in future releases.
    /// </remarks>
    public static class Hashing
    {
        // A fallback hash code to use for null values.  Avoid zero just to give a better distribution.
        public const int NullHashCode = -0xBEEF;

        public static uint CombineHash(uint u1, uint u2)
        {
            // TODO - 1055744: Implement a better general-purpose hash combining function
            return ((u1 << 7) | (u1 >> 25)) ^ u2;
        }

        public static int CombineHash(int n1, int n2)
        {
            return (int)CombineHash((uint)n1, (uint)n2);
        }

        public static int CombineHash(int n1, int n2, int n3)
        {
            uint hash = CombineHash((uint)n1, (uint)n2);
            return (int)CombineHash(hash, (uint)n3);
        }

        public static int CombineHash(int n1, int n2, int n3, int n4)
        {
            uint hash;
            hash = CombineHash((uint)n1, (uint)n2);
            hash = CombineHash(hash, (uint)n3);
            hash = CombineHash(hash, (uint)n4);
            return (int)hash;
        }

        public static int CombineHash(int n1, int n2, int n3, int n4, int n5)
        {
            uint hash;
            hash = CombineHash((uint)n1, (uint)n2);
            hash = CombineHash(hash, (uint)n3);
            hash = CombineHash(hash, (uint)n4);
            hash = CombineHash(hash, (uint)n5);
            return (int)hash;
        }

        public static int CombineHash(int n1, int n2, int n3, int n4, int n5, int n6)
        {
            uint hash;
            hash = CombineHash((uint)n1, (uint)n2);
            hash = CombineHash(hash, (uint)n3);
            hash = CombineHash(hash, (uint)n4);
            hash = CombineHash(hash, (uint)n5);
            hash = CombineHash(hash, (uint)n6);
            return (int)hash;
        }

        public static int CombineHash(int n1, int n2, int n3, int n4, int n5, int n6, int n7)
        {
            uint hash;
            hash = CombineHash((uint)n1, (uint)n2);
            hash = CombineHash(hash, (uint)n3);
            hash = CombineHash(hash, (uint)n4);
            hash = CombineHash(hash, (uint)n5);
            hash = CombineHash(hash, (uint)n6);
            hash = CombineHash(hash, (uint)n7);
            return (int)hash;
        }

        public static int CombineHash(int n1, int n2, int n3, int n4, int n5, int n6, int n7, int n8)
        {
            uint hash;
            hash = CombineHash((uint)n1, (uint)n2);
            hash = CombineHash(hash, (uint)n3);
            hash = CombineHash(hash, (uint)n4);
            hash = CombineHash(hash, (uint)n5);
            hash = CombineHash(hash, (uint)n6);
            hash = CombineHash(hash, (uint)n7);
            hash = CombineHash(hash, (uint)n8);
            return (int)hash;
        }

        public static int CombineHash(int n1, int n2, int n3, int n4, int n5, int n6, int n7, int n8, int n9)
        {
            uint hash;
            hash = CombineHash((uint)n1, (uint)n2);
            hash = CombineHash(hash, (uint)n3);
            hash = CombineHash(hash, (uint)n4);
            hash = CombineHash(hash, (uint)n5);
            hash = CombineHash(hash, (uint)n6);
            hash = CombineHash(hash, (uint)n7);
            hash = CombineHash(hash, (uint)n8);
            hash = CombineHash(hash, (uint)n9);
            return (int)hash;
        }

        /// <summary>
        /// Combines the hash code from each object in the specified list.  The specified comparer
        /// is used to generate the hash code.  The position of each value is reflected in the resulting 
        /// hash code.
        /// </summary>
        public static int CombineHash<T>(IList<T> values, IEqualityComparer<T> comparer = null)
        {
            Contract.AssertValue(values, "values");

            int hash = values.Count;
            comparer = comparer ?? EqualityComparer<T>.Default;

            for (int i = 0; i < values.Count; i++)
            {
                T value = values[i];
                int valueHash = 0;
                if (value != null)
                    valueHash = comparer.GetHashCode(value);

                // This does a couple of interesting things:
                // 1) By shifting, we account for the position of the value within the
                //    overall sequence
                // 2) By adding back the hash, we ensure we don't just shift all the 
                //    meaningful bytes off the left edge of the value.
                hash = ((hash << 5) + hash) ^ valueHash;
            }

            return hash;
        }

        /// <summary>
        /// Combines the hash code from each object in the specified enumeration.  The specified comparer
        /// is used to generate the hash code.  The position of each value is reflected in the resulting 
        /// hash code.
        /// </summary>
        public static int CombineHash<T>(IEnumerable<T> values, int count, IEqualityComparer<T> comparer = null)
        {
            Contract.AssertValue(values, "values");

            int hash = count;
            comparer = comparer ?? EqualityComparer<T>.Default;

            foreach (var value in values)
            {
                int valueHash = 0;
                if (value != null)
                    valueHash = comparer.GetHashCode(value);

                // This does a couple of interesting things:
                // 1) By shifting, we account for the position of the value within the
                //    overall sequence
                // 2) By adding back the hash, we ensure we don't just shift all the 
                //    meaningful bytes off the left edge of the value.
                hash = ((hash << 5) + hash) ^ valueHash;
            }

            return hash;
        }

        /// <summary>
        /// Computes a unique hash code for a set
        /// </summary>
        /// <remarks>
        /// This implementation is found in <see cref="HashSetEqualityComparer"/> implementation in .NET framework
        /// </remarks>
        public static int CombineHash<T>(ISet<T> values, IEqualityComparer<T> comparer = null)
        {
            Contract.AssertValue(values, "values");

            comparer = comparer ?? EqualityComparer<T>.Default;

            int hash = values.Count;
            foreach (T value in values)
                hash ^= CombineHashCommutative(GetHashCode(value, comparer));

            return hash;
        }

        /// <summary>
        /// Computes a unique hash code for a dictionary
        /// </summary>
        /// <remarks>
        /// Please note: 
        /// 1) this implementation is slow, please be aware while using it with Big dictionaries
        /// 2) always pass the same key comparer that is used in the dictionary.
        /// </remarks>
        public static int CombineHash<TKey, TValue>(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> keyComparer = null, IEqualityComparer<TValue> valueComparer = null)
        {
            Contract.AssertValue(dictionary, "dictionary");

            keyComparer = keyComparer ?? EqualityComparer<TKey>.Default;
            valueComparer = valueComparer ?? EqualityComparer<TValue>.Default;

            int hash = dictionary.Count;
            foreach (var pair in dictionary)
                hash ^= CombineHashCommutative(CombineHash(GetHashCode(pair.Key, keyComparer), GetHashCode(pair.Value, valueComparer)));

            return hash;
        }

        /// <summary>
        /// Uses the <paramref name="comparer"/> to get the hash code of the <paramref name="value"/>.
        /// Avoids calling <paramref name="comparer"/> if <paramref name="value"/> is null, because most comparers
        /// do not support this.
        /// </summary>
        public static int GetHashCode<T>(T value, IEqualityComparer<T> comparer = null)
        {
            Contract.AssertValueOrNull(comparer, "comparer");

            if (!typeof(T).GetTypeInfo().IsValueType && object.ReferenceEquals(value, null))
                return NullHashCode;

            return comparer != null ? comparer.GetHashCode(value) : value.GetHashCode();
        }

        /// <summary>
        /// This method used to compute the impact of one element's hash code on the parent collection hash code
        /// The order of the element in the collection is irrelevant
        /// </summary>
        public static int CombineHashCommutative(int elementHashcode)
        {
            return elementHashcode ^ int.MaxValue;
        }
    }
}
