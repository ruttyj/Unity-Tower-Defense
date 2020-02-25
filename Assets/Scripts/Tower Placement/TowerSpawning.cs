using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class TowerSpawning : MonoBehaviour
{
    [SerializeField]
    GameObject tower;

    [SerializeField]
    LayerMask nodeMask;

    [SerializeField]
    TextMeshProUGUI guiStatusText;

    [SerializeField] bool spawnable = false;

    // Update is called once per frame
    void Update()
    {
        if (!IsPaused() && spawnable)
        {
            SpawnThatTower();
        }
    }

    /*This Works, Y Position is hardcoded
    could possibly change that later*/
    public void SpawnThatTower()
    {
        if (Input.GetMouseButtonDown(0))
        {

            //Test for UI element
            if (EventSystem.current.IsPointerOverGameObject())
                return;


            //send a Ray to the Node
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            /*  
                Check if the Node hits something, store Hit data in RacastHit
                LayerMask is the the mask that the nodes are in, only registers hits
                on that layer
            */
            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, nodeMask))
            {
                NodeV2 node = hit.transform.parent.gameObject.GetComponent<NodeV2>();
                //check if there is a tower already placed
                if (!IsThereATower(node) && node.GetIsTowerPlacable())
                {
                    //Grab pos of the hit object add 0.5 to y value so it's not inside
                    Vector3 position = node.transform.position + node.GetEnvOffset();
                    node.DeleteEnvObject();
                    GameObject temp = Instantiate(tower, position, Quaternion.identity) as GameObject;
                    node.SetTowerObject(temp);

                    //Mark the node as having a placed tower
                    node.towerPlaced = true;

                    //Make tower child to Node
                    temp.transform.parent = hit.transform;

                    spawnable = false;
                }
                else
                {
                    guiStatusText.text = "A tower is not placeable here.";
                }
            }
        }
    }

    public void AssignTower(GameObject tower_to_place)
    {
        tower = tower_to_place;
    }

    /*
     *   Check if there is a tower already placed on the node
    */
    bool IsThereATower(NodeV2 node)
    {
        return node.towerPlaced && node.GetIsTowerPlacable();
    }

    //
    //GUI bottom left button helper methods
    //
    public void SetSpawnable()
    {
        spawnable = true;
    }

    public bool GetSpawnAble()
    {
        return spawnable;
    }
    //
    //



    // ==================================================

    //                  IS PAUSED?

    // ==================================================
    PauseController pauseController = null;

    PauseController GetPauseController()
    {
        if (pauseController == null)
        {
            GameObject obj = GameObject.FindGameObjectWithTag("PauseController");
            if (obj != null)
                pauseController = obj.GetComponent<PauseController>();
        }
        return pauseController;
    }

    bool IsPaused()
    {
        bool isPaused = false;
        PauseController pc = GetPauseController();
        if (pc != null)
            isPaused = pc.IsPaused();
        return isPaused;
    }
}