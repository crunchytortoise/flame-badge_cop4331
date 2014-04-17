/*
 * GameBoard.cs - Flame Badge
 *      -- Canvas and board for the pieces to move on.
 *      -- Should also be connected to terrain.
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
    public class GameBoard
    {
        public static Char[,] board { get; set; }
        public static Char[,] overlay { get; set; }
        private String game_map { get; set; }

        private const Int16 MAP_SIZE = 20;

        public GameBoard(String loaded_map = null)
        {
            // TODO: make MAP_SIZE a configurable value


            if (String.IsNullOrEmpty(loaded_map))
            {
                game_map = String.Format(@"..\..\{0}.map", Config.project_name);
                //game_map = Map.create(1);
            }
                
            else
                game_map = loaded_map;

            if(!this._parseMap(game_map, MAP_SIZE))
                Environment.Exit(1);

            if (String.IsNullOrEmpty(loaded_map))
            {
                //game_map = String.Format(@"..\..\{0}.map", Config.project_name);
                board = Map.create(1);
            }

            Console.SetWindowSize(Console.WindowWidth + board.GetLength(0), Console.WindowHeight + board.GetLength(1));
            
            // board should never change, so overlay is a copy that will have the characters as well
            // this way we know what terrain was at a certain spot before when a character leaves it
            overlay = (Char[,])board.Clone();
            redraw();
        }

        
         /// <summary> 
         /// Parses the map file provided (it must exist) and stores it to
         /// our board private variable. This method will quietly die
         /// if no such map file exists.</summary>
         /// <param name="map"> the location of the map file</param>
         /// <param name="map_size"> constant indicating the size of the grid</param>
         /// <returns> true on success, false if we run into a StreamReader exception</returns>
        private Boolean _parseMap(String map, Int16 map_size)
        {
            board = new Char[map_size, map_size];
            Logger.log(@"Initializing map...");
            
            try {
                using (StreamReader f = new StreamReader(map))
                {
                    for(int i = 0; i < map_size; i++) 
                    {
                        for(int j = 0; j < map_size; j++) 
                        {
                            Char curr = (Char)f.Read();
                            while (Char.IsWhiteSpace(curr))
                                curr = (Char)f.Read();
                            
                            board[i,j] = curr;
                        }
                    }
                }
            }
            catch (Exception e) {
                String msg = String.Format(@"The map file at {0} could not be read. Reason: " + e, game_map);
                Logger.log(msg, "error");
                return false;
            }

            Logger.log(@"Map initialized succesfully!");
            return true;
        }

        public static Tuple<Int16, Int16> getPlayerCastle()
        {
            Tuple<Int16, Int16> loc = null;
            for (Int16 i = 0; i < MAP_SIZE; i++)
            {
                for (Int16 j = 0; j < MAP_SIZE; j++)
                {
                    if (board[i, j] == '*')
                    {
                        loc = new Tuple<Int16, Int16>(i, j);
                        return loc;
                    }
                }
            }
            return loc;
        }
        public static Tuple<Int16, Int16> getCPUCastle()
        {
            Tuple<Int16, Int16> loc = null;
            for (Int16 i = 0; i < MAP_SIZE; i++)
            {
                for (Int16 j = 0; j < MAP_SIZE; j++)
                {
                    if (board[i, j] == '+')
                    {
                        loc = new Tuple<Int16, Int16>(i, j);
                        return loc;
                    }
                }
            }
            return loc;
        }

         /// <summary>
         /// Void method to redraw the overlay canvas so we can simulate
         /// movement. The underlying <c>board</c> variable is untouched.</summary>
         /// <returns> true on success</returns>
        public static Boolean redraw()
        {
            Logger.log(@"Redrawing map...");
            Console.Clear();

            for(int i = 0; i < overlay.GetLength(0); i++)
            {
                for (int j = 0; j < overlay.GetLength(1); j++)
                {
                    Console.Write(overlay[i, j] + "  ");
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            return true;
        }


        public char getTextureAtLocation(int x, int y)
        {
            return overlay[x, y];
        }

//        public Character getUnitAtLocation(int x, int y)
 //       {
  //      }

        /// <summary>
        /// Moves a unit on the game board and redraws to show updated
        /// positions.
        /// </summary>
        /// <param name="character">ID of the piece, so we know how to redraw.</param>
        /// <param name="newX">new x-coordinate position of the piece</param>
        /// <param name="newY">new y-coordinate position of the piece</param>
        /// <returns>true if the update happened, false otherwise</returns>
        public static Boolean update(Character character, Int16 newX, Int16 newY)
        {
            if (isValidMove(newX, newY))
            {
                Logger.log(@"Valid move detected, attempting to reassign position.", "debug");
                overlay[character.yPos, character.xPos] = board[character.yPos, character.xPos];
                overlay[newY, newX] = character.id;
                character.xPos = newX;
                character.yPos = newY;
                Logger.log(@"Reassignment successful, map should redraw.", "debug");

                redraw();
                return true;
            }
            else
            {
                Logger.log(String.Format(@"Invalid move detected. Tried to place at {0}, {1}", newX, newY), "warning");
                return false;
            }
        }

        /// <summary>
        /// Detects whether a proposed move is going to go out of bounds or not.
        /// </summary>
        /// <param name="x">x-coordinate number of proposed move</param>
        /// <param name="y">y-coordinate number of proposed move</param>
        /// <returns>true if the move is valid, false otherwise</returns>
        protected static Boolean isValidMove(Int16 x, Int16 y)
        {
            if(x >= 0 && x <= overlay.GetLength(0) - 1
                && y >= 0 && y <= overlay.GetLength(1) - 1)
                if(!isOccupied(x, y))
                    return true;
                else
                    return false;
            else return false;

        }

        /// <summary>
        /// Attacker deals damage to the defender
        /// </summary>
        /// <param name="attackerID"></param>
        /// <param name="defenderID"></param>
        /// <param name="player">true- player, false- cpu</param>
        public static void attack(int attackerID, int defenderID, bool player)
        {
            int id = 0;
            if(player)
                for (int i = 0; i < FlameBadge.player_units.Count; i++)
                {
                    if (FlameBadge.player_units[i].id == attackerID)
                    {
                        id = i;
                    }
                } 
            else
            {
                for (int i = 0; i < FlameBadge.cpu_units.Count; i++)
                {
                    if (FlameBadge.cpu_units[i].id == attackerID)
                    {
                        id = i;
                    }
                } 
            }
            if (player)
            {
                for (int i = 0; i < FlameBadge.cpu_units.Count; i++)
                {
                    if (FlameBadge.cpu_units[i].id == defenderID)
                    {
                        FlameBadge.cpu_units[i].damage(FlameBadge.player_units[id].dpsMod);
                        if (FlameBadge.cpu_units[i].health <= 0)
                        {
                            overlay[FlameBadge.cpu_units[i].yPos, FlameBadge.cpu_units[i].xPos] = board[FlameBadge.cpu_units[i].yPos, FlameBadge.cpu_units[i].xPos];
                            FlameBadge.cpu_units.RemoveAt(i);
                            FlameBadge.player_units[id].levelUp();
                        }
                    }
                }
            }
            else
            {
                for (int i = 0; i < FlameBadge.player_units.Count; i++)
                {
                    if (FlameBadge.player_units[i].id == defenderID)
                    {
                        FlameBadge.player_units[i].damage(FlameBadge.cpu_units[id].dpsMod);
                        if (FlameBadge.player_units[i].health <= 0)
                        {
                            overlay[FlameBadge.player_units[i].yPos, FlameBadge.player_units[i].xPos] = board[FlameBadge.player_units[i].yPos, FlameBadge.player_units[i].xPos];
                            FlameBadge.player_units.RemoveAt(i);
                            FlameBadge.cpu_units[id].levelUp();
                        }
                    }
                }
            }
        }

        public static Boolean isOccupied(Int32 x, Int32 y)
        {
            if (overlay[y, x].Equals('%') || overlay[y, x].Equals('=') || overlay[y, x].Equals('&') || overlay[y, x].Equals('_') || overlay[y, x].Equals('*') || overlay[y, x].Equals('+'))
                return false;
            else
            {
                Logger.log(String.Format(@"Ran into occupied space at {0}, {1}, retrying...", x, y));
                return true;
            }
        }

        /// <summary>
        /// Returns all possible moves from the given location
        /// </summary>
        /// <param name="x">Location x.</param>
        /// <param name="y">Location y.</param>
        /// <param name="moveSpeed">(Optional) Number of spaces the unit can move in one turn. (default is 1)</param>
        /// <returns>List of x,y positions that are reachable from the location.</returns>
        public static List<Tuple<int, int>> getPossibleMovesFromLoc(int x, int y, int moveSpeed = 1)
        {
            List<Tuple<int, int>> moves = new List<Tuple<int, int>>();
            if (GameBoard.isValidMove((short) (x - 1),(short) y))
            {
                moves.Add(new Tuple<int, int>(x - 1, y));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x - 1, y, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short) (x + 1),(short) y))
            {
                moves.Add(new Tuple<int, int>(x + 1, y));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x + 1, y, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short) x, (short) (y - 1)))
            {
                moves.Add(new Tuple<int, int>(x, y - 1));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x, y - 1, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short) x, (short) (y + 1)))
            {
                moves.Add(new Tuple<int, int>(x, y + 1));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x, y + 1, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short)(x - 1), (short)(y - 1)))
            {
                moves.Add(new Tuple<int, int>(x - 1, y - 1));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x - 1, y - 1, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short)(x + 1), (short)(y - 1)))
            {
                moves.Add(new Tuple<int, int>(x + 1, y - 1));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x + 1, y - 1, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short)(x - 1), (short)(y + 1)))
            {
                moves.Add(new Tuple<int, int>(x - 1, y + 1));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x - 1, y + 1, moveSpeed - 1));
            }
            if (GameBoard.isValidMove((short)(x + 1), (short)(y + 1)))
            {
                moves.Add(new Tuple<int, int>(x + 1, y + 1));
                if (moveSpeed > 1)
                    moves.AddRange(getPossibleMovesFromLoc(x + 1, y + 1, moveSpeed - 1));
            }
            return moves;
        }

        /// <summary>
        /// Returns units which can be attacked from the given position
        /// </summary>
        /// <param name="x">Location x.</param>
        /// <param name="y">Location y.</param>
        /// <param name="minAttackDistance">(Optional) Minimum distance of attack in gameboard tiles. (Default is 1)</param>
        /// <param name="maxAttackDistance">(Optional) Maximum distance of attack in gameboard tiles. (Default is 1)</param>
        /// <returns>List of units which can be attacked from the given position.</returns>
        public static List<Character> getAttackableUnits(int x, int y, List<Character> enemies, int maxAttackDistance = 1, int minAttackDistance = 1)
        {
            List<Character> victims = new List<Character>();
            foreach (Character c in enemies)
            {
                int dist = distance(c.xPos, c.yPos, x, y);
                if ((dist >= minAttackDistance) && (dist <= maxAttackDistance))
                    victims.Add(c);
            }
            return victims;
        }

        /// <summary>
        /// Returns the integer board tile distance between two points
        /// </summary>
        /// <param name="x1">x for location 1.</param>
        /// <param name="y1">y for location 1.</param>
        /// <param name="x2">x for location 2.</param>
        /// <param name="y3">y for location 2.</param>
        /// <returns>Board tile distance between the two points.</returns>
        public static int distance(int x1, int y1, int x2, int y2)
        {
            return (int)Math.Sqrt((x1 - x2) * (x1 - x2) + (y1 - y2) * (y1 - y2));
        }

        public static String saveGame(Char whose_turn)
        {

            System.Console.Clear();
            System.Console.Write(@"Save name: ");
            String save_name = System.Console.ReadLine();
            String file_name;

            if (save_name == "")
            {
                String timestamp = DateTime.Now.ToString(@"yyyyMMddHHmmffff");
                file_name = FlameBadge.save_dir + timestamp + @".fbsave";
            }
            else
            {
                file_name = FlameBadge.save_dir + save_name + @".fbsave";
                if (File.Exists(file_name))
                {
                    System.Console.WriteLine("Are you sure you want to overwrite save '{0}'? (Y/n)", save_name);
                    Boolean need_answer = true;
                    while (need_answer)
                    {
                        ConsoleKeyInfo answer;
                        answer = System.Console.ReadKey();

                        switch (answer.KeyChar)
                        {
                            case 'n':
                            case 'N':
                                need_answer = false;
                                GameBoard.saveGame(whose_turn);
                                break;
                            case 'y':
                            case 'Y':
                                need_answer = false;
                                File.Delete(file_name);
                                break;
                            default:
                                System.Console.WriteLine(@"Please input either 'y' or 'n': ");
                                break;
                        }
                    }
                }
            }

            try
            {
                using (StreamWriter writer = new StreamWriter(file_name))
                {
                    // write the map
                    for (int i = 0; i < MAP_SIZE; i++)
                    {
                        for (int j = 0; j < MAP_SIZE; j++)
                        {
                            writer.Write(board[i, j]);
                            writer.Write(@" ");
                        }
                        writer.WriteLine();
                    }
                    writer.WriteLine();
                    writer.WriteLine();

                    // write the living player units and their positions, and stats
                    writer.Write(@"Player {0}", FlameBadge.player_units.Count);
                    writer.WriteLine();
                    foreach (var unit in FlameBadge.player_units)
                    {
                        writer.Write(@"{0} {1} {2} {3} {4} {5}", unit.id, unit.xPos, unit.yPos, unit.health, unit.level, unit.dpsMod);
                        writer.WriteLine();
                    }

                    writer.WriteLine();
                    writer.WriteLine();                    
                    // write the living computer units and their positions, and stats
                    writer.Write(@"Computer {0}", FlameBadge.cpu_units.Count);
                    writer.WriteLine();
                    foreach (var unit in FlameBadge.cpu_units)
                    {
                        writer.Write(@"{0} {1} {2} {3} {4} {5}", unit.id, unit.xPos, unit.yPos, unit.health, unit.level, unit.dpsMod);
                        writer.WriteLine();
                    }

                    writer.WriteLine();
                    writer.WriteLine();
                    // write who of the player units was taking his turn
                    writer.Write(@"TURN {0}", whose_turn);
                }
                System.Console.WriteLine("Game Saved!");
                System.Threading.Thread.Sleep(1000);
                GameBoard.redraw();
                Sidebar.announce(String.Format(@"{0} taking turn...", whose_turn), true);
            }
            catch (Exception e)
            {
                String msg = String.Format(@"The game could not be saved. Reason: " + e);
                Logger.log(msg, "error");
                return null;
            }

            Logger.log("Game saved.");
            return file_name;
        }
    }
}
