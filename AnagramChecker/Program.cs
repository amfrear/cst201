namespace AnagramChecker
{
    internal class Program
    {
        static void Main(string[] args)
        {
            while (true)  // Infinite loop to keep the program running
            {
                // Input two words from the user
                Console.WriteLine("Enter the first word (or type 'exit' to quit): ");
                string word1 = Console.ReadLine() ?? string.Empty;

                if (word1.ToLower() == "exit") break;

                Console.WriteLine("Enter the second word: ");
                string word2 = Console.ReadLine() ?? string.Empty;

                // Check if the words are anagrams
                bool result = AreAnagrams(word1, word2);

                // Output the result
                Console.WriteLine($"Are the words \"{word1}\" and \"{word2}\" anagrams? {result}");
                Console.WriteLine("\n-----------------------------\n");
            }

            Console.WriteLine("Program ended. Press any key to close...");
            Console.ReadKey();
        }

        static bool AreAnagrams(string word1, string word2)
        {
            // Step 1: Check if lengths are equal
            if (word1.Length != word2.Length)
                return false;

            // Step 2: Convert to lowercase
            word1 = word1.ToLower();
            word2 = word2.ToLower();

            // Step 3: Sort and compare
            char[] word1Array = word1.ToCharArray();
            char[] word2Array = word2.ToCharArray();

            Array.Sort(word1Array);
            Array.Sort(word2Array);

            return new string(word1Array) == new string(word2Array);
        }
    }
}
