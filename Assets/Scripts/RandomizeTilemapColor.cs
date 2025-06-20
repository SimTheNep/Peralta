using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomizeTilemapColor : MonoBehaviour
{
    private Tilemap tilemap;

    void Start()
    {
        tilemap = GetComponent<Tilemap>();

        float r = Random.Range(165, 211) / 255f;
        float g = Random.Range(165, 211) / 255f;
        float b = Random.Range(165, 211) / 255f;

        tilemap.color = new Color(r, g, b, 1f);
    }
}
