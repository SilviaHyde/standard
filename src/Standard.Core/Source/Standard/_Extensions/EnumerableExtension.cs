using System;
using System.Collections.Generic;
using System.Linq;
using Standard.Core;

namespace Standard
{
    /// <summary>
    /// Extension methods for <c>IEnumerable</c> class.
    /// </summary>
    public static class EnumerableExtension
    {
        /// <summary>
        /// Determines if an enumerable is a subset of another enumerable.
        /// </summary>
        /// <typeparam name="T">The enumerable type.</typeparam>
        /// <param name="source">The enumerable that is being evaluated.</param>
        /// <param name="other">The enumerable to be tested.</param>
        /// <param name="comparer">The equality comparer.</param>
        /// <returns>
        /// `true` if <paramref name="source"/> is a subset of <paramref name="other"/>; otherwise `false`.
        /// </returns>
        /// <remarks>
        /// This extension implements selected ISet methods on <see cref="IList{T}"/> and <see cref="IDictionary{TKey, TValue}"/>.
        /// </remarks>
        public static bool IsSubsetOf<T> (this IList<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (other == null)
				throw new ArgumentNullException(nameof(other));

			// empty set is a subset of all sets
			if (source.Count == 0)
				return true;

			// better perf than hashset
			if (comparer == null)
				return !source.Except(other).Any();
			else
				return !source.Except(other, comparer).Any();
		}

		/// <summary>
        /// @ref <see cref="IsSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
		public static bool IsSubsetOf<T>(this IList<T> source, IEnumerable<T> other)
			=> IsSubsetOf(source, other, null);

        /// <summary>
        /// Determines if an enumerable is a proper subset of another enumerable. @ref <see cref="IsSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
        /// <returns>
        /// `true` if <paramref name="source"/> is a proper subset of <paramref name="other"/>; otherwise `false`.
        /// </returns>
        public static bool IsProperSubsetOf<T>(this IList<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (other == null)
				throw new ArgumentNullException(nameof(other));

			// other is a superset of source if source is an empty set and other is a non-empty set.
			if (source.Count == 0 && other.Any())
				return true;

			// better perf than hashset
			if (comparer == null)
				return other.Except(source).Any();
			else
				return other.Except(source, comparer).Any();
		}

		/// <summary>
        /// @ref <see cref="IsProperSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
		public static bool IsProperSubsetOf<T>(this IList<T> source, IEnumerable<T> other)
			=> IsProperSubsetOf(source, other, null);

        /// <summary>
        /// Determines whether an enumerable is a superset of another enumerable. @ref <see cref="IsSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
        /// <returns>
        /// `true` if <paramref name="source"/> is a superset of <paramref name="other"/>; otherwise, `false`.
        /// </returns>
		public static bool IsSupersetOf<T>(this IList<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (other == null)
				throw new ArgumentNullException(nameof(other));

			// source is always superset of an empty set
			if (!other.Any())
				return true;

			// better perf than hashset
			if (comparer == null)
				return !other.Except(source).Any();
			else
				return !other.Except(source, comparer).Any();
		}

		/// <summary>
        /// @ref <see cref="IsSupersetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
		public static bool IsSupersetOf<T>(this IList<T> source, IEnumerable<T> other)
			=> IsSupersetOf(source, other, null);

        /// <summary>
        /// Determines whether an enumerable is a proper superset of another enumerable. @ref <see cref="IsSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
        /// <returns>
        /// `true` if <paramref name="source"/> is a proper superset of <paramref name="other"/>; otherwise, `false`.
        /// </returns>
        public static bool IsProperSupersetOf<T>(this IList<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (other == null)
				throw new ArgumentNullException(nameof(other));

			// source is a proper superset of other if it is not empty and other is an empty set
			if (source.Count != 0 && !other.Any())
				return true;

			// better perf than hashset
			if (comparer == null)
				return source.Except(other).Any();
			else
				return source.Except(other, comparer).Any();			
		}

		/// <summary>
        /// @ref <see cref="IsProperSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
		public static bool IsProperSupersetOf<T>(this IList<T> source, IEnumerable<T> other)
			=> IsProperSupersetOf(source, other, null);

        /// <summary>
        /// Determines whether two enumerables have common items. @ref <see cref="IsSubsetOf{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
        /// <returns>
        /// `true` if <paramref name="source"/> and <paramref name="other"/> have common items; otherwise, `false`.
        /// </returns>
        public static bool Overlaps<T>(this IList<T> source, IEnumerable<T> other, IEqualityComparer<T> comparer)
		{
			if (source == null)
				throw new ArgumentNullException(nameof(source));

			if (other == null)
				throw new ArgumentNullException(nameof(other));

			// empty set is always overlaps with non-empty sets
			if (other.Any())
			{
				if (source.Count == 0)
					return false;
			}
			else if (source.Count == 0)
			{
				return true;
			}

			// better perf than hashset
			if (comparer == null)
				return source.Intersect(other).Any();
			else
				return source.Intersect(other, comparer).Any();
		}

        /// <summary>
        /// @ref <see cref="Overlaps{T}(IList{T}, IEnumerable{T}, IEqualityComparer{T})"/>
        /// </summary>
        public static bool Overlaps<T>(this IList<T> source, IEnumerable<T> other)
			=> Overlaps(source, other, null);

		/// <summary>
		/// Split an enumerable into N equal enumerables (to prevent issues with "multiple enumeration of IEnumerable"). After being split,
		/// the original enumerable should not be used anywhere else, otherwise the tee enumerables may lose data.
        ///
        /// This method is not thread-safe. Always use the <see cref="ConcurrentTee" /> method in a multi-thread scenario.
		/// </summary>
		/// <typeparam name="T">Type of object.</typeparam>
		/// <param name="source">Source to split.</param>
		/// <param name="subCount">Number of sub enumerables to create.</param>
		/// <remarks>
		/// Values consumed by the sub enumerables are cached in memory until ALL sub enumerables have consumed that value,
		/// which may consume a significant amount of storage if one enumerable is evaluated many times more than the others. 
		/// </remarks>
		public static IEnumerable<T>[] Tee<T>(this IEnumerable<T> source, int subCount)
        {
            EnumerableCache<T> cache = new EnumerableCache<T>(source.GetEnumerator());
            IEnumerable<T>[] arr = new IEnumerable<T>[subCount];
            for (int i = 0; i < subCount; i++)
            {
                arr[i] = SubEnumerable(cache.Register());
            }
            return arr;
        }

        /// <summary>
        /// Split an enumerable into N equal enumerables (to prevent issues with "multiple enumeration of IEnumerable"). After being split,
        /// the original enumerable should not be used anywhere else, otherwise the tee enumerables may lose data.
        ///
        /// This method is thread-safe. You can use the <see cref="Tee" /> method in a single-thread scenario.
        /// </summary>
        /// <typeparam name="T">Type of object.</typeparam>
        /// <param name="source">Source to split.</param>
        /// <param name="subCount">Number of sub enumerables to create.</param>
        /// <remarks>
        /// Values consumed by the sub enumerables are cached in memory until ALL sub enumerables have consumed that value,
        /// which may consume a significant amount of storage if one enumerable is evaluated many times more than the others. 
        /// </remarks>
        public static IEnumerable<T>[] ConcurrentTee<T>(this IEnumerable<T> source, int subCount)
        {
            EnumerableCache<T> cache = new EnumerableCache<T>(source.GetEnumerator());
            IEnumerable<T>[] arr = new IEnumerable<T>[subCount];
            for (int i = 0; i < subCount; i++)
            {
                arr[i] = ConcurrentSubEnumerable(cache.Register());
            }
            return arr;
        }

        private static IEnumerable<T> SubEnumerable<T>(EnumerableToken<T> tokens)
        {
            T item;
            while (tokens.TryConsume(out item))
            {
                yield return item;
            }
        }

        private static IEnumerable<T> ConcurrentSubEnumerable<T>(EnumerableToken<T> tokens)
        {
            T item;
            while (tokens.TryConsumeConcurrent(out item))
            {
                yield return item;
            }
        } 

        private class EnumerableCache<T>
        {
            private readonly List<T> _cache;
            private readonly IEnumerator<T> _iter;
            private readonly object _lock = new object();

            private Dictionary<EnumerableToken<T>, int> ConsumedIndex { get; set; } 

            public EnumerableCache(IEnumerator<T> iter)
            {
                _cache = new List<T>();
                _iter = iter;
                ConsumedIndex = new Dictionary<EnumerableToken<T>, int>();
            }

            public EnumerableToken<T> Register()
            {
                EnumerableToken<T> tokens = new EnumerableToken<T>(this);
                ConsumedIndex[tokens] = 0;
                return tokens;
            }

            public bool TryConsumeConcurrent(EnumerableToken<T> tokens, out T item)
            {
                lock (_lock)
                {
                    return TryConsume(tokens, out item);
                }
            }

            public bool TryConsume(EnumerableToken<T> tokens, out T item)
            {
                item = default(T);
                if (ConsumedIndex.ContainsKey(tokens))
                {
                    int offset = ConsumedIndex[tokens];
                    if (ConsumedIndex.Count > 1 &&
                        offset == 0 && 
                        ConsumedIndex.Values.Count(o => o == 0) == 1) // This is the only one at index 0
                    {
                        foreach (EnumerableToken<T> key in ConsumedIndex.Keys.ToList())
                        {
                            if (!ReferenceEquals(key, tokens))
                                ConsumedIndex[key]--; // Decrement the others8
                        }
                        item = _cache[0];
                        _cache.RemoveAt(0);
                        return true;
                    }

                    offset++;
                    ConsumedIndex[tokens] = offset;

                    if (offset > _cache.Count)
                    {
                        if (!_iter.MoveNext())
                        {
                            ConsumedIndex[tokens]--;  // We can't move any further. Turn back!
                            return false;
                        }
                        _cache.Add(_iter.Current);
                    }

                    item = _cache[offset - 1];
                    return true;
                }

                return false;
            }
        }

        private class EnumerableToken<T>
        {
            private EnumerableCache<T> Cache { get; set; }

            public EnumerableToken(EnumerableCache<T> cache)
            {
                Cache = cache;
            }

            public bool TryConsume(out T item)
            {
                return Cache.TryConsume(this, out item);
            }

            public bool TryConsumeConcurrent(out T item)
            {
                return Cache.TryConsumeConcurrent(this, out item);
            }
        }
    }
}
