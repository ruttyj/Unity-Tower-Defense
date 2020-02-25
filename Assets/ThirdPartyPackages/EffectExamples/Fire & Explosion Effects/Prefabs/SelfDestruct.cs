using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruct : MonoBehaviour
{
    [SerializeField] float m_maxLife = 0;
    void Start()
    {
        Destroy(gameObject, m_maxLife);
    }
}
