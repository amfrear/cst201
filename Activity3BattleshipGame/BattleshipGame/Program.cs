using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    internal class Program
    {
        // Constants representing board size and cell states
        const int BoardSize = 10;       // Size of the board (10x10)
        const char EmptyCell = '.';    // Empty cell on the board
        const char HitCell = 'X';      // Cell that was hit
        const char MissCell = 'O';     // Cell that was missed
        const char Destroyer = 'D';    // Symbol for the destroyer ship
        const char Submarine = 'S';    // Symbol for the submarine ship
        const char Cruiser = 'C';      // Symbol for the cruiser ship

        // Enum representing the computer's AI state
        enum ComputerAiState { Hunt, TargetOneHit, OrientationLocked }

        // AI variables to track its state and targeting information
        static ComputerAiState computerState = ComputerAiState.Hunt; // Current state of the AI
        static (int row, int col) firstHit = (-1, -1);  // Coordinates of the first successful hit
        static (int row, int col) secondHit = (-1, -1); // Coordinates of the second successful hit
        static (int dr, int dc) orientationOffset = (0, 0); // Direction to continue when orientation is locked
        static char targetShipSymbol = '\0'; // The ship type currently being targeted

        static void Main()
        {
            // Initialize boards for the player and computer
            char[,] playerBoard = InitializeBoard();
            char[,] computerBoard = InitializeBoard();
            char[,] computerVisibleBoard = InitializeBoard(); // Player's view of the computer's board

            // Display initial empty boards
            DisplayBothBoards(playerBoard, computerVisibleBoard);

            Console.WriteLine("Welcome to Battleship!");
            Console.WriteLine("Place your ships on the board.\n");

            // Player places their ships
            PlacePlayerShips(playerBoard);

            // Computer places its ships randomly
            RandomlyPlaceComputerShips(computerBoard);

            Console.WriteLine("\nGame starts now!\n");
            bool playerTurn = true; // Track whose turn it is (true = player, false = computer)

            // Main game loop
            while (true)
            {
                if (playerTurn)
                {
                    // Player's turn
                    bool hit = PlayerTurn(computerBoard, computerVisibleBoard);
                    DisplayBothBoards(playerBoard, computerVisibleBoard);

                    // If the player misses, switch to the computer's turn
                    if (!hit)
                        playerTurn = false;
                }
                else
                {
                    // Computer's turn
                    bool hit = ComputerTurn(playerBoard);
                    DisplayBothBoards(playerBoard, computerVisibleBoard);

                    // If the computer misses, switch to the player's turn
                    if (!hit)
                        playerTurn = true;
                }

                // Check if the game is over
                if (IsGameOver(computerBoard))
                {
                    Console.WriteLine("Congratulations! You sank all the computer's ships. You win!");
                    break;
                }
                else if (IsGameOver(playerBoard))
                {
                    Console.WriteLine("The computer sank all your ships. You lose!");
                    break;
                }
            }
        }

        // Initializes a game board with empty cells
        static char[,] InitializeBoard()
        {
            char[,] board = new char[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = EmptyCell; // Fill each cell with the EmptyCell character
                }
            }
            return board;
        }

        // Displays the player's board and the computer's visible board side by side
        static void DisplayBothBoards(char[,] playerBoard, char[,] computerVisibleBoard)
        {
            Console.WriteLine("   Your Board                  Computer's Board");
            Console.WriteLine("   0 1 2 3 4 5 6 7 8 9        0 1 2 3 4 5 6 7 8 9");
            Console.WriteLine("  +---------------------+    +---------------------+");

            for (int i = 0; i < BoardSize; i++)
            {
                Console.Write(i + " | ");
                for (int j = 0; j < BoardSize; j++)
                {
                    // Display the player's board
                    Console.Write(playerBoard[i, j] + " ");
                }

                Console.Write("|    " + i + " | ");

                for (int j = 0; j < BoardSize; j++)
                {
                    // Display the computer's visible board (hides unhit ships)
                    Console.Write(computerVisibleBoard[i, j] + " ");
                }

                Console.WriteLine("|");
            }

            Console.WriteLine("  +---------------------+    +---------------------+");
        }

        // Allows the player to place their ships on the board
        static void PlacePlayerShips(char[,] board)
        {
            // Place each ship type with its respective size and symbol
            PlaceShip(board, "Destroyer (2x2 square)", 2, Destroyer);
            PlaceShip(board, "Submarine (3 diagonal cells)", 3, Submarine);
            PlaceShip(board, "Cruiser (3 consecutive cells)", 3, Cruiser);
        }

        // Handles the placement of a single ship on the board
        static void PlaceShip(char[,] board, string shipName, int size, char symbol)
        {
            Console.WriteLine($"Place your {shipName}:");
            while (true)
            {
                Console.Write("Enter starting row and column (row col): ");
                string? rawInput = Console.ReadLine(); // Get user input
                string[] input = rawInput?.Split() ?? Array.Empty<string>();

                // Validate input
                if (input.Length != 2 ||
                    !int.TryParse(input[0], out int row) ||
                    !int.TryParse(input[1], out int col))
                {
                    Console.WriteLine("Invalid input. Please enter two numbers separated by a space.");
                    continue;
                }

                Console.Write("Enter orientation (h for horizontal, v for vertical, d for diagonal): ");
                char orientation = Console.ReadKey().KeyChar; // Get ship orientation
                Console.WriteLine();

                // Get the cells occupied by the ship
                List<(int, int)> shipCells = GetShipCells(row, col, size, orientation);
                if (IsValidPlacement(board, shipCells))
                {
                    // Place the ship on the board if placement is valid
                    PlaceShipOnBoard(board, shipCells, symbol);
                    Console.WriteLine($"Successfully placed your {shipName}!");
                    DisplayBothBoards(board, InitializeBoard());
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid placement. Try again.");
                }
            }
        }

        // Randomly places the computer's ships on its board
        static void RandomlyPlaceComputerShips(char[,] board)
        {
            Random rand = new Random();
            PlaceRandomShip(board, 2, Destroyer, rand); // Place the destroyer
            PlaceRandomShip(board, 3, Submarine, rand); // Place the submarine
            PlaceRandomShip(board, 3, Cruiser, rand);  // Place the cruiser
        }

        // Randomly places a single ship on the board
        static void PlaceRandomShip(char[,] board, int size, char symbol, Random rand)
        {
            while (true)
            {
                int row = rand.Next(BoardSize); // Random starting row
                int col = rand.Next(BoardSize); // Random starting column
                char orientation = new[] { 'h', 'v', 'd' }[rand.Next(3)]; // Random orientation
                List<(int, int)> shipCells = GetShipCells(row, col, size, orientation);

                if (IsValidPlacement(board, shipCells))
                {
                    // Place the ship if the placement is valid
                    PlaceShipOnBoard(board, shipCells, symbol);
                    break;
                }
            }
        }

        // Calculates the cells occupied by a ship based on its size and orientation
        static List<(int, int)> GetShipCells(int row, int col, int size, char orientation)
        {
            List<(int, int)> cells = new();
            for (int i = 0; i < size; i++)
            {
                if (orientation == 'h') cells.Add((row, col + i)); // Horizontal placement
                else if (orientation == 'v') cells.Add((row + i, col)); // Vertical placement
                else if (orientation == 'd') cells.Add((row + i, col + i)); // Diagonal placement
            }
            return cells;
        }

        // Validates that the ship placement is within bounds and does not overlap with other ships
        static bool IsValidPlacement(char[,] board, List<(int, int)> cells)
        {
            foreach (var (row, col) in cells)
            {
                // Check if cell is within bounds and not already occupied
                if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize || board[row, col] != EmptyCell)
                    return false;
            }
            return true;
        }

        // Places a ship on the board by filling the specified cells with the ship's symbol
        static void PlaceShipOnBoard(char[,] board, List<(int, int)> cells, char symbol)
        {
            foreach (var (row, col) in cells)
            {
                board[row, col] = symbol; // Mark the cell with the ship's symbol
            }
        }

        // Handles the player's turn to attack the computer's board
        static bool PlayerTurn(char[,] computerBoard, char[,] computerVisibleBoard)
        {
            while (true) // Loop until the player makes a valid move
            {
                Console.Write("Enter your attack coordinates (row col): ");
                string? rawInput = Console.ReadLine(); // Read input from the player
                string[] input = rawInput?.Split() ?? Array.Empty<string>(); // Split input into row and column

                // Validate the player's input (ensure it's numeric and in the correct format)
                if (input.Length != 2 ||
                    !int.TryParse(input[0], out int row) ||
                    !int.TryParse(input[1], out int col))
                {
                    Console.WriteLine("Invalid input. Please enter two numbers separated by a space.");
                    continue; // Ask for input again
                }

                // Check if the entered coordinates are within the bounds of the board
                if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
                {
                    Console.WriteLine("Invalid coordinates. Try again.");
                    continue; // Ask for input again
                }

                // Check if the cell has already been targeted
                if (computerVisibleBoard[row, col] != EmptyCell)
                {
                    Console.WriteLine("Cell already targeted. Try again.");
                    continue; // Ask for input again
                }

                // Determine what is in the targeted cell on the computer's board
                char targetCell = computerBoard[row, col];

                // Check if the target is part of a ship (Destroyer, Submarine, or Cruiser)
                if (targetCell == Destroyer || targetCell == Submarine || targetCell == Cruiser)
                {
                    Console.WriteLine("Hit!"); // Inform the player of a successful hit
                    computerBoard[row, col] = HitCell;      // Mark the hidden computer board
                    computerVisibleBoard[row, col] = HitCell; // Mark the player's visible board

                    // Check if the entire ship has been sunk
                    if (!SymbolRemaining(computerBoard, targetCell))
                    {
                        // Notify the player of the ship that was sunk
                        switch (targetCell)
                        {
                            case Destroyer: Console.WriteLine("You sank the Destroyer!"); break;
                            case Submarine: Console.WriteLine("You sank the Submarine!"); break;
                            case Cruiser: Console.WriteLine("You sank the Cruiser!"); break;
                        }
                    }

                    return true; // Allow the player to take another turn
                }
                else
                {
                    // Missed shot
                    Console.WriteLine("Miss!");
                    computerVisibleBoard[row, col] = MissCell; // Mark the missed shot on the visible board
                    return false; // Switch to the computer's turn
                }
            }
        }

        // Handles the computer's turn to attack the player's board
        static bool ComputerTurn(char[,] playerBoard)
        {
            while (true) // Loop until the computer makes a valid move
            {
                // Decide the next cell to target based on the AI's state (Hunt, TargetOneHit, OrientationLocked)
                (int row, int col) cellToShoot = PickNextShot(playerBoard);

                // If no valid cells are left to shoot, return false (should not happen in practice)
                if (cellToShoot.row == -1)
                {
                    return false; // No valid move available
                }

                // Fire at the chosen cell
                char cellValue = playerBoard[cellToShoot.row, cellToShoot.col];

                // Skip if the cell has already been shot
                if (cellValue == HitCell || cellValue == MissCell)
                    continue;

                // If the cell contains a ship, it is a hit
                if (cellValue == Destroyer || cellValue == Submarine || cellValue == Cruiser)
                {
                    Console.WriteLine($"Computer hit your ship at ({cellToShoot.row}, {cellToShoot.col})!");
                    playerBoard[cellToShoot.row, cellToShoot.col] = HitCell; // Mark the cell as hit

                    // Handle the AI's state transitions
                    if (computerState == ComputerAiState.Hunt)
                    {
                        // If in Hunt mode, transition to TargetOneHit mode
                        computerState = ComputerAiState.TargetOneHit;
                        firstHit = cellToShoot;       // Store the coordinates of the first hit
                        targetShipSymbol = cellValue; // Remember the type of ship being targeted
                    }
                    else if (computerState == ComputerAiState.TargetOneHit)
                    {
                        // If a second hit is made, lock the orientation
                        secondHit = cellToShoot; // Store the coordinates of the second hit
                        orientationOffset = ComputeOffset(firstHit, secondHit); // Calculate orientation
                        computerState = ComputerAiState.OrientationLocked; // Transition to OrientationLocked mode
                    }

                    // Check if the entire ship has been sunk
                    if (!SymbolRemaining(playerBoard, cellValue))
                    {
                        PrintShipSunk(cellValue); // Notify the player of the sunk ship
                        ResetToHunt(); // Reset the AI to Hunt mode
                    }

                    return true; // Allow the computer to take another turn if it hit
                }
                else
                {
                    // If the cell does not contain a ship, it is a miss
                    Console.WriteLine($"Computer missed at ({cellToShoot.row}, {cellToShoot.col}).");
                    playerBoard[cellToShoot.row, cellToShoot.col] = MissCell; // Mark the missed shot
                    return false; // End the computer's turn
                }
            }
        }

        // Determines the next cell for the computer to target based on its current AI state
        static (int row, int col) PickNextShot(char[,] board)
        {
            switch (computerState)
            {
                case ComputerAiState.Hunt:
                    // In Hunt mode, pick a random untried cell
                    return HuntRandomCell(board);

                case ComputerAiState.TargetOneHit:
                    // In TargetOneHit mode, find an untried neighbor of the first hit
                    var neighbor = FindUntriedNeighbor(board, firstHit.row, firstHit.col);
                    if (neighbor.row != -1)
                    {
                        return neighbor; // Return a valid neighbor
                    }
                    else
                    {
                        // If no valid neighbors remain, reset to Hunt mode
                        ResetToHunt();
                        return HuntRandomCell(board);
                    }

                case ComputerAiState.OrientationLocked:
                    // In OrientationLocked mode, continue shooting along the locked direction
                    var orientedShot = FindNextOrientedShot(board);
                    if (orientedShot.row != -1)
                    {
                        return orientedShot; // Return the next oriented shot
                    }
                    else
                    {
                        // If no valid oriented shots remain, reset to Hunt mode
                        ResetToHunt();
                        return HuntRandomCell(board);
                    }

                default:
                    return (-1, -1); // Fallback case (should not occur)
            }
        }

        // Picks a random untried cell for the computer to target
        static (int row, int col) HuntRandomCell(char[,] board)
        {
            Random rand = new Random();

            for (int attempt = 0; attempt < 200; attempt++) // Attempt up to 200 times to find a valid cell
            {
                int row = rand.Next(BoardSize); // Random row
                int col = rand.Next(BoardSize); // Random column

                // If the cell has not been hit or missed, it is a valid target
                if (board[row, col] != HitCell && board[row, col] != MissCell)
                {
                    return (row, col); // Return the chosen cell
                }
            }

            return (-1, -1); // If no valid cell is found, return an invalid cell
        }

        // Finds an untried neighbor of a given cell (row, col)
        static (int row, int col) FindUntriedNeighbor(char[,] board, int r, int c)
        {
            int[] deltaR = { -1, -1, -1, 0, 0, 1, 1, 1 }; // Row offsets for up, down, left, right, diagonals
            int[] deltaC = { -1, 0, 1, -1, 1, -1, 0, 1 }; // Column offsets for up, down, left, right, diagonals

            for (int i = 0; i < deltaR.Length; i++) // Loop through all four directions
            {
                int nr = r + deltaR[i]; // Calculate the neighbor's row
                int nc = c + deltaC[i]; // Calculate the neighbor's column

                // Ensure the neighbor is within bounds
                if (nr >= 0 && nr < BoardSize && nc >= 0 && nc < BoardSize)
                {
                    // If the neighbor has not been hit or missed, it is a valid target
                    if (board[nr, nc] != HitCell && board[nr, nc] != MissCell)
                    {
                        return (nr, nc); // Return the neighbor's coordinates
                    }
                }
            }

            return (-1, -1); // If no valid neighbor is found, return an invalid cell
        }

        // Determines the next cell to target when the computer's AI is in "OrientationLocked" state
        static (int row, int col) FindNextOrientedShot(char[,] board)
        {
            // Try extending forward from the second hit using the current orientation
            var shot = ExtendInDirection(board, secondHit, orientationOffset.dr, orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // If forward doesn't work, try extending backward from the second hit
            shot = ExtendInDirection(board, secondHit, -orientationOffset.dr, -orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // If no valid cells found around the second hit, try forward from the first hit
            shot = ExtendInDirection(board, firstHit, orientationOffset.dr, orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // If forward from the first hit also fails, try backward from the first hit
            shot = ExtendInDirection(board, firstHit, -orientationOffset.dr, -orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // If none of the above directions work, no oriented shot remains
            return (-1, -1);
        }

        // Finds the next cell to target in a specific direction, starting from a given cell
        static (int row, int col) ExtendInDirection(char[,] board, (int row, int col) start, int dr, int dc)
        {
            int r = start.row + dr; // Move to the next row based on the direction
            int c = start.col + dc; // Move to the next column based on the direction

            // Continue while the coordinates remain within the board bounds
            while (r >= 0 && r < BoardSize && c >= 0 && c < BoardSize)
            {
                // If the cell is marked as a miss, stop searching in this direction
                if (board[r, c] == MissCell)
                {
                    return (-1, -1); // Invalid cell; cannot proceed further
                }
                // If the cell is already hit, continue searching further along the same direction
                else if (board[r, c] == HitCell)
                {
                    r += dr; // Move further along the row direction
                    c += dc; // Move further along the column direction
                    continue;
                }
                // If the cell is untried or contains part of a ship, return it as the next target
                return (r, c);
            }

            // If no valid cell is found in this direction, return an invalid cell
            return (-1, -1);
        }

        // Calculates the direction (dr, dc) based on two hits to determine the orientation of a ship
        static (int dr, int dc) ComputeOffset((int row, int col) first, (int row, int col) second)
        {
            int dr = second.row - first.row; // Row difference between the first and second hit
            int dc = second.col - first.col; // Column difference between the first and second hit

            // Normalize the direction values to {-1, 0, 1} for easier processing
            if (dr != 0) dr = dr / Math.Abs(dr); // Scale the row difference
            if (dc != 0) dc = dc / Math.Abs(dc); // Scale the column difference

            return (dr, dc); // Return the normalized direction as (row offset, column offset)
        }

        // Resets the computer's AI state to "Hunt" mode after successfully sinking a ship
        static void ResetToHunt()
        {
            computerState = ComputerAiState.Hunt; // Set the state back to Hunt
            firstHit = (-1, -1); // Clear the coordinates of the first hit
            secondHit = (-1, -1); // Clear the coordinates of the second hit
            orientationOffset = (0, 0); // Reset the orientation offset
            targetShipSymbol = '\0'; // Clear the target ship symbol
        }

        // Prints a message indicating which ship was sunk by the computer
        static void PrintShipSunk(char symbol)
        {
            switch (symbol)
            {
                case Destroyer:
                    Console.WriteLine("Computer sank your Destroyer!"); // Notify the player about the Destroyer
                    break;
                case Submarine:
                    Console.WriteLine("Computer sank your Submarine!"); // Notify the player about the Submarine
                    break;
                case Cruiser:
                    Console.WriteLine("Computer sank your Cruiser!"); // Notify the player about the Cruiser
                    break;
            }
        }

        // Checks if any cells of a specific ship symbol remain on the board
        static bool SymbolRemaining(char[,] board, char shipSymbol)
        {
            // Iterate through the entire board
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    // If any cell still contains the ship symbol, return true
                    if (board[r, c] == shipSymbol)
                        return true;
                }
            }
            // If no cells of the ship symbol are found, return false
            return false;
        }

        // Checks if the game is over by determining if any ships remain on the board
        static bool IsGameOver(char[,] board)
        {
            // Iterate through all cells on the board
            foreach (char cell in board)
            {
                // If any cell contains a ship symbol, the game is not over
                if (cell == Destroyer || cell == Submarine || cell == Cruiser)
                    return false;
            }
            // If no ships remain, the game is over
            return true;
        }
    }
}
