using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LitJson;


public class MapTemplate
{

    List<MapTemplate.WallCoord> m_leftWall = null;
    List<MapTemplate.WallCoord> m_rightWall = null;

    NodeTemplate[,] m_nodeMap;
    List<PathSegment> m_pathSegments;
    int m_mapSeed = 0; // Default unspecified


    public struct WallCoord
    {
        public NodeTemplate.Wall wall;
        public Vector2Int coord;
        public WallCoord(NodeTemplate.Wall wall, Vector2Int coord)
        {
            this.wall = wall;
            this.coord = new Vector2Int(coord.x, coord.y);
        }
        
    }
    

    public MapTemplate(string json)
    {
        LoadFromJson(json);
    }

    public MapTemplate(int rows, int cols)
    {
        InitGrid(rows, cols);
    }

    public void SetPathSegments(List<PathSegment> value)
    {
        m_pathSegments = value;
    }

    public List<PathSegment> GetPathSegments()
    {
        return m_pathSegments;
    }

    public void SetMapSeed(int seed)
    {
        m_mapSeed = seed;
    }

    public int GetMapSeed()
    {
        return m_mapSeed;
    }


    void InitGrid(int rows, int cols)
    {
        m_nodeMap = new NodeTemplate[rows, cols];
        for (int row = 0; row < rows; ++row)
            for (int col = 0; col < cols; ++col)
            {
                m_nodeMap[row, col] = new NodeTemplate();
            }
    }

    public void LoadFromJson(string json)
    {
        /*
        JsonData data = JsonMapper.ToObject(json);

        InitGrid((int)data["width"], (int)data["height"]);

        // Take the data from the json file and put in 
        for (int row = 0; row < data["template"].Count; ++row)
        {
            for (int col = 0; col < data["template"][row].Count; ++col)
            {
                GetNode(row, col).type = (string)data["template"][row][col]["type"];
            }
        }
        */
    }

    public void SerializeToJson()
    {
        /*
         * @TODO
         */
    }


    public NodeTemplate GetNode(int row, int col)
    {
        return m_nodeMap[row, col];
    }

    public void SetNode(int row, int col, NodeTemplate nodeTemplate)
    {
        m_nodeMap[row, col] = nodeTemplate;
    }



    public void SetLeftWall(List<MapTemplate.WallCoord> wall)
    {
        m_leftWall = wall;
    }
    public List<MapTemplate.WallCoord> GetLeftWall()
    {
        return m_leftWall;
    }



    public void SetRightWall(List<MapTemplate.WallCoord> wall)
    {
        m_rightWall = wall;
    }

    public List<MapTemplate.WallCoord> GetRightWall()
    {
        return m_rightWall;
    }

}
