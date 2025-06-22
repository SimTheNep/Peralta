using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Rigidbody2D))]
public class ToggleWaterOnBox : MonoBehaviour
{
    public Tilemap waterTilemap;
    public Collider2D[] triggerColliders;
    public float checkInterval = 0.2f;

    private bool waterDisabled = false;
    private ContactFilter2D boxFilter;
    private Collider2D[] parentColliders;

    void Awake()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.simulated = true;

        boxFilter = new ContactFilter2D();
        boxFilter.useLayerMask = true;
        boxFilter.layerMask = LayerMask.GetMask("Box");

        parentColliders = GetComponents<Collider2D>();
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

        foreach (Collider2D trigger in triggerColliders)
        {
            if (trigger == null) continue;

            List<Collider2D> results = new List<Collider2D>();
            int count = trigger.Overlap(boxFilter, results);

            if (count == 0)
            {
                allCovered = false;
                break;
            }
        }

        if (allCovered && !waterDisabled)
        {
            waterDisabled = true;
            StartCoroutine(FadeOutWater());
            Debug.Log("Todas zonas cobertas — desativando água.");
        }
        else if (!allCovered && waterDisabled)
        {
            waterDisabled = false;
            StartCoroutine(FadeInWater());
            Debug.Log("Zona descoberta — reativando água.");
        }
    }

    IEnumerator FadeOutWater()
    {
        float duration = 0.5f;
        float elapsed = 0f;

        Color startColor = waterTilemap.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            waterTilemap.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        waterTilemap.color = endColor;
        waterTilemap.gameObject.SetActive(false);

        foreach (Collider2D col in parentColliders)
        {
            col.enabled = false;
        }

        Debug.Log("Água desativada.");
    }

    IEnumerator FadeInWater()
    {
        waterTilemap.gameObject.SetActive(true);

        float duration = 0.5f;
        float elapsed = 0f;

        Color startColor = new Color(waterTilemap.color.r, waterTilemap.color.g, waterTilemap.color.b, 0f);
        Color endColor = new Color(waterTilemap.color.r, waterTilemap.color.g, waterTilemap.color.b, 1f);
        waterTilemap.color = startColor;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;
            waterTilemap.color = Color.Lerp(startColor, endColor, t);
            yield return null;
        }

        waterTilemap.color = endColor;

        // Re-enable all colliders on parent
        foreach (Collider2D col in parentColliders)
        {
            col.enabled = true;
        }

        Debug.Log("Água ativada.");
    }
}
