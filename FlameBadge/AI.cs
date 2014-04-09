/*
 * AI.cs - Flame Badge
 *      -- Game Artificial Intelligence class.
 * 
 */

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlameBadge
{
    class AI
    {
        public AI (List<PlayerCharacter> playerUnits, List<EnemyCharacter> cpuUnits, Tuple<int,int> playerBasePos, Tuple<int,int> cpuBasePos, int level = 0) {
            this.playerUnits = playerUnits;
            this.cpuUnits = cpuUnits;
            this.playerBasePos = playerBasePos;
            this.cpuBasePos = cpuBasePos;
            if (level < 0) { this.level = 0; }
            else if (level > 5) { this.level = 5; }
            else { this.level = level; }
        }
        private List<PlayerCharacter> playerUnits { get; set; }
        private List<EnemyCharacter> cpuUnits { get; set; }
        private Tuple<int,int> playerBasePos {get; set;}
        private Tuple<int,int> cpuBasePos {get; set;}
        private int level { get; set; }

        // Stores the move order of the cpu units
        private List<EnemyCharacter> cpuMoveOrder;

        // Represents the # of player units per # of cpu units that is considered an overwhelming force
        private float OVERWHELMING_FORCE_PERCENT = 2.1f;

        // Heuristic values for certain predefined actions
        private int HEURISTIC_ATTACK_BASE_ATTACKER   = 80;
        private int HEURISTIC_DAMAGE_OPPONENT_BASE   = 79;
        private int HEURISTIC_FLEE_FROM_OVER_FORCE   = 45;
        private int HEURISTIC_ATTACK_OPPONENT_UNIT   = 30;
        private int HEURISTIC_TRAVEL_TO_ENEMY_BASE   = 25;
        private int HEURISTIC_GUARD_BASE             = 20;
        private int HEURISTIC_TRAVEL_TO_HOME_BASE    = 15;
        private int HEURISTIC_INTERCEPT_OPPONENT     = 10;


        /// <summary>
        /// Returns the AIs choice of the next unit to move
        /// </summary>
        /// <returns>Reference to the next cpu unit which should move.</returns>
        public EnemyCharacter getNextUnitToMove() 
        {
            if (cpuMoveOrder.Count == 0)
            {
                setupMoveOrder();
            }
            EnemyCharacter next = cpuMoveOrder[0];
            cpuMoveOrder.RemoveAt(0);
            return next;
        }

        /// <summary>
        /// Gets the unit which should be attacked by the given unit
        /// </summary>
        /// <param name="unit">Computer controlled unit for which to decide who to attack</param>
        /// <returns>Player unit to be attacked</returns>
        public PlayerCharacter getUnitToAttack(EnemyCharacter unit)
        {
            List<Character> enemies = GameBoard.getAttackableUnits(unit.xPos, unit.yPos, playerUnits.Cast<Character>().ToList());
            // TODO: Implement some kind of decision strategy
            // for now, just pick an attackable unit at random
            Random r = new Random();
            return (PlayerCharacter) enemies[r.Next(0, enemies.Count - 1)];
        }


        /// <summary>
        /// Gets the determined best x,y coordinates for the given unit's next move
        /// </summary>
        /// <param name="unit">Unit for which to calculate the best move</param>
        /// <returns>Tuple x,y coordinates</returns>
        public Tuple<int,int> getNextMove(EnemyCharacter unit)
        {
            return calculateNextMove(unit, level + 1);
        }

        private void setupMoveOrder()
        {
            // since the mechanics aren't very complicated yet, so just order at random

            cpuMoveOrder = new List<EnemyCharacter>(cpuUnits);
            Random rng = new Random();
            cpuMoveOrder.OrderBy(a => rng.Next());
        }

        private int canAttackBaseAttacker(Character unit, List<Character> enemy_units, Tuple<int,int> base_pos)
        {
            // if there is no base, obviously this will never be true
            if(base_pos.Item1 < 0) return 0;
            foreach(Character enemy_unit in enemy_units){
                if(GameBoard.distance(enemy_unit.xPos, enemy_unit.yPos, base_pos.Item1, base_pos.Item2) < 2)
                {
                    if(GameBoard.distance(enemy_unit.xPos, enemy_unit.yPos, unit.xPos, unit.yPos) < 2) {
                        return HEURISTIC_ATTACK_BASE_ATTACKER;
                    }
                    else { return 0; }
                }
                else { return 0; }
            }
            return 0;
        }

        private int canDamageOpponentBase(Character unit, Tuple<int,int> basePos)
        {
            if(basePos.Item1 < 0) return 0;

            if (GameBoard.distance(unit.xPos, unit.yPos, basePos.Item1, basePos.Item2) < 2)
            {
                return HEURISTIC_DAMAGE_OPPONENT_BASE;
            }
            else { return 0; }
        }

        private int canAttackOpponent(Character unit, List<Character> enemyUnits)
        {
            List<Character> attackable = GameBoard.getAttackableUnits(unit.xPos, unit.yPos, enemyUnits);
            if (attackable.Any()) { return HEURISTIC_ATTACK_OPPONENT_UNIT; }
            return 0;
        }

        private int isFleeingFromOverwhelmingForce(Character unit, List<Character> enemy_units, List<Character> friendly_units, int prev_x, int prev_y)
        {
            List<Character> unitsToConsider = new List<Character>();
            int enemy_count = 0;
            int unit_count = 0;
            // get all units within 3 spaces of the current unit
            foreach( Character enemy in enemy_units){
                if(GameBoard.distance(enemy.xPos, enemy.yPos, unit.xPos, unit.yPos) <= 3) {
                    unitsToConsider.Add(enemy);
                    enemy_count++;
                }
            }
            foreach( Character friendly_unit in friendly_units){
                // don't include the given unit
                if(friendly_unit == unit) continue;
                if(GameBoard.distance(friendly_unit.xPos, friendly_unit.yPos, unit.xPos, unit.yPos) <= 3){
                    unit_count++;
                }
            }

            // if the player force is at least the overwhelming percentage, fleeing is an option
            if(enemy_count/(unit_count+1) > OVERWHELMING_FORCE_PERCENT) {
                // now we need to get which direction they are all mainly coming from
                // get the average x and y vector they are coming from
                float avg_x = 0.0f;
                float avg_y = 0.0f;
                List<int> vectors = new List<int>();
                foreach( Character enemy in unitsToConsider)
                {
                    avg_x += (enemy.xPos-unit.xPos);
                    avg_y += (enemy.yPos-unit.yPos);
                }
                // if the resultant additions so far are very small, we're surrounded, so there's nowhere to run to
                if((avg_x < 1 && avg_x > -1) && (avg_y < 1 && avg_y > -1)) { return 0;}

                // get the unit vector of current motion
                float motion_x = unit.xPos - prev_x;
                float motion_y = unit.yPos - prev_y;
                motion_x = motion_x/((float) Math.Sqrt(motion_x*motion_x + motion_y*motion_y));
                motion_y = motion_x/((float) Math.Sqrt(motion_x*motion_x + motion_y*motion_y));

                // make the averages unit vectors
                avg_x = avg_x/((float) Math.Sqrt(avg_x*avg_x + avg_y*avg_y));
                avg_y = avg_y/((float) Math.Sqrt(avg_x*avg_x + avg_y*avg_y));

                // now just get the angle between the vectors
                float theta = (float) Math.Acos(motion_x*avg_x + motion_y*avg_y);

                // if current motion is greater than 90 degrees away from the enemy positions, unit is fleeing
                if(theta > Math.PI/2){ return HEURISTIC_FLEE_FROM_OVER_FORCE; }
                else { return 0; }

            }
            else { return 0; }

        }

        private int isTravellingToBase(Character unit, int prev_x, int prev_y, Tuple<int,int> basePos, Boolean isEnemyBase = true)
        {
            // if the base is not defined, the unit can't be traveling to it
            if(basePos.Item1 < 0) return 0;

            if (GameBoard.distance(unit.xPos, unit.yPos, basePos.Item1, basePos.Item2) < GameBoard.distance(prev_x, prev_y, basePos.Item1, basePos.Item2))
            { 
                if(isEnemyBase){
                    return HEURISTIC_TRAVEL_TO_ENEMY_BASE; 
                }
                else {
                    int numGuarding = 0;
                    foreach (EnemyCharacter friendly in cpuUnits)
                    {
                        if (GameBoard.distance(friendly.xPos, friendly.yPos, cpuBasePos.Item1, cpuBasePos.Item2) < 3)
                        {
                            numGuarding++;
                        }
                    }
                    return HEURISTIC_TRAVEL_TO_HOME_BASE - numGuarding;
                }
            }
            else {return 0; }
        }

        private int isGuardingBase(Character unit, Tuple<int,int> basePos)
        {
            if(GameBoard.distance(unit.xPos, unit.yPos, basePos.Item1, basePos.Item2) < 2) {
                return HEURISTIC_GUARD_BASE;
            }
            else { return 0; }
        }

        private int isMovingToInterceptOpponent(Character unit, List<Character> enemy_units, int prev_x, int prev_y) 
        {
            // as long as the cpu unit is moving toward another player unit, return true
            foreach(Character e_unit in enemy_units)
            {
                if(GameBoard.distance(unit.xPos, unit.yPos, e_unit.xPos, e_unit.yPos) < GameBoard.distance(prev_x, prev_y, e_unit.xPos, e_unit.yPos)){
                    return HEURISTIC_INTERCEPT_OPPONENT;
                }
            }
            return 0;
        }

        private int miniMax(int n, int m, List<int> l)
        {
            if(l.Count == 1) return l[0];
            return 0;
        }

        private int maxMini(int n, int m, List<int> l)
        {
            return 0;
        }

        private Tuple<int, int> calculateNextMove(EnemyCharacter c, int numLevels)
        {
            List<Tuple<int, int>> moves = c.getPossibleMoves();
            int bestMoveHeuristic = -999999;
            Tuple<int, int> bestMove = null;
            List<Character> enemyUnits = playerUnits.Cast<Character>().ToList();
            List<Character> units = cpuUnits.Cast<Character>().ToList();

            Logger.log(String.Format(@"AI is calculating moves for EnemyCharacter {0} with depth of {1}.",c.id, numLevels), "debug");

            foreach (Tuple<int, int> move in moves)
            {
                int heuristic = 0;

                Character potentialUnit = null;

                // create a new character so that we don't override the original
                potentialUnit = new EnemyCharacter(c.id, (short)move.Item1, (short)move.Item2, c.health, c.level, c.dpsMod);
                heuristic += canAttackBaseAttacker(potentialUnit, enemyUnits, cpuBasePos);
                heuristic += canDamageOpponentBase(potentialUnit, playerBasePos);
                heuristic += isGuardingBase(potentialUnit, cpuBasePos);
                heuristic += isTravellingToBase(potentialUnit, c.xPos, c.yPos, playerBasePos, true);
                heuristic += isTravellingToBase(potentialUnit, c.xPos, c.yPos, cpuBasePos, false);
                heuristic += isFleeingFromOverwhelmingForce(potentialUnit, enemyUnits, units, c.xPos, c.yPos);
                heuristic += canAttackOpponent(potentialUnit, enemyUnits);
                heuristic += isMovingToInterceptOpponent(potentialUnit, enemyUnits, c.xPos, c.yPos);

                int negHeuristic = calculateNextMoveHeuristic(c, enemyUnits, units, numLevels - 1, false);

                // subtract the heuristic points gained by the enemy
                heuristic -= negHeuristic;

                // get the best move for that unit
                if (heuristic > bestMoveHeuristic)
                {
                    bestMoveHeuristic = heuristic;
                    bestMove = move;
                }
                else if (heuristic == bestMoveHeuristic)
                {
                    // if the values are equal, flip a coin to see which to choose
                    Random r = new Random();
                    if (r.Next(0, 2) == 0)
                        bestMove = move;
                }
                
            }
            Logger.log(String.Format(@"AI has completed calculating moves for EnemyCharacter {0}.", c.id), "debug");

            return bestMove;
        }

        private int calculateNextMoveHeuristic(EnemyCharacter c, List<Character> units, List<Character> enemyUnits, int numLevels, Boolean cpuTurn)
        {
            if (numLevels == 0) return 0;

            List<Character> futureUnits = new List<Character>();
            int averageHeuristic = 0;

            // move each of the units tentatively
            foreach (Character unit in units)
            {
                if (unit == c) continue;
                // get all possible moves for the cpu unit
                List<Tuple<int, int>> moves = unit.getPossibleMoves();
                int bestMoveHeuristic = -999999;
                Character best = null;

                foreach (Tuple<int, int> move in moves)
                {
                    int heuristic = 0;

                    Character potentialUnit = null;
                    if (cpuTurn)
                    {
                        // create a new character so that we don't override the original
                        potentialUnit = new EnemyCharacter(unit.id, (short)move.Item1, (short)move.Item2, unit.health, unit.level, unit.dpsMod);
                        heuristic += canAttackBaseAttacker(potentialUnit, enemyUnits, cpuBasePos);
                        heuristic += canDamageOpponentBase(potentialUnit, playerBasePos);
                        heuristic += isGuardingBase(potentialUnit, cpuBasePos);
                        heuristic += isTravellingToBase(potentialUnit, unit.xPos, unit.yPos, playerBasePos, true);
                        heuristic += isTravellingToBase(potentialUnit, unit.xPos, unit.yPos, cpuBasePos, false);
                    }
                    else
                    {
                        // create a new character so that we don't override the original
                        potentialUnit = new PlayerCharacter(unit.id, (short)move.Item1, (short)move.Item2, unit.health, unit.level, unit.dpsMod);
                        heuristic += canAttackBaseAttacker(potentialUnit, enemyUnits, playerBasePos);
                        heuristic += canDamageOpponentBase(potentialUnit, cpuBasePos);
                        heuristic += isGuardingBase(potentialUnit, playerBasePos);
                        heuristic += isTravellingToBase(potentialUnit, unit.xPos, unit.yPos, cpuBasePos, true);
                        heuristic += isTravellingToBase(potentialUnit, unit.xPos, unit.yPos, playerBasePos, false);
                    }


                    heuristic += isFleeingFromOverwhelmingForce(potentialUnit, enemyUnits, units, unit.xPos, unit.yPos);
                    heuristic += canAttackOpponent(potentialUnit, enemyUnits);
                    heuristic += isMovingToInterceptOpponent(potentialUnit, enemyUnits, unit.xPos, unit.yPos);

                    // get the best move for that unit
                    if (heuristic > bestMoveHeuristic)
                    {
                        bestMoveHeuristic = heuristic;
                        best = potentialUnit;
                    }
                    else if (heuristic == bestMoveHeuristic)
                    {
                        // if the values are equal, flip a coin to see which to choose
                        Random r = new Random();
                        if (r.Next(0, 2) == 0)
                            best = potentialUnit;
                    }
                }
                averageHeuristic += bestMoveHeuristic;
                futureUnits.Add(best);
            }
            averageHeuristic = averageHeuristic / units.Count();

            if (cpuTurn)
            {
                List<Tuple<int, int>> moves = c.getPossibleMoves();
                int bestMoveHeuristic = -999999;

                foreach (Tuple<int, int> move in moves)
                {
                    int heuristic = 0;

                    Character potentialUnit = null;

                    // create a new character so that we don't override the original
                    potentialUnit = new EnemyCharacter(c.id, (short)move.Item1, (short)move.Item2, c.health, c.level, c.dpsMod);
                    heuristic += canAttackBaseAttacker(potentialUnit, enemyUnits, cpuBasePos);
                    heuristic += canDamageOpponentBase(potentialUnit, playerBasePos);
                    heuristic += isGuardingBase(potentialUnit, cpuBasePos);
                    heuristic += isTravellingToBase(potentialUnit, c.xPos, c.yPos, playerBasePos, true);
                    heuristic += isTravellingToBase(potentialUnit, c.xPos, c.yPos, cpuBasePos, false);
                    heuristic += isFleeingFromOverwhelmingForce(potentialUnit, enemyUnits, units, c.xPos, c.yPos);
                    heuristic += canAttackOpponent(potentialUnit, enemyUnits);
                    heuristic += isMovingToInterceptOpponent(potentialUnit, enemyUnits, c.xPos, c.yPos);

                    int negHeuristic = calculateNextMoveHeuristic(c, enemyUnits, futureUnits, numLevels - 1, !cpuTurn);

                    // subtract the heuristic points gained by the enemy
                    heuristic -= negHeuristic;

                    // get the best move for that unit
                    if (heuristic > bestMoveHeuristic)
                    {
                        bestMoveHeuristic = heuristic;
                    }
                }
                return bestMoveHeuristic;
            }
            else
            {
                // if this was the tentative player turn, go down the next level and return the heuristic
                int negHeuristic = calculateNextMoveHeuristic(c, enemyUnits, futureUnits, numLevels - 1, !cpuTurn);
                averageHeuristic -= negHeuristic;
                return averageHeuristic;
            }
        }


    }


}