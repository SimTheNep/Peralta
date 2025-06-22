using UnityEngine;
using UnityEngine.Tilemaps;

public class RandomizeTilemapColor : MonoBehaviour
{
    public Tilemap[] tilemaps;

    void Start()
    {
        if (tilemaps == null || tilemaps.Length == 0)
        {
            Debug.LogWarning("Não há tilemaps para randomizar.");
            return;
        }

        foreach (Tilemap tilemap in tilemaps)
        {
            if (tilemap == null) continue;

            float r = Random.Range(165, 211) / 255f;
            float g = Random.Range(165, 211) / 255f;
            float b = Random.Range(165, 211) / 255f;

            tilemap.color = new Color(r, g, b, 1f);
        }
    }
}
