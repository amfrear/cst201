using System;
using System.Collections.Generic;

namespace KnapsackExample
{
    /// <summary>
    /// Represents an item with a weight, a value, a computed value-to-weight ratio,
    /// and an original index (for identification in outputs).
    /// </summary>
    public class Item
    {
        public int Weight { get; set; }
        public int Value { get; set; }
        public double Ratio => (double)Value / Weight;
        public int Index { get; set; }  // Original index of the item

        public Item(int weight, int value, int index)
        {
            Weight = weight;
            Value = value;
            Index = index;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Data for the 0/1 knapsack problem
            int[] weights = { 20, 30, 40, 60, 70, 90 };
            int[] values = { 70, 80, 90, 110, 120, 200 };
            int capacity = 280;

            Console.WriteLine("=== 0/1 Knapsack Problem using Dynamic Programming (DP) ===\n");

            // Solve 0/1 knapsack using DP (with backtracking to find selected items)
            var (dpValue, dpSteps, dpSelectedIndices) = KnapsackDP01(weights, values, capacity);
            Console.WriteLine("DP -> Maximum Value: " + dpValue);
            Console.WriteLine("DP Steps (number of iterations): " + dpSteps);
            Console.WriteLine("DP Selected Items (by original index):");
            foreach (var index in dpSelectedIndices)
            {
                Console.WriteLine($"Item {index}: Weight = {weights[index]}, Value = {values[index]}");
            }
            Console.WriteLine("\n----------------------------------------------\n");

            Console.WriteLine("=== 0/1 Knapsack Problem using Greedy Technique ===\n");

            // Solve 0/1 knapsack using Greedy Technique (with detailed output)
            var (greedyValue, greedySteps, greedySelectedItems) = KnapsackGreedy01(weights, values, capacity);
            Console.WriteLine("\nGreedy -> Total Value: " + greedyValue);
            Console.WriteLine("Greedy Steps (number of iterations): " + greedySteps);
            Console.WriteLine("Greedy Selected Items (with details):");
            foreach (var item in greedySelectedItems)
            {
                Console.WriteLine($"Item {item.Index}: Weight = {item.Weight}, Value = {item.Value}, Ratio = {item.Ratio:F2}");
            }
            Console.WriteLine("\n----------------------------------------------\n");

            Console.WriteLine("=== Bounded Knapsack Problem using Dynamic Programming (DP) ===\n");

            // Data for the bounded knapsack problem (each item has a given quantity)
            int[] quantities = { 1, 2, 1, 3, 1, 2 };
            var (boundedValue, boundedSteps) = KnapsackDPBounded(weights, values, quantities, capacity);
            Console.WriteLine("Bounded DP -> Maximum Value: " + boundedValue);
            Console.WriteLine("Bounded DP Steps (number of iterations): " + boundedSteps);

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        /// <summary>
        /// Solves the 0/1 knapsack problem using dynamic programming.
        /// It builds a DP table where dp[i, w] is the maximum value using the first i items and capacity w.
        /// After the table is built, backtracking is used to reconstruct which items were selected.
        /// Returns a tuple: (maximum value, total steps, list of selected item indices).
        /// Complexity: O(n * capacity)
        /// </summary>
        static (int, int, List<int>) KnapsackDP01(int[] weights, int[] values, int capacity)
        {
            int n = weights.Length;
            int[,] dp = new int[n + 1, capacity + 1];
            int steps = 0;

            // Build the DP table
            for (int i = 1; i <= n; i++)
            {
                for (int w = 0; w <= capacity; w++)
                {
                    steps++; // Count each inner loop iteration
                    if (weights[i - 1] <= w)
                    {
                        dp[i, w] = Math.Max(dp[i - 1, w], dp[i - 1, w - weights[i - 1]] + values[i - 1]);
                    }
                    else
                    {
                        dp[i, w] = dp[i - 1, w];
                    }
                }
            }

            // Backtrack to determine which items were selected.
            List<int> selectedIndices = new List<int>();
            int remainingCapacity = capacity;
            for (int i = n; i > 0; i--)
            {
                // If the value differs from the row above, item i-1 was included.
                if (dp[i, remainingCapacity] != dp[i - 1, remainingCapacity])
                {
                    selectedIndices.Add(i - 1);
                    remainingCapacity -= weights[i - 1];
                }
            }
            selectedIndices.Reverse();  // Optional: reverse to show in original order

            return (dp[n, capacity], steps, selectedIndices);
        }

        /// <summary>
        /// Solves the 0/1 knapsack problem using a greedy approach.
        /// First, items are sorted by their value-to-weight ratio (descending order).
        /// Then, items are selected as long as the total weight does not exceed the capacity.
        /// Returns a tuple: (total value, total steps, list of selected items).
        /// Note: This approach works optimally for the fractional knapsack, but not necessarily for 0/1.
        /// Complexity: O(n log n) for sorting plus O(n) for selection.
        /// </summary>
        static (int, int, List<Item>) KnapsackGreedy01(int[] weights, int[] values, int capacity)
        {
            int n = weights.Length;
            List<Item> items = new List<Item>();
            for (int i = 0; i < n; i++)
            {
                items.Add(new Item(weights[i], values[i], i));
            }

            // Sort items by value-to-weight ratio in descending order.
            items.Sort((a, b) => b.Ratio.CompareTo(a.Ratio));

            // Output the sorted items for clarity.
            Console.WriteLine("Items sorted by value-to-weight ratio:");
            foreach (var item in items)
            {
                Console.WriteLine($"Item {item.Index}: Weight = {item.Weight}, Value = {item.Value}, Ratio = {item.Ratio:F2}");
            }

            int totalWeight = 0;
            int totalValue = 0;
            int steps = 0;
            List<Item> selectedItems = new List<Item>();

            Console.WriteLine("\nGreedy selection process:");
            foreach (var item in items)
            {
                steps++; // Each item considered counts as one step.
                if (totalWeight + item.Weight <= capacity)
                {
                    totalWeight += item.Weight;
                    totalValue += item.Value;
                    selectedItems.Add(item);
                    Console.WriteLine($"Selected -> Item {item.Index} (Weight: {item.Weight}, Value: {item.Value}). " +
                                      $"Cumulative Weight: {totalWeight}, Cumulative Value: {totalValue}");
                }
                else
                {
                    Console.WriteLine($"Skipped -> Item {item.Index} (Weight: {item.Weight}, Value: {item.Value}) " +
                                      $"because adding it would exceed capacity.");
                }
            }

            Console.WriteLine($"\nFinal Total Weight: {totalWeight}, Final Total Value: {totalValue}");
            return (totalValue, steps, selectedItems);
        }

        /// <summary>
        /// Solves the bounded knapsack problem using dynamic programming.
        /// Each item i can be taken up to quantities[i] times.
        /// This method uses a 1D DP array and processes each copy of an item separately.
        /// Returns a tuple: (maximum value, total steps).
        /// Complexity: O((total copies of items) * capacity)
        /// </summary>
        static (int, int) KnapsackDPBounded(int[] weights, int[] values, int[] quantities, int capacity)
        {
            int[] dp = new int[capacity + 1];
            int steps = 0;
            int n = weights.Length;

            // Process each item and each of its copies.
            for (int i = 0; i < n; i++)
            {
                // For each copy of item i, process in reverse order to prevent reusing the same copy.
                for (int q = 0; q < quantities[i]; q++)
                {
                    for (int w = capacity; w >= weights[i]; w--)
                    {
                        steps++;
                        dp[w] = Math.Max(dp[w], dp[w - weights[i]] + values[i]);
                    }
                }
            }
            return (dp[capacity], steps);
        }
    }
}
