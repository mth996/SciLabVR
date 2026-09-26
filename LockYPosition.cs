using UnityEngine;

public class LockYPosition : MonoBehaviour
{
    private float floorY;

    void Start()
    {
        floorY = transform.position.y;
    }

    void LateUpdate()
    {
        if (transform.position.y != floorY)
        {
            Vector3 pos = transform.position;
            pos.y = floorY;
            transform.position = pos;
        }
    }
}