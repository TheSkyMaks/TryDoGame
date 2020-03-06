/*
 * 1. Добавить меню
 * 2. Убрать мигание (возможное решение через функцию курсора и величину консоли)
 */
using System;
using System.Threading;

namespace GameForWarDrago
{
    class Program
    {
        static void Main()
        {
            while (true)
            {
                //menu 
                // --
                //start
                //setings
                int HP = 100,
                    NowBoxes = 0,
                    level = 10,
                    NeedToNextLvLBoxes = level * 10,
                    Max_X = 30,
                    Max_Y = 15;
                if (IF(Max_X, Max_Y, level))
                    continue;
                //exit

                //game
                string[,] BackgroundOfScreen = new string[Max_Y, Max_X];
                Random rnd = new Random();
                int oMove_X = rnd.Next(1, Max_X - 2);
                int Move_Y = 1;
                int pMove_X = Max_X / 2 - 5;
                int VTime = 2310;
                ConsoleKeyInfo PlayerPosX;

                while (true)
                {
                    do
                    {
                        while (!Console.KeyAvailable)
                        {
                            Background(Max_X, Max_Y, BackgroundOfScreen);
                            PlayerMove(BackgroundOfScreen, Max_Y, pMove_X);
                            ObjectsMove(BackgroundOfScreen, Move_Y, oMove_X);
                            GameScreen(Max_X, Max_Y, BackgroundOfScreen);
                            StatsOfPlayer(HP, level, NowBoxes, NeedToNextLvLBoxes);

                            if (Move_Y > Max_Y - 7)
                            {
                                if (oMove_X == pMove_X || oMove_X == pMove_X + 1 || oMove_X == pMove_X + 2)
                                {
                                    BackgroundOfScreen[Move_Y - 1, oMove_X + 1] = ". ";
                                    oMove_X = rnd.Next(1, Max_X - 2);
                                    Move_Y = 1;
                                    NowBoxes++;
                                    if (NowBoxes > NeedToNextLvLBoxes)
                                    {
                                        level++;
                                        NeedToNextLvLBoxes = level * 10;
                                        NowBoxes = 0;
                                        if (level > 10)
                                        {
                                            break;
                                        }
                                    }
                                }
                                else if (Move_Y == Max_Y - 2)
                                {
                                    BackgroundOfScreen[Move_Y, oMove_X + 1] = ". ";
                                    BackgroundOfScreen[Move_Y - 1, oMove_X + 1] = ". ";
                                    oMove_X = rnd.Next(1, Max_X - 2);
                                    Move_Y = 1;
                                    HP -= level;
                                    if (HP < 1)
                                    {
                                        break;
                                    }
                                }
                            }
                            Move_Y++;
                            Thread.Sleep(VTime / (level + 1));
                        }
                        if (HP < 1)
                        {
                            break;
                        }
                        PlayerPosX = Console.ReadKey(true);
                        pMove_X = PlayerPositionX(pMove_X, Max_X, PlayerPosX);
                    } while (PlayerPosX.Key != ConsoleKey.Enter && PlayerPosX.Key != ConsoleKey.Escape);
                    if (HP < 1)
                    {
                        break;
                    }
                }
                EndGame(HP);
            }
        }
        static bool IF(int Max_X, int Max_Y, int level)
        {
            if (Max_X < 5)
            {
                Console.WriteLine("Enter X > 5");
                return true;
            }
            if (Max_X > 40)
            {
                Console.WriteLine("Enter X <= 40");
                return true;
            }
            if (Max_Y < 7)
            {
                Console.WriteLine("Enter Y >= 7");
                return true;
            }
            if (Max_Y > 20)
            {
                Console.WriteLine("Enter Y <= 20");
                return true;
            }
            if (level > 10)
            {
                Console.WriteLine("Enter level <= 10");
                return true;
            }
            return false;
        }

        static string[,] Background(int Max_X, int Max_Y, string[,] BackgroundOfScreen)
        {
            for (int i = 0; i < Max_Y; i++)
            {
                for (int j = 0; j < Max_X; j++)
                {
                    BackgroundOfScreen[i, j] = ". ";
                }
            }
            for (int i = 0; i < Max_Y; i++)
            {
                BackgroundOfScreen[i, 0] = "* ";
                BackgroundOfScreen[i, Max_X - 1] = "* ";
            }
            for (int i = 0; i < Max_X; i++)
            {
                BackgroundOfScreen[Max_Y - 1, i] = "* ";
            }
            return BackgroundOfScreen;
        }
        static void GameScreen(int Max_X, int Max_Y, string[,] BackgroundOfScreen)
        {//x, y
            Console.Clear();
            for (int i = 0; i < Max_Y; i++)
            {
                Console.Write("\n                                  ");
                for (int j = 0; j < Max_X; j++)
                {
                    Console.Write(BackgroundOfScreen[i, j]);
                }
            }
        }
        static void StatsOfPlayer(int HP, int level, int NowBoxes, int NeedToNextLvLBoxes)
        {
            Console.WriteLine($@"

                                 . O .    HP - {HP}%
                                 ./i\.    
                                 /.i.\    Taken Boxes - {NowBoxes}/{NeedToNextLvLBoxes}
                                 ./.\.
                                 / . \    Level - {level}"
            );
        }
        static void PlayerMove(string[,] BackgroundOfScreen, int Max_Y, int pMove_X)
        {
            BackgroundOfScreen[Max_Y - 6, pMove_X + 1] = "  "; BackgroundOfScreen[Max_Y - 6, pMove_X + 2] = "O "; BackgroundOfScreen[Max_Y - 6, pMove_X + 3] = "  ";
            BackgroundOfScreen[Max_Y - 5, pMove_X + 1] = " /"; BackgroundOfScreen[Max_Y - 5, pMove_X + 2] = @"i\"; BackgroundOfScreen[Max_Y - 5, pMove_X + 3] = "  ";
            BackgroundOfScreen[Max_Y - 4, pMove_X + 1] = "/ "; BackgroundOfScreen[Max_Y - 4, pMove_X + 2] = @"i "; BackgroundOfScreen[Max_Y - 4, pMove_X + 3] = @"\ ";
            BackgroundOfScreen[Max_Y - 3, pMove_X + 1] = " /"; BackgroundOfScreen[Max_Y - 3, pMove_X + 2] = @" \"; BackgroundOfScreen[Max_Y - 3, pMove_X + 3] = "  ";
            BackgroundOfScreen[Max_Y - 2, pMove_X + 1] = "/ "; BackgroundOfScreen[Max_Y - 2, pMove_X + 2] = "  "; BackgroundOfScreen[Max_Y - 2, pMove_X + 3] = @"\ ";
        }
        static void ObjectsMove(string[,] BackgroundOfScreen, int Move_Y, int oMove_X)
        {
            BackgroundOfScreen[Move_Y, oMove_X + 1] = "@@"; BackgroundOfScreen[Move_Y - 1, oMove_X + 1] = "@@";
            //if (Move_Y > 1)
            //{
            //    BackgroundOfScreen[Move_Y - 2, oMove_X + 1] = ". ";
            //    if (Move_Y > 2)
            //    {
            //        BackgroundOfScreen[Move_Y - 3, oMove_X + 1] = ". ";
            //    }
            //}
        }
        static int PlayerPositionX(int pMove_X, int Max_X, ConsoleKeyInfo PlayerPositionX)
        {
            switch (PlayerPositionX.Key)
            {
                case ConsoleKey.LeftArrow:
                    if (pMove_X > 0)
                        pMove_X--;
                    break;
                case ConsoleKey.RightArrow:
                    if (pMove_X < Max_X - 5)
                        pMove_X++;
                    break;
                default:
                    break;
            }
            return pMove_X;
        }
        static void EndGame(int HP)
        {
            Console.Clear();
            if (HP < 1)
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
            Console.WriteLine("\n\n\n\nPress E to return to main menu ...");
            ConsoleKeyInfo EndKey;
            while (true)
            {
                EndKey = Console.ReadKey(true);
                if (EndKey.Key == ConsoleKey.E)
                {
                    break;
                }
            }
        }
    }
}
