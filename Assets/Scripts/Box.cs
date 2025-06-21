using UnityEngine;
using UnityEngine.Tilemaps;

public enum BoxType { Light, Heavy }

public class Box : MonoBehaviour
{
    public BoxType boxType = BoxType.Light;
    private Tilemap Water;

    private void Start()
    {
        Water = GetComponent<Tilemap>();

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Water_Entry"))
        {
            print("Tapou");
            Tilemap tilemap = GetComponent<Tilemap> ();
            tilemap.SetTile(new Vector3Int(0, 0, 0), null);
        }
    }
}


