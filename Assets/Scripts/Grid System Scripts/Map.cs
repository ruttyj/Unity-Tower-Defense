using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;



public class Map : MonoBehaviour
{
    [SerializeField]
    public GameObject m_nodeParent;

    [SerializeField]
    public GameObject m_mapObject;

    [SerializeField]
    public int m_mapSeed = 652697955;


    [SerializeField]
    public bool m_useRandomMap;

    // Define odds
    [SerializeField]
    float m_exitDirectionWeight     = 0.0f; // says the priority of progressing toward the exit 
    [SerializeField]
    float m_sameDirectionWeight     = 5.0f; // will result in less direction changes -> simpler path
    [SerializeField]
    float m_windyWeight             = 5.0f; // says the likley hood of going horizontal
    [SerializeField]
    float m_randomDirectionWeight   = 3.0f; // filler weight to shift the odds towards randomness

    // Default Map size
    const int m_mapSizeX = 16;
    const int m_mapSizeY = 16;

    // Reference to nodes
    Node[,] m_nodeMap;
    List<PathSegment> m_pathSegments;

    MapTemplate m_currentMapTemplate;
    public InputField m_userSeedField;

    void Start()
    {
        // Build up a map of each nodes location - this will probably eventually initialize them 
        InitNodes();

        if (m_useRandomMap)
        {
            // Apply a random map template
            m_currentMapTemplate = GenerateRandomMapTemplate();
            m_mapSeed = m_currentMapTemplate.GetMapSeed();
        }
        else
        {
            m_currentMapTemplate = GenerateMapTemplateFromSeed(m_mapSeed);

            // Load map template from file
            //m_currentMapTemplate = LoadMapTemplateFromFile("defaultMap.json");
        }

        ApplyTemplateToMap(m_currentMapTemplate);
    }

    public void ResetMap()
    {
        Start();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            GenerateMap();
        }
    }

    public List<PathSegment> GetPathSegments()
    {
        return m_pathSegments;
    }

    public Node GetNode(int row, int col)
    {
        return m_nodeMap[row, col];
    }



    /**
     * Initialize Nodes
     * Gets the nodes on the screen ready to have settings applied
     */
    void InitNodes()
    {
        // Get nodes from container
        Node[] nodes = m_nodeParent.GetComponentsInChildren<Node>();

        Debug.Log("Node count: " + nodes.Length);

        // Create map of the nodes on the screen

        m_nodeMap = new Node[m_mapSizeX, m_mapSizeY];
        for (int i = 0; i < nodes.Length; ++i)
        {
            int row = (int)Mathf.Floor(i / m_mapSizeX);
            int col = i % m_mapSizeX;
            m_nodeMap[row, col] = nodes[i];
        }
    }




    /**
     * Apply Template To Map
     * Method responsible for modifying the nodes to reflect the template.
     */
    public void ApplyTemplateToMap(MapTemplate mapTemplate)
    {
        Color pathStartColor = Color.cyan;
        Color pathEndColor = Color.red;

        Color pathColor = new Color(49.0f / 255.0f, 27.0f / 255.0f, 5.0f / 255.0f, 1.0f); // brown;
        Color terrainColor = new Color(22.0f / 255.0f, 117.0f / 255.0f, 22.0f / 255.0f, 1.0f); // green;
        Color chosenColor = terrainColor;


        // Load path segments from template
        m_pathSegments = mapTemplate.GetPathSegments();


        int row;
        int col;

        // Everything is terrain
        for (row = 0; row < m_mapSizeX; ++row)
            for (col = 0; col < m_mapSizeY; ++col)
                if (m_nodeMap[row, col])
                    m_nodeMap[row, col].SetOriginalColor(terrainColor);

        /*
        // Render directly from path segments
        
        int globalPathTileCount = 0;
        int segmentLargestIndex = m_pathSegments.Count - 1;
        Debug.Log("segmetn count "+ (segmentLargestIndex+1));
        for (int s = 0; s <= segmentLargestIndex; ++s)
        {
            bool isLastSegment = s == segmentLargestIndex;

            PathSegment segment = m_pathSegments[s];

            int deltaX = segment.end.x - segment.start.x;
            int deltaY = segment.end.y - segment.start.y;

            int directionX = deltaX == 0 ? 0 : deltaX / Mathf.Abs(deltaX);
            int directionY = deltaY == 0 ? 0 : deltaY / Mathf.Abs(deltaY);


            // Set up 2D iteration 
            int dy = 0;
            int dx = 0;
            bool isLastIteration = false;
            bool isDone = false;
            while (!isDone)
            {
                // Set loop ending flag
                if (isLastIteration)
                    isDone = true;
                //----------------------
                // Execute for-loop contents with dx and dy

                // Get tile location
                row = segment.start.y + dy;
                col = segment.start.x + dx;

                // Select colour
                bool isFirstTile = globalPathTileCount == 0;
                bool isLastTile = isLastSegment && isLastIteration;
                if (isFirstTile)
                    chosenColor = pathStartColor;
                else if(isLastTile)
                    chosenColor = pathEndColor;
                else
                    chosenColor = pathColor;

                //Set node colour
                m_nodeMap[row, col].SetOriginalColor(chosenColor);

                // Increment tile could to tell first tile
                ++globalPathTileCount;
                //----------------------
                // Increment loop
                if (dx != deltaX)
                    dx += directionX;
                else if (dy != deltaY)
                    dy += directionY;
                // Detect going on last iteration
                isLastIteration = (dx == deltaX && dy == deltaY);
            }
        }
        */

        //*
        // Read directly from the map
        NodeTemplate template;

        for (row = 0; row < m_mapSizeY; ++row)
            for (col = 0; col < m_mapSizeX; ++col)
            {
                if (m_nodeMap[row, col])
                {
                    template = mapTemplate.GetNode(row, col);

                    chosenColor = terrainColor;
                    if (template.type == "path")
                        chosenColor = pathColor;

                    if (template.type == "start")
                        chosenColor = pathStartColor;

                    if (template.type == "end")
                        chosenColor = pathEndColor;

                    if (terrainColor != chosenColor)
                    {
                        m_nodeMap[row, col].SetOriginalColor(chosenColor);
                        m_nodeMap[row, col].SetNodeType(template.type);
                    }
                }
            }
         //*/
    }

    /**
     * Generates a random map template
     */
    MapTemplate GenerateRandomMapTemplate()
    {
        MapGenerator mapGenerator = new MapGenerator(m_mapSizeX, m_mapSizeY);

        // Set map Seed
        mapGenerator.SetSeed(Random.Range(0, System.Int32.MaxValue));

        // Set map generation values
        mapGenerator.SetExitDirectionWeight(m_exitDirectionWeight);
        mapGenerator.SetSameDirectionWeight(m_sameDirectionWeight);
        mapGenerator.SetWindyWeight(m_windyWeight);
        mapGenerator.SetRandomDirectionWeight(m_randomDirectionWeight);

        MapTemplate randomMap = mapGenerator.GenerateRandomMap();

        Debug.Log("Map Seed: " + randomMap.GetMapSeed());
        return randomMap;
    }


    /**
     * Generates a random map template
     */
    MapTemplate GenerateMapTemplateFromSeed(int seed)
    {
        MapGenerator mapGenerator = new MapGenerator(m_mapSizeX, m_mapSizeY);
        mapGenerator.SetSeed(seed);
        return mapGenerator.GenerateRandomMap();
    }

    /**
     * Retreives a map template from file
     * @WARNING in future may throw exception if file is invalid 
     */
    MapTemplate LoadMapTemplateFromFile(string fileName)
    {
        string mapFile = "Assets/Data/" + fileName;
        string dataAsJson = File.ReadAllText(mapFile);
        return new MapTemplate(dataAsJson);
    }


    /**
     * Destroys current map object and generates a new one
     */
    public void GenerateMap()
    {
        Destroy(m_mapObject);
        ResetMap();
    }

    public void GenerateMapFromSeed()
    {
        int result;
        string userInput = m_userSeedField.text;
        if (userInput != "")
        {
            try
            {
                result = System.Int32.Parse(userInput);
                Destroy(m_mapObject);
                InitNodes();
                m_currentMapTemplate = GenerateMapTemplateFromSeed(result);
                ApplyTemplateToMap(m_currentMapTemplate);
                ResetMap();
            }
            catch (System.FormatException)
            {
                System.Console.WriteLine($"Unable to parse '{userInput}'");
            }


        }

    }


}