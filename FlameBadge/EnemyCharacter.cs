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
            AI smartCPU = new AI(FlameBadge.player_units, FlameBadge.cpu_units, new Tuple<int, int>(-1, -1), new Tuple<int, int>(-1, -1), 3);

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
