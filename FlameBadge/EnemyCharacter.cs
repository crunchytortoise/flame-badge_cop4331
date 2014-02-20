using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    class EnemyCharacter : Character
    {
        public EnemyCharacter(Char id, Int16 x, Int16 y) : base(id, x, y) {}

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

        public override void takeTurn()
        {
            Sidebar.announce(String.Format(@"CPU taking turn...", this.id.ToString()), false);

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
            }
        }
    }
}
