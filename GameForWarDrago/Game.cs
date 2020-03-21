using System;
using System.Threading;
////////////////////////////// Убрать мигание (возможное решение через функцию курсора и величину консоли) //////////////////////////////
namespace GameForWarDrago
{
    class Program
    {
        private static void Main()
        {
            Menu(10, 30, 15, 0);
        }
        private static void Menu(int level, int maxX, int maxY, int nowBoxes)
        { //menu 
            while (true)
            {
                // --
                Console.Clear();
                Console.WriteLine(@"
                1. Start game 
                2. Setings                
                3. Exit 
                ");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        //start
                        Game(100, nowBoxes, level, maxX, maxY);
                        return;
                    case ConsoleKey.D2:
                        //setings
                        GameSetings(level, maxX, maxY, nowBoxes);
                        return;
                    case ConsoleKey.D3:
                        //exit 
                        return;
                    default:
                        Console.WriteLine("Enter correct number. Press Enter to continue");
                        Console.ReadLine();
                        break;
                }
            }
        }
        private static void Game(int hp, int nowBoxes, int level, int maxX, int maxY)
        {
            int needToNextLvLBoxes = level * 10;
            string[,] backgroundOfScreen = new string[maxY, maxX];
            Random rnd = new Random();
            int oMoveX = rnd.Next(1, maxX - 2);
            int moveY = 1;
            int pMoveX = (maxX / 2) - 5;

            while (true)
            {
                do
                {
                    while (!Console.KeyAvailable)
                    {
                        Background(maxX, maxY, backgroundOfScreen);
                        PlayerMove(backgroundOfScreen, maxY, pMoveX);
                        ObjectsMove(backgroundOfScreen, moveY, oMoveX);
                        GameScreen(maxX, maxY, backgroundOfScreen);
                        StatsOfPlayer(hp, level, nowBoxes, needToNextLvLBoxes);

                        if (moveY > maxY - 7)
                        {
                            if (oMoveX == pMoveX || oMoveX == pMoveX + 1 || oMoveX == pMoveX + 2)
                            {
                                backgroundOfScreen[moveY - 1, oMoveX + 1] = ". ";
                                oMoveX = rnd.Next(1, maxX - 2);
                                moveY = 1;
                                nowBoxes++;
                                if (nowBoxes > needToNextLvLBoxes)
                                {
                                    level++;
                                    needToNextLvLBoxes = level * 10;
                                    nowBoxes = 0;
                                    if (level > 10)
                                    {
                                        break;
                                    }
                                }
                            }
                            else if (moveY == maxY - 2)
                            {
                                backgroundOfScreen[moveY, oMoveX + 1] = ". ";
                                backgroundOfScreen[moveY - 1, oMoveX + 1] = ". ";
                                oMoveX = rnd.Next(1, maxX - 2);
                                moveY = 1;
                                hp -= level;
                                if (hp < 1)
                                {
                                    break;
                                }
                            }
                        }
                        moveY++;
                        Thread.Sleep(2310 / (level + 1));
                    }
                    if (level > 10 || hp < 1)
                    {
                        EndGame(hp, maxX, maxY);
                        return;
                    }
                    ConsoleKeyInfo playerPosX = Console.ReadKey(true);
                    if (playerPosX.Key == ConsoleKey.Enter || playerPosX.Key == ConsoleKey.Escape)
                    {
                        Menu(level, maxX, maxY, nowBoxes);
                        return;
                    }
                    pMoveX = PlayerPositionX(pMoveX, maxX, playerPosX);
                } while (true);
            }
        }
        private static void GameSetings(int level, int maxX, int maxY, int nowBoxes)
        {
            while (true)
            {
                Console.Clear();
                Console.WriteLine(@$"
                        Choose option:
                        1. level = {level},
                        2. Width = {maxX},
                        3. Height = {maxY},
                        4. Return to main menu
                        ");
                switch (Console.ReadKey(true).Key)
                {
                    case ConsoleKey.D1:
                        Console.WriteLine("Enter new level:");
                        int.TryParse(Console.ReadLine(), out level);
                        break;
                    case ConsoleKey.D2:
                        Console.WriteLine("Enter new Width:");
                        int.TryParse(Console.ReadLine(), out maxX);
                        break;
                    case ConsoleKey.D3:
                        Console.WriteLine("Enter new Height:");
                        int.TryParse(Console.ReadLine(), out maxY);
                        break;
                    case ConsoleKey.D4:
                        Menu(level, maxX, maxY, nowBoxes);
                        return;
                    default:
                        Console.WriteLine("Enter correct number. Press Enter to continue");
                        Console.ReadLine();
                        break;
                }
                if (CheckSetings(maxX, maxY, level))
                {
                    Console.WriteLine("Press any key to continue");
                    Console.ReadLine();
                }
            }
        }
        private static bool CheckSetings(int maxX, int maxY, int level)
        {
            Console.WriteLine();
            if (maxX < 5)
            {
                Console.WriteLine("Error 1: Enter X > 5");
                return true;
            }
            if (maxX > 40)
            {
                Console.WriteLine("Error 2: Enter X <= 40");
                return true;
            }
            if (maxY < 7)
            {
                Console.WriteLine("Error 3: Enter Y >= 7");
                return true;
            }
            if (maxY > 20)
            {
                Console.WriteLine("Error 4: Enter Y <= 20");
                return true;
            }
            if (level > 10)
            {
                Console.WriteLine("Error 5: Enter level <= 10");
                return true;
            }
            return false;
        }
        private static void Background(int maxX, int maxY, string[,] backgroundOfScreen)
        {
            for (int i = 0; i < maxY; i++)
            {
                for (int j = 0; j < maxX; j++)
                {
                    backgroundOfScreen[i, j] = ". ";
                    backgroundOfScreen[maxY - 1, j] = "* ";
                }
                backgroundOfScreen[i, 0] = "* ";
                backgroundOfScreen[i, maxX - 1] = "* ";
            }
        }
        private static void GameScreen(int maxX, int maxY, string[,] backgroundOfScreen)
        {//x, y
            Console.Clear();
            for (int i = 0; i < maxY; i++)
            {
                Console.Write("\n                                  ");
                for (int j = 0; j < maxX; j++)
                {
                    Console.Write(backgroundOfScreen[i, j]);
                }
            }
        }
        private static void StatsOfPlayer(int hp, int level, int nowBoxes, int needToNextLvLBoxes)
        {
            Console.WriteLine($@"

                                   O      HP - {hp}%
                                  /i\     
                                 / i \    Taken Boxes - {nowBoxes}/{needToNextLvLBoxes}
                                  / \ 
                                 /   \    Level - {level}"
            );
        }
        private static void PlayerMove(string[,] backgroundOfScreen, int maxY, int pMoveX)
        {
            backgroundOfScreen[maxY - 6, pMoveX + 1] = "  "; backgroundOfScreen[maxY - 6, pMoveX + 2] = "O "; backgroundOfScreen[maxY - 6, pMoveX + 3] = "  ";
            backgroundOfScreen[maxY - 5, pMoveX + 1] = " /"; backgroundOfScreen[maxY - 5, pMoveX + 2] = @"i\"; backgroundOfScreen[maxY - 5, pMoveX + 3] = "  ";
            backgroundOfScreen[maxY - 4, pMoveX + 1] = "/ "; backgroundOfScreen[maxY - 4, pMoveX + 2] = @"i "; backgroundOfScreen[maxY - 4, pMoveX + 3] = @"\ ";
            backgroundOfScreen[maxY - 3, pMoveX + 1] = " /"; backgroundOfScreen[maxY - 3, pMoveX + 2] = @" \"; backgroundOfScreen[maxY - 3, pMoveX + 3] = "  ";
            backgroundOfScreen[maxY - 2, pMoveX + 1] = "/ "; backgroundOfScreen[maxY - 2, pMoveX + 2] = "  "; backgroundOfScreen[maxY - 2, pMoveX + 3] = @"\ ";
        }
        private static void ObjectsMove(string[,] backgroundOfScreen, int moveY, int oMoveX)
        {
            backgroundOfScreen[moveY, oMoveX + 1] = "@@";
            backgroundOfScreen[moveY - 1, oMoveX + 1] = "@@";
        }
        private static int PlayerPositionX(int pMoveX, int maxX, ConsoleKeyInfo playerPositionX)
        {
            switch (playerPositionX.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (pMoveX > 0)
                        pMoveX--;
                    break;
                case ConsoleKey.RightArrow:
                    if (pMoveX < maxX - 5)
                        pMoveX++;
                    break;
            }
            return pMoveX;
        }
        private static void EndGame(int hp, int maxX, int maxY)
        {
            Console.Clear();
            if (hp < 1)
            {
                Console.WriteLine("you lose :(");
            }
            else
            {
                Console.WriteLine(@"

   ...@.........@....@@@....@.......@........@.................@.................@....@@@....@@.........@..@..
   ....@.......@....@...@...@.......@.........@...............@.@...............@....@...@...@.@........@..@..
   .... @.....@....@.....@..@.......@..........@.............@...@.............@....@.....@..@..@.......@..@..
   ......@...@.....@.....@..@.......@...........@...........@.....@...........@.....@.....@..@...@......@..@..
   .......@.@......@.....@..@.......@............@.........@.......@.........@......@.....@..@....@.....@..@..
   ........@.......@.....@..@.......@.............@.......@.........@.......@.......@.....@..@.....@....@..@..
   ........@.......@.....@..@.......@..............@.....@...........@.....@........@.....@..@......@...@..@..
   ........@.......@.....@...@.....@................@...@.............@...@.........@.....@..@.......@..@..@..
   ........@........@...@.....@...@..................@.@...............@.@...........@...@...@........@.@.....
   ........@.........@@@.......@@@....................@.................@.............@@@....@.........@@..@..
                ");
            }
            Console.WriteLine("\n\n\nPress Enter to return to main menu ...");
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Menu(0, maxX, maxY, 0);
                    return;
                }
            }
        }
    }
}