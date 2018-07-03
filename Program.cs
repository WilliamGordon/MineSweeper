using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minesweeper
{
    class Program

    {
        ////////////////
        //GAME DISPLAY//
        ////////////////

        //PRINT TABLE
        public static void PrintTable(int[,,] grid)
        {
            Console.Clear();
            Console.SetCursorPosition(28, 2);
            Console.WriteLine("MINESWEEPER");
            Console.SetCursorPosition(10, 3);
            Console.WriteLine("Press ENTER to Uncover a case, or 'F' to Flag it");
            Console.SetCursorPosition(0, 5);

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[i, y, 1] == 1)
                    {
                        if (grid[i, y, 0] == -1)
                        {
                            Console.Write(" |");
                            Console.BackgroundColor = ConsoleColor.Red;
                            Console.Write(" B ");
                            Console.ResetColor();
                            Console.Write("| ");
                        }
                        else if (grid[i, y, 0] == 0)
                        {
                            Console.Write(" |");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write("   ");
                            Console.ResetColor();
                            Console.Write("| ");
                        }
                        else if (grid[i, y, 0] == 1)
                        {
                            Console.Write(" |");
                            Console.ForegroundColor = ConsoleColor.Blue;
                            Console.Write(" 1 ");
                            Console.ResetColor();
                            Console.Write("| ");
                        }
                        else if (grid[i, y, 0] == 2)
                        {
                            Console.Write(" |");
                            Console.ForegroundColor = ConsoleColor.Green;
                            Console.Write(" 2 ");
                            Console.ResetColor();
                            Console.Write("| ");
                        }
                        else if (grid[i, y, 0] == 3)
                        {
                            Console.Write(" |");
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" 3 ");
                            Console.ResetColor();
                            Console.Write("| ");
                        }
                        else if (grid[i, y, 0] == 4)
                        {
                            Console.Write(" |");
                            Console.ForegroundColor = ConsoleColor.DarkBlue;
                            Console.Write(" 4 ");
                            Console.ResetColor();
                            Console.Write("| ");
                        }
                        else
                        {
                            Console.Write(" | " + grid[i, y, 0] + " | ");
                        }
                    }
                    else if (grid[i, y, 1] == -1)
                    {
                        Console.Write(" |");
                        Console.BackgroundColor = ConsoleColor.Blue;
                        Console.Write(" F ");
                        Console.ResetColor();
                        Console.Write("| ");
                    }
                    else if (grid[i, y, 1] == 0)
                    {
                        Console.Write(" |");
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.Write("   ");
                        Console.ResetColor();
                        Console.Write("| ");
                    }
                }
                Console.WriteLine();
                Console.WriteLine();
            }
        }

        //////////////
        //GAME SETUP//
        //////////////

        //CREATE TABLE AND POPULATING IT
        public static int[,,] createTable(int rows, int columns, int deph)
        {
            int[,,] grid = new int[rows, columns, 2];
            int counter = 0;
            for (int j = 0; j < rows; j++)
            {
                for (int k = 0; k < columns; k++)
                {
                    grid[j, k, 0] = counter;
                    counter++;
                }
            }
            return grid;
        }


        //GET LAST NUMBER OF THE TABLE BOTTOM FAR RIGHT 
        public static int GetLastNumberOfTable(int[,,] grid)
        {
            return grid[grid.GetLength(0) - 1, grid.GetLength(1) - 1, 0];
        }

        //GET RANDOM NUMBER
        public static int GetRandomNumber(int range)
        {
            Random newRandom = new Random();
            return newRandom.Next(1, range);
        }

        //GET A SET OF NUMBER REPRESENTING THE POSITION IN THE TABLE WHERE THE BOMB WILL BE DISPOSED
        public static int[] GetBombPosition(int nbBombs, int[,,] grid)
        {
            int[] PositionBomb = new int[nbBombs];
            for (int i = 0; i < nbBombs; i++)
            {
                int randomNumber = GetRandomNumber(GetLastNumberOfTable(grid));
                while (PositionBomb.Contains<int>(randomNumber))
                {
                    randomNumber = GetRandomNumber(GetLastNumberOfTable(grid));
                }
                PositionBomb[i] = randomNumber;
            }
            return PositionBomb;
        }

        //GIVE BOMB COORDONATE WHEN GIVEN THE POSITION IN TABLE
        public static int[] GetCoordinate(int[,,] grid, int position)
        {
            int[] coordinate = new int[2];
            int x = position / grid.GetLength(1);
            int y = (position % grid.GetLength(1));
            coordinate[0] = x;
            coordinate[1] = y;
            return coordinate;
        }

        //REPOPULATING TABLE WITH BOMBS  = -1
        public static void SetupBombsInTable(int[] positionBomb, int[,,] grid)
        {
            for (int i = 0; i < positionBomb.Length; i++)
            {
                int x = positionBomb[i] / grid.GetLength(1);
                int y = (positionBomb[i] % grid.GetLength(1));
                if (y == -1) { y = grid.GetLength(1) - 1; }
                grid[x, y, 0] = -1;
            }
        }

        //SET VALUE OF 2D ARRAY FIRST LAYER TO 0 EXCEPT THE BOMBS WHICH ARE REPRESENTED BY A -1 
        public static void SetupValueToZeroInTable(int[,,] grid)
        {
            for (int i = grid.GetLength(0) - 1; i >= 0; i--)
            {
                for (int y = 0; y < grid.GetLength(1); y++)
                {
                    if (grid[i, y, 0] != -1) { grid[i, y, 0] = 0; }
                }
            }
        }

        //INCREMENT THE VALUES NEAR A BOMB
        public static void incrementValuesNearBombs(int[,,] grid, int numberOfBomb, int numberOfRows, int numberOfColumns, int[] positionBOOM)
        {
            for (int i = 0; i <= numberOfBomb - 1; i++)
            {
                int[] coordinateOfBombs = GetCoordinate(grid, positionBOOM[i]);
                for (int j = -1; j <= 1; j++)        //need to loop 3 times for the 3 possibles x coordinate (-1, 0, 1)
                {
                    for (int k = -1; k <= 1; k++)    //need to loop 3 times for the 3 possibles y coordinate (-1, 0, 1)
                    {
                        if (!(j == 0 && k == 0)     //Don't increment the bomb ow position
                            && ((int)coordinateOfBombs[0] + j) >= 0                 // x can't go out of range -1
                            && ((int)coordinateOfBombs[0] + j) < numberOfRows       // x can't go out of range nbrows
                            && ((int)coordinateOfBombs[1] + k) >= 0                 // x can't go out of range -1
                            && ((int)coordinateOfBombs[1] + k) < numberOfColumns    // y can't go out of range nbcolumns
                            && (true))                                              //if the value holded in the coordinate is equal to -1 bomb do not increment
                        {
                            if (grid[((int)coordinateOfBombs[0] + j), ((int)coordinateOfBombs[1] + k), 0] != -1)
                            {
                                grid[((int)coordinateOfBombs[0] + j), ((int)coordinateOfBombs[1] + k), 0]++;
                            }
                        }
                    }
                }
            }
        }

        ////////////
        //GAMEPLAY//
        ////////////

        // REFRESH TABLE WHEN USER SELECT A CASE
        public static void Selected(int[,,] grid, int x, int y, bool flaged, int numberOfBombs)
        {                                           //CHANGE THE VALUE FROM 0 TO 1 in the SECOND LAYER
            if (!flaged)
            {
                grid[x, y, 1] = 1;

                if (grid[x, y, 0] == 0)     //IF THE VALUE ON FIRST LAYER IS ZERO UNCOVER THE VALUES AROUND IT + BASE CASE RECURSION!!
                {
                    for (int j = -1; j <= 1; j++)           //need to loop 3 times for the 3 possibles x coordinate (-1, 0, 1)
                    {
                        for (int k = -1; k <= 1; k++)       //need to loop 3 times for the 3 possibles y coordinate (-1, 0, 1)
                        {
                            // OUT OF RANGE CONDITION FOR EDGE CASE
                            if (!(j == 0 && k == 0)
                                && (x + j) >= 0                                // x can't go out of range -1
                                && (x + j) < grid.GetLength(0)                 // x can't go out of range nbrows
                                && (y + k) >= 0                                // x can't go out of range -1
                                && (y + k < grid.GetLength(1)))                // y can't go out of range nbcolumns
                            {
                                if (grid[x + j, y + k, 1] == 0)     //RECURSION
                                {
                                    Selected(grid, x + j, y + k, flaged, numberOfBombs);
                                }
                            }
                        }
                    }
                }

                if (grid[x, y, 0] == -1)    //YOU LOST BY SELECTING A BOMB!!
                {
                    for (int i = 0; i < grid.GetLength(0); i++)
                    {
                        for (int j = 0; j < grid.GetLength(1); j++)
                        {
                            grid[i, j, 1] = 1;
                        }
                    }
                }
            }
            else if (flaged)
            {
                if (grid[x, y, 1] == -1)
                {
                    grid[x, y, 1] = 0;
                }
                else
                {
                    grid[x, y, 1] = -1;
                }
            }
        }

        public static int hasWon(int[,,] grid, int numberOfBombs)
        {
            int numberOfCaseWithoutBombs = (grid.GetLength(0) * grid.GetLength(1)) - numberOfBombs;
            int counter = 0;
            int otherCounter = 0;

            for (int i = 0; i < grid.GetLength(0); i++)
            {
                for (int j = 0; j < grid.GetLength(1); j++)
                {
                    if (grid[i, j, 0] != -1 && grid[i, j, 1] == 1) { counter++; }
                    if (grid[i, j, 0] == -1 && grid[i, j, 1] == 1) { otherCounter++; }
                }
            }
            if (otherCounter > 0) { return -1; }                        // won
            else if (numberOfCaseWithoutBombs == counter) { return 1; } // lost 
            else { return 0; }                                           // continue
        }

        //////////////
        //NAVIGATION//
        //////////////

        //CREATE NAVIGATION DISPLAY MATCHING THE TABLE (SET THE CURSOR TO THE MATCHING COORDINATE OF THE TABLE) + RETURN BOOLEAN VALUE IN CASE USER WON OR LOST
        public static bool Navigation(int[,,] grid, int numberOfBombs)
        {
            int x = 3;
            int y = 5;

            ConsoleKey Key = new ConsoleKey();
            Key = Console.ReadKey().Key;

            while (true)
            {
                if (hasWon(grid, numberOfBombs) == 1) { return true; }
                else if (hasWon(grid, numberOfBombs) == -1) { return false; }
                else
                {
                    Console.Clear();
                    if (Key == ConsoleKey.LeftArrow && x > 3) { x -= 7; }
                    else if (Key == ConsoleKey.RightArrow && x < ((7 * (grid.GetLength(1) - 1)) + 3)) { x += 7; }
                    else if (Key == ConsoleKey.UpArrow && y > 5) { y -= 2; }
                    else if (Key == ConsoleKey.DownArrow && y < ((2 * grid.GetLength(0)) + 3)) { y += 2; }
                    else if (Key == ConsoleKey.Enter)
                    {
                        int xcoordinate = (x - 3) / 7;
                        int ycoordinate = (y - 5) / 2;
                        Selected(grid, ycoordinate, xcoordinate, false, numberOfBombs);
                    }
                    else if (Key == ConsoleKey.F)
                    {
                        int xcoordinate = (x - 3) / 7;
                        int ycoordinate = (y - 5) / 2;
                        Selected(grid, ycoordinate, xcoordinate, true, numberOfBombs);
                    }
                    PrintTable(grid);
                    Console.SetCursorPosition(x, y);
                    Key = Console.ReadKey().Key;
                }
            }
        }

        static void Main(string[] args)
        {
            while (true)
            {
                Console.Clear();
                var asciMinesweeper = new[]
                    {
                            @"      $$\      $$\   $$$$$$\  $$\   $$\  $$$$$$$$\   $$$$$$\   $$\      $$\  $$$$$$$$\  $$$$$$$$\  $$$$$$$\   $$$$$$$$\  $$$$$$$\        ",
                            @"      $$$\    $$$ |  \_$$  _| $$$\  $$ | $$  _____| $$  __$$\  $$ | $\  $$ | $$  _____| $$  _____| $$  __$$\  $$  _____| $$  __$$\       ",
                            @"      $$$$\  $$$$ |    $$ |   $$$$\ $$ | $$ |       $$ /  \__| $$ |$$$\ $$ | $$ |       $$ |       $$ |  $$ | $$ |       $$ |  $$ |      ",
                            @"      $$\$$\$$ $$ |    $$ |   $$ $$\$$ | $$$$$\     \$$$$$$\   $$ $$ $$\$$ | $$$$$\     $$$$$\     $$$$$$$  | $$$$$\     $$$$$$$  |      ",
                            @"      $$ \$$$  $$ |    $$ |   $$ \$$$$ | $$  __|     \____$$\  $$$$  _$$$$ | $$  __|    $$  __|    $$  ____/  $$  __|    $$  __$$<       ",
                            @"      $$ |\$  /$$ |    $$ |   $$ |\$$$ | $$ |       $$\   $$ | $$$  / \$$$ | $$ |       $$ |       $$ |       $$ |       $$ |  $$ |      ",
                            @"      $$ | \_/ $$ |  $$$$$$\  $$ | \$$ | $$$$$$$$\  \$$$$$$  | $$  /   \$$ | $$$$$$$$\  $$$$$$$$\  $$ |       $$$$$$$$\  $$ |  $$ |      ",
                            @"      \__|     \__|  \______| \__|  \__| \________|  \______/  \__/     \__| \________| \________| \__|       \________| \__|  \__|      ",
                    };

                Console.WindowWidth = 160;
                Console.WriteLine("\n\n");
                foreach (string line in asciMinesweeper)
                    Console.WriteLine(line);
                Console.SetCursorPosition(5, 14);
                Console.Write("HOW MANY ROWS: ");
                int numberOfRows = Convert.ToInt32(Console.ReadLine());
                Console.SetCursorPosition(5, 16);
                Console.Write("HOW MANY COLUMNS: ");
                int numberOfColumns = Convert.ToInt32(Console.ReadLine());
                Console.SetCursorPosition(5, 18);
                Console.Write("HOW MANY BOMBS (no more than {0}): ", (numberOfColumns * numberOfRows));
                int numberOfBomb = Convert.ToInt32(Console.ReadLine());
                Console.Clear();

                if (numberOfBomb < numberOfRows * numberOfColumns && numberOfRows > 1 && numberOfColumns > 1)
                {
                    int[,,] grid = createTable(numberOfRows, numberOfColumns, 2);
                    int[] positionBOOM = GetBombPosition(numberOfBomb, grid);
                    Array.Sort(positionBOOM);
                    SetupBombsInTable(positionBOOM, grid);
                    SetupValueToZeroInTable(grid);
                    incrementValuesNearBombs(grid, numberOfBomb, numberOfRows, numberOfColumns, positionBOOM);
                    Console.Clear();
                    PrintTable(grid);
                    Console.WriteLine();

                    while (true)
                    {
                        Navigation(grid, numberOfBomb);
                        if (Navigation(grid, numberOfBomb))
                        {
                            Console.Clear();
                            var arr = new[]
                            {
                            @"      $$\     $$\                         $$\      $$\                           $$\        ",
                            @"      \$$\   $$  |                        $$ | $\  $$ |                          $$ |       ",
                            @"       \$$\ $$  /$$$$$$\  $$\   $$\       $$ |$$$\ $$ | $$$$$$\  $$$$$$$\        $$ |       ",
                            @"        \$$$$  /$$  __$$\ $$ |  $$ |      $$ $$ $$\$$ |$$  __$$\ $$  __$$\       $$ |       ",
                            @"         \$$  / $$ /  $$ |$$ |  $$ |      $$$$  _$$$$ |$$ /  $$ |$$ |  $$ |      \__|       ",
                            @"          $$ |  $$ |  $$ |$$ |  $$ |      $$$  / \$$$ |$$ |  $$ |$$ |  $$ |                 ",
                            @"          $$ |  \$$$$$$  |\$$$$$$  |      $$  /   \$$ |\$$$$$$  |$$ |  $$ |      $$\        ",
                            @"          \__|   \______ /  \______/      \__/     \__| \______/ \__|  \__|      \__|       ",

                      };
                            Console.WindowWidth = 160;
                            Console.WriteLine("\n\n");
                            foreach (string line in arr)
                                Console.WriteLine(line);
                            Console.ReadKey();
                            break;
                        }
                        else
                        {
                            Console.Clear();
                            var asciiGameOver = new[]
                                    {
                            @"     $$$$$$\    $$$$$$\    $$\      $$\  $$$$$$$$\        $$$$$$\    $$\    $$\  $$$$$$$$\  $$$$$$$\        ",
                            @"     $$  __$$\  $$  __$$\  $$$\    $$$ | $$  _____ |      $$  __$$\  $$ |   $$ | $$  _____| $$  __$$\       ",
                            @"     $$ /  \__| $$ /  $$ | $$$$\  $$$$ | $$ |             $$ /  $$ | $$ |   $$ | $$ |       $$ |  $$ |      ",
                            @"     $$ |$$$$\  $$$$$$$$ | $$\$$\$$ $$ | $$$$$\           $$ |  $$ | \$$\  $$  | $$$$$\     $$$$$$$  |      ",
                            @"     $$ |\_$$ | $$  __$$ | $$ \$$$  $$ | $$  __ |         $$ |  $$ |  \$$\$$  /  $$  __|    $$  __$$<       ",
                            @"     $$ |  $$ | $$ |  $$ | $$ |\$  /$$ | $$ |             $$ |  $$ |   \$$$  /   $$ |       $$ |  $$ |      ",
                            @"     \$$$$$$  | $$ |  $$ | $$ | \_/ $$ | $$$$$$$$\        $$$$$$  |     \$  /    $$$$$$$$\  $$ |  $$ |      ",
                            @"      \_______/ \__|  \__|\__|     \__|  \________|       \______/       \_/     \________| \__|  \__|      ",

                    };
                            Console.WindowWidth = 160;
                            Console.WriteLine("\n\n");
                            foreach (string line in asciiGameOver)
                                Console.WriteLine(line);
                            Console.ReadKey();
                            break;
                        }
                    }
                }
                else
                {
                    Console.Clear();
                    Console.SetCursorPosition(10, 6);
                    Console.WriteLine("THERE IS TO MUCH BOMBS!!");
                    Console.SetCursorPosition(5, 10);
                    Console.Write("TRY AGAIN PRESS ENTER OR 'E' TO EXIT");
                    Console.WriteLine();
                    ConsoleKey key = new ConsoleKey();
                    Console.SetCursorPosition(25, 12);
                    key = Console.ReadKey().Key;
                    if (key == ConsoleKey.Enter)
                    {
                        Console.Clear();
                        continue;
                    }
                    else if (key == ConsoleKey.Escape)
                    {
                        Console.Clear();
                        break;
                    }
                }
                break;
            }
        }
    }
}