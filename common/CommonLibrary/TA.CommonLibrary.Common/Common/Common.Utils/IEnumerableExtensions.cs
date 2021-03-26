//-----------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
//-----------------------------------------------------------------------

namespace TA.CommonLibrary.Common.Utils
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class IEnumerableExtensions
    {
        public static (IList<T>, IList<T>) CompareWith<T, TKey>(this IEnumerable<T> left, IEnumerable<T> right, Func<T, TKey> keyFunc, IEqualityComparer<TKey> comparer = null)
        {
            var leftLookup = left?.ToDictionaryIgnoreDupes(r => keyFunc(r), comparer) ?? new Dictionary<TKey, T>(comparer);
            var rightLookup = right?.ToDictionaryIgnoreDupes(r => keyFunc(r), comparer) ?? new Dictionary<TKey, T>(comparer);
            IList<T> leftUnique = leftLookup.Where(r => !rightLookup.ContainsKey(r.Key)).Select(r => r.Value).ToArray();
            IList<T> rightUnique = rightLookup.Where(r => !leftLookup.ContainsKey(r.Key)).Select(r => r.Value).ToArray();
            return (leftUnique, rightUnique);
        }

        public static Dictionary<TKey, TElement> ToDictionaryIgnoreDupes<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer = null)
        {
            var result = new Dictionary<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                result[keySelector(item)] = elementSelector(item);
            }
            return result;
        }

        public static Dictionary<TKey, TSource> ToDictionaryIgnoreDupes<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, IEqualityComparer<TKey> comparer = null)
        {
            var result = new Dictionary<TKey, TSource>(comparer);
            foreach (var item in source)
            {
                result[keySelector(item)] = item;
            }
            return result;
        }

        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector)
        {
            var result = new ConcurrentDictionary<TKey, TElement>();
            foreach (var item in source)
            {
                result.TryAdd(keySelector(item), elementSelector(item));
            }
            return result;
        }

        public static ConcurrentDictionary<TKey, TElement> ToConcurrentDictionary<TSource, TKey, TElement>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector, Func<TSource, TElement> elementSelector, IEqualityComparer<TKey> comparer)
        {
            var result = new ConcurrentDictionary<TKey, TElement>(comparer);
            foreach (var item in source)
            {
                result.TryAdd(keySelector(item), elementSelector(item));
            }
            return result;
        }

        public static void AddRange<T, TKey, TValue>(this IDictionary<TKey, TValue> dictionary, IEnumerable<T> values, Func<T, TKey> getKey, Func<T, TValue> getValue)
        {
            foreach (var value in values)
            {
                dictionary[getKey(value)] = getValue(value);
            }
        }

        public static TValue GetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key)
            where TValue : new()
        {
            if (!dictionary.TryGetValue(key, out var value))
            {
                value = new TValue();
                dictionary.Add(key, value);
            }
            return value;
        }

        public static T RemoveFirst<T>(this List<T> list, Predicate<T> predicate)
            where T : class
        {
            var index = list.FindIndex(predicate);
            if (index == -1)
            {
                return null;
            }
            var item = list[index];
            list.RemoveAt(index);
            return item;
        }

        public static async Task AsyncParallelForeach<T>(this IEnumerable<T> items, int parallel, Func<T, Task> function)
        {
            var tasks = new List<Task>();
            foreach (var item in items)
            {
                tasks.Add(Task.Run(() => function(item)));
                if (tasks.Count >= parallel)
                {
                    var finishedTask = await Task.WhenAny(tasks);
                    tasks.Remove(finishedTask);
                }
            }
            await Task.WhenAll(tasks);
        }
    }
}
