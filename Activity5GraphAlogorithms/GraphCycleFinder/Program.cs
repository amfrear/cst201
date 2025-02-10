using System;
using System.Collections.Generic;

namespace GraphCycleFinder
{
    class Program
    {
        // The weight matrix representing the directed graph.
        static float[,] W = new float[,]
        {
            { 0f, 3f, float.PositiveInfinity, float.PositiveInfinity, 1f },
            { float.PositiveInfinity, 0f, 6f, float.PositiveInfinity, 3f },
            { 1f, float.PositiveInfinity, 0f, float.PositiveInfinity, float.PositiveInfinity },
            { -4f, float.PositiveInfinity, 5f, 0f, float.PositiveInfinity },
            { float.PositiveInfinity, float.PositiveInfinity, 2f, 2f, 0f }
        };

        // Number of vertices in the graph.
        static int numVertices = 5;

        // Global variables for tracking the minimum cycle weight and cycles with that weight.
        static float minCycleWeight = float.PositiveInfinity;
        static List<List<int>> minCycles = new List<List<int>>();

        // Additional counters to provide extra details about the algorithm’s work.
        static int totalCyclesFound = 0;     // How many cycles were detected
        static int totalDFSCalls = 0;        // How many DFS calls (attempts) were made
        static int totalComparisons = 0;     // A rough count of comparisons made in the DFS
        static int totalDataExchanges = 0;   // A rough count of data exchanges (like adding/removing from lists)

        // Recursive DFS (Depth-First Search) method to explore all possible cycles.
        // 'start' is the vertex where the search began.
        // 'current' is the current vertex.
        // 'currentWeight' holds the running sum of edge weights.
        // 'path' records the current sequence of vertices.
        // 'usedEdges' tracks which edges have been used so far in this path.
        static void DFS(int start, int current, float currentWeight, List<int> path, HashSet<(int, int)> usedEdges)
        {
            totalDFSCalls++;  // Count this DFS call
            // Iterate over all possible next vertices.
            for (int next = 0; next < numVertices; next++)
            {
                totalComparisons++; // For checking each vertex option.

                // Check if there is an edge from 'current' to 'next'.
                float weightEdge = W[current, next];
                if (float.IsPositiveInfinity(weightEdge))
                    continue;  // No edge exists, skip.

                // Skip if this edge is already used in the current path.
                if (usedEdges.Contains((current, next)))
                    continue;

                // To ensure a simple cycle, do not revisit a vertex (except when closing the cycle by returning to 'start').
                if (next != start && path.Contains(next))
                    continue;

                float newWeight = currentWeight + weightEdge;

                // "Exchange" data: add the next vertex to the path and mark the edge as used.
                path.Add(next);
                usedEdges.Add((current, next));
                totalDataExchanges++;

                // If we've returned to the starting vertex and the path length is greater than 1, a cycle is found.
                if (next == start && path.Count > 1)
                {
                    totalCyclesFound++; // A cycle is detected.
                    // Check if this cycle has a lower weight than the best found so far.
                    if (newWeight < minCycleWeight)
                    {
                        minCycleWeight = newWeight;
                        minCycles.Clear();
                        minCycles.Add(new List<int>(path));
                    }
                    else if (newWeight == minCycleWeight)
                    {
                        minCycles.Add(new List<int>(path));
                    }
                }
                else
                {
                    // Continue exploring from the 'next' vertex.
                    DFS(start, next, newWeight, path, usedEdges);
                }

                // Backtracking: remove the edge and vertex, so other paths can be explored.
                usedEdges.Remove((current, next));
                path.RemoveAt(path.Count - 1);
            }
        }

        static void Main(string[] args)
        {
            // Start DFS from each vertex so that every possible cycle is considered.
            for (int i = 0; i < numVertices; i++)
            {
                DFS(i, i, 0f, new List<int> { i }, new HashSet<(int, int)>());
            }

            // Output the minimum cycle weight and the cycles with that weight.
            Console.WriteLine("Minimum cycle weight: " + minCycleWeight);
            Console.WriteLine("Cycles with minimum weight:");
            foreach (var cycle in minCycles)
            {
                Console.WriteLine(string.Join(" -> ", cycle));
            }

            // Output additional statistics.
            Console.WriteLine("\nAdditional Information:");
            Console.WriteLine("Total number of cycles found: " + totalCyclesFound);
            Console.WriteLine("Total DFS calls (attempts): " + totalDFSCalls);
            Console.WriteLine("Total comparisons made: " + totalComparisons);
            Console.WriteLine("Total data exchanges: " + totalDataExchanges);

            Console.WriteLine("\nPress any key to exit.");
            Console.ReadKey();
        }
    }
}
