using System;
using System.Collections.Generic;

namespace PriorityQueueDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            // A priority queue is a special type of queue where elements are served based on their priority rather than the order they were added.
            // In this example, I'm simulating an emergency room triage system using a priority queue.

            // Create a Priority Queue using a SortedDictionary
            SortedDictionary<int, Queue<string>> priorityQueue = new SortedDictionary<int, Queue<string>>();


            // I'm using a SortedDictionary where the key represents priority (lower numbers indicate higher priority).
            // Each key holds a queue of patients with the same priority level.

            // Enqueue patients (priority: lower number = higher priority)
            Enqueue(priorityQueue, 1, "Heart Attack");  // Highest priority
            Enqueue(priorityQueue, 3, "Broken Arm");    // Medium priority
            Enqueue(priorityQueue, 2, "Fever");         // Lower priority than heart attack but higher than broken arm


            // I've added three patients: one with a heart attack (priority 1), one with a fever (priority 2), and one with a broken arm (priority 3).
            // Since the heart attack is the most critical, it should be treated first.

            // Dequeue and display patients in priority order
            Console.WriteLine("Processing patients:");
            while (priorityQueue.Count > 0)
            {
                string patient = Dequeue(priorityQueue);
                Console.WriteLine(patient);
            }

            // As expected, the heart attack patient is treated first, followed by the fever patient, and then the broken arm.
            // This shows how a priority queue ensures that more urgent tasks are handled first.
            // In a real-world scenario, this could be implemented using a Min-Heap for better performance.
            // That's a quick look at how priority queues work!
        }

        static void Enqueue(SortedDictionary<int, Queue<string>> pq, int priority, string value)
        {
            // To add a patient to the priority queue, we check if a queue already exists for that priority.
            // If not, we create one, then we add the patient's name to that queue.

            if (!pq.ContainsKey(priority))
            {
                pq[priority] = new Queue<string>();
            }
            pq[priority].Enqueue(value);
        }

        static string Dequeue(SortedDictionary<int, Queue<string>> pq)
        {
            // To process a patient, we find the highest-priority queue (the lowest key value).
            // We remove and return the first patient from that queue.
            // If no patients remain at that priority level, we remove that queue from the dictionary.

            if (pq.Count == 0)
                throw new InvalidOperationException("The queue is empty.");

            var firstKey = pq.Keys.Min();  // Find the highest priority (smallest key)
            var value = pq[firstKey].Dequeue(); // Get the first patient in that priority queue

            if (pq[firstKey].Count == 0)
            {
                pq.Remove(firstKey); // Remove the empty queue
            }

            return value;
        }
    }
}
