using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{
    [SerializeField]
    TowerSpawning towerSpawner;
    [SerializeField]
    TextMeshProUGUI guiStatusText;


    public void PlaceTower()
    {

        //Can a base tower be purchased
        //if true it buys it right away.
        //Checks if already purchased

        if (!towerSpawner.GetSpawnAble())
        {
            if (GameController.Purchase(CostTemplate.GetCost("tower_base")))
            {
                towerSpawner.SetSpawnable();
            }
            else
            {
                //GUI Status Text
                guiStatusText.text = "You don't have enough gold to purchase that.";
            }
        }
        else
        {
            guiStatusText.text = "A tower is already purchased and ready to be placed.";
        }

    }
}
