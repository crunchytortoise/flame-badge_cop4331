/*
 * PlayerCharacter.cs - Flame Badge
 *      -- Character class for player controlled pieces.
 *      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    class PlayerCharacter : Character
    {
        public PlayerCharacter(Char id, Int16 x, Int16 y) : base(id, x, y) {}

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
