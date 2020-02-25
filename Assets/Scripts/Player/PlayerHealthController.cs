using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerHealthController : MonoBehaviour
{
    [SerializeField]
    private const int starting_health = 20;
    [SerializeField]
    private int current_health;

    //GUI
    [SerializeField]
    TextMeshProUGUI guiHealthText;

    // Start is called before the first frame update
    void Start()
    {
        current_health = starting_health;
        guiHealthText.text = current_health.ToString();
    }

    public void AddHealth(int add_amt)
    {
        current_health += add_amt;
        guiHealthText.text = current_health.ToString();
    }

    //returns true if game over 
    public bool SubtractHealth(int subtr_amt)
    {
        if (current_health <= subtr_amt)
        {
            guiHealthText.text = current_health.ToString();
            current_health = 0;
            return true;
        }

        guiHealthText.text = current_health.ToString();

        current_health -= subtr_amt;
        return false;
    }

    public void ResetHealth()
    {
        current_health = starting_health;
        guiHealthText.text = current_health.ToString();
    }



}
