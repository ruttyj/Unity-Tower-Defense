using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StatusScript : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI statusText;


    // Start is called before the first frame update
    void Start()
    {
        

    }

    // Update is called once per frame

    void Update()
    {
        if (!string.IsNullOrEmpty(statusText.text))
        {
            StartCoroutine(MakeBlank());
        }
    }

    IEnumerator MakeBlank()
    {
        yield return new WaitForSeconds(3);
        statusText.text = null;
    }

}
