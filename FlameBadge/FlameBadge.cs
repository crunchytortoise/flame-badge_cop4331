/*
 * FlameBadge.cs - Flame Badge
 *      -- Game initialization and mainloop class.
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FlameBadge
{
    public class FlameBadge
    {
        public static Boolean hasEnded = false;
        public static Boolean cpuCapture = false;
        public static char curr_turn;
        public static List<PlayerCharacter> player_units = new List<PlayerCharacter>();
        public static List<EnemyCharacter> cpu_units = new List<EnemyCharacter>();
        public static String save_dir = Config.project_path + @"\\saves\\";
        public static GameBoard game_board;
        public static PlayerCharacter unitSelected;
        public static Form1 window;
        public FlameBadge(Form1 w)
        {

            window = w;
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

            // if we loaded we need to know whose turn it was
            curr_turn = '0';
            if (is_loaded)
                curr_turn = _getTurn(loaded_file);

            // Draw the game board.
            game_board = new GameBoard(is_loaded ? loaded_file : null);

            // Put the pieces on the board.
            PlayerCharacter.placePieces(is_loaded, loaded_file);
            EnemyCharacter.placePieces(is_loaded, loaded_file);


            //// mainloop
            //while (true)
            //{
            //    for (int i = 0; i < player_units.Count; i++)
            //    {
            //        // if we loaded a game, skip over everyone until the rightful
            //        // unit goes
            //        if (curr_turn != '0')
            //        {
            //            if (player_units[i].id != curr_turn)
            //                continue;
            //            else
            //                curr_turn = '0';
            //        }

            //        //THIS WILL SOON REPLACE THE PLAYER TAKE TURN
            //        //window.takeTurn(player_units[i]);
            //        player_units[i].takeTurn();
            //        List<Character> victims = GameBoard.getAttackableUnits(player_units[i].xPos, player_units[i].yPos, cpu_units.Cast<Character>().ToList());
            //        if (victims.Count > 0)
            //        {
            //            //pass in true to signify player
            //            GameBoard.attack(player_units[i].id, victims[0].id, true);
            //            GameBoard.redraw();
            //            if (cpu_units.Count == 0)
            //                FlameBadge.hasEnded = true;
            //        }
            //        Tuple<Int16, Int16> enemyCastle = GameBoard.getCPUCastle();
            //        if ((int)enemyCastle.Item2 == (int)player_units[i].xPos && (int)enemyCastle.Item1 == (int)player_units[i].yPos)
            //        {
            //            FlameBadge.hasEnded = true;
            //        }
            //            if (FlameBadge.hasEnded)
            //                _endGame();
            //    }

            //    for (int i = 0; i < cpu_units.Count; i++)
            //    {
            //        cpu_units[i].takeTurn();
            //        List<Character> victims = GameBoard.getAttackableUnits(cpu_units[i].xPos, cpu_units[i].yPos, player_units.Cast<Character>().ToList());
            //        if (victims.Count > 0)
            //        {
            //            //pass in false to signify AI
            //            GameBoard.attack(cpu_units[i].id, victims[0].id, false);
            //            GameBoard.redraw();
            //            if (player_units.Count == 0)
            //                FlameBadge.hasEnded = true;
            //        }
            //        Tuple<Int16, Int16> enemyCastle = GameBoard.getPlayerCastle();
            //        if ((int)enemyCastle.Item2 == (int)cpu_units[i].xPos && (int)enemyCastle.Item1 == (int)cpu_units[i].yPos)
            //        {
            //            FlameBadge.hasEnded = true;
            //            FlameBadge.cpuCapture = true;
            //        }
            //        if (FlameBadge.hasEnded)
            //            _endGame();
            //    }
            //}

        }

        public void unselectUnit()
        {
            unitSelected = null;
        }
        public void selectUnit(int x, int y)
        {
            unitSelected = null;
            foreach(PlayerCharacter s in player_units)
            {
               if( s.xPos == x && s.yPos == y && s.unitHasTakenAction()>0)
               {
                   unitSelected = s;
                   return;
               }
            }
        }

        public PlayerCharacter selectUnit()
        {
            return unitSelected; 
        }
 
        public void checkForTurnChange()
        {
        
            foreach(PlayerCharacter x in getPlayerCharacters())
            {
                if (x.unitHasTakenAction() != 0)
                {
                    return;
                }
            }

            enemyTakeTurn();

        }

        public void resetActionPoints()
        {
        
            foreach(PlayerCharacter x in getPlayerCharacters())
            {
                x.unitHasTakenAction(2);
            }
        }


        public GameBoard getGameBoard()
        {
            return game_board;
        }

        public List<PlayerCharacter> getPlayerCharacters()
        {
            return player_units;
        }

        public List<EnemyCharacter> getEnemyCharacters()
        {
            return cpu_units;
        }
        private void _endGame()
        {
            String msg;

            if (player_units.Count == 0 || FlameBadge.cpuCapture)
                msg = @"The computer wins! Press any key to quit...";
            else
                msg = @"You win! Press any key to quit...";

            Console.WriteLine(msg);
            System.Environment.Exit(0);
        }

        private Boolean _offerContinue()
        {
            //ConsoleKeyInfo cmd;
            //while (true)
            //{

            //    switch (cmd.KeyChar)
            //    {
            //        case '1':
            //            return false;
            //        case '2':
            //            return true;
            //        default:
            //            System.Console.WriteLine(@"Invalid command, please enter 1 or 2: ");
            //            break;
            //    }
            //}
            return true;
        }

        public void enemyTakeTurn()
        {
            for (int i = 0; i < cpu_units.Count; i++)
            {
                //window.panel1.Invalidate();
                
                cpu_units[i].takeTurn();
                List<Character> victims = GameBoard.getAttackableUnits(cpu_units[i].xPos, cpu_units[i].yPos, player_units.Cast<Character>().ToList());
                if (victims.Count > 0)
                {
                    //pass in false to signify AI
                    GameBoard.attack(cpu_units[i].id, victims[0].id, false);
                    GameBoard.redraw();
                    if (player_units.Count == 0)
                        FlameBadge.hasEnded = true;
                }
                Tuple<Int16, Int16> enemyCastle = GameBoard.getPlayerCastle();
                if ((int)enemyCastle.Item2 == (int)cpu_units[i].xPos && (int)enemyCastle.Item1 == (int)cpu_units[i].yPos)
                {
                    FlameBadge.hasEnded = true;
                    FlameBadge.cpuCapture = true;
                }
                if (FlameBadge.hasEnded)
                    _endGame();

                //System.Threading.Thread.Sleep(1000);
            }
            
            resetActionPoints(); 
        }

        private String _getSavedGame(DirectoryInfo dir)
        {
            //for(int i=0; i < dir.GetFiles().Length; i++)
            //{
            //}

            //ConsoleKeyInfo cmd;
            //while (true)
            //{
            //    Int32 val = (int)Char.GetNumericValue(cmd.KeyChar);
                
            //    if (val == 0)
            //    {
            //        return "";
            //    }
            //    else if (val > dir.GetFiles().Length + 1)
            //    {
            //        continue;
            //    }
               
            //    else
            //    {
            //        return dir.GetFiles()[val - 1].Name;
            //    }

            //}
            return " "; 
        }

        private Char _getTurn(String loaded_file) 
        {
            try
            {
                using (StreamReader sr = new StreamReader(loaded_file))
                {
                    while (sr.Peek() > -1)
                    {
                        String line = sr.ReadLine();
                        if (line.StartsWith("TURN"))
                        {
                            String[] turn_info = line.Split();
                            Char curr_turn = Convert.ToChar(turn_info[1]);
                            Logger.log(String.Format(@"It has been decided that it is {0}'s turn currently.", curr_turn));
                            return curr_turn;
                        }
                        else
                            continue;
                    }
                    Logger.log("Never found a TURN directive in save file. Starting over...", "error");
                    return '0';
                }
            }
            catch (Exception)
            {
                Logger.log("Could not establish whose turn it was. Oh well, starting over...", "error");
                return '0';
            }
        }
    }
}
