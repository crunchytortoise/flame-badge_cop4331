using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    public class EnemyCharacter : Character
    {
        public EnemyCharacter(Char id, Int16 x, Int16 y, int health, int level, int dpsMod) : base(id, x, y, health, level, dpsMod) {}

        public static Tuple<Int16, Int16> getStartingPosition()
        {
            Random rand = new Random();

            while (true)
            {
                Int32 y = rand.Next(0, 3);
                Int32 x = rand.Next(0, GameBoard.overlay.GetLength(0));

                if (GameBoard.isOccupied(x, y))
                    continue;
                else
                    return Tuple.Create((short)x, (short)y);
            }
        }

        public static void placePieces(Boolean is_loaded, String loaded_file)
        {
            if (!is_loaded)
            {
                //Logger.log(@"Placing enemy pieces in random positions.");
                Tuple<Int16, Int16> castleCoords = GameBoard.getCPUCastle();
                Int16 j = castleCoords.Item1;
                Int16 i = castleCoords.Item2;

                //Logger.log(@"Placing player pieces in random positions.");
                if (i - 1 >= 0)
                {
                    if (GameBoard.overlay[i - 1, j] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i - 1), (short)j);
                        EnemyCharacter character = new EnemyCharacter('A', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i + 1 < GameBoard.overlay.GetLength(0))
                {
                    if (GameBoard.overlay[i + 1, j] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i + 1), (short)j);
                        EnemyCharacter character = new EnemyCharacter('B', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (j - 1 >= 0)
                {
                    if (GameBoard.overlay[i, j - 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i), (short)(j - 1));
                        EnemyCharacter character = new EnemyCharacter('C', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (j + 1 < GameBoard.overlay.GetLength(1))
                {
                    if (GameBoard.overlay[i, j + 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i), (short)(j + 1));
                        EnemyCharacter character = new EnemyCharacter('D', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }

                //diagonals
                if (i - 1 >= 0 && j + 1 < GameBoard.overlay.GetLength(1))
                {
                    if (GameBoard.overlay[i - 1, j + 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i - 1), (short)(j + 1));
                        EnemyCharacter character = new EnemyCharacter('E', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i + 1 < GameBoard.overlay.GetLength(0) && j - 1 >= 0)
                {
                    if (GameBoard.overlay[i + 1, j - 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i + 1), (short)(j - 1));
                        EnemyCharacter character = new EnemyCharacter('F', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (GameBoard.overlay[i - 1, j - 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i - 1), (short)(j - 1));
                        EnemyCharacter character = new EnemyCharacter('G', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i + 1 < GameBoard.overlay.GetLength(0) && j + 1 < GameBoard.overlay.GetLength(1))
                {
                    if (GameBoard.overlay[i + 1, j + 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i + 1), (short)(j + 1));
                        EnemyCharacter character = new EnemyCharacter('H', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.cpu_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }

                }
                
            }
            else
            {
                Logger.log(@"Placing computer pieces according to loaded save file.");
                try
                {
                    using (StreamReader sr = new StreamReader(loaded_file))
                    {
                        while (sr.Peek() > -1)
                        {
                            String line = sr.ReadLine();
                            if (line.StartsWith("Computer"))
                            {
                                for (int i = 0; i < Convert.ToInt16(line.Split()[1]); i++)
                                {
                                    String[] unit_info = sr.ReadLine().Split();
                                    EnemyCharacter character = new EnemyCharacter(Convert.ToChar(unit_info[0]), Convert.ToInt16(unit_info[1]), Convert.ToInt16(unit_info[2]), Convert.ToInt32(unit_info[3]), Convert.ToInt32(unit_info[4]), Convert.ToInt32(unit_info[5]));
                                    FlameBadge.cpu_units.Add(character);
                                    GameBoard.update(character, Convert.ToInt16(unit_info[1]), Convert.ToInt16(unit_info[2]));
                                    Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), Convert.ToInt16(unit_info[1]), Convert.ToInt16(unit_info[2]), Convert.ToInt32(unit_info[3]), Convert.ToInt32(unit_info[4]), Convert.ToInt32(unit_info[5])));
                                }
                            }
                        }



                    }
                }
                catch (Exception)
                {
                    Logger.log("Could not load computer pieces from save file. Quitting...", "error");
                    Environment.Exit(1);
                }
            }
        }

        private static Char _toAlpha(int c)
        {
            return (Char)(65 + c);
        }

        public override void takeTurn()
        {
            /*Sidebar.announce(String.Format(@"CPU taking turn...", this.id.ToString()), false);

            Random rand = new Random();

            // MAGIC NUMBER ALERT!!!
            // 8 - Move Up          7 - MoveUpLeft
            // 4 - Move Left        9 - MoveUpRight
            // 6 - Move Right       1 - MoveDownLeft
            // 2 - Move Left        3 - MoveDownRight
            List<Char> moves = new List<Char> {'1', '2', '3', '4', '6', '7', '8', '9'};

            while (true)
            {
                // The computer is an idiot
                Char move = moves[rand.Next(moves.Count)];

                if (makeMove(move))
                {
                    Sidebar.announce(String.Format(@"CPU's turn, please wait...", this.id.ToString()), false);
                    System.Threading.Thread.Sleep(1500);
                   
                    break;
                }
            }*/

            // EXAMPLE AI CODE (AI object should be declared at a higher level though, not for each individual unit).
            // The 3rd and 4th parameters are base positions. If given with x and y less than 0, the AI assumes no bases exist.
            // The 5th parameter is the AI level, which controls how far into the future the search goes. The level caps at 5 (because any higher takes forever).
            Tuple<Int16, Int16> castleLoc = GameBoard.getCPUCastle();
            Tuple<int, int> convertedLoc = Tuple.Create((int)castleLoc.Item2, (int)castleLoc.Item1);
            Tuple<Int16, Int16> castlePlayerLoc = GameBoard.getPlayerCastle();
            Tuple<int, int> convertedPlayerLoc = Tuple.Create((int)castlePlayerLoc.Item2, (int)castlePlayerLoc.Item1);
            
            AI smartCPU = new AI(FlameBadge.player_units, FlameBadge.cpu_units, convertedPlayerLoc, convertedLoc, 2);

            Tuple<int, int> nextMoveLoc = smartCPU.getNextMove(this);
            if (nextMoveLoc.Item1 < this.xPos && nextMoveLoc.Item2 > this.yPos) { makeMove('1'); }
            else if (nextMoveLoc.Item1 == this.xPos && nextMoveLoc.Item2 > this.yPos) { makeMove('2'); }
            else if (nextMoveLoc.Item1 > this.xPos && nextMoveLoc.Item2 > this.yPos) { makeMove('3'); }
            else if (nextMoveLoc.Item1 < this.xPos && nextMoveLoc.Item2 == this.yPos) { makeMove('4'); }
            else if (nextMoveLoc.Item1 > this.xPos && nextMoveLoc.Item2 == this.yPos) { makeMove('6'); }
            else if (nextMoveLoc.Item1 < this.xPos && nextMoveLoc.Item2 < this.yPos) { makeMove('7'); }
            else if (nextMoveLoc.Item1 == this.xPos && nextMoveLoc.Item2 < this.yPos) { makeMove('8'); }
            else if (nextMoveLoc.Item1 > this.xPos && nextMoveLoc.Item2 < this.yPos) { makeMove('9'); }
            else { Logger.log(String.Format(@"Error getting move for cpu unit {0}", this.id), "Error"); }

        }
    }
}
