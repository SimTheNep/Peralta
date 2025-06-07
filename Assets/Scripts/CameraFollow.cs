using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    public Transform target; // O alvo que a câmara vai seguir
    public float smoothSpeed = 5f; // Velocidade da suavização
    public Vector3 offset; // Offset da câmara (posição relativa ao alvo)


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    
    void LateUpdate()
    {
        if (target == null) return;
        // Lerp significa Linear Interpolation. Move suavemente entre 2 pontos com base numa % de 0 a 1
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}
