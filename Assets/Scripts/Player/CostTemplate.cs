using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class is to keep a static definition of all costs relavant to the game.
public class CostTemplate : MonoBehaviour
{
    // Tower Costs:
    public static Dictionary<string, int> cost_dictionary = new Dictionary<string, int>()
    {
        // towers
        { "tower_base", 50 },

        // upgrades
        { "tower_fire", 100 },
        { "tower_ice", 100 },
        { "tower_nature", 100 },

        {"tower_fire_2", 200},
        {"tower_ice_2", 200},
        {"tower_nature_2", 200},

        {"tower_fire_3", 300},
        {"tower_ice_3", 300},
        {"tower_nature_3", 300}



    };

    /*
     * public method to get cost associated with specific item type
     * params:
     *  item is the name of the object (ie. "base_tower"
     */
    public static int GetCost(string item)
    {
        return cost_dictionary[item];
    }
}
