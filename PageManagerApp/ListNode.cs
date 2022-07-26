using System.Collections.Generic;

namespace PageManagerApp
{
    /// <summary>
    ///     Class representation of ListNode of type T
    ///     used in the DoublyLinkedList implementation.
    /// </summary>
    /// <typeparam name="T">Data type of a literal or class structure</typeparam>
    internal class ListNode<T>
    {
        public ListNode(T data)
        {
            Data = data;
            Next = null;
            Previous = null;
        }

        public T Data { get; }
        public ListNode<T> Next { get; set; }
        public ListNode<T> Previous { get; set; }


        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ListNode<T>) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = EqualityComparer<T>.Default.GetHashCode(Data);
                hashCode = (hashCode * 397) ^ (Next != null ? Next.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (Previous != null ? Previous.GetHashCode() : 0);
                return hashCode;
            }
        }

        public static bool operator ==(ListNode<T> left, ListNode<T> right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ListNode<T> left, ListNode<T> right)
        {
            return !Equals(left, right);
        }

        protected bool Equals(ListNode<T> other)
        {
            return EqualityComparer<T>.Default.Equals(Data, other.Data) && Equals(Next, other.Next) &&
                   Equals(Previous, other.Previous);
        }
    }
}