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
        public Char id { get; set; }
        public Int16 xPos { get; set; }
        public Int16 yPos { get; set; }

        public abstract void moveUp();
        public abstract void moveDown();
        public abstract void moveLeft();
        public abstract void moveRight();
    }
}
