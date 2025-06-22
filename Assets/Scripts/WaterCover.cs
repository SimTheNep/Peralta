using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ToggleWaterOnBox : MonoBehaviour
{
    public Tilemap waterTilemap;
    public Collider2D[] triggerColliders;
    public float checkInterval = 0.2f;

    private bool waterDisabled = false;
    private ContactFilter2D boxFilter;

    void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

        boxFilter = new ContactFilter2D();
        boxFilter.useLayerMask = true;
        boxFilter.layerMask = LayerMask.GetMask("Box");

    }

    void OnEnable()
    {
        waterDisabled = false;
        StartCoroutine(CheckTriggersRepeatedly());
    }

    IEnumerator CheckTriggersRepeatedly()
    {
        while (true)
        {
            yield return new WaitForSeconds(checkInterval);
            CheckState();
        }
    }

    void CheckState()
    {
        bool allCovered = true;

        for (int i = 0; i < triggerColliders.Length; i++)
        {
            Collider2D trigger = triggerColliders[i];
            if (trigger == null)
                continue;

            Collider2D[] results = new Collider2D[5];
            int count = trigger.Overlap(boxFilter, results);

            if (count == 0)
            {
                allCovered = false;
                break;
            }
        }

        if (allCovered && !waterDisabled)
        {
            Debug.Log("todas zonas cobertas desativando agua");
            waterDisabled = true;
            StartCoroutine(FadeOutWater());
        }
        else if (!allCovered && waterDisabled)
        {
            Debug.Log("zona descoberta reativando agua");
            waterDisabled = false;
            StartCoroutine(FadeInWater());
        }
    }

    IEnumerator FadeOutWater()
    {

        Tilemap tilemap = waterTilemap;
        float duration = 0.5f;
        float elapsed = 0f;

        Color startColor = tilemap.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            tilemap.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        tilemap.color = endColor;
        waterTilemap.gameObject.SetActive(false);
        GetComponent<Collider2D>().enabled = false;

        Debug.Log("agua desativada");
    }

    IEnumerator FadeInWater()
    {

        waterTilemap.gameObject.SetActive(true);
        Tilemap tilemap = waterTilemap;

        float duration = 0.5f;
        float elapsed = 0f;

        Color startColor = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 0f);
        Color endColor = new Color(tilemap.color.r, tilemap.color.g, tilemap.color.b, 1f);
        tilemap.color = startColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            tilemap.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        tilemap.color = endColor;
        GetComponent<Collider2D>().enabled = true;

        Debug.Log("agua ativada");
    }
}
