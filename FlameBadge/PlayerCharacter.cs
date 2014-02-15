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
        /// <summary>
        /// Moves the player character up one space.
        /// </summary>
        public override void moveUp()
        {
            if (yPos < GameBoard.overlay.GetLength(1) - 1)
            {
                Logger.log(String.Format("Moving {0} up one space...", this.id), "debug");
                GameBoard.update(this, xPos, (short)(yPos + 1));
            }

        }

        /// <summary>
        /// Moves the player character down one space.
        /// </summary>
        public override void moveDown()
        {
            if (yPos > 0)
            {
                Logger.log(String.Format("Moving {0} down one space...", this.id), "debug");
                GameBoard.update(this, xPos, (short)(yPos - 1));
            }
        }

        /// <summary>
        /// Moves the player character left one space.
        /// </summary>
        public override void moveLeft()
        {
            if (xPos > 0)
            {
                Logger.log(String.Format("Moving {0} left one space...", this.id), "debug");
                GameBoard.update(this, (short)(xPos - 1), yPos);
            }
        }

        /// <summary>
        /// Moves the player character right one space.
        /// </summary>
        public override void moveRight()
        {
            if (xPos < GameBoard.overlay.GetLength(0) - 1)
            {
                Logger.log(String.Format("Moving {0} right one space...", this.id), "debug");
                GameBoard.update(this, (short)(xPos + 1), yPos);
            }
        }
    }
}
