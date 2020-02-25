using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TowerRightClickMenu : MonoBehaviour
{

    [SerializeField]
    GameObject rightClickMenu;

    //GUI Menu Dropdown
    private Dropdown m_Dropdown;
    private GameObject selected_tower;

    //GUI Status Text
    [SerializeField]
    TextMeshProUGUI guiStatusText;


    [SerializeField]
    GameObject base_tower;
    [SerializeField]
    GameObject fire_tower;
    [SerializeField]
    GameObject ice_tower;
    [SerializeField]
    GameObject nature_tower;

    //Used to ignore the massive round collider the towers have when accessing the menu
    //Make sure to ignore one layer Mask you need to add the tilda (~) to invert the mask
    [SerializeField]
    LayerMask towerSphereCollider;

    [SerializeField]
    Wallet playerWallet;

    // Start is called before the first frame update
    void Start()
    {
        m_Dropdown = rightClickMenu.GetComponentInChildren<Dropdown>();

        m_Dropdown.onValueChanged.AddListener(delegate
        {
            ChangeTowerColor(m_Dropdown, selected_tower);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit, Mathf.Infinity, ~towerSphereCollider))
            {
                if (hit.transform.gameObject.CompareTag("Tower") || hit.transform.gameObject.CompareTag("FireTower") || hit.transform.gameObject.CompareTag("IceTower") || hit.transform.gameObject.CompareTag("NatureTower"))
                {

                    //Attach selected_tower to tower that will potentially change
                    //Needed to change to parent GameObject
                    selected_tower = hit.transform.parent.gameObject;

                    if (!rightClickMenu.activeSelf)
                    {
                        rightClickMenu.transform.position = Input.mousePosition;
                        rightClickMenu.SetActive(true);
                    }

                }
                else
                {
                    selected_tower = null;
                }
            }


        }
    }

    void ChangeTowerColor(Dropdown change, GameObject selected_tower)
    {
        if (change.value == 1)
        {
            //Before changing to fire check if there is enough money in wallet
            //Call tower right click menu script
            //Hard coding values now
            if (GameController.Purchase(CostTemplate.GetCost("tower_fire")))
            {
                ChangeTower(selected_tower, "fire");
            }
            else
            {
                guiStatusText.text = "Not enough money to upgrade to Fire";
            }
        }
        else if (change.value == 2)
        {
            if (GameController.Purchase(CostTemplate.GetCost("tower_ice")))
            {
                ChangeTower(selected_tower, "ice");

            }
            else
            {
                guiStatusText.text = "Not enough money to upgrade to Ice";
            }
        }
        else if (change.value == 3)
        {
            if (GameController.Purchase(CostTemplate.GetCost("tower_nature")))
            {
                ChangeTower(selected_tower, "nature");
            }
            else
            {
                guiStatusText.text = "Not enough money to upgrade to Nature";
            }
        }
        else if(change.value == 4)
        {
            //Sell Tower
            SellTower(selected_tower);
            guiStatusText.text = "Tower sold.";

        }
        else if(change.value == 5)
        {
            //Check if tower is a tower_base;

            if (selected_tower.GetComponent<TowerController>().CompareTag("Tower"))
            {
                guiStatusText.text = "Cannot downgrade base tower.";
            }
            else
            {
                GameController.SellTowerGetMoney(100);
                ChangeTower(selected_tower, "Base");
                guiStatusText.text = "Tower downgraded.";

            }
        }
        else
        {
            //Default action maybe?
        }

    }

    public void ChangeTower(GameObject tower, string type)
    {

        NodeV2 node = tower.transform.parent.transform.gameObject.GetComponent<NodeV2>();
        node.RemoveTower();
        Vector3 pos = node.GetTowerObject().transform.position;
        GameObject newTower = null;

        switch (type)
        {
            case "fire":
                newTower = Instantiate(fire_tower, pos, Quaternion.identity) as GameObject;
                break;
            case "ice":
                newTower = Instantiate(ice_tower, pos, Quaternion.identity) as GameObject;
                break;
            case "nature":
                newTower = Instantiate(nature_tower, pos, Quaternion.identity) as GameObject;
                break;
            case "base":
                newTower = Instantiate(base_tower, pos, Quaternion.identity) as GameObject;
                break;

        }

        if (!newTower.Equals(null))
        {
            newTower.transform.parent = node.transform;
        }

        node.SetTowerObject(newTower);

    }

    public void SellTower(GameObject selectedTower)
    {
        //Set the Node that the tower was previously placed on to false
        selectedTower.transform.parent.transform.gameObject.GetComponent<Node>().towerPlaced = false;

        string towerType = selectedTower.GetComponent<TowerController>().GetTowerType();

        int amountReturned = CostTemplate.cost_dictionary[towerType];

        GameController.SellTowerGetMoney(amountReturned);

        Destroy(selectedTower);
    }

    public void DeselectTower()
    {
        selected_tower = null;
    }


}
