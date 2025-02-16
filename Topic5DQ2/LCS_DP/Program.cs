using System;

namespace LCS_DP
{
    internal class Program
    {
        // Function to compute the length of the Longest Common Subsequence (LCS)
        static int LongestCommonSubsequence(string str1, string str2, out int[,] dp)
        {
            int m = str1.Length; // Length of first string
            int n = str2.Length; // Length of second string

            // Create a DP table with dimensions (m+1) x (n+1)
            // dp[i, j] stores the length of LCS for str1[0..i-1] and str2[0..j-1]
            dp = new int[m + 1, n + 1];

            // Fill the DP table using bottom-up approach
            for (int i = 1; i <= m; i++) // Loop through each character in str1
            {
                for (int j = 1; j <= n; j++) // Loop through each character in str2
                {
                    if (str1[i - 1] == str2[j - 1])
                    {
                        // If characters match, increment the LCS length from the previous diagonal
                        dp[i, j] = dp[i - 1, j - 1] + 1;
                    }
                    else
                    {
                        // If characters don't match, take the maximum from top or left cell
                        dp[i, j] = Math.Max(dp[i - 1, j], dp[i, j - 1]);
                    }
                }
            }

            return dp[m, n]; // The last cell contains the length of LCS
        }

        // Function to reconstruct the actual LCS from the DP table
        static string GetLCS(string str1, string str2, int[,] dp)
        {
            int i = str1.Length, j = str2.Length;
            string lcs = "";

            // Traverse the DP table from bottom-right to top-left to reconstruct the LCS
            while (i > 0 && j > 0)
            {
                if (str1[i - 1] == str2[j - 1])
                {
                    // If characters match, add them to LCS result and move diagonally
                    lcs = str1[i - 1] + lcs;
                    i--;
                    j--;
                }
                else if (dp[i - 1, j] > dp[i, j - 1])
                {
                    // Move up if the top cell has a greater value
                    i--;
                }
                else
                {
                    // Move left if the left cell has a greater or equal value
                    j--;
                }
            }

            return lcs; // Return the reconstructed LCS string
        }

        // Function to print the DP table for visualization
        static void PrintDPTable(int[,] dp, string str1, string str2)
        {
            Console.WriteLine("\nDynamic Programming Table:");

            // Print column headers (characters of str2)
            Console.Write("    "); // Align headers
            foreach (char ch in str2)
                Console.Write($"{ch,3} ");
            Console.WriteLine("\n   " + new string('-', (str2.Length + 1) * 4));

            for (int i = 0; i <= str1.Length; i++)
            {
                // Print row headers (characters of str1)
                Console.Write((i > 0 ? str1[i - 1] : ' ') + " |");
                for (int j = 0; j <= str2.Length; j++)
                {
                    // Print each cell of the DP table
                    Console.Write($"{dp[i, j],3} ");
                }
                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            // Define two sample strings to compare
            string str1 = "ACDBE";
            string str2 = "ABCDE";

            // Compute LCS length and get the DP table
            int[,] dp;
            int lcsLength = LongestCommonSubsequence(str1, str2, out dp);

            // Get the actual LCS sequence
            string lcs = GetLCS(str1, str2, dp);

            // Print results
            Console.WriteLine($"String 1: {str1}");
            Console.WriteLine($"String 2: {str2}");
            Console.WriteLine($"Longest Common Subsequence Length: {lcsLength}");
            Console.WriteLine($"Longest Common Subsequence: {lcs}");

            // Print DP table for visualization
            PrintDPTable(dp, str1, str2);
        }
    }
}
