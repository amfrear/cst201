using System;

namespace FindLargestNumber
{
    class Program
    {
        static void Main(string[] args)
        {
            // Step 1: Create an array of 100 integers
            int[] numbers = new int[100];

            // Step 2: Create a Random object to generate random numbers
            Random random = new Random();

            // Step 3: Populate the array with random integers between 1 and 1000
            for (int i = 0; i < numbers.Length; i++)
            {
                numbers[i] = random.Next(1, 1001);  // Generates a random number from 1 to 1000
            }

            // Step 4: Initialize a variable to store the largest number found
            // Start with the first number in the array as the initial "max"
            int max = numbers[0];

            // Step 5: Initialize a counter to track the number of computational steps
            int steps = 0;

            // Step 6: Loop through the array starting from the second element (index 1)
            for (int i = 1; i < numbers.Length; i++)
            {
                // Increment the step counter for each comparison
                steps++;

                // Check if the current number is greater than the current max
                if (numbers[i] > max)
                {
                    // If it is, update the max value
                    max = numbers[i];
                }
            }

            // Step 7: Output the largest number found
            Console.WriteLine("Largest number: " + max);

            // Step 8: Output the total number of computational steps taken
            Console.WriteLine("Total steps: " + steps);

            // Pause the console to see the output before the program closes
            Console.WriteLine("\nPress any key to exit...");
            Console.ReadKey();
        }
    }
}
