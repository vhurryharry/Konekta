using System;
using System.Collections.Generic;
using System.Linq;

namespace WCA.Domain.Models
{
    /// <summary>
    /// Wrapper for a generic List<> that only accepts IRange types. It only
    /// allows non-overlapping IRange objects to be added.
    /// </summary>
    /// <typeparam name="TRangeType">The type of the range type.</typeparam>
    /// <seealso cref="System.Collections.Generic.IList{TRangeType}" />
#pragma warning disable CA1710 // Identifiers should have correct suffix: This is an IList, so ends in "List"
    public class RangeList<TRangeType> : IList<TRangeType>
#pragma warning restore CA1710 // Identifiers should have correct suffix
        where TRangeType : IRange<TRangeType>
    {
        private readonly List<TRangeType> innerList;

        public RangeList()
        {
            innerList = new List<TRangeType>();
        }

        public RangeList(int capacity)
        {
            innerList = new List<TRangeType>(capacity);
        }

        public RangeList(IEnumerable<TRangeType> collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException(nameof(collection));
            }

            innerList = new List<TRangeType>(collection);

            // Check for overlapping items in range
            for (int i = 0; i < innerList.Count - 1; i++)
            {
                for (int j = i + 1; j < innerList.Count; j++)
                {
                    // Skip comparing current tier with itself
                    if (i == j) { break; }

                    // Check if outer loop tier conflicts with inner tier
                    TRangeType outer = innerList[i];
                    TRangeType inner = innerList[j];
                    if (outer.Intersects(inner))
                    {
                        throw new ArgumentOutOfRangeException(nameof(collection), "parameter tiers contains overlapping ranges");
                    }
                }
            }
        }

        public int Count
        {
            get { return innerList.Count; }
        }

        public bool IsReadOnly
        {
            get { return ((IList<TRangeType>)innerList).IsReadOnly; }
        }

        public TRangeType this[int index]
        {
            get
            {
                return innerList[index];
            }
            set
            {
                innerList[index] = EnsureUniqueRange(value);
            }
        }

        public void Add(TRangeType item)
        {
            innerList.Add(EnsureUniqueRange(item));
        }

        public void Clear()
        {
            innerList.Clear();
        }

        public bool Contains(TRangeType item)
        {
            return innerList.Contains(item);
        }

        public void CopyTo(TRangeType[] array, int arrayIndex)
        {
            innerList.CopyTo(array, arrayIndex);
        }

        public IEnumerator<TRangeType> GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return innerList.GetEnumerator();
        }

        public int IndexOf(TRangeType item)
        {
            return innerList.IndexOf(item);
        }

        public void Insert(int index, TRangeType item)
        {
            innerList.Insert(index, EnsureUniqueRange(item));
        }

        public bool Remove(TRangeType item)
        {
            return innerList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            innerList.RemoveAt(index);
        }

        public void Sort()
        {
            innerList.Sort();
        }

        public void Sort(IComparer<TRangeType> comparer)
        {
            innerList.Sort(comparer);
        }

        public void Sort(Comparison<TRangeType> comparison)
        {
            innerList.Sort(comparison);
        }

        public void Sort(int index, int count, IComparer<TRangeType> comparer)
        {
            innerList.Sort(index, count, comparer);
        }

        private TRangeType EnsureUniqueRange(TRangeType range)
        {
            if (!IsUniqueRange(range))
            {
                throw new ArgumentOutOfRangeException(nameof(range), "The specified range overlaps with one or more other ranges in this collection.");
            }

            return range;
        }

        private bool IsUniqueRange(TRangeType range)
        {
            if (range == null)
            {
                throw new ArgumentNullException(nameof(range));
            }

            return !(innerList.Any(range.Intersects));
        }
    }
}