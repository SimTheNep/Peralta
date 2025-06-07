using UnityEngine;

public class testenemy : MonoBehaviour
{

    public float Life = 10f;

    void Update()
    {
        if (Life <= 0)
        {
            Debug.Log($"{gameObject.name} morreu!");
            Destroy(gameObject);
        }
    }
}
