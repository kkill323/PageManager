using System;
using System.Collections.Generic;

namespace PageManagerApp
{
    /// <summary>
    ///     Data Structure that contains a set of sequentially
    ///     linked records with a reference to the previous and
    ///     next items.  The list is terminated in the front
    ///     and back with a node value of null.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    internal class DoublyLinkedList<T>

    {
        private readonly Dictionary<T, ListNode<T>> _lookupDictionary;

        /// <summary>
        ///     Creates an empty list of specified capacity
        /// </summary>
        /// <param name="capacity">Maximum size of the list</param>
        public DoublyLinkedList(int capacity = 10)
        {
            Capacity = capacity;
            _lookupDictionary = new Dictionary<T, ListNode<T>>(capacity);
        }

        public int Capacity { get; }
        public int Count { get; private set; }
        public ListNode<T> FirstNode { get; private set; }

        public ListNode<T> LastNode { get; private set; }

        public bool IsEmpty => Count == 0;
        public bool IsFull => Count == Capacity;

        /// <summary>
        ///     Adds a record to the end of the list.
        /// </summary>
        /// <param name="value">Item to store in the list</param>
        public void Append(T value)
        {
            CheckIfFull();
            var node = new ListNode<T>(value);

            if (IsEmpty)
                SetDefaultState(node);
            else
                MakeLastNode(node);

            AddToLookup(value, node);
            // Increment item counter
            ++Count;
        }

        public void CheckIfFull()
        {
            if (Count + 1 > Capacity)
                throw new IndexOutOfRangeException("The list is full.");
        }

        /// <summary>
        ///     Removes all items in the list.
        /// </summary>
        public void Clear()
        {
            if (IsEmpty)
                return;

            _lookupDictionary.Clear();

            var current = FirstNode;
            while (current != null)
            {
                Remove(current);

                current = current.Next;
            }

            Count = 0;
        }

        /// <summary>
        ///     Determines if key is in list.
        /// </summary>
        /// <param name="value">Item to search for</param>
        /// <returns></returns>
        public bool Contains(T value)
        {
            return _lookupDictionary.ContainsKey(value);
        }

        /// <summary>
        ///     Finds a node based on the specified data
        /// </summary>
        /// <param name="value">Item to search for</param>
        /// <returns>Returns item, or null if not found</returns>
        public ListNode<T> Find(T value)
        {
            return _lookupDictionary.TryGetValue(value, out var item) ? item : null;
        }

        /// <summary>
        ///     Inserts a record after the specified node.
        /// </summary>
        /// <param name="node">Reference to the item in the list</param>
        /// <param name="value">Item to store in list</param>
        public void InsertAfter(ListNode<T> node, T value)
        {
            CheckIfFull();

            if (node.Equals(LastNode))
            {
                Append(value);
            }
            else
            {
                var newNode = new ListNode<T>(value) {Next = node.Next, Previous = node};
                node.Next = newNode;
            }

            AddToLookup(value, node);
            ++Count;
        }


        /// <summary>
        ///     Inserts a record before the specified node.
        /// </summary>
        /// <param name="node">Reference to the item in the list</param>
        /// <param name="value">Item to store in list</param>
        public void InsertBefore(ListNode<T> node, T value)
        {
            CheckIfFull();
            if (node.Equals(FirstNode))
            {
                Prepend(value);
            }
            else
            {
                var newNode = new ListNode<T>(value) {Previous = node.Previous, Next = node};
                node.Previous = newNode;
            }

            AddToLookup(value, node);
            ++Count;
        }

        /// <summary>
        ///     Move an existing node to the end of the list
        /// </summary>
        /// <param name="node"></param>
        public void MoveToBack(ListNode<T> node)
        {
            if (node.Equals(LastNode))
                return;

            if (node.Equals(FirstNode))
            {
                // Reset FirstNode
                node.Next.Previous = null;
                FirstNode = node.Next;
            }
            else
            {
                //Link Nodes
                LinkNodeExtents(node);
            }

            MakeLastNode(node);
        }

        public void MoveToBack(T value)
        {
            var node = Find(value);
            if (node != null)
                MoveToBack(node);
        }

        /// <summary>
        ///     Moves an existing node to the front of the list
        /// </summary>
        /// <param name="node"></param>
        public void MoveToFront(ListNode<T> node)

        {
            if (node.Equals(FirstNode))
                return;

            if (node.Equals(LastNode))
            {
                // Reset LastNode
                node.Previous.Next = null;
                LastNode = node.Previous;
            }
            else
            {
                //Link Nodes
                LinkNodeExtents(node);
            }

            MakeLastNode(node);
        }

        public void MoveToFront(T value)
        {
            var node = Find(value);
            if (node != null)
                MoveToFront(node);
        }

        /// <summary>
        ///     Adds a record to the head of the list
        /// </summary>
        /// <param name="value">The item to store in the list.</param>
        public void Prepend(T value)
        {
            CheckIfFull();
            var node = new ListNode<T>(value);

            if (IsEmpty)
                SetDefaultState(node);
            else
                MakeFirstNode(node);

            AddToLookup(value, node);
            ++Count;
        }

        /// <summary>
        ///     Display list contents to console
        /// </summary>
        public void Print(string header = "List Print")
        {
            Console.WriteLine(header.BuildFramedHeader());
            var current = FirstNode;
            while (current != null)
            {
                Console.WriteLine(current.Data.ToString());
                current = current.Next;
            }

            Console.WriteLine("END");
        }

        /// <summary>
        ///     Removes an item from the list
        /// </summary>
        /// <param name="node">The node to remove from list</param>
        public void Remove(ListNode<T> node)
        {
            // Exit if null key passed in.
            if (node == null)
                return;

            RemoveFromLookup(node.Data);

            // If there is only one node remove it
            // and reset First and Last node reference
            if (node.Equals(FirstNode) && node.Equals(LastNode))
            {
                node = null;
                FirstNode = null;
                LastNode = null;
                Count = 0;
                return;
            }

            // Reset FirstNode reference if removing First Node
            if (node.Equals(FirstNode))
            {
                node.Next.Previous = null;
                FirstNode = node.Next;
                node = null;
                --Count;
                return;
            }

            // Reset LastNode reference if removing Last Node
            if (node.Equals(LastNode))
            {
                node.Previous.Next = null;
                LastNode = node.Previous;
                node = null;
                --Count;
                return;
            }

            // Connect Previous and Next nodes before removing
            LinkNodeExtents(node);


            --Count;
            node = null;
        }

        /// <summary>
        ///     Remove an item by its key
        /// </summary>
        /// <param name="value">Item to remove</param>
        public void RemoveByValue(T value)
        {
            var node = Find(value);
            Remove(node);
        }

        /// <summary>
        ///     Populates Lookup table
        /// </summary>
        /// <param name="key">Lookup up table key</param>
        /// <param name="node">Lookup table key</param>
        private void AddToLookup(T key, ListNode<T> node)
        {
            if (!_lookupDictionary.ContainsKey(key))
                _lookupDictionary.Add(key, node);
        }

        private static void LinkNodeExtents(ListNode<T> node)
        {
            node.Previous.Next = node.Next;
            node.Next.Previous = node.Previous;
        }

        private void MakeFirstNode(ListNode<T> node)
        {
            // Add to begin of list
            var temp = FirstNode;
            temp.Previous = node;
            node.Next = temp;
            node.Previous = null;
            FirstNode = node;
        }

        private void MakeLastNode(ListNode<T> node)
        {
            // Set Last Node;
            var temp = LastNode;
            temp.Next = node;
            node.Previous = temp;
            node.Next = null;
            LastNode = node;
        }

        /// <summary>
        ///     Removes item from Lookup table
        /// </summary>
        /// <param name="key">Lookup up table key</param>
        private void RemoveFromLookup(T key)
        {
            if (_lookupDictionary.ContainsKey(key))
                _lookupDictionary.Remove(key);
        }

        /// <summary>
        ///     Sets up the list state after first item is added
        /// </summary>
        /// <param name="node">Item to make the first item</param>
        private void SetDefaultState(ListNode<T> node)
        {
            FirstNode = node;
            LastNode = node;
        }
    }
}