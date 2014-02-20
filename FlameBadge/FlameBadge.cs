/*
 * FlameBadge.cs - Flame Badge
 *      -- Game initialization and mainloop class.
 * 
 */

using System;
using System.Collections.Generic;
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
        
        public FlameBadge()
        {
            // Draw the game board.
            GameBoard game_board = new GameBoard();

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
    }
}
