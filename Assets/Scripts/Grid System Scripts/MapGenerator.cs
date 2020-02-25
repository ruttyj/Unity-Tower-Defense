using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class MapGenerator
{

    int m_mapSizeX;
    int m_mapSizeY;
    int m_pathLength = 1000;
    int m_mapBorder = 1;
    int m_endScene = 0;
    int m_bufferEnd = 4;
    int m_randomSeed = 42;

    float m_exitDirectionWeight = 0.0f; // says the priority of progressing toward the exit 
    float m_previousDirectionWeight = 5.0f; // will result in less direction changes -> simpler path
    float m_windyWeight = 5.0f; // says the likley hood of going horizontal
    float m_randomDirectionWeight = 3.0f; // filler weight to shift the odds towards randomness


    public MapGenerator(int rows, int cols)
    {
        m_randomSeed = Random.Range(0, System.Int32.MaxValue);
        Random.InitState(m_randomSeed);
        m_mapSizeX = rows;
        m_mapSizeY = cols;
    }

    public void SetSeed(int seed)
    {
        m_randomSeed = seed;
        Random.InitState(m_randomSeed);
    }

    public void SetExitDirectionWeight(float val)
    {
        if (val >= 0)
            m_exitDirectionWeight = val;
    }

    public void SetSameDirectionWeight(float val)
    {
        if (val >= 0)
            m_previousDirectionWeight = val;
    }

    public void SetWindyWeight(float val)
    {
        if (val >= 0)
            m_windyWeight = val;
    }

    public void SetRandomDirectionWeight(float val)
    {
        if (val >= 0)
            m_randomDirectionWeight = val;
    }


    List<Vector2Int> GenerateRandomPath()
    {
        // What is the delta in any coordinate before the path for be forced toward the exit
        int endGameBuffer = 2 + m_bufferEnd + m_endScene;


        // Define odds
        float exitDirectionWeight       = m_exitDirectionWeight;        // says the priority of progressing toward the exit 
        float previousDirectionWeight   = m_previousDirectionWeight;    // will result in less direction changes -> simpler path
        float windyWeight               = m_windyWeight;                // says the likley hood of going horizontal
        float randomDirectionWeight     = m_randomDirectionWeight;      // filler weight to shift the odds towards randomness

        // Build odd ranges 
        float oddsTotal = 0;
        SortedDictionary<string, Vector2> Odds = new SortedDictionary<string, Vector2>();
        Odds.Add("exitDirection",       new Vector2(oddsTotal, oddsTotal += exitDirectionWeight));
        Odds.Add("previousDirection",   new Vector2(oddsTotal, oddsTotal += previousDirectionWeight));
        Odds.Add("windy",               new Vector2(oddsTotal, oddsTotal += windyWeight));
        Odds.Add("randomDirection",     new Vector2(oddsTotal, oddsTotal += randomDirectionWeight));


        //##################################################################################


        Vector2Int start = new Vector2Int(
            BetterRandomRange(m_mapBorder + m_bufferEnd, m_mapSizeX - 1 - m_mapBorder - m_bufferEnd),
            0
        );
        Vector2Int end = new Vector2Int(
            BetterRandomRange(m_mapBorder + m_bufferEnd, m_mapSizeX - 1 - m_mapBorder - m_bufferEnd),
            m_mapSizeY - 1 - m_bufferEnd
        );
        

            

        // Define a map for where the pathing currently exists
        bool[,] pathExistsOnNode = new bool[m_mapSizeX, m_mapSizeY];
        for (int x=0; x < m_mapSizeX; ++x)
            for (int y=0; y < m_mapSizeY; ++y)
                pathExistsOnNode[x, y] = false;

        // Initialize path
        List<Vector2Int> pathNodes = new List<Vector2Int>();



        // Add starting point 
        Vector2Int next = start;
        Vector2Int current = next;
        pathNodes.Add(current);
        pathExistsOnNode[current.x, current.y] = true;
        

        Vector2Int previousDirection = new Vector2Int(0, 1);
        Vector2Int moveDirection = new Vector2Int(0, 1);

        while (current != end && pathNodes.Count < m_pathLength)
        {
            // Getting toward the end... don't mess around
            if (current.y + endGameBuffer >= end.y)
            {
                if (current.y + endGameBuffer-2 >= end.y) {
                    // At the very end, align then straight out
                    moveDirection = AlignWithExitDirection(current, end);
                }
                else
                {
                    // Move forward to give little space between previous pathing
                    moveDirection = ApproachExitDirection(current, end);
                }
            }
            else
            {
                // Path can wander a bit
                List<Vector2Int> possibleDirections = GetAvailableDirections(current, end, ref pathExistsOnNode);

                // Analyze the possible directions 
                // Seperate into notible directions we would like to go and other possabilities for randoness
                bool previousDirectionPossible = false;
                bool towardExitDirectionPossible = false;
                bool horizontalDirectionsPossible = false;
                List<Vector2Int> horizontalDirections = new List<Vector2Int>();
                for (int i = 0; i < possibleDirections.Count; ++i)
                {
                    // Can move toward exit?
                    if (possibleDirections[i] == moveDirection)
                        towardExitDirectionPossible = true;

                    // Can keep current direction?
                    if (possibleDirections[i] == previousDirection)
                        previousDirectionPossible = true;

                    // Can move horizontally?
                    if (possibleDirections[i].y == 0)
                    {
                        horizontalDirectionsPossible = true;
                        horizontalDirections.Add(possibleDirections[i]);
                    }
                }



                // Decide direction to go randomly
                bool isDirectionApplied = false;
                string directionMode = "exitDirection"; // default go to the exit
                float randomNumber = Random.Range(0.0f, oddsTotal);
                foreach (KeyValuePair<string, Vector2> entry in Odds)
                {
                    if (entry.Value.x <= randomNumber && randomNumber <= entry.Value.y)
                    {
                        directionMode = entry.Key;
                        break;
                    }
                }


                // Choose from possible directions

                // Windy path
                if (horizontalDirectionsPossible && directionMode == "windy")
                {
                    isDirectionApplied = true;
                    moveDirection = horizontalDirections[BetterRandomRange(0, horizontalDirections.Count - 1)];
                }

                // Move toward exit
                if (towardExitDirectionPossible && directionMode == "exitDirection")
                {
                    isDirectionApplied = true;
                    moveDirection = ApproachExitDirection(current, end);
                }

                // Keep current direction
                if (previousDirectionPossible && directionMode == "previousDirection")
                {
                    isDirectionApplied = true;
                    moveDirection = previousDirection;
                }

                // Pick a random direction form what is allowed
                if (!isDirectionApplied)
                {
                    if (possibleDirections.Count > 0)
                        moveDirection = possibleDirections[BetterRandomRange(0, possibleDirections.Count - 1)];
                    else
                    {   // Something messed up bad!
                        Debug.Log("No possabilities exist");
                        break;
                    }
                }
            }
            
            
            // Set tile
            next = current + moveDirection;
            current = next;
            pathNodes.Add(current);
            pathExistsOnNode[current.x, current.y] = true;
            previousDirection = moveDirection;

        }
        return pathNodes;
    }


    int BetterRandomRange(int min, int max)
    {
        return Mathf.RoundToInt(Random.Range(min * 100.0f, max * 100.0f) / 100.0f);
    }


    List<Vector2Int> GetAvailableDirections(Vector2Int current, Vector2Int end, ref bool[,] pathExistsOnNode)
    {

        List<Vector2Int> possibleDirections = new List<Vector2Int>();
        possibleDirections.Add(Vector2Int.right);
        possibleDirections.Add(Vector2Int.left);
        possibleDirections.Add(Vector2Int.up);


        List<Vector2Int> availableDirections = new List<Vector2Int>();

        bool isNotAlreadyPath;
        bool isTooCloseToAnother;
        bool isPossibleToReachEnd = true; // @TODO
        Vector2Int possiblePosition;

        for (int i = 0; i < possibleDirections.Count; ++i)
        {
            // If that direction is still on the map
            possiblePosition = current + possibleDirections[i];
            if (IsPositionInBounds(possiblePosition))
            {
                // Check if the tile is directly next to another part of the path
                isTooCloseToAnother = CheckIfTooCloseToPath(current, possibleDirections[i], ref pathExistsOnNode);
                if (!isTooCloseToAnother && isPossibleToReachEnd) {
                    availableDirections.Add(possibleDirections[i]);
                }
            }
        }

        return availableDirections;
    }

    bool IsPositionInBounds(Vector2Int pos)
    {
        return pos.x >= m_mapBorder && pos.x < m_mapSizeX-m_mapBorder
           &&  pos.y >= m_mapBorder && pos.y < m_mapSizeY-m_mapBorder;
    }


    bool CheckIfTooCloseToPath(Vector2Int current, Vector2Int direction, ref bool[,] pathExistsOnNode)
    {
        // Is already a path - dont go there
        Vector2Int possiblePosition = current + direction;
        if (pathExistsOnNode[possiblePosition.x, possiblePosition.y])
            return true;


        // Is it too close to a part of the path?
        Vector2Int temp;
        Vector2Int vVector = new Vector2Int(0, 1);
        Vector2Int hVector = new Vector2Int(1, 0);
        List<Vector2Int> checkPositions = new List<Vector2Int>();

        // Check the tile on either side of the proposed tile perpendicular to the move direction
        if (direction.y == 0) { 
            // moving horizontally
            temp = possiblePosition + vVector;
            if(IsPositionInBounds(temp))
                checkPositions.Add(temp);

            temp = possiblePosition - vVector;
            if (IsPositionInBounds(temp))
                checkPositions.Add(temp);
        }
        else
        {
            temp = possiblePosition + hVector;
            if (IsPositionInBounds(temp))
                checkPositions.Add(temp);

            temp = possiblePosition - hVector;
            if (IsPositionInBounds(temp))
                checkPositions.Add(temp);
        }
        for (int i = 0; i < checkPositions.Count; ++i)
        {
            if (pathExistsOnNode[checkPositions[i].x, checkPositions[i].y])
                return true;
        }
        return false;
    }

    
    // Moves towards exit then adjusts allignment
    Vector2Int ApproachExitDirection(Vector2Int current, Vector2Int end)
    {
        Vector2Int exitDirection = new Vector2Int(0, 0);
        if(end.y > current.y)
            exitDirection.y = 1;
        else if (end.y < current.y)
            exitDirection.y = -1;
        else if(end.x > current.x)
            exitDirection.x = 1;
        else if (end.x < current.x)
            exitDirection.x = -1;
        return exitDirection;
    }


    Vector2Int AlignWithExitDirection(Vector2Int current, Vector2Int end)
    {
        Vector2Int exitDirection = new Vector2Int(0, 0);
        if (end.x > current.x)
            exitDirection.x = 1;
        else if (end.x < current.x)
            exitDirection.x = -1;
        else if (end.y > current.y)
            exitDirection.y = 1;
        else if (end.y < current.y)
            exitDirection.y = -1;
        return exitDirection;
    }


    // Returns list of movements which could be done to go towards exit 
    List<Vector2Int> MovesTowardsExit(Vector2Int current, Vector2Int end)
    {
        List<Vector2Int> exitMoves = new List<Vector2Int>();
        if (end.y > current.y)
            exitMoves.Add(new Vector2Int(0, 1));
        if (end.y < current.y)
            exitMoves.Add(new Vector2Int(0, -1));
        if (end.x > current.x)
            exitMoves.Add(new Vector2Int(1, 0));
        if (end.x < current.x)
            exitMoves.Add(new Vector2Int(-1, 0));
        return exitMoves;
    }

    bool IsMoveSideways(Vector2Int direction)
    {
        return direction.x != 0;
    }



    public MapTemplate GenerateRandomMap()
    {

        List<Vector2Int> pathList = GenerateRandomPath();

        // Initialize map
        MapTemplate generatedMap = new MapTemplate(m_mapSizeY, m_mapSizeX);

        // Attach the seed used to generate it
        generatedMap.SetMapSeed(m_randomSeed);

        ApplyPathToMap(pathList, generatedMap);

        return generatedMap;
    }





    void ApplyPathToMap(List<Vector2Int> pathList, MapTemplate template)
    {


        // Path segment building 
        List<PathSegment> pathSegments = new List<PathSegment>();
        Vector2Int previousCoord = new Vector2Int(0, 0);
        PathSegment currentSegment = new PathSegment(previousCoord);
        Vector2Int previousDirection = new Vector2Int(0, 0);
        Vector2Int currentDirection = new Vector2Int(0, 0);     // used to see if we still going the same direction


        string currentAxis = "x";
        string previousAxis = currentAxis;


        List<MapTemplate.WallCoord> leftWall = new List<MapTemplate.WallCoord>();
        List<MapTemplate.WallCoord> rightWall = new List<MapTemplate.WallCoord>();

        NodeTemplate.Wall currentLeftWall = NodeTemplate.Wall.N;
        NodeTemplate.Wall currentRightWall = NodeTemplate.Wall.S;

        NodeTemplate.Wall previousLeftWall = currentLeftWall;
        NodeTemplate.Wall previousRightWall = currentRightWall;


        // Shorten the cardinal directions 
        NodeTemplate.Wall N = NodeTemplate.Wall.N;
        NodeTemplate.Wall E = NodeTemplate.Wall.E;
        NodeTemplate.Wall S = NodeTemplate.Wall.S;
        NodeTemplate.Wall W = NodeTemplate.Wall.W;

        // Translate path onto map 
        var arr = pathList.ToArray();
        NodeTemplate node;
        int segmentC = 1;

        bool isLastTile = false;
        bool isFirstTile = false;
        bool isSecondTile = false;
        bool updateWallSides = false;
        for (int i = 0; i < arr.Length; ++i)
        {
            updateWallSides = false;
            isLastTile = i == arr.Length - 1;
            isFirstTile = i == 0;
            isSecondTile = i == 1;

            Vector2Int currentCoord = arr[i];



            // #############################################################

            //                      ASSIGN TILE DATA

            // #############################################################
            // Assign node type to the template
            node = template.GetNode(currentCoord.y, currentCoord.x);
            node.isTowerPlacable = false;
            if (i == 0)
                node.type = "start";
            else if (i == arr.Length - 1)
            {
                node.type = "end";


                for(int p=-2; p<3; ++p)
                {
                    for(int j=-2; j<4; ++j)
                    {
                        Vector2Int tempCoord = currentCoord + new Vector2Int(p, j);
                        NodeTemplate temp = template.GetNode(tempCoord.y, tempCoord.x);

                        if(temp != null)
                        {
                            temp.isEnvPlacable = false;
                            temp.isTowerPlacable = false;
                        }
                    }
                }

            }
            else
                node.type = "path";



            // #############################################################

            //                      BUILD PATH SEGMENTS

            // #############################################################

            // Is first node?
            if (isFirstTile)
            {
                // Start the first segment
                currentSegment.start = currentCoord;

            } else {
                // Is second node
                if (isSecondTile)
                {
                    // Establish the movement direction
                    currentDirection = currentCoord - previousCoord;
                    previousDirection = currentDirection;

                    currentSegment.end = currentCoord;
                    currentAxis = previousCoord.x != currentCoord.x ? "y" : "x";
                    currentSegment.SetAxis(currentAxis);

                    // Track wall
                    leftWall.Add(new MapTemplate.WallCoord(previousLeftWall, previousCoord));
                    rightWall.Add(new MapTemplate.WallCoord(previousRightWall, previousCoord));
                }
                else
                {
                    // Is going same direction?
                    currentDirection = currentCoord - previousCoord;
                    if (currentDirection == previousDirection)
                    {
                        // Increment segment
                        currentSegment.end = currentCoord;

                        // Track wall
                        leftWall.Add(new MapTemplate.WallCoord(previousLeftWall, previousCoord));
                        rightWall.Add(new MapTemplate.WallCoord(previousRightWall, previousCoord));
                    }
                    // Is changing direction?
                    else
                    {
                        ++segmentC;
                        // Save segment
                        pathSegments.Add(currentSegment);
                        // Start a new segment from the previous position to the current 
                        currentSegment = new PathSegment(previousCoord, currentCoord);

                        currentAxis = previousCoord.x != currentCoord.x ? "y" : "x";
                        currentSegment.SetAxis(currentAxis);

                        updateWallSides = true;

                    }
                }
            }

            // Was this the last segment?
            if (isLastTile)
            {
                // Add it to the list
                pathSegments.Add(currentSegment);
                updateWallSides = true;
            }








            // #############################################################

            //                  TOGGLE WALLS AND PILLARS

            // #############################################################

            // Reminder Y represent the incrementing in the column ->
            // Reminder X represent the incrementing in the Row ^

            // Handle start and end 
            if (isFirstTile || isLastTile)
            {
                NodeTemplate currentNodeTemplate = template.GetNode(currentCoord.y, currentCoord.x);
                // Vertical
                currentNodeTemplate.hasWall[(int)NodeTemplate.Wall.N] = true;
                currentNodeTemplate.hasWall[(int)NodeTemplate.Wall.S] = true;

                //Excluse regular pillars at the ends of the path
                if (isFirstTile)
                {
                    currentNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                    currentNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.SE] = true;
                }
            }



            if (previousDirection == currentDirection)
            {
                NodeTemplate previousNodeTemplate = template.GetNode(previousCoord.y, previousCoord.x);
                if (previousDirection.y == 0)
                {
                    // Vertical
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.E] = true;
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.W] = true;


                    // We wont put regular pillars at the extreams of the path... we got fancy ones for that
                    if (!isLastTile)
                    {
                        previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                        previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NW] = true;
                    }
                }
                else
                {
                    // Horizontal
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.N] = true;
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.S] = true;

                    if (!isLastTile)
                    {
                        previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.SE] = true;
                        previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                    }
                }
            }
            else if (i != 1)
            {
                NodeTemplate previousNodeTemplate = template.GetNode(previousCoord.y, previousCoord.x);

                // Cap off the previous direction

                // Handle Corner

                // Vertically
                if (-1 * previousDirection.x == 1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.N] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                }
                if (-1 * previousDirection.x == -1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.S] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.SE] = true;
                }

                // Horizontally
                if (-1 * previousDirection.y == 1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.W] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NW] = true;
                }
                if (-1 * previousDirection.y == -1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.E] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                }



                // Call off the beginning of the new direction
                // Handle Corner
                if (-1 * currentDirection.x == 1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.S] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.SE] = true;
                }
                if (-1 * currentDirection.x == -1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.N] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                }
                if (-1 * currentDirection.y == 1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.E] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                }
                if (-1 * currentDirection.y == -1)
                {
                    previousNodeTemplate.hasWall[(int)NodeTemplate.Wall.W] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NW] = true;
                }



                // Pillar edge case
                if (-1 * previousDirection.x == -1 && currentDirection.x == 0) // Down to horizontal
                {
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.SW] = true;
                    previousNodeTemplate.hasPillar[(int)NodeTemplate.Pillar.NE] = true;
                }
            }


            // END TOGGLE WALLS AND PILLARS ________________________________







            // #############################################################

            //                      KEEP TRACK OF WALLS

            // #############################################################
            if (updateWallSides)
            {
                if (isLastTile)
                {
                    previousAxis = currentAxis;

                    previousLeftWall = currentLeftWall;
                    previousRightWall = currentRightWall;

                    previousCoord = currentCoord;
                    previousDirection = currentDirection;
                }


                // Translate the vectors to N,E,S,W
                NodeTemplate.Wall previousTowardWall = NodeTemplate.Wall.E;
                NodeTemplate.Wall currentTowardWall = NodeTemplate.Wall.E;
                if (previousDirection.y == 0)
                    previousTowardWall = (previousDirection.x < 0) ? N : S;
                else
                    previousTowardWall = (previousDirection.y < 0) ? W : E;

                if (currentDirection.y == 0)
                    currentTowardWall = (currentDirection.x < 0) ? N : S;
                else
                    currentTowardWall = (currentDirection.y < 0) ? W : E;



                /*
                Hashtable dirMap = new Hashtable();
                dirMap.Add(NodeTemplate.Wall.N, "North");
                dirMap.Add(NodeTemplate.Wall.E, "East");
                dirMap.Add(NodeTemplate.Wall.S, "South");
                dirMap.Add(NodeTemplate.Wall.W, "West");

                Debug.Log("Dir: (" + previousDirection.x + "," + previousDirection.y + ") (" + currentDirection.x + ", " + currentDirection.y + ")");
                Debug.Log("COO: (" + (string)dirMap[currentTowardWall] + ")");
                */

                bool turnedLeft = false;
                // Right -> up
                if (previousTowardWall == E && currentTowardWall == N)
                    turnedLeft = true;

                // Down -> Right
                if (previousTowardWall == S && currentTowardWall == E)
                    turnedLeft = true;

                // Left -> Down
                if (previousTowardWall == W && currentTowardWall == S)
                    turnedLeft = true;

                // UP -> Left
                if (previousTowardWall == N && currentTowardWall == W)
                    turnedLeft = true;



                // Rotate Coordiantes (N, E, S, W = 0, 1, 2, 3) % 4
                // Base cases are relative to that respecive side being North... IE = 0 there for all end directions can be transformed into offsets
                if (turnedLeft)
                {
                    // Based on LEFTWALL = N
                    // Relative to left wall
                    rightWall.Add(new MapTemplate.WallCoord(NodeTemplate.IncrementWall(previousLeftWall, 2), previousCoord)); // Add S wall
                    rightWall.Add(new MapTemplate.WallCoord(NodeTemplate.IncrementWall(previousLeftWall, 1), previousCoord)); // Add E wall
                    currentLeftWall = NodeTemplate.IncrementWall(previousLeftWall, 3);  // W
                    currentRightWall = NodeTemplate.IncrementWall(previousLeftWall, 1); // E
                }
                else
                {
                    // Based on RIGHTWALL = N
                    // Relative to right wall
                    leftWall.Add(new MapTemplate.WallCoord(NodeTemplate.IncrementWall(previousRightWall, 2), previousCoord)); // Add S wall
                    leftWall.Add(new MapTemplate.WallCoord(NodeTemplate.IncrementWall(previousRightWall, 3), previousCoord)); // Add W wall
                    currentLeftWall = NodeTemplate.IncrementWall(previousRightWall, 3); // W
                    currentRightWall = NodeTemplate.IncrementWall(previousRightWall, 1);// E
                }
            }
            // END KEEP TRACK OF WALLS _____________________________________








            previousAxis = currentAxis;

            previousLeftWall = currentLeftWall;
            previousRightWall = currentRightWall;

            previousCoord = currentCoord;
            previousDirection = currentDirection;
            //  END BUILD PATH SEGMENTS ____________________________________


        }// end for



        // Indicate the parimiter on the map
        for (int row = 0; row < m_mapSizeY; ++row) 
        {
            template.GetNode(row, 0).isPlayablePerimeter = true;
            template.GetNode(row, m_mapSizeX-1).isPlayablePerimeter = true;
        }

        for (int col = 0; col < m_mapSizeX; ++col)
        {
            template.GetNode(0, col).isPlayablePerimeter = true;
            template.GetNode(m_mapSizeY-1, col).isPlayablePerimeter = true;
        }


        // ADD LAMPS
        // Even on left side
        for (int i = 0; i < leftWall.Count; ++i)
        {
            if (i % 4 == 0)
            {
                MapTemplate.WallCoord walCoord = leftWall[i];
                NodeTemplate nodeTemplate = template.GetNode(walCoord.coord.y, walCoord.coord.x);

                NodeTemplate.Wall wall = walCoord.wall;
                //nodeTemplate.hasWall[(int)wall] = false;

                nodeTemplate.hasLamp[(int)wall] = true;
            }
        }

        // Off on right side
        for (int i = 0; i < rightWall.Count; ++i)
        {
            if ((i+2) % 4 == 0)
            {
                MapTemplate.WallCoord walCoord = rightWall[i];
                NodeTemplate nodeTemplate = template.GetNode(walCoord.coord.y, walCoord.coord.x);

                NodeTemplate.Wall wall = walCoord.wall;
                //nodeTemplate.hasWall[(int)wall] = false;

                nodeTemplate.hasLamp[(((int)wall)%4)] = true;
            }
        }


        template.SetLeftWall(leftWall);
        template.SetRightWall(rightWall);
        template.SetPathSegments(pathSegments);
    }


}
