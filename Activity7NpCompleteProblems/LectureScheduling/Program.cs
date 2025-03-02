using System;

class Program
{
    /*
     * Function: GetSmallestAvailableColor
     * -----------------------------------
     * This function finds the smallest available color (timeslot) 
     * that can be assigned to a given lecture without conflicting 
     * with previously assigned lectures.
     */
    static int GetSmallestAvailableColor(int lecture, int[] colors, int[,] conflictMatrix)
    {
        int numLectures = colors.Length;
        // Boolean array to keep track of which colors are already used by conflicting lectures
        bool[] used = new bool[numLectures + 1]; // Extra slot for safety

        // Check every lecture to see if it conflicts with the current lecture
        for (int i = 0; i < numLectures; i++)
        {
            // If there's a conflict (i.e., shared students) and the lecture has been assigned a color
            if (conflictMatrix[lecture, i] == 1 && colors[i] != -1)
            {
                used[colors[i]] = true; // Mark this color as used
            }
        }

        // Find the smallest available color that is not used
        int color;
        for (color = 0; color < used.Length; color++)
        {
            if (!used[color]) // If this color is free, assign it
                break;
        }
        return color;
    }

    /*
     * Function: GreedyColoring
     * ------------------------
     * This function assigns timeslots to lectures using a greedy graph coloring approach.
     * The goal is to use the fewest timeslots while ensuring that conflicting lectures 
     * do not get the same timeslot.
     */
    static int[] GreedyColoring(int[,] conflictMatrix)
    {
        int numLectures = conflictMatrix.GetLength(0);
        int[] colors = new int[numLectures];

        // Initialize all lectures with no timeslot (-1 means no color assigned).
        for (int i = 0; i < numLectures; i++)
        {
            colors[i] = -1;
        }

        // Assign the first lecture the first available timeslot (color 0).
        colors[0] = 0;
        Console.WriteLine($"Lecture 0 assigned to Timeslot 0.");

        // Assign timeslots for the rest of the lectures.
        for (int lecture = 1; lecture < numLectures; lecture++)
        {
            // Get the smallest available color that does not cause conflicts
            int chosenColor = GetSmallestAvailableColor(lecture, colors, conflictMatrix);
            colors[lecture] = chosenColor;
            Console.WriteLine($"Lecture {lecture} assigned to Timeslot {chosenColor}.");
        }
        Console.WriteLine();
        return colors;
    }

    /*
     * Function: CalculateConflicts
     * ----------------------------
     * This function counts the number of students who still have scheduling conflicts.
     * A conflict occurs if two lectures that share students end up in the same timeslot.
     */
    static int CalculateConflicts(int[,] conflictMatrix, int[] colors)
    {
        int numLectures = colors.Length;
        int conflicts = 0;

        for (int i = 0; i < numLectures; i++)
        {
            for (int j = i + 1; j < numLectures; j++)
            {
                // If there is a conflict (shared student) and both lectures have the same timeslot
                if (conflictMatrix[i, j] == 1 && colors[i] == colors[j])
                {
                    conflicts++;
                }
            }
        }
        return conflicts;
    }

    /*
     * Function: PrintMatrix
     * ---------------------
     * This function prints the conflict matrix to help visualize the lecture conflicts.
     * A '1' indicates that two lectures share at least one student.
     */
    static void PrintMatrix(int[,] matrix)
    {
        int size = matrix.GetLength(0);
        Console.WriteLine("Conflict Matrix (Adjacency Matrix):");
        for (int i = 0; i < size; i++)
        {
            Console.Write("Lecture " + i + ": ");
            for (int j = 0; j < size; j++)
            {
                Console.Write(matrix[i, j] + " ");
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    static void Main()
    {
        // Define the conflict matrix where:
        // 1 means two lectures share a student, and 0 means no shared students.
        int[,] conflictMatrix = new int[,]
        {
            { 0, 1, 1, 0, 1, 0, 0 },
            { 1, 0, 1, 1, 1, 0, 1 },
            { 1, 1, 0, 1, 0, 1, 1 },
            { 0, 1, 1, 0, 0, 1, 1 },
            { 1, 1, 0, 0, 0, 1, 1 },
            { 0, 0, 1, 1, 1, 0, 0 },
            { 0, 1, 1, 1, 1, 0, 0 }
        };

        // Step 1: Print the matrix to help the user understand the conflicts
        PrintMatrix(conflictMatrix);

        // Step 2: Perform greedy graph coloring to assign timeslots
        Console.WriteLine("Assigning Timeslots (Greedy Coloring Algorithm):");
        int[] colors = GreedyColoring(conflictMatrix);

        // Step 3: Determine how many distinct timeslots (colors) are used.
        int numTimeslots = 0;
        foreach (int color in colors)
        {
            if (color + 1 > numTimeslots)
                numTimeslots = color + 1;
        }

        // Step 4: Display the final assignment of lectures to timeslots.
        Console.WriteLine("Final Lecture Timeslot Assignments:");
        for (int i = 0; i < colors.Length; i++)
        {
            Console.WriteLine($"Lecture {i} -> Timeslot {colors[i]}");
        }

        Console.WriteLine($"\nMinimum number of timeslots required: {numTimeslots}");

        // Step 5: Calculate and display the number of conflicts.
        int conflicts = CalculateConflicts(conflictMatrix, colors);
        Console.WriteLine($"Number of conflicting students: {conflicts}");

        // Step 6: Pause the console so the user can read the output.
        Console.WriteLine("\nPress any key to exit...");
        Console.ReadKey();
    }
}
