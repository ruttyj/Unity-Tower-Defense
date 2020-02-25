using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class TinyMoveScript : MonoBehaviour
{
    enum Direction
    {
        X,
        Y,
        Z
    };

    [SerializeField] float speed = 5.0f;
    [SerializeField] float startRange = 1.0f;
    [SerializeField] float endRange = 5.0f;
    [SerializeField] private Direction direction = Direction.Y;

    private float oscillateRange;
    private float oscillateOffset;
    private Vector3 startPosition;

    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;

        oscillateRange = (endRange - startRange) / 2;
        oscillateOffset = oscillateRange + startRange;
    }

    // Update is called once per frame
    void Update()
    {
        float oscillate = oscillateOffset + Mathf.Sin(Time.time * speed) * oscillateRange;
        Vector3 newPos = startPosition;
        switch (direction) {
            case Direction.X:
                newPos += new Vector3(oscillate, 0f, 0f);
                break;
            case Direction.Y:
                newPos += new Vector3(0f, oscillate, 0f);
                break;
            case Direction.Z:
                newPos += new Vector3(0f, 0f, oscillate);
                break;
            default:
                Debug.Log("Call from switch direction");
                break;
        }
        transform.position = newPos;
    }
}
