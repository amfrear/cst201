using System;
using System.Collections.Generic;

namespace BridgeCrossing
{
    internal class Program
    {
        class Person
        {
            public string Name { get; set; }
            public int CrossingTime { get; set; }

            public Person(string name, int crossingTime)
            {
                Name = name;
                CrossingTime = crossingTime;
            }
        }

        static void Main(string[] args)
        {
            // Prompt the user for the number of people
            Console.WriteLine("Enter the number of people:");
            if (!int.TryParse(Console.ReadLine(), out int numPeople) || numPeople < 2)
            {
                Console.WriteLine("Invalid input. The number of people must be at least 2.");
                return;
            }

            // Create a list to store people
            List<Person> people = new List<Person>();

            // Prompt the user for each person's crossing time
            for (int i = 1; i <= numPeople; i++)
            {
                Console.WriteLine($"Enter the crossing time for Person {i}:");
                if (int.TryParse(Console.ReadLine(), out int crossingTime) && crossingTime > 0)
                {
                    people.Add(new Person($"Person {i}", crossingTime));
                }
                else
                {
                    Console.WriteLine("Invalid crossing time. Please enter a positive integer.");
                    return;
                }
            }

            // Sort people by their crossing times
            people.Sort((p1, p2) => p1.CrossingTime.CompareTo(p2.CrossingTime));

            int totalTime = 0; // Variable to track total time spent

            Console.WriteLine("\nSimulation starts...\n");

            // While more than 3 people remain
            while (people.Count > 3)
            {
                int fastest1 = people[0].CrossingTime;
                int fastest2 = people[1].CrossingTime;
                int slowest1 = people[people.Count - 1].CrossingTime;
                int slowest2 = people[people.Count - 2].CrossingTime;

                // Strategy 1: Send the two fastest first, the fastest returns,
                // then send the two slowest, and the second fastest returns
                int option1 = fastest2 + fastest1 + slowest1 + fastest2;

                // Strategy 2: Send the two slowest first, the fastest returns,
                // then send the two fastest, and the fastest returns
                int option2 = slowest1 + slowest2 + 2 * fastest1;

                if (option1 < option2)
                {
                    // Execute Strategy 1
                    Console.WriteLine($"Send {people[0].Name} and {people[1].Name} across (Time: {fastest2} minutes)");
                    Console.WriteLine($"{people[0].Name} returns with the flashlight (Time: {fastest1} minutes)");
                    Console.WriteLine($"Send {people[people.Count - 2].Name} and {people[people.Count - 1].Name} across (Time: {slowest1} minutes)");
                    Console.WriteLine($"{people[1].Name} returns with the flashlight (Time: {fastest2} minutes)");

                    totalTime += option1;
                }
                else
                {
                    // Execute Strategy 2
                    Console.WriteLine($"Send {people[0].Name} and {people[people.Count - 1].Name} across (Time: {slowest1} minutes)");
                    Console.WriteLine($"{people[0].Name} returns with the flashlight (Time: {fastest1} minutes)");
                    Console.WriteLine($"Send {people[0].Name} and {people[1].Name} across (Time: {fastest2} minutes)");
                    Console.WriteLine($"{people[0].Name} returns with the flashlight (Time: {fastest1} minutes)");

                    totalTime += option2;
                }

                // Remove the two slowest individuals who have crossed
                people.RemoveAt(people.Count - 1); // Remove the slowest
                people.RemoveAt(people.Count - 1); // Remove the second slowest
            }

            // Handle the remaining 2 or 3 people
            if (people.Count == 3)
            {
                Console.WriteLine($"Send {people[0].Name} and {people[1].Name} across (Time: {people[1].CrossingTime} minutes)");
                Console.WriteLine($"{people[0].Name} returns with the flashlight (Time: {people[0].CrossingTime} minutes)");
                Console.WriteLine($"Send {people[0].Name} and {people[2].Name} across (Time: {people[2].CrossingTime} minutes)");

                totalTime += people[2].CrossingTime + people[0].CrossingTime + people[1].CrossingTime;
            }
            else if (people.Count == 2)
            {
                Console.WriteLine($"Send {people[0].Name} and {people[1].Name} across (Time: {people[1].CrossingTime} minutes)");
                totalTime += people[1].CrossingTime;
            }

            // Output the total time taken
            Console.WriteLine($"\nTotal time taken: {totalTime} minutes");
        }
    }
}
