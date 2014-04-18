using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*namespace MapGeneration
{
    class Map
    {
        static void Main(string[] args)
        {

        }
    }
}*/

public class Map 
{
    private static int mapMult = 20;
	private static int copseMult = 6;
	private static int copseRadDiv = 4;
	private static int mountainMult = 1;
	private static int waterMult = 1;
	
	// trees won't exceed 33% of total available area if treeMaxDiv = 3
	// or 25% of total available area if treeMaxDiv = 4
	// however, the more trees placed, the higher the number that may be
	// overwritten by mountain, water, or roadway
	private static int treeMaxDiv = 3;
	// mountains and water work in a similar fashion as trees
	// each won't exceed 10% of total available area if xxxMaxDiv = 10
	private static int mountainMaxDiv = 10;
	private static int waterMaxDiv = 10;
		
	private static char border = '@';
	private static char grass = '%';
	private static char tree = '#';
	private static char mountain = '^';
	private static char water = '~';
	private static char roadway = '&';
	private static char bridge = '=';
	private static char tunnel = '_';
	private static char castle1 = '+';
	private static char castle2 = '*';
	
	private static char mazeWall = 'X';
	private static char mazePath = ' ';
	private static char mazeStart = 'S';
	private static char mazeEnd = 'E';

	private static Random rnd = new Random();
	
	private const bool DEBUG = false;
	private const bool FIND = false;

    public static char[,] create(int mapSize)
	{
		// 3 predefined map sizes
		if (mapSize < 1 || mapSize > 3)
			return null;
		int height = mapSize * mapMult + 1;
		int width = height; // square map
		char[,] map = new char[height,width];
        
        //Porting Arrays.fill from Java to C#
        for (int i = 0; i < width; i++)
        {
            //Arrays.fill(map[,0], border); // top border
            map[0, i] = border;
            //Arrays.fill(map[height - 1], border); // bottom border
            map[height - 1, i] = border;
        }
        
		
		for (int i = height - 2; i > 0; i--)
		{
			map[i,0] = map[i,width - 1] = border; // left and right borders
			for (int j = width - 2; j > 0; j--)
				map[i,j] = grass;
		}
		
		// number of groups of trees is determined by ratio of height and width
		addTrees(map, rnd.Next(0,(height < width ? width / height : height / width) * copseMult),
				height * width / treeMaxDiv);
		
		// using a maze gives randomness to the elements (mountain ranges, rivers, roadways)
		// placed on the map; using the same maze for like elements (e.g. using the same maze to place
		// 3 mountain ranges) increases the chances that those like elements will cross or connect
		// to each other, while using different mazes for different elements decreases (but doesn't
		// eliminate) the chances that the new element will overwrite previous elements by following the
		// same pathway; therefore, a new maze is generated for each new element/set of elements placed
		
		// mountain always replace grass and tree elements
		char[,] maze = createMaze(width / 2, height / 2);
		addElement(map, maze, mountain, rnd.Next(0,mapSize * mountainMult + 2));
		
		// water always replaces grass, tree, and mountain elements
		maze = createMaze(width / 2, height / 2);
		addElement(map, maze, water, rnd.Next(0,mapSize * waterMult + 2));
		
		// roadway always replaces grass and tree elements
		// it also replaces water elements with a bridge and mountain elements with a tunnel
		maze = createMaze(width / 2, height / 2);
		addRoadway(map, maze);
		
        surroundCastle(map);

		return map;
		
	} // end method

    private static void surroundCastle(char[,] map) {

        for (int i = 0; i < /*map.length*/map.GetLength(0); i++)
        {
            for (int j = 0; j < /*map[0].length*/map.GetLength(1); j++)
            {
                if (map[i, j] == '+' || map[i, j] == '*') {
                    //up down left and right
                    if (i - 1 >= 0){
                        if(map[i-1, j] != '@')
                            map[i - 1, j] = '%';
                    }
                    if (i + 1 < map.GetLength(0)){
                        if(map[i+1, j] != '@')
                            map[i + 1, j] = '%';
                    }
                    if (j - 1 >= 0){
                       if(map[i, j-1] != '@')
                            map[i, j-1] = '%';
                    }
                    if (j + 1 < map.GetLength(1)){
                        if(map[i, j+1] != '@')
                            map[i, j + 1] = '%';
                    }

                    //diagonals
                    if (i - 1 >= 0 && j + 1 < map.GetLength(1)){
                        if(map[i-1, j+1] != '@')
                            map[i - 1, j + 1] = '%';
                    }
                    if (i + 1 < map.GetLength(0) && j - 1 >= 0){
                        if(map[i+1, j-1] != '@')
                            map[i + 1, j - 1] = '%';
                    }
                    if (i - 1 >= 0 && j - 1 >= 0){
                        if(map[i-1, j-1] != '@')    
                            map[i - 1, j - 1] = '%';
                    }
                    if (i + 1 < map.GetLength(0) && j + 1 < map.GetLength(1)){
                        if(map[i+1, j+1] != '@')
                            map[i + 1, j + 1] = '%';
                    }

                    //second ring (top)
                    if (i + 2 < map.GetLength(0) && j - 2 >= 0)
                    {
                        if (map[i+2, j-2] != '@')
                            map[i+2, j-2] = '&';
                    }
                    if (i + 2 < map.GetLength(0) && j - 1 >= 0)
                    {
                        if (map[i+2, j-1] != '@')
                            map[i+2, j-1] = '&';
                    }
                    if (i + 2 < map.GetLength(0))
                    {
                        if (map[i+2, j] != '@')
                            map[i+2, j] = '&';
                    }
                    if (i + 2 < map.GetLength(0) && j + 1 < map.GetLength(1))
                    {
                        if (map[i+2, j+1] != '@')
                            map[i+2, j+1] = '&';
                    }
                    if (i + 2 < map.GetLength(0) && j + 2 < map.GetLength(1))
                    {
                        if (map[i+2, j+2] != '@')
                            map[i+2, j+2] = '&';
                    }

                    //second ring (bottom)
                    if (i - 2 >= 0 && j - 2 >= 0)
                    {
                        if (map[i - 2, j - 2] != '@')
                            map[i - 2, j - 2] = '&';
                    }
                    if (i - 2 >= 0 && j - 1 >= 0)
                    {
                        if (map[i - 2, j - 1] != '@')
                            map[i - 2, j - 1] = '&';
                    }
                    if (i - 2 >= 0)
                    {
                        if (map[i - 2, j] != '@')
                            map[i - 2, j] = '&';
                    }
                    if (i - 2 >= 0 && j + 1 < map.GetLength(1))
                    {
                        if (map[i - 2, j + 1] != '@')
                            map[i - 2, j + 1] = '&';
                    }
                    if (i - 2 >= 0 && j + 2 < map.GetLength(1))
                    {
                        if (map[i - 2, j + 2] != '@')
                            map[i - 2, j + 2] = '&';
                    }

                    //second ring (left)
                    if (i + 1 < map.GetLength(0) && j - 2 >= 0)
                    {
                        if (map[i + 1, j - 2] != '@')
                            map[i + 1, j - 2] = '&';
                    }
                    if (j - 2 >= 0)
                    {
                        if (map[i, j - 2] != '@')
                            map[i, j - 2] = '&';
                    }
                    if (i - 1 >= 0 && j - 2 >= 0)
                    {
                        if (map[i - 1, j - 2] != '@')
                            map[i - 1, j - 2] = '&';
                    }

                    //second ring (right)
                    if (i + 1 < map.GetLength(0) && j + 2 < map.GetLength(1))
                    {
                        if (map[i + 1, j + 2] != '@')
                            map[i + 1, j + 2] = '&';
                    }
                    if (j + 2 < map.GetLength(1))
                    {
                        if (map[i, j + 2] != '@')
                            map[i, j + 2] = '&';
                    }
                    if (i - 1 >= 0 && j + 2 < map.GetLength(1))
                    {
                        if (map[i - 1, j + 2] != '@')
                            map[i - 1, j + 2] = '&';
                    }


                }
                    
            }

            
        } // end for
        return;
    }

    private static void addTrees(char[,] map, int numCopses, int treeMax)
	{
		int cnt = 0;
		
        //outerloop:
        // to break out of nested loop when treeMax is reached
        //C# does not have loop labels like Java, so use a breakout variable that breaks out of everything if it is set instead
        bool breakout = false;
		for (int i = 0; i < numCopses; i++)
		{
            int xSize = /*map[0].length*/map.GetLength(1) - 2;
            int ySize = /*map.length*/map.GetLength(0) - 2;
			int row = rnd.Next(0,ySize) + 1;
			int col = rnd.Next(0,xSize) + 1;
			int rad = rnd.Next(0,(ySize < xSize ? ySize : xSize) / copseRadDiv) + 1;
			for (int y = -rad; y < rad; y++)
			{ 
	            for (int x = -rad; x < rad; x++)
	            {
	            	int xx = x * x;
	                int yy = y * y;
	                if ( (rad > 1 && (xx + yy + 2) < (rad * rad))
	                		 || (rad < 2 && (xx + yy) < (rad * rad)) )
	                {
	                	map[((ySize + row + y) % ySize) + 1,((xSize + col + x) % xSize) + 1] = tree;
	                	if (++cnt >= treeMax){
	                		breakout = true;
                            break;
                        }
	                }
	            }
                if(breakout)
                    break;
	        }
            if(breakout)
                break;
		}
		if (DEBUG) System.Console.WriteLine("trees placed = " + cnt);
		//if (FIND && cnt >= treeMax) MapTest.maxTrees = true;
	} // end method

    private static void addElement(char[,] map, char[,] maze, char marker, int num)
	{
		int height = /*map.length*/map.GetLength(0) - 2;
		int width = /*map[0].length*/map.GetLength(1) - 2;
		int startRow,
			startCol,
			endRow,
			endCol;
		int cnt = 0;
		int size = height * width;
		int maxElements = 0;
		if (marker == mountain)
			maxElements = size / mountainMaxDiv;
		else if (marker == water)
			maxElements = size / waterMaxDiv;			
		if (DEBUG) System.Console.WriteLine("maxElements = " + maxElements);
		
		for (int i = 0; i < num; i++)
		{
			startRow = rnd.Next(0,height) + 1;
			do
			{
				startCol = rnd.Next(0,width) + 1;
			} while (maze[startRow,startCol] == mazeWall);
			maze[startRow,startCol] = mazeStart;
			endRow = rnd.Next(0,height) + 1;
			do
			{
				endCol = rnd.Next(0,width) + 1;
			} while (maze[endRow,endCol] == mazeWall || maze[endRow,endCol] == mazeEnd);
			maze[endRow,endCol] = mazeEnd;
			if (DEBUG) System.Console.WriteLine("start (" + startCol + "," + startRow + "),  end (" + endCol + "," + endRow + ")");
			
			if (cnt < maxElements)
				cnt = markMap(maze, map, marker, cnt, maxElements, startRow - 1, startCol - 1);
			
			maze[startRow,startCol] = mazePath;
			maze[endRow,endCol] = mazePath;
			
			if (DEBUG) System.Console.WriteLine("total elements (" + marker + ") placed = " + cnt);
			if (FIND && cnt >= maxElements)
			{
				//if (marker == mountain)
					//MapTest.maxMountains = true;
				//if (marker == water)
					//MapTest.maxWater = true;
			}

		}
	} // end method

    private static void addRoadway(char[,] map, char[,] maze)
	{
		int xSize = /*map[0].length*/map.GetLength(1) - 2;
		int ySize = /*map.length*/map.GetLength(0) - 2;
		int size = xSize + ySize - 2;
		
		int startIndex = rnd.Next(0,size);
		if (startIndex >= xSize)
			startIndex = (startIndex % xSize) * xSize;
		int endIndex = (xSize * ySize) - startIndex - 1;	
		if (DEBUG) System.Console.WriteLine("start (" + startIndex + ")  end (" + endIndex + ")");
		
		int columns = /*map[0].length*/ map.GetLength(1) - 2;
		int startRow = startIndex / columns + 1; // row of current spot
		int startCol = startIndex % columns + 1; // column of current spot
		int endRow = endIndex / columns + 1; // row of current spot
		int endCol = endIndex % columns + 1; // column of current spot
		
		maze[startRow,startCol] = mazeStart;
		maze[endRow,endCol] = mazeEnd;
		
		// unlike mountains and water, cnt is not used when marking the roadway
		markMap(maze, map, roadway, 0, 0, startRow - 1, startCol - 1);
		if (DEBUG) System.Console.WriteLine("start (" + startCol + "," + startRow + "),  end (" + endCol + "," + endRow + ")");

		map[startRow,startCol] = castle1;
		map[endRow,endCol] = castle2;
		
	} // end method

    // given positive integers width and height, create a width (m) × height (n)
    // maze; returns a 2D char array that contains the new maze
    private static char[,] createMaze(int width, int height)
	{
		// total width by height dimensions of maze including all internal
		// walls and maze borders
		int mazeHeight = height * 2 + 1;
		int mazeWidth = width * 2 + 1;
		
		// 2D array that holds entire maze, including borders
		char[,] maze = new char[mazeHeight,mazeWidth];
		
		// fill maze array; m by n spots in the maze will be clear; each clear
		// space will be completely surrounded by walls;
		// e.g. a 3 x 1 maze has 3 clear spaces as shown
		// *******
		// * * * *
		// *******
		for (int row = 0; row < maze.GetLength(0); row++)
		{
			for (int col = 0; col < maze.GetLength(1)/*maze[0].length*/; col++)
			{
				// only spots with odd col,row coordinates are clear;
				// all other spots start as walls
				maze[row,col] = ((row + 1) % 2 == 0 && (col + 1) % 2 == 0 ? mazePath
						: mazeWall);
				
			} // end for
			
		} // end for
		
		// number of removable walls in the first 2 rows based on requested
		// width; for example, when width = 4, then pattern is 3 (row 1)
		// then 4 (row 2); wallPattern = 3 + 4 = 7
		// '1#2#3#4' <= 3 removable walls '#' when width = 4
		// '#X#X#X#' <= 4 removable walls (note: 'X' are non-removable walls)
		// ' # # # ' <= pattern repeats
		// '#X#X#X#' <= pattern repeats
		// used when calculating a wall's col,row based in an index
		int wallPattern = 2 * width - 1;
		
		// numWalls is the total number of removable internal walls;
		// this value does not include non-removable internal walls
		int numWalls = 2 * width * height - width - height;
		
		// walls array contains the indices of all internal walls that can be
		// considered for removal from the maze
		int[] walls = new int[numWalls];
		
		// initialize random number generator using current time as seed
		//Random rnd = new Random(System.currentTimeMillis());
		
		// randomize walls array
		// Fisher-Yates shuffle modified to initialize the elements as the
		// shuffle is performed thus eliminating the extra loop that would be
		// needed to separately initialize the array
		for (int i = walls.Length - 1; i > 0; i--)
		{
			int j = rnd.Next(0, i + 1);			
			int temp = walls[i];
			walls[i] = walls[j] > 0 ? walls[j] : j;
			walls[j] = temp > 0 ? temp : i;
			
		} // end for

		// index of next wall to consider from the walls array
		int nextWall = -1;
		
		// row,col coordinates of wall being considered for removal and clear
		// spaces adjacent to that wall
		int wallRow, wallCol, row1, col1, row2, col2;
		
		// disjoint set array tracks all initial m x n clear spaces while
		// internal walls are removed
		int[] set = makeset(height * width);
		
		// count number of unions to determine when maze is complete
		int unions = 0;
		
		// remove internal walls that separate clear spaces that are not
		// elements of the same set until m x n - 1 unions are made;
		// do not remove internal walls that separate clear spaces that are
		// already in the same set since that would incorrectly provide
		// multiple paths to solve the maze (i.e. a cycle)
		while (unions < set.Length - 1)
		{
			int wallIndex = walls[++nextWall];
			
			// calculating the col,row coordinates of a wall and its adjacent
			// clear spaces is dependent on the row, even or odd, in which the
			// wall is located; odd row walls have clear spaces to the left and
			// right while even row walls have spaces above and below
			if (wallIndex % wallPattern < width - 1) // odd rows
			{
				wallRow = ((wallIndex / wallPattern) * 2) + 1;
				wallCol = ((wallIndex % wallPattern) + 1) * 2;
				row1 = row2 = wallRow; // clear spots in same row as wall
				col1 = wallCol - 1; // clear spot directly left of the wall
				col2 = wallCol + 1; // clear spot directly right of the wall
				
			} // end if
			
			// even rows
			else
			{
				wallRow = ((wallIndex / wallPattern) * 2) + 2;
				wallCol = ((wallIndex % wallPattern) * 2) - (wallPattern - 2);
				row1 = wallRow - 1; // clear spot in row directly above the wall
				row2 = wallRow + 1; // clear spot in row directly below the wall
				col1 = col2 = wallCol; // clear spots in same column as wall
				
			} // end else
			
			// calculate indices for the 2 clear spaces adjacent to wall
			int index1 = ((row1 - 1) / 2) * width + ((col1 - 1) / 2);
			int index2 = ((row2 - 1) / 2) * width + ((col2 - 1) / 2);
			
			// if clear spots adjacent to the selected wall are not already in
			// the same set, remove the wall from the maze (overwrite with clear
			// spot) and perform a union on the adjacent clear spots;
			// note: the false case - the clear spots are already in the same
			// set via a previous union - requires no action
			if (findset(set, index1) != findset(set, index2))
			{
				maze[wallRow,wallCol] = mazePath;
				union(set, index1, index2);
				unions++;
				
			} // end if
			
		} // end while
		
		//if (DEBUG) MapTest.print(maze);
		return maze; // return completed maze
		
	} // end method

    // creates and returns a 1D array with all values set to -1 (a negative
    // value designates the root of a set) to indicate 'size' disjoint sets
    private static int[] makeset(int size)
    {
        int[] set = new int[size];
        //Arrays.fill(set, -1); // negative number indicates root/rank
        for (int i = 0; i < size; i++) {
            set[i] = -1;
        }
        return set;

    } // end method

    // returns the root index of the set/graph/subtree containing 'node';
    // performs path compression by pointing each node touched during a
    // find to its set root
    private static int findset(int[] set, int node)
    {
        if (set[node] < 0) // found set root (base case)
        {
            return node;

        } // end if

        else
        {
            set[node] = findset(set, set[node]);
            return set[node];

        } // end else

    } // end method

    // unions 2 sets/graphs/subtrees together
    // performs union-by-rank optimization by comparing each set's rank and
    // moving the smaller/shorter set under the larger/taller set
    // note that in this implementation, lower numbers are higher rank
    // i.e. -3 is higher rank that -2, so a set with rank -2 would be
    // placed under a set with rank -3
    private static void union(int[] set, int node1, int node2)
    {
        int root1 = findset(set, node1); // root of set containing node1
        int root2 = findset(set, node2); // root of set containing node2

        // union should never be called unless the nodes have already been
        // determined to be in disjoint sets using the findset method;
        // however, to be safe, see if union was called erroneously
        if (root1 == root2)
        {
            return; // already in the same set, do nothing

        } // end if

        if (set[root1] > set[root2]) // set1 lower rank than set2
        {
            set[root1] = root2; // move set1 under set2

        } // end if

        // set1 taller than set2, or both sets are equal rank
        else
        {
            if (set[root1] == set[root2]) // only when sets are equal rank
                set[root1]--; // change rank of receiving set

            set[root2] = root1; // move set2 under set1

        } // end else

    } // end method

    // given a 2D char array that contains a maze, determine the shortest path
    // from start to end and mark the solution on the map array using marker
    public static int markMap(char[,] maze, char[,] map, char marker, int cnt,
            int maxElements, int startRow, int startCol)
	{
		// including all walls/borders...
		int mazeHeight = maze.GetLength(0);//maze.length; // height of maze
        int mazeWidth = maze.GetLength(1);//maze[0].length; // width of maze
		
		// use queue to line up clear spots when ready to be processed
		Queue<int> q = new Queue<int>();
		
		// mark off each spot when seen so it isn't looked at again
		bool[,] seen = new bool[mazeHeight,mazeWidth];
		
		// array to keep track of solution path for backtracking
		int[] solution = makeset((mazeHeight - 2) * (mazeWidth - 2));
		
		// used to calculate row,col coordinates
		int columns = mazeWidth - 2; // don't include left/right borders
		
		// index of location marked mazeStart
		int startIndex = startRow * columns + startCol;
		
		// place the starting location in the queue
		q.Enqueue(startIndex);
		
		// process spots in the queue until a solution is found;
		// the queue shouldn't empty before the solution is found for most
		// proper mazes of decent height x width, but any maze that meets the
		// requirement of having no cycles will find the solution by the time
		// the final space is removed from the queue
		while (q.Count != 0)
		{
			int root = q.Dequeue(); // next spot in queue to process
			int row = root / columns + 1; // row of current spot
			int col = root % columns + 1; // column of current spot
			
			// maze end has been found; mark solution on maze field
			if (maze[row,col] == mazeEnd)
			{
				map[row,col] = marker;
				++cnt; // cnt isn't used for roadway so no harm in incrementing here
				int index = root;
				
				// backtrack and mark the path that got us to the end
				while (solution[index] > 0) // start located at index 0
				{
					// row,col of previous spot on the solution path
					int prevRow = solution[index] / columns + 1;
					int prevCol = solution[index] % columns + 1;
					if (marker == roadway)
						if (map[prevRow,prevCol] == water)
							map[prevRow,prevCol] = bridge; // mark path with bridge
						// remove this 'else if' to completely remove tunnels
						else if (map[prevRow,prevCol] == mountain)
							map[prevRow,prevCol] = tunnel; // mark path with tunnel
						else
							map[prevRow,prevCol] = roadway; // mark path
					else
					{
	                	if (cnt >= maxElements)
	                		return cnt;
						map[prevRow,prevCol] = marker; // mark path
						++cnt;
					}
					index = solution[index]; // next step backwards
					
				} // end while
				
				return cnt; // solution marked; exit method
				
			} // end if
			
			seen[row,col] = true; // don't consider this spot again
			
			// if not a wall and if not previously seen, add the clear spot
			// directly to the right of the current clear spot to the queue
			if (maze[row,col + 1] != mazeWall && !seen[row,col + 1])
			{
				int adjIndex = ((row - 1) * columns) + col;
				q.Enqueue(adjIndex); // queue index of adjacent clear spot
				solution[adjIndex] = root; // track how we got to this spot
				
			} // end if
			
			// if not a wall and if not previously seen, add the spot
			// directly below the current spot to the queue
			if (maze[row + 1,col] != mazeWall && !seen[row + 1,col])
			{
				int adjIndex = (row * columns) + (col - 1);
				q.Enqueue(adjIndex); // queue index of adjacent clear spot
				solution[adjIndex] = root; // track how we got to this spot
				
			} // end if
			
			// if not a wall and if not previously seen, add the spot
			// directly to the left of the current spot to the queue
			if (maze[row,col - 1] != mazeWall && !seen[row,col - 1])
			{
				int adjIndex = ((row - 1) * columns) + (col - 2);
				q.Enqueue(adjIndex); // queue index of adjacent clear spot
				solution[adjIndex] = root; // track how we got to this spot
				
			} // end if
			
			// if not a wall and if not previously seen, add the spot
			// directly above the current spot to the queue
			if (maze[row - 1,col] != mazeWall && !seen[row - 1,col])
			{
				int adjIndex = ((row - 2) * columns) + (col - 1);
				q.Enqueue(adjIndex); // queue index of adjacent clear spot
				solution[adjIndex] = root; // track how we got to this spot
				
			} // end if
			
		} // end while
		
		return cnt;
		
	} // end method

}

