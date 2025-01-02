// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

// This is a template and not intended to be used as is
// This template List<Something> will be concerted into ListSomething
// Once generated, this will be usable as a normal ListSomething

using Iot.Device.Buzzer.Samples;
using System;

namespace System.Collections.Generic
{
    /// <summary>
    /// List class for type MelodyElement that has been automatically generated
    /// </summary>
    public class ListMelodyElement : IEnumerable
    {
        private ArrayList _list;

        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.List class that
        /// is empty and has the default initial capacity.
        /// </summary>
        public ListMelodyElement()
        {
            _list = new ArrayList();
        }

        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.List class that
        /// contains elements copied from the specified collection and has sufficient capacity
        /// to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException">collection is null</exception>
        public ListMelodyElement(IEnumerable collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            _list = new ArrayList();
            foreach (var elem in collection)
            {
                _list.Add(elem);
            }
        }

        /// <summary>
        /// Initializes a new instance of the System.Collections.Generic.List class that
        /// is empty and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">The number of elements that the new list can initially store.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">capacity is less than 0.</exception>
        public ListMelodyElement(int capacity)
        {
            if (capacity < 0)
            {
                throw new ArgumentOutOfRangeException();
            }
            _list = new ArrayList();
            _list.Capacity = capacity;
        }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- index is equal to or greater than System.Collections.Generic.List.Count.</exception>
        public MelodyElement this[int index]
        {
            get
            {
                if ((index < 0) || (index >= _list.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }

                return (MelodyElement)_list[index];
            }

            set
            {
                if ((index < 0) || (index >= _list.Count))
                {
                    throw new ArgumentOutOfRangeException();
                }

                _list[index] = value;
            }
        }

        /// <summary>
        /// Gets the number of elements contained in the System.Collections.Generic.List
        /// </summary>
        public int Count => _list.Count;

        /// <summary>
        /// Gets or sets the total number of elements the internal data structure can hold
        /// without resizing.
        /// </summary>
        /// <exception cref="System.ArgumentOutOfRangeException">System.Collections.Generic.List.Capacity is set to a value that is less than System.Collections.Generic.List.Count</exception>
        public int Capacity
        {
            get => _list.Capacity;

            set
            {
                if (value < _list.Count)
                {
                    throw new ArgumentOutOfRangeException();
                }

                _list.Capacity = value;
            }
        }

        /// <summary>
        /// Adds an object to the end of the System.Collections.Generic.List.
        /// </summary>
        /// <param name="item">The object to be added to the end of the System.Collections.Generic.List. The value can be null for reference types.</param>
        public void Add(MelodyElement item)
        {
            _list.Add(item);
        }

        /// <summary>
        /// Adds the elements of the specified collection to the end of the System.Collections.Generic.List.
        /// </summary>
        /// <param name="collection">The collection whose elements should be added to the end of the System.Collections.Generic.List.
        /// The collection itself cannot be null, but it can contain elements that are null, if type MelodyElement is a reference type.
        /// </param>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        public void AddRange(IEnumerable collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            foreach (var elem in collection)
            {
                _list.Add(elem);
            }
        }

        /// <summary>
        /// Returns an enumerator that iterates through the System.Collections.Generic.List.
        /// </summary>
        /// <returns>A System.Collections.Generic.List.Enumerator for the System.Collections.Generic.List.</returns>
        public IEnumerator GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        /// <summary>
        /// Enumerates the elements of a System.Collections.Generic.List.
        /// </summary>
        public struct Enumerator : IEnumerator, IDisposable
        {
            private int _index;
            private ListMelodyElement _collection;

            /// <summary>
            /// Create an enumerator of the collection
            /// </summary>
            /// <param name="collection"></param>
            public Enumerator(ListMelodyElement collection)
            {
                _index = -1;
                _collection = collection;
            }

            /// <summary>
            /// Gets the element at the current position of the enumerator.
            /// </summary>
            public MelodyElement Current => _collection[_index == -1 ? 0 : _index];

            object IEnumerator.Current => Current;

            /// <summary>
            /// Releases all resources used by the System.Collections.Generic.List.Enumerator.
            /// </summary>
            public void Dispose()
            { }

            /// <summary>
            /// Advances the enumerator to the next element of the System.Collections.Generic.List.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element; false if
            /// the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                if ((_index + 1) >= _collection.Count)
                {
                    return false;
                }

                _index++;
                return true;
            }

            /// <summary>
            /// Move back to first position
            /// </summary>
            public void Reset()
            {
                _index = -1;
            }
        }

        /// <summary>
        /// Determines whether an element is in the System.Collections.Generic.List.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <returns>true if item is found in the System.Collections.Generic.List; otherwise, false.</returns>
        public bool Contains(MelodyElement item)
        {
            foreach (var elem in _list)
            {
                if (((MelodyElement)elem).GetHashCode() == item.GetHashCode())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Copies the entire System.Collections.Generic.List to a compatible one-dimensional
        /// array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied
        /// from System.Collections.Generic.List. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">arrayIndex is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source System.Collections.Generic.List is greater
        /// than the available space from arrayIndex to the end of the destination array.</exception>
        public void CopyTo(MelodyElement[] array, int arrayIndex)
        {
            CopyTo(0, array, arrayIndex, _list.Count);
        }

        /// <summary>
        /// Copies the entire System.Collections.Generic.List to a compatible one-dimensional
        /// array, starting at the beginning of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied
        /// from System.Collections.Generic.List. The System.Array must have zero-based indexing.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source System.Collections.Generic.List is greater
        /// than the number of elements that the destination array can contain.</exception>
        public void CopyTo(MelodyElement[] array)
        {
            CopyTo(0, array, 0, _list.Count);
        }

        /// <summary>
        /// Copies a range of elements from the System.Collections.Generic.List to a compatible
        /// one-dimensional array, starting at the specified index of the target array.
        /// </summary>
        /// <param name="index">The zero-based index in the source System.Collections.Generic.List at which
        /// copying begins.</param>
        /// <param name="array">The one-dimensional System.Array that is the destination of the elements copied
        /// from System.Collections.Generic.List. The System.Array must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in array at which copying begins.</param>
        /// <param name="count">The number of elements to copy.</param>
        /// <exception cref="System.ArgumentNullException">array is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- arrayIndex is less than 0. -or- count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index is equal to or greater than the System.Collections.Generic.List.Count
        /// of the source System.Collections.Generic.List. -or- The number of elements
        /// from index to the end of the source System.Collections.Generic.List is greater than the available space from arrayIndex to the end of the destination array.</exception>
        public void CopyTo(int index, MelodyElement[] array, int arrayIndex, int count)
        {
            if (array == null)
            {
                throw new ArgumentNullException();
            }

            if ((index < 0) || (arrayIndex < 0) || (count < 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            if ((index >= _list.Count) || (_list.Count - index < count) || (count > array.Length - arrayIndex) || (arrayIndex + count > array.Length))
            {
                throw new ArgumentException();
            }
            for (int i = index; i < count; i++)
            {
                array[arrayIndex + i] = (MelodyElement)_list[i];
            }
        }

        /// <summary>
        /// Creates a shallow copy of a range of elements in the source System.Collections.Generic.List.
        /// </summary>
        /// <param name="index">The zero-based System.Collections.Generic.List index at which the range starts.</param>
        /// <param name="count">The number of elements in the range.</param>
        /// <returns> A shallow copy of a range of elements in the source System.Collections.Generic.List.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the System.Collections.Generic.List.</exception>
        public ListMelodyElement GetRange(int index, int count)
        {
            if ((index < 0) || (count < 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (count > _list.Count - index)
            {
                throw new ArgumentException();
            }

            var list = new ListMelodyElement();
            for (int i = index; i < count; i++)
            {
                list.Add((MelodyElement)_list[i]);
            }

            return list;
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the range of elements in the System.Collections.Generic.List
        /// that starts at the specified index and contains the specified number of elements.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements
        /// in the System.Collections.Generic.List that starts at index and contains count
        /// number of elements, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the System.Collections.Generic.List.
        /// -or- count is less than 0. -or- index and count do not specify a valid section
        /// in the System.Collections.Generic.List.</exception>
        public int IndexOf(MelodyElement item, int index, int count) => _list.IndexOf(item, index, count);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the range of elements in the System.Collections.Generic.List
        /// that extends from the specified index to the last element.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the search. 0 (zero) is valid in an empty list.</param>
        /// <returns>The zero-based index of the first occurrence of item within the range of elements
        /// in the System.Collections.Generic.List that extends from index to the last
        /// element, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the System.Collections.Generic.List.</exception>
        public int IndexOf(MelodyElement item, int index) => IndexOf(item, index, _list.Count - index);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the first
        /// occurrence within the entire System.Collections.Generic.List.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the first occurrence of item within the entire System.Collections.Generic.List, if found; otherwise, -1.</returns>
        public int IndexOf(MelodyElement item) => IndexOf(item, 0, _list.Count);

        /// <summary>
        /// Inserts an element into the System.Collections.Generic.List at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which item should be inserted.</param>
        /// <param name="item">The object to insert. The value can be null for reference types.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- index is greater than System.Collections.Generic.List.Count.</exception>
        public void Insert(int index, MelodyElement item) => _list.Insert(index, item);

        /// <summary>
        /// Inserts the elements of a collection into the System.Collections.Generic.List at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which the new elements should be inserted.</param>
        /// <param name="collection">The collection whose elements should be inserted into the System.Collections.Generic.List. The collection itself cannot be null, but it can contain elements that are null, if type MelodyElement is a reference type.</param>
        /// <exception cref="System.ArgumentNullException">collection is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- index is greater than System.Collections.Generic.List.Count.</exception>
        public void InsertRange(int index, IEnumerable collection)
        {
            if (collection == null)
            {
                throw new ArgumentNullException();
            }

            if ((index < 0) || (index > _list.Count))
            {
                throw new ArgumentOutOfRangeException();
            }

            foreach (var elem in collection)
            {
                _list.Insert(index++, elem);
            }
        }

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last
        /// occurrence within the entire System.Collections.Generic.List.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <returns>The zero-based index of the last occurrence of item within the entire the System.Collections.Generic.List, if found; otherwise, -1.</returns>
        public int LastIndexOf(MelodyElement item) => LastIndexOf(item, _list.Count - 1, _list.Count);
 
        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last
        /// occurrence within the range of elements in the System.Collections.Generic.List
        /// that extends from the first element to the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements
        /// in the System.Collections.Generic.List that extends from the first element
        /// to index, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the System.Collections.Generic.List.</exception>
        public int LastIndexOf(MelodyElement item, int index) => LastIndexOf(item, index, _list.Count - index);

        /// <summary>
        /// Searches for the specified object and returns the zero-based index of the last
        /// occurrence within the range of elements in the System.Collections.Generic.List
        /// that contains the specified number of elements and ends at the specified index.
        /// </summary>
        /// <param name="item">The object to locate in the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <param name="index">The zero-based starting index of the backward search.</param>
        /// <param name="count">The number of elements in the section to search.</param>
        /// <returns>The zero-based index of the last occurrence of item within the range of elements
        /// in the System.Collections.Generic.List that contains count number of elements
        /// and ends at index, if found; otherwise, -1.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException">index is outside the range of valid indexes for the System.Collections.Generic.List. -or- count is less than 0. -or- index and count do not specify a valid section in the System.Collections.Generic.List.</exception>
        public int LastIndexOf(MelodyElement item, int index, int count)
        {
            if ((index < 0) || (count < 0) || (index + count > _list.Count))
            {
                throw new ArgumentOutOfRangeException();
            }

            for (int i = index; i >= _list.Count - count - index; i--)
            {
                if (((MelodyElement)_list[i]).GetHashCode() == item.GetHashCode())
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object from the System.Collections.Generic.List.
        /// </summary>
        /// <param name="item">The object to remove from the System.Collections.Generic.List. The value can be null for reference types.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns
        /// false if item was not found in the System.Collections.Generic.List.</returns>
        public bool Remove(MelodyElement item)
        {
            if (_list.Contains(item))
            {
                _list.Remove(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Removes the element at the specified index of the System.Collections.Generic.List.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        public void RemoveAt(int index) => _list.RemoveAt(index);

        /// <summary>
        /// Removes a range of elements from the System.Collections.Generic.List.
        /// </summary>
        /// <param name="index">The zero-based starting index of the range of elements to remove.</param>
        /// <param name="count">The number of elements to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException">index is less than 0. -or- count is less than 0.</exception>
        /// <exception cref="System.ArgumentException">index and count do not denote a valid range of elements in the System.Collections.Generic.List.</exception>
        public void RemoveRange(int index, int count)
        {
            if ((index < 0) || (count < 0))
            {
                throw new ArgumentOutOfRangeException();
            }

            if (index + count > _list.Count)
            {
                throw new ArgumentException();
            }

            for (int i = 0; i < count; i++)
            {
                _list.RemoveAt(index);
            }
        }

        /// <summary>
        /// Copies the elements of the System.Collections.Generic.List to a new array.
        /// </summary>
        /// <returns> An array containing copies of the elements of the System.Collections.Generic.List.</returns>
        public MelodyElement[] ToArray()
        {
            MelodyElement[] array = new MelodyElement[_list.Count];
            for (int i = 0; i < array.Length; i++)
            {
                array[i] = (MelodyElement)_list[i];
            }

            return array;
        }

    }
}
