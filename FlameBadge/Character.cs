/*
 * Character.cs - Flame Badge
 *      -- Abstract class used as skeleton for player and computer pieces.
 *      -- Movement functions must be implemented.
 *      
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    public abstract class Character
    {
        public Character(Char id, Int16 x, Int16 y)
        {
            this.id = id;
            this.xPos = x;
            this.yPos = y;
        }

        public Char id { get; set; }
        public Int16 xPos { get; set; }
        public Int16 yPos { get; set; }

        /// <summary>
        /// Moves the character up one space.
        /// </summary>
        public Boolean moveUp()
        {
            Logger.log(String.Format(@"Moving {0} up one space...", this.id), "debug");
            return GameBoard.update(this, xPos, (short)(yPos - 1));
        }
        /// <summary>
        /// Moves the character down one space.
        /// </summary>
        public Boolean moveDown()
        {
            Logger.log(String.Format(@"Moving {0} down one space...", this.id), "debug");
            return GameBoard.update(this, xPos, (short)(yPos + 1));
        }

        /// <summary>
        /// Moves the character left one space.
        /// </summary>
        public Boolean moveLeft()
        {
            Logger.log(String.Format(@"Moving {0} left one space...", this.id), "debug");
            return GameBoard.update(this, (short)(xPos - 1), yPos);
        }

        /// <summary>
        /// Moves the character right one space.
        /// </summary>
        public Boolean moveRight()
        {
            Logger.log(String.Format(@"Moving {0} right one space...", this.id), "debug");
            return GameBoard.update(this, (short)(xPos + 1), yPos);
        }

        /// <summary>
        /// Moves the character up and left one space.
        /// </summary>
        public Boolean moveUpLeft()
        {
            Logger.log(String.Format(@"Moving {0} up and left (diagonally) one space...", this.id), "debug");
            return GameBoard.update(this, (short)(xPos - 1), (short)(yPos - 1));
        }

        /// <summary>
        /// Moves the character down and left one space.
        /// </summary>
        public Boolean moveDownLeft()
        {
            Logger.log(String.Format(@"Moving {0} down and left (diagonally) one space...", this.id), "debug");
            return GameBoard.update(this, (short)(xPos - 1), (short)(yPos + 1));
        }

        /// <summary>
        /// Moves the character up and right one space.
        /// </summary>
        public Boolean moveUpRight()
        {
            Logger.log(String.Format(@"Moving {0} up and right (diagonally) one space...", this.id), "debug");
            return GameBoard.update(this, (short)(xPos + 1), (short)(yPos - 1));
        }

        /// <summary>
        /// Moves the character down and right one space.
        /// </summary>
        public Boolean moveDownRight()
        {
            Logger.log(String.Format(@"Moving {0} down and right (diagonally) one space...", this.id), "debug");
            return GameBoard.update(this, (short)(xPos + 1), (short)(yPos + 1));
        }

        public Boolean makeMove(Char cmd)
        {
            switch (cmd)
            {
                case '8':
                    return moveUp();

                case '2':
                    return moveDown();

                case '4':
                    return moveLeft();

                case '6':
                    return moveRight();

                case '7':
                    return moveUpLeft();

                case '9':
                    return moveUpRight();

                case '1':
                    return moveDownLeft();

                case '3':
                    return moveDownRight();

                default:
                    Sidebar.announce(String.Format(@"Invalid move, try again {0}.", this.id.ToString()), true);
                    return false;
            }
        }

        public abstract void takeTurn();
    }
}
