using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// class with static information about revenue for each level and enemy type.
public class RevenueTemplate : MonoBehaviour
{
    //TODO:
    public static Dictionary<string, int> revenueDictMonsters = new Dictionary<string, int>()
    {
        //revenu per enemy death by types
        {"normalSmall", 5},
        {"fireSmall", 10},
        {"natureSmall", 20},
        {"iceSmall", 30},
        {"arcaneSmall", 50},
        {"normal", 50},
        {"fire", 70},
        {"nature", 90},
        {"ice", 80},
    };

    public static Dictionary<int, int> revenueDictWaves = new Dictionary<int, int>()
    {
        // { waveLevel, rewardAmt }
        {1, 10 },
        {2, 10 },
        {3, 10 },
        {4, 10 },
        {5, 50 },
        {6, 30 },
        {7, 30 },
        {8, 30 },
        {9, 30 },
        {10, 80 },
        {11, 70 },
        {12, 70 },
        {13, 70 },
        {14, 70 },
        {15, 100 },
        {16, 90 },
        {17, 90 },
        {18, 90 },
        {19, 90 },
        {20, 120 },
        {21, 100 },
        {22, 100 },
        {23, 100 },
        {24, 100 },
        {25, 100 },
        {26, 100 },
        {27, 100 },
        {28, 100 },
        {29, 100 },
        {30, 100 },
        {31, 100 },
        {32, 100 },
        {33, 100 },
        {34, 100 },
        {35, 100 },
        {36, 100 },
        {37, 100 },
        {38, 100 },
        {39, 100 },
        {40, 100 },
        {41, 100 },
        {42, 200 },
        {43, 230 },
        {44, 230 },
        {45, 230 },
        {46, 250 },
        {47, 300 },
        {48, 300 },
        {49, 300 },
        {50, 300 }
    };
}
