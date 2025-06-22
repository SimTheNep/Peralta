using UnityEngine;

public class DoorLogic : MonoBehaviour
{
    public float teleportOffset = 5f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        Transform obj = other.transform;

        float doorY = transform.position.y;
        float objY = obj.position.y;

        if (objY < doorY)
        {
            obj.position += Vector3.up * teleportOffset;
        }
        else
        {
            // Coming from above
            obj.position += Vector3.down * teleportOffset;
        }
    }
}
