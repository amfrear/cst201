using System;
using System.Collections.Generic;

namespace BattleshipGame
{
    internal class Program
    {
        const int BoardSize = 10;
        const char EmptyCell = '.';
        const char HitCell = 'X';
        const char MissCell = 'O';
        const char Destroyer = 'D';
        const char Submarine = 'S';
        const char Cruiser = 'C';

        // Tells whether we’re hunting (no current hits), 
        // have one hit (“target1” mode), or have locked orientation (“target2” mode).
        enum ComputerAiState { Hunt, TargetOneHit, OrientationLocked }

        // Current AI state
        static ComputerAiState computerState = ComputerAiState.Hunt;

        // We remember the coordinates of the first and second hits to figure out orientation
        static (int row, int col) firstHit = (-1, -1);
        static (int row, int col) secondHit = (-1, -1);

        // The orientation offset once we detect it (dr, dc), e.g. (0,1) for horizontal-right
        static (int dr, int dc) orientationOffset = (0, 0);

        // We track the symbol (D, S, or C) of the ship we're trying to finish off
        static char targetShipSymbol = '\0';

        static void Main()
        {
            char[,] playerBoard = InitializeBoard();
            char[,] computerBoard = InitializeBoard();
            char[,] computerVisibleBoard = InitializeBoard();

            // Display empty boards at startup:
            DisplayBothBoards(playerBoard, computerVisibleBoard);

            Console.WriteLine("Welcome to Battleship!");
            Console.WriteLine("Place your ships on the board.\n");

            // Player places ships
            PlacePlayerShips(playerBoard);

            // Computer places ships randomly
            RandomlyPlaceComputerShips(computerBoard);

            Console.WriteLine("\nGame starts now!\n");
            bool playerTurn = true;

            // Game loop
            while (true)
            {
                if (playerTurn)
                {
                    // Take the shot and see if it was a hit or miss:
                    bool hit = PlayerTurn(computerBoard, computerVisibleBoard);

                    // Show the new state:
                    DisplayBothBoards(playerBoard, computerVisibleBoard);

                    // If the shot was a miss, switch to the computer:
                    if (!hit)
                        playerTurn = false;
                }
                else
                {
                    bool hit = ComputerTurn(playerBoard);

                    DisplayBothBoards(playerBoard, computerVisibleBoard);

                    if (!hit)
                        playerTurn = true;
                }

                // Check for game over:
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

        static char[,] InitializeBoard()
        {
            char[,] board = new char[BoardSize, BoardSize];
            for (int i = 0; i < BoardSize; i++)
            {
                for (int j = 0; j < BoardSize; j++)
                {
                    board[i, j] = EmptyCell;
                }
            }
            return board;
        }

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
                    char playerCell = playerBoard[i, j];

                    Console.Write(playerCell + " ");
                }

                Console.Write("|    " + i + " | ");

                for (int j = 0; j < BoardSize; j++)
                {
                    char computerCell = computerVisibleBoard[i, j];
                    Console.Write(computerCell + " ");
                }

                Console.WriteLine("|");
            }

            Console.WriteLine("  +---------------------+    +---------------------+");
        }

        static void PlacePlayerShips(char[,] board)
        {
            PlaceShip(board, "Destroyer (2x2 square)", 2, Destroyer);
            PlaceShip(board, "Submarine (3 diagonal cells)", 3, Submarine);
            PlaceShip(board, "Cruiser (3 consecutive cells)", 3, Cruiser);
        }

        static void PlaceShip(char[,] board, string shipName, int size, char symbol)
        {
            Console.WriteLine($"Place your {shipName}:");
            while (true)
            {
                Console.Write("Enter starting row and column (row col): ");
                string? rawInput = Console.ReadLine();
                string[] input = rawInput?.Split() ?? Array.Empty<string>();

                if (input.Length != 2 ||
                    !int.TryParse(input[0], out int row) ||
                    !int.TryParse(input[1], out int col))
                {
                    Console.WriteLine("Invalid input. Please enter two numbers separated by a space.");
                    continue;
                }

                Console.Write("Enter orientation (h for horizontal, v for vertical, d for diagonal): ");
                char orientation = Console.ReadKey().KeyChar;
                Console.WriteLine();

                List<(int, int)> shipCells = GetShipCells(row, col, size, orientation);
                if (IsValidPlacement(board, shipCells))
                {
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

        static void RandomlyPlaceComputerShips(char[,] board)
        {
            Random rand = new Random();
            PlaceRandomShip(board, 2, Destroyer, rand);
            PlaceRandomShip(board, 3, Submarine, rand);
            PlaceRandomShip(board, 3, Cruiser, rand);
        }

        static void PlaceRandomShip(char[,] board, int size, char symbol, Random rand)
        {
            while (true)
            {
                int row = rand.Next(BoardSize);
                int col = rand.Next(BoardSize);
                char orientation = new[] { 'h', 'v', 'd' }[rand.Next(3)];
                List<(int, int)> shipCells = GetShipCells(row, col, size, orientation);

                if (IsValidPlacement(board, shipCells))
                {
                    PlaceShipOnBoard(board, shipCells, symbol);
                    break;
                }
            }
        }

        static List<(int, int)> GetShipCells(int row, int col, int size, char orientation)
        {
            List<(int, int)> cells = new();
            for (int i = 0; i < size; i++)
            {
                if (orientation == 'h') cells.Add((row, col + i));
                else if (orientation == 'v') cells.Add((row + i, col));
                else if (orientation == 'd') cells.Add((row + i, col + i));
            }
            return cells;
        }

        static bool IsValidPlacement(char[,] board, List<(int, int)> cells)
        {
            foreach (var (row, col) in cells)
            {
                if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize || board[row, col] != EmptyCell)
                    return false;
            }
            return true;
        }

        static void PlaceShipOnBoard(char[,] board, List<(int, int)> cells, char symbol)
        {
            foreach (var (row, col) in cells)
            {
                board[row, col] = symbol;
            }
        }

        static bool PlayerTurn(char[,] computerBoard, char[,] computerVisibleBoard)
        {
            while (true)
            {
                Console.Write("Enter your attack coordinates (row col): ");
                string? rawInput = Console.ReadLine();
                string[] input = rawInput?.Split() ?? Array.Empty<string>();

                if (input.Length != 2 ||
                    !int.TryParse(input[0], out int row) ||
                    !int.TryParse(input[1], out int col))
                {
                    Console.WriteLine("Invalid input. Please enter two numbers separated by a space.");
                    continue;
                }

                if (row < 0 || row >= BoardSize || col < 0 || col >= BoardSize)
                {
                    Console.WriteLine("Invalid coordinates. Try again.");
                    continue;
                }

                if (computerVisibleBoard[row, col] != EmptyCell)
                {
                    Console.WriteLine("Cell already targeted. Try again.");
                    continue;
                }

                // Check what’s in the computer's board at [row, col]
                char targetCell = computerBoard[row, col];

                // If it's any ship (D, S, or C), it's a hit
                if (targetCell == Destroyer || targetCell == Submarine || targetCell == Cruiser)
                {
                    Console.WriteLine("Hit!");
                    computerBoard[row, col] = HitCell;      // Mark the hidden computer board
                    computerVisibleBoard[row, col] = HitCell; // Mark the "visible" computer board

                    // Check if that entire ship was sunk
                    if (!SymbolRemaining(computerBoard, targetCell))
                    {
                        switch (targetCell)
                        {
                            case Destroyer: Console.WriteLine("You sank the Destroyer!"); break;
                            case Submarine: Console.WriteLine("You sank the Submarine!"); break;
                            case Cruiser: Console.WriteLine("You sank the Cruiser!"); break;
                        }
                    }

                    return true;
                }
                else
                {
                    // Miss
                    Console.WriteLine("Miss!");
                    computerVisibleBoard[row, col] = MissCell;
                    return false;
                }
            }
        }

        static bool ComputerTurn(char[,] playerBoard)
        {
            // We'll loop until we find a valid cell to shoot
            while (true)
            {
                // Decide where to shoot based on our current AI state
                (int row, int col) cellToShoot = PickNextShot(playerBoard);

                // If we can’t find any new cell (shouldn’t happen unless board is full of X/O), break
                if (cellToShoot.row == -1)
                {
                    // Fallback: no valid cells left
                    return false;
                }

                // Fire at this cell
                char cellValue = playerBoard[cellToShoot.row, cellToShoot.col];

                // If already shot, skip it
                if (cellValue == HitCell || cellValue == MissCell)
                    continue;

                if (cellValue == Destroyer || cellValue == Submarine || cellValue == Cruiser)
                {
                    // We got a hit
                    Console.WriteLine($"Computer hit your ship at ({cellToShoot.row}, {cellToShoot.col})!");
                    playerBoard[cellToShoot.row, cellToShoot.col] = HitCell;

                    // If we're currently "hunting", switch to "one-hit" mode
                    if (computerState == ComputerAiState.Hunt)
                    {
                        computerState = ComputerAiState.TargetOneHit;
                        firstHit = cellToShoot;       // store where we hit
                        targetShipSymbol = cellValue; // remember which ship we’re hunting
                    }
                    else if (computerState == ComputerAiState.TargetOneHit)
                    {
                        // We have a second hit => we can lock orientation
                        secondHit = cellToShoot;
                        orientationOffset = ComputeOffset(firstHit, secondHit);

                        computerState = ComputerAiState.OrientationLocked;
                        // We already know the ship symbol from the first hit
                    }
                    // If we’re already orientation-locked, we just keep going in that direction next time

                    // Check if that entire ship is now sunk
                    if (!SymbolRemaining(playerBoard, cellValue))
                    {
                        PrintShipSunk(cellValue);
                        ResetToHunt();
                    }

                    // Return true => the computer stays on turn (if you allow consecutive hits)
                    return true;
                }
                else
                {
                    // It's a miss
                    Console.WriteLine($"Computer missed at ({cellToShoot.row}, {cellToShoot.col}).");
                    playerBoard[cellToShoot.row, cellToShoot.col] = MissCell;

                    // If we were in orientation-locked mode, that direction is probably exhausted. 
                    // But we might still keep going the other direction. 
                    // We'll handle that in the next shot via `PickNextShot`.

                    return false; // Computer turn ends on a miss
                }
            }
        }

        static (int row, int col) PickNextShot(char[,] board)
        {
            switch (computerState)
            {
                case ComputerAiState.Hunt:
                    // Pick a random cell that isn't X or O
                    return HuntRandomCell(board);

                case ComputerAiState.TargetOneHit:
                    // We have exactly one hit => check the 8 neighbors of firstHit
                    // for an untried cell
                    var neighbor = FindUntriedNeighbor(board, firstHit.row, firstHit.col);
                    if (neighbor.row != -1)
                    {
                        return neighbor;
                    }
                    else
                    {
                        // No neighbors left => revert to hunt
                        ResetToHunt();
                        return HuntRandomCell(board);
                    }

                case ComputerAiState.OrientationLocked:
                    // We have two hits => we know (dr, dc). 
                    // We'll try continuing in that direction from secondHit forward or backward.
                    var orientedShot = FindNextOrientedShot(board);
                    if (orientedShot.row != -1)
                    {
                        return orientedShot;
                    }
                    else
                    {
                        // If we can’t find any more along orientation, revert to hunt
                        ResetToHunt();
                        return HuntRandomCell(board);
                    }

                default:
                    // Fallback
                    return (-1, -1);
            }
        }

        static (int row, int col) HuntRandomCell(char[,] board)
        {
            Random rand = new Random();

            for (int attempt = 0; attempt < 200; attempt++)
            {
                int row = rand.Next(BoardSize);
                int col = rand.Next(BoardSize);
                // If it's not X or O, we can shoot here
                if (board[row, col] != HitCell && board[row, col] != MissCell)
                {
                    return (row, col);
                }
            }

            // If we somehow failed to find any cell in 200 tries (board almost full?), fail out
            return (-1, -1);
        }

        static (int row, int col) FindUntriedNeighbor(char[,] board, int r, int c)
        {
            // 8 directions: up/down/left/right + diagonals
            int[] deltaR = { -1, -1, -1, 0, 0, 1, 1, 1 };
            int[] deltaC = { -1, 0, 1, -1, 1, -1, 0, 1 };

            for (int i = 0; i < deltaR.Length; i++)
            {
                int nr = r + deltaR[i];
                int nc = c + deltaC[i];

                // Check bounds
                if (nr < 0 || nr >= BoardSize || nc < 0 || nc >= BoardSize)
                    continue;

                // If we haven't shot here, let's try it
                if (board[nr, nc] != HitCell && board[nr, nc] != MissCell)
                {
                    return (nr, nc);
                }
            }

            // No untried neighbors
            return (-1, -1);
        }

        static (int row, int col) FindNextOrientedShot(char[,] board)
        {
            // #1 Forward from secondHit
            var shot = ExtendInDirection(board, secondHit, orientationOffset.dr, orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // #2 Backward from secondHit
            shot = ExtendInDirection(board, secondHit, -orientationOffset.dr, -orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // #3 Forward from firstHit
            shot = ExtendInDirection(board, firstHit, orientationOffset.dr, orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // #4 Backward from firstHit
            shot = ExtendInDirection(board, firstHit, -orientationOffset.dr, -orientationOffset.dc);
            if (shot.row != -1)
                return shot;

            // If all four directions yield nothing, no oriented shot remains
            return (-1, -1);
        }

        static (int row, int col) ExtendInDirection(char[,] board,
    (int row, int col) start, int dr, int dc)
        {
            int r = start.row + dr;
            int c = start.col + dc;

            while (r >= 0 && r < BoardSize && c >= 0 && c < BoardSize)
            {
                // If it's a miss, stop **immediately** and return -1
                if (board[r, c] == MissCell)
                {
                    return (-1, -1);
                }
                // If it's a hit, skip and keep going
                else if (board[r, c] == HitCell)
                {
                    r += dr;
                    c += dc;
                    continue;
                }
                // If it's unshot or a ship cell, let's shoot there
                return (r, c);
            }

            return (-1, -1);
        }

        static (int dr, int dc) ComputeOffset((int row, int col) first, (int row, int col) second)
        {
            int dr = second.row - first.row;
            int dc = second.col - first.col;

            // Normalize so we don’t end up with (2,2) if the hits are 2 cells apart
            // Typically ships are adjacent, so we expect dr,dc in {-1,0,1}. 
            // But if your code allows 2 or more empty cells between hits, you may need to scale down.
            if (dr != 0) dr = dr / Math.Abs(dr);
            if (dc != 0) dc = dc / Math.Abs(dc);

            return (dr, dc);
        }
        static void ResetToHunt()
        {
            computerState = ComputerAiState.Hunt;
            firstHit = (-1, -1);
            secondHit = (-1, -1);
            orientationOffset = (0, 0);
            targetShipSymbol = '\0';
        }

        static void PrintShipSunk(char symbol)
        {
            switch (symbol)
            {
                case Destroyer:
                    Console.WriteLine("Computer sank your Destroyer!");
                    break;
                case Submarine:
                    Console.WriteLine("Computer sank your Submarine!");
                    break;
                case Cruiser:
                    Console.WriteLine("Computer sank your Cruiser!");
                    break;
            }
        }


        static bool SymbolRemaining(char[,] board, char shipSymbol)
        {
            for (int r = 0; r < BoardSize; r++)
            {
                for (int c = 0; c < BoardSize; c++)
                {
                    if (board[r, c] == shipSymbol)
                        return true; // We found at least one cell of that ship
                }
            }
            return false; // No cells of that ship remain
        }

        static bool IsGameOver(char[,] board)
        {
            foreach (char cell in board)
            {
                if (cell == Destroyer || cell == Submarine || cell == Cruiser)
                    return false;
            }
            return true;
        }
    }
}
