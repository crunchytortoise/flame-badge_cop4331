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
    class GameBoard
    {
        private static Char[,] board { get; set; }
        public static Char[,] overlay { get; set; }
        private String game_map { get; set; }

        public GameBoard()
        {
            // TODO: make MAP_SIZE a configurable value
            const Int16 MAP_SIZE = 20;
            game_map = String.Format(@"{0}\{1}.map", Config.project_path, Config.project_name);

            if(!this._parseMap(game_map, MAP_SIZE))
                Environment.Exit(1);

            Console.SetWindowSize(Console.WindowWidth + board.GetLength(0), Console.WindowHeight + board.GetLength(1));
            
            // board should never change, so overlay is a copy that will have the characters as well
            // this way we know what terrain was at a certain spot before when a character leaves it
            overlay = board;
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
            Logger.log("Initializing map...");
            
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

            Logger.log("Map initialized succesfully!");
            return true;
        }

         /// <summary>
         /// Void method to redraw the overlay canvas so we can simulate
         /// movement. The underlying <c>board</c> variable is untouched.</summary>
         /// <returns> true on success</returns>
        public static Boolean redraw()
        {
            Logger.log("Redrawing map...");

            for(int i = 0; i < overlay.GetLength(0); i++)
            {
                for (int j = 0; j < overlay.GetLength(1); j++)
                {
                    Console.Write(overlay[i, j] + "  ");
                }
                Console.Write(Environment.NewLine + Environment.NewLine);
            }
            Console.Read();
            return true;
        }

        public static void update(Character character, Int16 newX, Int16 newY)
        {
            overlay[character.xPos, character.yPos] = board[character.xPos, character.yPos];
            overlay[newX, newY] = character.id;
            redraw();
        }
    }
}
