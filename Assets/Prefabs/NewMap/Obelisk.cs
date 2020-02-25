using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obelisk : MonoBehaviour
{
    [SerializeField] float frequency = 0.3F;
    [SerializeField] float amplitude = 0.5F;
    Vector3 posOffset;
    float randomOffset = 0;

    void Start()
    {
        posOffset = gameObject.transform.position;
        randomOffset = Random.Range(0, 1);
    }

    void Update()
    {
        Vector3 tempPos = posOffset;
        tempPos.y += Mathf.Sin((randomOffset+Time.fixedTime) * Mathf.PI * frequency) * amplitude;
        gameObject.transform.position = tempPos;
    }
}
