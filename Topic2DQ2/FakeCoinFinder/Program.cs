using System;

class FakeCoinFinder
{
    static void Main(string[] args)
    {
        // Example usage: Find the fake coin among 9 coins
        // Coins array: 0 = genuine, positive = heavier fake, negative = lighter fake
        int[] coins = { 0, 0, 0, 0, -1, 0, 0, 0, 0 }; // Example: Fake coin at index 4 is lighter
        FindFakeCoin(coins);
    }

    static void FindFakeCoin(int[] coins)
    {
        int n = coins.Length;

        // Ensure there are enough coins to perform the algorithm
        if (n < 3)
        {
            Console.WriteLine("There must be at least 3 coins to use this algorithm.");
            return;
        }

        // Step 1: Divide the coins into three groups
        int groupSize = n / 3; // Base size of each group
        int remainder = n % 3; // Remaining coins after dividing by 3

        // Group A: First group of coins
        int[] groupA = new int[groupSize];
        Array.Copy(coins, 0, groupA, 0, groupSize);

        // Group B: Second group of coins
        int[] groupB = new int[groupSize];
        Array.Copy(coins, groupSize, groupB, 0, groupSize);

        // Group C: Third group plus any leftover coins
        int[] groupC = new int[groupSize + remainder];
        Array.Copy(coins, 2 * groupSize, groupC, 0, groupSize + remainder);

        // Step 2: Compare Group A and Group B on the balance scale
        int resultAB = CompareGroups(groupA, groupB);

        if (resultAB == 0)
        {
            // If the scale is balanced, the fake coin must be in Group C
            Console.WriteLine("The fake coin is in Group C.");
            IdentifyFakeCoin(groupC, groupOffset: 2 * groupSize); // Offset for Group C
        }
        else if (resultAB < 0) // Group B is heavier (or A is lighter)
        {
            // If Group A is lighter, the fake coin is in Group A and is lighter
            Console.WriteLine("The fake coin is in Group A and is lighter.");
            IdentifyFakeCoin(groupA, isHeavier: false, groupOffset: 0); // Offset for Group A
        }
        else // resultAB > 0
        {
            // If Group B is lighter, the fake coin is in Group B and is lighter
            Console.WriteLine("The fake coin is in Group B and is lighter.");
            IdentifyFakeCoin(groupB, isHeavier: false, groupOffset: groupSize); // Offset for Group B
        }
    }

    static int CompareGroups(int[] group1, int[] group2)
    {
        int sum1 = 0, sum2 = 0;

        // Calculate the total weight of Group 1
        foreach (int coin in group1)
        {
            sum1 += coin;
        }

        // Calculate the total weight of Group 2
        foreach (int coin in group2)
        {
            sum2 += coin;
        }

        // Return the difference between the two groups
        // Positive value: Group 1 is heavier
        // Negative value: Group 2 is heavier
        // Zero: Both groups are balanced
        return sum1 - sum2;
    }

    static void IdentifyFakeCoin(int[] group, bool isHeavier = false, int groupOffset = 0)
    {
        // Loop through each coin in the group to find the fake one
        for (int i = 0; i < group.Length; i++)
        {
            // Check if the coin is heavier or lighter based on the flag
            if ((isHeavier && group[i] > 0) || (!isHeavier && group[i] < 0))
            {
                Console.WriteLine($"The fake coin is at index {i + groupOffset} in the original array.");
                return;
            }
        }

        // If no fake coin is found (this shouldn't happen with valid input)
        Console.WriteLine("No fake coin found in this group.");
    }
}
