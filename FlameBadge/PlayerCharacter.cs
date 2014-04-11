/*
 * PlayerCharacter.cs - Flame Badge
 *      -- Character class for player controlled pieces.
 *      
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    class PlayerCharacter : Character
    {
        public PlayerCharacter(Char id, Int16 x, Int16 y, int health, int level, int dpsMod) : base(id, x, y, health, level, dpsMod) {}

        public static Tuple<Int16, Int16> getStartingPosition()
        {
            Random rand = new Random();

            while (true)
            {
                Int32 y = rand.Next(GameBoard.overlay.GetLength(1) - 3, GameBoard.overlay.GetLength(1));
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
                Tuple<Int16,Int16> castleCoords = GameBoard.getCPUCastle();
                Int16 j = castleCoords.Item1;
                Int16 i = castleCoords.Item2;

                //Logger.log(@"Placing player pieces in random positions.");
                if (i - 1 >= 0)
                {
                    if (GameBoard.overlay[i - 1, j] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i-1),(short)j);
                        PlayerCharacter character = new PlayerCharacter('1', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i + 1 < GameBoard.overlay.GetLength(0))
                {
                    if (GameBoard.overlay[i + 1, j] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i+1),(short)j);
                        PlayerCharacter character = new PlayerCharacter('2', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (j - 1 >= 0)
                {
                    if (GameBoard.overlay[i, j - 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i),(short)(j-1));
                        PlayerCharacter character = new PlayerCharacter('3', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (j + 1 < GameBoard.overlay.GetLength(1))
                {
                    if (GameBoard.overlay[i, j + 1] != '@') 
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i),(short)(j+1));
                        PlayerCharacter character = new PlayerCharacter('4', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }

                //diagonals
                if (i - 1 >= 0 && j + 1 < GameBoard.overlay.GetLength(1))
                {
                    if (GameBoard.overlay[i - 1, j + 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i-1),(short)(j+1));
                        PlayerCharacter character = new PlayerCharacter('5', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i + 1 < GameBoard.overlay.GetLength(0) && j - 1 >= 0)
                {
                    if (GameBoard.overlay[i + 1, j - 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i+1),(short)(j-1));
                        PlayerCharacter character = new PlayerCharacter('6', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i - 1 >= 0 && j - 1 >= 0)
                {
                    if (GameBoard.overlay[i - 1, j - 1] != '@')
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i-1),(short)(j-1));
                        PlayerCharacter character = new PlayerCharacter('7', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }
                }
                if (i + 1 < GameBoard.overlay.GetLength(0) && j + 1 < GameBoard.overlay.GetLength(1))
                {
                    if (GameBoard.overlay[i + 1, j + 1] != '@') 
                    {
                        Tuple<Int16, Int16> coords = Tuple.Create((short)(i + 1), (short)(j+1));
                        PlayerCharacter character = new PlayerCharacter('8', coords.Item1, coords.Item2, 10, 1, 1);
                        FlameBadge.player_units.Add(character);
                        GameBoard.update(character, coords.Item1, coords.Item2);
                        Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), coords.Item1, coords.Item2, 10, 1, 1));
                    }

                }
            }
            else
            {
                Logger.log(@"Placing player pieces according to loaded save file.");
                try
                {
                    using (StreamReader sr = new StreamReader(loaded_file))
                    {
                        while (sr.Peek() > -1)
                        {
                            String line = sr.ReadLine();
                            if (line.StartsWith("Player"))
                            {
                                for (int i = 0; i < Convert.ToInt16(line.Split()[1]); i++)
                                {
                                    String[] unit_info = sr.ReadLine().Split();
                                    PlayerCharacter character = new PlayerCharacter(Convert.ToChar(unit_info[0]), Convert.ToInt16(unit_info[1]), Convert.ToInt16(unit_info[2]), Convert.ToInt32(unit_info[3]), Convert.ToInt32(unit_info[4]), Convert.ToInt32(unit_info[5]));
                                    FlameBadge.player_units.Add(character);
                                    GameBoard.update(character, Convert.ToInt16(unit_info[1]), Convert.ToInt16(unit_info[2]));
                                    Logger.log(String.Format(@"Placed {0} at {1}, {2} with health {3}, level {4}, and dpsMod {5}", character.id.ToString(), Convert.ToInt16(unit_info[1]), Convert.ToInt16(unit_info[2]), Convert.ToInt32(unit_info[3]), Convert.ToInt32(unit_info[4]), Convert.ToInt32(unit_info[5])));
                                }
                            }
                        }



                    }
                }
                catch (Exception)
                {
                    Logger.log("Could not load player pieces from save file. Quitting...", "error");
                    Environment.Exit(1);
                }

            }
        }

        public override void takeTurn()
        {
            Sidebar.announce(String.Format(@"{0} taking turn...", this.id.ToString()), true);

            ConsoleKeyInfo cmd;
            while (true)
            {
                cmd = Console.ReadKey();

                if (cmd.KeyChar == 's' || cmd.KeyChar == 'S')
                {
                    GameBoard.saveGame(this.id);
                    continue;
                }

                if (makeMove(cmd.KeyChar))
                    break;
                else
                    Sidebar.announce(String.Format(@"Invalid move, try again {0}.", this.id.ToString()), true);
            }
        }
    }
}
