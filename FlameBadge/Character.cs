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
        
        public Character(Char id, Int16 x, Int16 y, int health, int level, int dpsMod)
        {
            this.id = id;
            this.xPos = x;
            this.yPos = y;
            this.health = health;
            this.dpsMod = dpsMod;
            this.level = level;
        }

        public Char id { get; set; }
        public Int16 xPos { get; set; }
        public Int16 yPos { get; set; }
        public int level { get; set; }
        public int health { get; set; }
        public int dpsMod { get; set; }

        /// <summary>
        /// Deals damage to the character.
        /// </summary>
        /// <param name="damage"></param>
        public void damage(int dpsMod)
        {
            Random rnd = new Random();
            int damage = rnd.Next(1, 4) + dpsMod;
            if (rnd.Next(1, 21) == 20)
            {
                Logger.log(String.Format(@"Critical hit!"), "debug");
                damage = 6 + dpsMod;
            }
            if (rnd.Next(1, 21) < 4)
            {
                Logger.log(String.Format(@"Miss!"), "debug");
                damage = 0;
            }
            Logger.log(String.Format(@"Dealing {0} damage to {1}...", damage, this.id), "debug");
            this.health -= damage;
        }
        /// <summary>
        /// Levels up the character, increases their damage modifier and health
        /// </summary>
        public void levelUp() 
        {
            Logger.log(String.Format(@"Leveling up {0} by one.", this.id), "debug");
            this.level++;
            this.dpsMod++;
            this.health += 5;
        }

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

        public Boolean validMovePerformed(int x, int y)
        {
           foreach(Tuple<int,int> s in getPossibleMoves())
           {
                if(s.Item1==x && s.Item2==y)
                {
                    return true;
                }
           }
           return false;
        }
          public Boolean makeMove( int x, int y )
        {
           if(validMovePerformed(x,y))
           {
               this.xPos=(short)x;
               this.yPos=(short)y;
               GameBoard.update(this, (short)x, (short)y);
               return true;
           }
           return false; 
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

        /// <summary>
        /// Returns all possible moves for the given unit
        /// </summary>
        /// <param name="c">Character unit</param>
        /// <returns>List of x,y positions that the unit can possibly move to.</returns>
        /// <remarks>Currenty assumes all units can only move 1 space per move.</remarks>
        public List<Tuple<int, int>> getPossibleMoves()
        {
            return GameBoard.getPossibleMovesFromLoc(xPos, yPos);
        }

    }
}
