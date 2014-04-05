﻿/*
 * FlameBadge.cs - Flame Badge
 *      -- Game initialization and mainloop class.
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
    class FlameBadge
    {
        public static Boolean hasEnded = false;
        public static List<PlayerCharacter> player_units = new List<PlayerCharacter>();
        public static List<EnemyCharacter> cpu_units = new List<EnemyCharacter>();
        public static String save_dir = Config.project_path + @"\\saves\\";
        
        public FlameBadge()
        {
            // Set up saves directory
            
            if (!Directory.Exists(save_dir))
                Directory.CreateDirectory(save_dir);

            // Check if any save files exist
            // If they don't we won't bother offering a load game option
            DirectoryInfo dir = new DirectoryInfo(save_dir);
            Boolean is_loaded = false;
            if (dir.GetFiles().Length != 0)
                is_loaded = _offerContinue();

            String loaded_file = "";
            if (is_loaded)
                loaded_file = _getSavedGame(dir);

            if (loaded_file == "")
                is_loaded = false;
            else
                loaded_file = save_dir + loaded_file;

            // Draw the game board.
            GameBoard game_board = new GameBoard(is_loaded ? loaded_file : null);

            // Put the pieces on the board.
            placePlayerPieces();
            placeEnemyPieces();

            // mainloop
            while (true)
            {
                for (int i = 0; i < player_units.Count; i++)
                {
                    player_units[i].takeTurn();
                    if (FlameBadge.hasEnded)
                        _endGame();
                }

                for (int i = 0; i < cpu_units.Count; i++)
                {
                    cpu_units[i].takeTurn();
                    if (FlameBadge.hasEnded)
                        _endGame();
                }
            }

        }

        public void placePlayerPieces()
        {
            Logger.log(@"Placing player pieces in random positions.");

            for (int i = 0; i < Config.NUM_PLAYER; i++)
            {
                Tuple<Int16, Int16> coords = PlayerCharacter.getStartingPosition();
                PlayerCharacter character = new PlayerCharacter(Convert.ToChar(i.ToString()), coords.Item1, coords.Item2);
                player_units.Add(character);
                GameBoard.update(character, coords.Item1, coords.Item2);
                Logger.log(String.Format(@"Placed {0} at {1}, {2}", character.id.ToString(), coords.Item1, coords.Item2));
            }
        }

        public void placeEnemyPieces()
        {
            Logger.log(@"Placing enemy pieces in random positions.");
            for (int i = 0; i < Config.NUM_CPU; i++)
            {
                Tuple<Int16, Int16> coords = EnemyCharacter.getStartingPosition();
                EnemyCharacter character = new EnemyCharacter(_toAlpha(i), coords.Item1, coords.Item2);
                cpu_units.Add(character);
                GameBoard.update(character, coords.Item1, coords.Item2);
            }
        }

        private Char _toAlpha(int c) 
        {
            return (Char)(65 + c);
        }

        private void _endGame()
        {
            String msg;

            if (player_units.Count == 0)
                msg = @"The computer wins! Press any key to quit...";
            else
                msg = @"You win! Press any key to quit...";

            Console.Clear();
            Console.WriteLine(msg);
            Console.ReadKey();
            System.Environment.Exit(0);
        }

        private Boolean _offerContinue()
        {
            System.Console.WriteLine(@"Previous saves detected...");
            System.Console.WriteLine(@"1. Start New Game");
            System.Console.WriteLine(@"2. Load Previous Save");
            System.Console.WriteLine();
            System.Console.Write(@"Please choose an option and press Enter (1 or 2): ");
            ConsoleKeyInfo cmd;
            while (true)
            {
                cmd = Console.ReadKey();

                switch (cmd.KeyChar)
                {
                    case '1':
                        return false;
                    case '2':
                        return true;
                    default:
                        System.Console.WriteLine(@"Invalid command, please enter 1 or 2: ");
                        break;
                }
            }
        }

        private String _getSavedGame(DirectoryInfo dir)
        {
            System.Console.WriteLine();
            System.Console.WriteLine(@"Found the following saved games:");
            for(int i=0; i < dir.GetFiles().Length; i++)
            {
                System.Console.WriteLine((i+1).ToString() + @". " + dir.GetFiles()[i].Name);
            }
            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.Write(@"Please choose a file or press '0' to start a new game: ");

            ConsoleKeyInfo cmd;
            while (true)
            {
                cmd = Console.ReadKey();
                Int32 val = (int)Char.GetNumericValue(cmd.KeyChar);
                
                if (val == 0)
                {
                    return "";
                }
                else if (val > dir.GetFiles().Length + 1)
                {
                    System.Console.WriteLine();
                    System.Console.WriteLine(@"Invalid selection, please choose a save file or press 0 to start a new game: ");
                    continue;
                }
               
                else
                {
                    return dir.GetFiles()[val - 1].Name;
                }

            }
        }
    }
}
