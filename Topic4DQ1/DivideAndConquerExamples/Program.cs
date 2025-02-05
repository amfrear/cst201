using System;
using System.Diagnostics;
using System.Linq;

namespace DivideAndConquerExamples
{
    // This program demonstrates several divide-and-conquer algorithms:
    // Merge Sort, Quick Sort, Binary Search, and Strassen’s Matrix Multiplication.
    internal class Program
    {
        // A single Random instance used throughout the program to avoid repeated reseeding.
        static Random rand = new Random();

        // The main entry point of the program.
        static void Main(string[] args)
        {
            Console.WriteLine("=== Running All Divide and Conquer Algorithms with Timing ===\n");

            // Each of these methods runs a particular algorithm and times its execution.
            RunMergeSort();
            RunQuickSort();
            RunBinarySearch();
            RunStrassenMatrixMultiplication();

            Console.WriteLine("\n=== All Algorithms Executed Successfully ===");
        }

        // -------------------------------------------------------------------
        // MERGE SORT IMPLEMENTATION
        // -------------------------------------------------------------------

        // RunMergeSort generates a random array, sorts it using Merge Sort,
        // and prints out the execution time.
        static void RunMergeSort()
        {
            // Generate an array of 10,000 random integers.
            int[] array = GenerateRandomArray(10000);
            Console.WriteLine("\n--- Merge Sort ---");

            // Start a stopwatch to measure the time taken by MergeSort.
            Stopwatch stopwatch = Stopwatch.StartNew();
            MergeSort(array, 0, array.Length - 1);
            stopwatch.Stop();

            // Display the execution time in milliseconds.
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        // MergeSort recursively splits the array into halves and then merges them in sorted order.
        static void MergeSort(int[] array, int left, int right)
        {
            // Continue only if there is more than one element.
            if (left < right)
            {
                // Find the middle index.
                int middle = (left + right) / 2;
                // Sort the left half.
                MergeSort(array, left, middle);
                // Sort the right half.
                MergeSort(array, middle + 1, right);
                // Merge the two sorted halves.
                Merge(array, left, middle, right);
            }
        }

        // Merge combines two sorted subarrays into one sorted array.
        static void Merge(int[] array, int left, int middle, int right)
        {
            // Determine the sizes of the two subarrays.
            int leftSize = middle - left + 1;
            int rightSize = right - middle;
            int[] leftArray = new int[leftSize];
            int[] rightArray = new int[rightSize];

            // Copy data to temporary arrays.
            for (int i = 0; i < leftSize; i++)
                leftArray[i] = array[left + i];
            for (int i = 0; i < rightSize; i++)
                rightArray[i] = array[middle + 1 + i];

            // Merge the temporary arrays back into the original array.
            int iLeft = 0, iRight = 0, iMerged = left;
            while (iLeft < leftSize && iRight < rightSize)
                array[iMerged++] = (leftArray[iLeft] <= rightArray[iRight])
                                    ? leftArray[iLeft++]
                                    : rightArray[iRight++];

            // Copy any remaining elements of leftArray.
            while (iLeft < leftSize)
                array[iMerged++] = leftArray[iLeft++];
            // Copy any remaining elements of rightArray.
            while (iRight < rightSize)
                array[iMerged++] = rightArray[iRight++];
        }

        // -------------------------------------------------------------------
        // QUICK SORT IMPLEMENTATION
        // -------------------------------------------------------------------

        // RunQuickSort generates a random array, sorts it using Quick Sort,
        // and prints the time taken.
        static void RunQuickSort()
        {
            // Generate an array of 10,000 random integers.
            int[] array = GenerateRandomArray(10000);
            Console.WriteLine("\n--- Quick Sort ---");

            // Start timing the quick sort.
            Stopwatch stopwatch = Stopwatch.StartNew();
            QuickSort(array, 0, array.Length - 1);
            stopwatch.Stop();

            // Output the execution time.
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        // QuickSort recursively partitions the array and sorts the partitions.
        static void QuickSort(int[] array, int left, int right)
        {
            if (left < right)
            {
                // Partition the array and obtain the pivot index.
                int pivotIndex = Partition(array, left, right);
                // Recursively sort elements before the pivot.
                QuickSort(array, left, pivotIndex - 1);
                // Recursively sort elements after the pivot.
                QuickSort(array, pivotIndex + 1, right);
            }
        }

        // Partition rearranges the elements so that all elements less than the pivot
        // are on its left, and all elements greater are on its right.
        static int Partition(int[] array, int left, int right)
        {
            // Choose the rightmost element as the pivot.
            int pivot = array[right];
            int i = left - 1;
            // Iterate through the array, moving elements smaller than the pivot to the left.
            for (int j = left; j < right; j++)
            {
                if (array[j] < pivot)
                {
                    i++;
                    // Swap array[i] and array[j].
                    int temp = array[i];
                    array[i] = array[j];
                    array[j] = temp;
                }
            }
            // Place the pivot in the correct sorted position.
            int temp2 = array[i + 1];
            array[i + 1] = array[right];
            array[right] = temp2;
            return i + 1;
        }

        // -------------------------------------------------------------------
        // BINARY SEARCH IMPLEMENTATION
        // -------------------------------------------------------------------

        // RunBinarySearch generates a sorted array, picks a random target,
        // performs binary search, and prints the results along with timing.
        static void RunBinarySearch()
        {
            // Generate a sorted array of 100,000 integers.
            int[] array = GenerateSortedArray(100000);
            // Pick a random target element from the array.
            int target = array[rand.Next(array.Length)];

            Console.WriteLine("\n--- Binary Search ---");
            // Start timing the binary search operation.
            Stopwatch stopwatch = Stopwatch.StartNew();
            // Perform binary search to locate the target.
            int index = BinarySearch(array, target, 0, array.Length - 1);
            stopwatch.Stop();

            // Print the result of the search and the time taken.
            Console.WriteLine($"Element {target} found at index {index}");
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        // BinarySearch recursively finds the target element in the sorted array.
        static int BinarySearch(int[] array, int target, int left, int right)
        {
            // If no elements remain, the target is not present.
            if (left > right) return -1;
            int mid = (left + right) / 2;

            // If the middle element is the target, return its index.
            if (array[mid] == target) return mid;
            // Otherwise, continue searching in the left or right half.
            return array[mid] > target
                   ? BinarySearch(array, target, left, mid - 1)
                   : BinarySearch(array, target, mid + 1, right);
        }

        // -------------------------------------------------------------------
        // STRASSEN’S MATRIX MULTIPLICATION IMPLEMENTATION
        // -------------------------------------------------------------------

        // RunStrassenMatrixMultiplication demonstrates the Strassen algorithm for multiplying matrices.
        static void RunStrassenMatrixMultiplication()
        {
            Console.WriteLine("\n--- Strassen’s Matrix Multiplication ---");

            // Define two 2x2 matrices A and B.
            int[,] A = {
                {1, 2},
                {3, 4}
            };

            int[,] B = {
                {5, 6},
                {7, 8}
            };

            // Start timing the matrix multiplication.
            Stopwatch stopwatch = Stopwatch.StartNew();
            // Multiply matrices A and B using the StrassenMultiply method.
            int[,] result = StrassenMultiply(A, B);
            stopwatch.Stop();

            // Print the resulting matrix.
            Console.WriteLine("Resultant Matrix:");
            PrintMatrix(result);
            // Display the time taken for the multiplication.
            Console.WriteLine($"Execution Time: {stopwatch.ElapsedMilliseconds} ms");
        }

        // StrassenMultiply recursively multiplies two matrices using Strassen’s algorithm.
        static int[,] StrassenMultiply(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int[,] C = new int[n, n];

            // Base case: when the matrix is 1x1, simply multiply the two elements.
            if (n == 1)
            {
                C[0, 0] = A[0, 0] * B[0, 0];
                return C;
            }

            // Divide the matrices into 4 submatrices each.
            int newSize = n / 2;
            int[,] A11 = new int[newSize, newSize];
            int[,] A12 = new int[newSize, newSize];
            int[,] A21 = new int[newSize, newSize];
            int[,] A22 = new int[newSize, newSize];
            int[,] B11 = new int[newSize, newSize];
            int[,] B12 = new int[newSize, newSize];
            int[,] B21 = new int[newSize, newSize];
            int[,] B22 = new int[newSize, newSize];

            // Split matrices A and B into quadrants.
            SplitMatrix(A, A11, 0, 0);
            SplitMatrix(A, A12, 0, newSize);
            SplitMatrix(A, A21, newSize, 0);
            SplitMatrix(A, A22, newSize, newSize);
            SplitMatrix(B, B11, 0, 0);
            SplitMatrix(B, B12, 0, newSize);
            SplitMatrix(B, B21, newSize, 0);
            SplitMatrix(B, B22, newSize, newSize);

            // Compute the 7 products using recursive multiplications.
            int[,] M1 = StrassenMultiply(Add(A11, A22), Add(B11, B22));
            int[,] M2 = StrassenMultiply(Add(A21, A22), B11);
            int[,] M3 = StrassenMultiply(A11, Subtract(B12, B22));
            int[,] M4 = StrassenMultiply(A22, Subtract(B21, B11));
            int[,] M5 = StrassenMultiply(Add(A11, A12), B22);
            int[,] M6 = StrassenMultiply(Subtract(A21, A11), Add(B11, B12));
            int[,] M7 = StrassenMultiply(Subtract(A12, A22), Add(B21, B22));

            // Combine the intermediate products to form the submatrices of the final result.
            int[,] C11 = Add(Subtract(Add(M1, M4), M5), M7);
            int[,] C12 = Add(M3, M5);
            int[,] C21 = Add(M2, M4);
            int[,] C22 = Add(Subtract(Add(M1, M3), M2), M6);

            // Join the 4 submatrices into a single resultant matrix.
            JoinMatrix(C, C11, 0, 0);
            JoinMatrix(C, C12, 0, newSize);
            JoinMatrix(C, C21, newSize, 0);
            JoinMatrix(C, C22, newSize, newSize);

            return C;
        }

        // -------------------------------------------------------------------
        // HELPER FUNCTIONS
        // -------------------------------------------------------------------

        // Add: Computes the sum of two matrices.
        static int[,] Add(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int[,] result = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    result[i, j] = A[i, j] + B[i, j];
            return result;
        }

        // Subtract: Computes the difference between two matrices.
        static int[,] Subtract(int[,] A, int[,] B)
        {
            int n = A.GetLength(0);
            int[,] result = new int[n, n];
            for (int i = 0; i < n; i++)
                for (int j = 0; j < n; j++)
                    result[i, j] = A[i, j] - B[i, j];
            return result;
        }

        // SplitMatrix: Copies a submatrix from the parent matrix into the child matrix.
        // 'x' and 'y' are the starting indices in the parent matrix.
        static void SplitMatrix(int[,] parent, int[,] child, int x, int y)
        {
            for (int i = 0; i < child.GetLength(0); i++)
                for (int j = 0; j < child.GetLength(1); j++)
                    child[i, j] = parent[x + i, y + j];
        }

        // JoinMatrix: Inserts the child matrix into the parent matrix starting at position (x, y).
        static void JoinMatrix(int[,] parent, int[,] child, int x, int y)
        {
            for (int i = 0; i < child.GetLength(0); i++)
                for (int j = 0; j < child.GetLength(1); j++)
                    parent[x + i, y + j] = child[i, j];
        }

        // PrintMatrix: Prints the matrix in a formatted grid layout.
        static void PrintMatrix(int[,] matrix)
        {
            int rows = matrix.GetLength(0);
            int cols = matrix.GetLength(1);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                    Console.Write(matrix[i, j] + "\t");  // Tab-separated values.
                Console.WriteLine();
            }
        }

        // GenerateRandomArray: Creates an array of the specified size with random integers.
        static int[] GenerateRandomArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = rand.Next(0, 10000); // Random integer between 0 and 9999.
            return array;
        }

        // GenerateSortedArray: Creates a sorted array of the specified size.
        static int[] GenerateSortedArray(int size)
        {
            int[] array = new int[size];
            for (int i = 0; i < size; i++)
                array[i] = i; // Sorted from 0 to size-1.
            return array;
        }
    }
}
