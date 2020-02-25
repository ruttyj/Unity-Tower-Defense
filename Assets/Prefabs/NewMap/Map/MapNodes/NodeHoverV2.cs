using UnityEngine;
using System.Collections;

public class NodeHoverV2 : MonoBehaviour
{
    [SerializeField] Material hoverMaterial;
    [SerializeField] Material originalMaterial;

    private void Start()
    {
        originalMaterial = GetComponent<Renderer>().material;
    }

    public void Hover()
    {
        GetComponent<Renderer>().material = hoverMaterial;
    }

    public void StopHovering()
    {
        GetComponent<Renderer>().material = originalMaterial;
    }
}
