using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RightClickMenuClose : MonoBehaviour
{
    [SerializeField]
    Dropdown m_Dropdown;

    [SerializeField]
    GameObject m_RightClickMenu;


    public void RighClickMenuCloseDropDown()
    {
        if (m_Dropdown != null && !m_Dropdown.Equals(null))
        {
            //Make it the blank choice on close
            m_Dropdown.value = 0;
        }

        if (m_RightClickMenu != null && !m_RightClickMenu.Equals(null))
        {
            m_RightClickMenu.SetActive(false);
        }
    }
}
