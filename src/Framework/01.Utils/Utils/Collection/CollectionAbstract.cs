using System.Collections;
using System.Collections.Generic;

namespace Wyn.Utils.Collection
{
    public abstract class CollectionAbstract<T> : IList<T>
    {
        protected readonly List<T> Collection = new();

        public T this[int index]
        {
            get => Collection[index];
            set => Collection[index] = value;
        }

        public bool IsReadOnly { get; } = true;

        public IEnumerator<T> GetEnumerator() => Collection.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public void Add(T item) => Collection.Add(item);

        public void Clear() => Collection.Clear();

        public bool Contains(T item) => Collection.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => Collection.CopyTo(array, arrayIndex);

        public bool Remove(T item) => Collection.Remove(item);

        public int Count => Collection.Count;

        public int IndexOf(T item) => Collection.IndexOf(item);

        public void Insert(int index, T item) => Collection.Insert(index, item);

        public void RemoveAt(int index) => Collection.RemoveAt(index);

    }
}
