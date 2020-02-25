using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class NodeTemplate
{
    public enum Wall {N,E,S,W};
    public static Wall IncrementWall(Wall wall, int delta)
    {
        // Make the N E S W a circular array - helpfull when rotating directions
        return (Wall)(((int)wall + delta) % 4);
    }

    public enum Pillar {NE, SE, SW, NW};
    public string type = "terrain";
    public bool isTowerPlacable = true;
    public bool isPlayablePerimeter = false;
    public bool isEnvPlacable = true;

    public bool[] hasWall = new bool[] { false, false, false, false };
    public bool[] hasPillar = new bool[] { false, false, false, false };
    public bool[] hasLamp = new bool[] { false, false, false, false };
}