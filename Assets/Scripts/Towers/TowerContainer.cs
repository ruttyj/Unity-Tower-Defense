using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerContainer : MonoBehaviour


{

    [SerializeField]
    GameObject base_tower;
    [SerializeField]
    GameObject fire_tower;
    [SerializeField]
    GameObject ice_tower;
    [SerializeField]
    GameObject nature_tower;

    [SerializeField]
    GameObject fire_tower_2;
    [SerializeField]
    GameObject ice_tower_2;
    [SerializeField]
    GameObject nature_tower_2;

    [SerializeField]
    GameObject fire_tower_3;
    [SerializeField]
    GameObject ice_tower_3;
    [SerializeField]
    GameObject nature_tower_3;

    public Dictionary<string, GameObject> towers;

    private void Awake()
    {
        towers = new Dictionary<string, GameObject>
        {
            { "tower_base", base_tower },
            { "tower_fire", fire_tower },
            { "tower_ice", ice_tower },
            { "tower_nature", nature_tower },

            {"tower_fire_2", fire_tower_2},
            {"tower_ice_2", ice_tower_2},
            {"tower_nature_2", nature_tower_2},

            {"tower_fire_3", fire_tower_3},
            {"tower_ice_3", ice_tower_3},
            {"tower_nature_3", nature_tower_3}
        };
    }


    public Dictionary<string, GameObject> GetTowerContainer()
    {
        return towers;
    }

}

