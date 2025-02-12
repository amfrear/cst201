using System;
using System.Collections.Generic;

namespace PriorityQueueHeap
{
    // Welcome to my Priority Queue implementation using a Binary Heap.
    // A priority queue is a data structure that allows us to process elements based on their priority.
    // The highest (or lowest) priority element is always processed first.

    public class PriorityQueue<T> where T : IComparable<T>
    {
        // We store the elements of our priority queue in a list.
        private List<T> heap = new List<T>();

        // This variable determines whether we are using a Min-Heap or a Max-Heap.
        // If true, it is a Min-Heap (lower values have higher priority).
        // If false, it is a Max-Heap (higher values have higher priority).
        private bool isMinHeap;

        // Constructor: We can create either a Min-Heap or a Max-Heap.
        public PriorityQueue(bool minHeap = true)
        {
            isMinHeap = minHeap;
        }

        // These methods help us navigate the heap.
        // The parent index is calculated by (index - 1) / 2.
        private int Parent(int index) => (index - 1) / 2;

        // The left child index is at 2 * index + 1.
        private int LeftChild(int index) => 2 * index + 1;

        // The right child index is at 2 * index + 2.
        private int RightChild(int index) => 2 * index + 2;

        // This method compares two elements to determine their priority.
        // If we are using a Min-Heap, smaller values should come first.
        // If we are using a Max-Heap, larger values should come first.
        private bool Compare(T a, T b)
        {
            return isMinHeap ? a.CompareTo(b) < 0 : a.CompareTo(b) > 0;
        }

        // The Enqueue method adds an element to the priority queue.
        public void Enqueue(T item)
        {
            // First, we add the item to the end of the heap.
            heap.Add(item);
            // Then, we restore the heap order by moving the item up if necessary.
            HeapifyUp(heap.Count - 1);
        }

        // The Dequeue method removes and returns the element with the highest priority.
        public T Dequeue()
        {
            // If the heap is empty, we throw an error.
            if (heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");

            // We store the highest priority element, which is at the root (index 0).
            T root = heap[0];

            // We replace the root with the last element in the heap.
            heap[0] = heap[heap.Count - 1];

            // We remove the last element since it's now at the root.
            heap.RemoveAt(heap.Count - 1);

            // We restore the heap order by moving the root element down if necessary.
            HeapifyDown(0);

            // Finally, we return the element that was removed.
            return root;
        }

        // The Peek method returns the highest-priority element without removing it.
        public T Peek()
        {
            if (heap.Count == 0)
                throw new InvalidOperationException("Priority queue is empty.");
            return heap[0];
        }

        // This property returns the number of elements in the queue.
        public int Count => heap.Count;

        // The HeapifyUp method moves an element up the heap to maintain order.
        private void HeapifyUp(int index)
        {
            // While the element is not the root and has a higher priority than its parent...
            while (index > 0 && Compare(heap[index], heap[Parent(index)]))
            {
                // Swap it with its parent.
                Swap(index, Parent(index));
                // Move up to the parent's index and continue.
                index = Parent(index);
            }
        }

        // The HeapifyDown method moves an element down the heap to maintain order.
        private void HeapifyDown(int index)
        {
            int left = LeftChild(index);
            int right = RightChild(index);
            int smallestOrLargest = index;

            // If the left child has a higher priority, update the index.
            if (left < heap.Count && Compare(heap[left], heap[smallestOrLargest]))
                smallestOrLargest = left;

            // If the right child has an even higher priority, update the index.
            if (right < heap.Count && Compare(heap[right], heap[smallestOrLargest]))
                smallestOrLargest = right;

            // If the highest priority element is not the current node, swap and continue heapifying.
            if (smallestOrLargest != index)
            {
                Swap(index, smallestOrLargest);
                HeapifyDown(smallestOrLargest);
            }
        }

        // The Swap method exchanges two elements in the heap.
        private void Swap(int i, int j)
        {
            T temp = heap[i];
            heap[i] = heap[j];
            heap[j] = temp;
        }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Priority Queue Example with a Heap:");

            // We define an array of elements to insert into the priority queue.
            int[] elements = { 5, 2, 8, 1, 7 };

            // First, we create a Min-Heap priority queue.
            // In a Min-Heap, the smallest element always has the highest priority.
            PriorityQueue<int> minHeap = new PriorityQueue<int>(minHeap: true);

            // Next, we create a Max-Heap priority queue.
            // In a Max-Heap, the largest element always has the highest priority.
            PriorityQueue<int> maxHeap = new PriorityQueue<int>(minHeap: false);

            // We insert elements into both the Min-Heap and the Max-Heap.
            foreach (var item in elements)
            {
                minHeap.Enqueue(item);
                maxHeap.Enqueue(item);
            }

            // Now, let's display the elements from the Min-Heap.
            Console.WriteLine("\nMin-Heap (Dequeuing in priority order):");
            while (minHeap.Count > 0)
            {
                // The elements should come out in ascending order.
                Console.Write(minHeap.Dequeue() + " ");
            }

            // Now, let's display the elements from the Max-Heap.
            Console.WriteLine("\n\nMax-Heap (Dequeuing in priority order):");
            while (maxHeap.Count > 0)
            {
                // The elements should come out in descending order.
                Console.Write(maxHeap.Dequeue() + " ");
            }

            Console.WriteLine(); // New line for readability
        }
    }
}
