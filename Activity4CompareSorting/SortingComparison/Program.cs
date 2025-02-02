using System;

namespace SortingComparison
{
    // A simple class to hold our counters for each sort:
    // 1) Comparisons: How many times we compare two elements
    // 2) Exchanges:   How many times we write elements to the array 
    //                 (including swaps, copying during merges, etc.)
    class SortStats
    {
        public long Comparisons = 0;
        public long Exchanges = 0;
    }

    class Program
    {
        // We use a single Random object for the whole program
        static Random rand = new Random();

        static void Main(string[] args)
        {
            // Prompt the user for n (the size of the array)
            Console.Write("Enter the size of the array (n): ");

            // Validate the user input for n
            if (!int.TryParse(Console.ReadLine(), out int n) || n <= 0)
            {
                Console.WriteLine("Please enter a positive integer.");
                return;  // Exit if invalid
            }

            // Define how many times (iterations) we repeat the process of:
            // 1) Shuffling the array
            // 2) Sorting with each algorithm
            // 3) Counting comparisons/exchanges
            const int iterations = 100;

            // Set up variables to accumulate total comparisons and exchanges
            // across all iterations, for each sorting algorithm
            long totalSelectionComparisons = 0, totalSelectionExchanges = 0;
            long totalBubbleComparisons = 0, totalBubbleExchanges = 0;
            long totalMergeComparisons = 0, totalMergeExchanges = 0;
            long totalQuickComparisons = 0, totalQuickExchanges = 0;
            long totalHybridComparisons = 0, totalHybridExchanges = 0;

            // Perform the sorting test 'iterations' times
            for (int iter = 0; iter < iterations; iter++)
            {
                // 1) Create an array of size n with elements 1..n
                int[] original = new int[n];
                for (int i = 0; i < n; i++)
                {
                    original[i] = i + 1;
                }

                // 2) Shuffle this array to get a random permutation of [1..n]
                Shuffle(original);

                // ---------------------- SELECTION SORT ----------------------
                int[] selectionArray = (int[])original.Clone();
                SortStats stats = new SortStats();
                SelectionSort(selectionArray, stats);
                totalSelectionComparisons += stats.Comparisons;
                totalSelectionExchanges += stats.Exchanges;

                // ---------------------- BUBBLE SORT -------------------------
                int[] bubbleArray = (int[])original.Clone();
                stats = new SortStats();
                BubbleSort(bubbleArray, stats);
                totalBubbleComparisons += stats.Comparisons;
                totalBubbleExchanges += stats.Exchanges;

                // ---------------------- MERGE SORT --------------------------
                int[] mergeArray = (int[])original.Clone();
                stats = new SortStats();
                MergeSort(mergeArray, 0, mergeArray.Length - 1, stats);
                totalMergeComparisons += stats.Comparisons;
                totalMergeExchanges += stats.Exchanges;

                // ---------------------- QUICK SORT --------------------------
                int[] quickArray = (int[])original.Clone();
                stats = new SortStats();
                QuickSort(quickArray, 0, quickArray.Length - 1, stats);
                totalQuickComparisons += stats.Comparisons;
                totalQuickExchanges += stats.Exchanges;

                // ---------------------- HYBRID SORT -------------------------
                // A "better" algorithm that uses Quick Sort + Insertion Sort
                // for small subarrays, and median-of-three pivot selection.
                int[] hybridArray = (int[])original.Clone();
                stats = new SortStats();
                HybridSort(hybridArray, 0, hybridArray.Length - 1, stats);
                totalHybridComparisons += stats.Comparisons;
                totalHybridExchanges += stats.Exchanges;
            }

            // Print the numeric results for each algorithm
            Console.WriteLine("\nAverages over {0} iterations:", iterations);

            Console.WriteLine("Selection Sort: Comparisons = {0}, Exchanges = {1}",
                totalSelectionComparisons / iterations, totalSelectionExchanges / iterations);
            Console.WriteLine("Bubble Sort:    Comparisons = {0}, Exchanges = {1}",
                totalBubbleComparisons / iterations, totalBubbleExchanges / iterations);
            Console.WriteLine("Merge Sort:     Comparisons = {0}, Exchanges = {1}",
                totalMergeComparisons / iterations, totalMergeExchanges / iterations);
            Console.WriteLine("Quick Sort:     Comparisons = {0}, Exchanges = {1}",
                totalQuickComparisons / iterations, totalQuickExchanges / iterations);
            Console.WriteLine("Hybrid Sort:    Comparisons = {0}, Exchanges = {1}",
                totalHybridComparisons / iterations, totalHybridExchanges / iterations);

            // Brief Explanations
            Console.WriteLine("\n--- Brief Explanation of the Output ---");
            Console.WriteLine("Comparisons: # of times we check if one element is less/greater than another.");
            Console.WriteLine("Exchanges:   # of data writes (including swaps, merges, insertions).");

            Console.WriteLine("\nSorting Algorithms:");
            Console.WriteLine("- Selection Sort (O(n^2)): Finds the minimum each pass and swaps it to the front.");
            Console.WriteLine("- Bubble Sort (O(n^2)):    Swaps adjacent out-of-order pairs repeatedly.");
            Console.WriteLine("- Merge Sort (O(n log n)): Recursively splits and merges; fewer compares but more writes.");
            Console.WriteLine("- Quick Sort (O(n log n)): Partitions around a pivot; performance can vary by pivot choice.");
            Console.WriteLine("- Hybrid Sort: A Quick Sort variant with median-of-three pivot & insertion sort on small subarrays.");

            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }

        // ---------------------------------------------------------
        // This method implements the Fisher-Yates shuffle:
        // It iterates from the end of the array down to the second element
        // and swaps each element with another randomly chosen from the front part.
        static void Shuffle(int[] array)
        {
            for (int i = array.Length - 1; i > 0; i--)
            {
                int j = rand.Next(i + 1); // Pick a random index from 0..i
                // Swap array[i] with array[j]
                int temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
        }

        // ---------------------- Selection Sort ----------------------
        // Repeatedly find the minimum element from the unsorted part 
        // and move it to the front.
        static void SelectionSort(int[] array, SortStats stats)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                int minIndex = i;
                for (int j = i + 1; j < n; j++)
                {
                    // Each check is a comparison
                    stats.Comparisons++;
                    if (array[j] < array[minIndex])
                    {
                        minIndex = j;
                    }
                }
                // Swap the found minimum with the element at index i
                if (i != minIndex)
                {
                    Swap(array, i, minIndex, stats);
                }
            }
        }

        // ---------------------- Bubble Sort ----------------------
        // Compare adjacent pairs, swap if out of order,
        // repeat until array is sorted.
        static void BubbleSort(int[] array, SortStats stats)
        {
            int n = array.Length;
            for (int i = 0; i < n - 1; i++)
            {
                for (int j = 0; j < n - i - 1; j++)
                {
                    stats.Comparisons++;
                    if (array[j] > array[j + 1])
                    {
                        Swap(array, j, j + 1, stats);
                    }
                }
            }
        }

        // ---------------------- Merge Sort ----------------------
        // A classic divide-and-conquer algorithm:
        // 1. Divide the array into halves
        // 2. Recursively sort each half
        // 3. Merge the two sorted halves
        static void MergeSort(int[] array, int left, int right, SortStats stats)
        {
            if (left < right)
            {
                int mid = (left + right) / 2;
                MergeSort(array, left, mid, stats);
                MergeSort(array, mid + 1, right, stats);
                Merge(array, left, mid, right, stats);
            }
        }

        // Merging two sorted subarrays:
        // Subarray 1 = array[left..mid]
        // Subarray 2 = array[mid+1..right]
        static void Merge(int[] array, int left, int mid, int right, SortStats stats)
        {
            // Sizes of the two subarrays
            int n1 = mid - left + 1;
            int n2 = right - mid;

            // Temporary arrays to hold the split data
            int[] L = new int[n1];
            int[] R = new int[n2];

            // Copy data from the main array into L and R
            Array.Copy(array, left, L, 0, n1);
            Array.Copy(array, mid + 1, R, 0, n2);

            int i = 0, j = 0, k = left;

            // Compare elements in L and R, copy the smaller one to 'array'
            while (i < n1 && j < n2)
            {
                stats.Comparisons++;
                if (L[i] <= R[j])
                {
                    array[k] = L[i];
                    stats.Exchanges++;  // Count the copy as 1 exchange
                    i++;
                }
                else
                {
                    array[k] = R[j];
                    stats.Exchanges++;
                    j++;
                }
                k++;
            }

            // Copy any remaining elements from L[]
            while (i < n1)
            {
                array[k] = L[i];
                stats.Exchanges++;
                i++;
                k++;
            }

            // Copy any remaining elements from R[]
            while (j < n2)
            {
                array[k] = R[j];
                stats.Exchanges++;
                j++;
                k++;
            }
        }

        // ---------------------- Quick Sort ----------------------
        // Another divide-and-conquer algorithm:
        // 1. Partition the array around a pivot
        // 2. Recursively sort the left side, then the right side
        static void QuickSort(int[] array, int low, int high, SortStats stats)
        {
            if (low < high)
            {
                int pivotIndex = Partition(array, low, high, stats);
                QuickSort(array, low, pivotIndex - 1, stats);
                QuickSort(array, pivotIndex + 1, high, stats);
            }
        }

        // Partition step: choose a pivot (here, the last element),
        // and rearrange so that all elements less than pivot
        // come before it, all greater go after it.
        static int Partition(int[] array, int low, int high, SortStats stats)
        {
            int pivot = array[high]; // using the last element as pivot
            int i = low - 1;         // i will track boundary of elements < pivot

            for (int j = low; j < high; j++)
            {
                stats.Comparisons++;
                if (array[j] < pivot)
                {
                    i++;
                    Swap(array, i, j, stats);
                }
            }
            // Finally, swap pivot into correct position
            Swap(array, i + 1, high, stats);
            return i + 1;
        }

        // ---------------------- Hybrid Sort (Better Algorithm) ----------------------
        // Quick Sort that:
        // 1) Switches to Insertion Sort for small subarrays
        // 2) Uses median-of-three for pivot selection
        static void HybridSort(int[] array, int low, int high, SortStats stats)
        {
            const int threshold = 10;

            // If the subarray is quite small, use Insertion Sort
            if (high - low + 1 < threshold)
            {
                InsertionSort(array, low, high, stats);
                return;
            }

            // Otherwise, pick pivot using median-of-three
            int pivotIndex = MedianOfThree(array, low, high, stats);
            Swap(array, pivotIndex, high, stats);

            // Partition and recurse
            pivotIndex = Partition(array, low, high, stats);
            HybridSort(array, low, pivotIndex - 1, stats);
            HybridSort(array, pivotIndex + 1, high, stats);
        }

        // Sort small segments with Insertion Sort
        static void InsertionSort(int[] array, int low, int high, SortStats stats)
        {
            for (int i = low + 1; i <= high; i++)
            {
                int key = array[i];
                int j = i - 1;

                // Shift larger elements to the right
                while (j >= low)
                {
                    stats.Comparisons++;
                    if (array[j] > key)
                    {
                        array[j + 1] = array[j];
                        stats.Exchanges++; // shifting one element
                        j--;
                    }
                    else
                    {
                        break;
                    }
                }
                // Place the key
                array[j + 1] = key;
                stats.Exchanges++; // final placement of key
            }
        }

        // Choose pivot via median-of-three: check array[low], array[mid], array[high]
        static int MedianOfThree(int[] array, int low, int high, SortStats stats)
        {
            int mid = (low + high) / 2;

            // Ensure array[low] <= array[mid]
            stats.Comparisons++;
            if (array[low] > array[mid])
                Swap(array, low, mid, stats);

            // Ensure array[low] <= array[high]
            stats.Comparisons++;
            if (array[low] > array[high])
                Swap(array, low, high, stats);

            // Ensure array[mid] <= array[high]
            stats.Comparisons++;
            if (array[mid] > array[high])
                Swap(array, mid, high, stats);

            return mid;
        }

        // Swap helper function (counts as 3 Exchanges per swap)
        static void Swap(int[] array, int i, int j, SortStats stats)
        {
            int temp = array[i];
            array[i] = array[j];
            array[j] = temp;
            stats.Exchanges += 3;
        }
    }
}
