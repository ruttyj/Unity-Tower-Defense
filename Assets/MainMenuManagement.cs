using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManagement : MonoBehaviour
{
    [SerializeField] public int seed;
    [SerializeField]
    public GameObject map;
    [SerializeField]
    public Text currentSeed;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {

        if (currentSeed == null)
        {
            GameObject seedPlaceHolderObject = GameObject.FindGameObjectWithTag("SeedPlaceHolder");
            if (seedPlaceHolderObject != null)
            {
                currentSeed = seedPlaceHolderObject.GetComponent<Text>();
            }
        }

        if (currentSeed != null)
        {
            GameObject mapObject = GameObject.FindGameObjectWithTag("Map");
            if (mapObject != null)
            {
                MapV2 map = mapObject.GetComponent<MapV2>();
                if (map != null)
                {
                    seed = map.m_mapSeed;
                    currentSeed.text = seed.ToString();
                }
                else
                {
                    Debug.Log("NO MAP");
                }
            }
            else
            {
                Debug.Log("NO MAP OBJECT");
            }
        }
    }

    public void loadMainGame()
    {
        seed = FindObjectOfType<MapV2>().m_mapSeed;
        Debug.Log(seed);
        SceneManager.LoadScene("MainGameSceneV2");
    }
}
