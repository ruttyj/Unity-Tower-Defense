using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomWard : MonoBehaviour
{
    
    OddsTable m_oddsTable;
    [SerializeField] GameObject m_ward1;
    [SerializeField] GameObject m_ward2;
    [SerializeField] GameObject m_ward3;
    [SerializeField] GameObject m_ward4;
    [SerializeField] GameObject m_ward5;
    [SerializeField] GameObject m_ward6;
    [SerializeField] GameObject m_placeholder = null;


    // Start is called before the first frame update
    void Start()
    {
        m_oddsTable = new OddsTable();
        m_oddsTable.Add("m_ward1", 1, m_ward1);
        m_oddsTable.Add("m_ward2", 1, m_ward2);
        m_oddsTable.Add("m_ward3", 1, m_ward3);
        m_oddsTable.Add("m_ward4", 1, m_ward4);
        m_oddsTable.Add("m_ward5", 1, m_ward5);
        m_oddsTable.Add("m_ward6", 1, m_ward6);


        // Pick a ward at random
        GameObject prefab = (GameObject)m_oddsTable.GetPayload(m_oddsTable.Roll());
        if(prefab != null)
        {
            DeletePlaceHolder();
            GameObject spawnNode = Instantiate(prefab, new Vector3(0,0,0), Quaternion.identity);
            spawnNode.transform.SetParent(this.gameObject.transform, false);
        }
    }

    void DeletePlaceHolder()
    {
        if(m_placeholder != null)
        {
            Destroy(m_placeholder);
            m_placeholder = null;
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
