using System;

namespace TowerOfHanoiMoves
{
    class Program
    {
        static void Main(string[] args)
        {
            // Welcome message for the user
            Console.WriteLine("Tower of Hanoi Disk Moves Calculator");

            // Boolean to control whether the program continues running
            bool keepRunning = true;

            // Main program loop: keeps running until the user decides to exit
            while (keepRunning)
            {
                try
                {
                    // Prompt the user to enter the total number of disks (n)
                    Console.Write("Enter the total number of disks (n): ");
                    string? inputN = Console.ReadLine();

                    // Check if the input is null or empty
                    if (string.IsNullOrEmpty(inputN))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue; // Skip to the next iteration of the loop
                    }

                    // Parse the input to an integer
                    int n = int.Parse(inputN);

                    // Prompt the user to enter the disk number (i)
                    Console.Write("Enter the disk number (i): ");
                    string? inputI = Console.ReadLine();

                    // Check if the input is null or empty
                    if (string.IsNullOrEmpty(inputI))
                    {
                        Console.WriteLine("Invalid input. Please enter a valid number.");
                        continue; // Skip to the next iteration of the loop
                    }

                    // Parse the input to an integer
                    int i = int.Parse(inputI);

                    // Validate the inputs: ensure that i is between 1 and n
                    if (i < 1 || i > n)
                    {
                        Console.WriteLine("Invalid disk number. Ensure 1 ≤ i ≤ n.");
                    }
                    else
                    {
                        // Calculate the number of moves for the ith disk
                        // Formula Explanation:
                        // - The Tower of Hanoi follows a specific pattern for disk movements:
                        //   1. The largest disk (disk n) moves only ONCE, as it is the last to move.
                        //   2. The second-largest disk (disk n-1) moves TWICE, as it needs to be shuffled twice to allow the largest disk to move.
                        //   3. The third-largest disk (disk n-2) moves FOUR times, and so on.
                        // - This doubling pattern is expressed by the formula 2^(n-i):
                        //   - 'n' is the total number of disks.
                        //   - 'i' is the specific disk number (1 for the smallest, n for the largest).
                        //   - The formula calculates how often each disk is involved in the recursive steps of solving the Tower of Hanoi.
                        int moves = (int)Math.Pow(2, n - i);

                        // Display the result to the user
                        Console.WriteLine($"The number of moves made by disk {i} is: {moves}");
                    }
                }
                catch (FormatException)
                {
                    // Handle cases where the user enters non-numeric input
                    Console.WriteLine("Please enter valid integers for n and i.");
                }
                catch (Exception ex)
                {
                    // Handle any unexpected errors
                    Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                }

                // Ask the user if they want to calculate another scenario
                Console.WriteLine("Would you like to calculate another disk move? (y/n): ");
                string? continueInput = Console.ReadLine();

                // If the user inputs anything other than 'y', exit the loop
                if (string.IsNullOrEmpty(continueInput) || continueInput.ToLower() != "y")
                {
                    keepRunning = false;
                }
            }

            // Farewell message before the program exits
            Console.WriteLine("Thank you for using the Tower of Hanoi Disk Moves Calculator. Goodbye!");
        }
    }
}
