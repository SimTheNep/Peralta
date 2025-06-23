using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class LevelEnding : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public AudioSource musicAudioSource;
    public TextMeshProUGUI statusText;

    public GameObject gabrielObject;
    public GameObject peraltaObject;

    public string targetLevel;

    private bool gabrielInside = false;
    private bool peraltaInside = false;

    // Static variables to persist inventory across scenes
    private static Item[] savedGabrielInventory = new Item[3];
    private static MagicItem[] savedPeraltaInventory = new MagicItem[3];
    private static bool shouldLoadInventory = false;

    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f; 
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
            StartCoroutine(InitFadeOut());
        }

        if (shouldLoadInventory)
        {
            shouldLoadInventory = false;

            if (gabrielObject != null)
            {
                var gabrielInv = gabrielObject.GetComponent<GabrielInventoryManager>();
                if (gabrielInv != null)
                {
                    for (int i = 0; i < gabrielInv.slots.Length; i++)
                    {
                        if (savedGabrielInventory[i] != null)
                            gabrielInv.slots[i] = new Item(savedGabrielInventory[i]);
                    }
                    gabrielInv.inventoryUI?.UpdateUI(gabrielInv.slots, gabrielInv.selectedSlot);
                }
            }

            if (peraltaObject != null)
            {
                var peraltaInv = peraltaObject.GetComponent<PeraltaInventoryManager>();
                if (peraltaInv != null)
                {
                    for (int i = 0; i < peraltaInv.slots.Length; i++)
                    {
                        if (savedPeraltaInventory[i] != null)
                            peraltaInv.slots[i] = new MagicItem(savedPeraltaInventory[i]);
                    }
                    peraltaInv.inventoryUI?.UpdateUI(peraltaInv.slots, peraltaInv.selectedSlot);
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject == gabrielObject)
        {
            gabrielInside = true;
            Debug.Log("Gabriel entered");
        }

        if (other.gameObject == peraltaObject)
        {
            peraltaInside = true;
            Debug.Log("Peralta entered");
        }

        VerificarEstado();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject == gabrielObject)
        {
            gabrielInside = false;
            Debug.Log("Gabriel exited");
        }

        if (other.gameObject == peraltaObject)
        {
            peraltaInside = false;
            Debug.Log("Peralta exited");
        }

        VerificarEstado();
    }

    void VerificarEstado()
    {
        if (gabrielInside && peraltaInside)
        {
            statusText.text = "";
            StartCoroutine(FadeInCanvas());
            StartCoroutine(FadeOutMusic());
        }
        else if (gabrielInside && !peraltaInside)
        {
            statusText.text = "À espera de Peralta.";
        }
        else if (peraltaInside && !gabrielInside)
        {
            statusText.text = "À espera de Gabriel.";
        }
        else
        {
            statusText.text = "";
        }
    }

    IEnumerator FadeInCanvas()
    {
        if (canvasGroup == null) yield break;

        float duration = 1f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator InitFadeOut()
    {
        if (canvasGroup == null) yield break;

        float duration = 0.25f;
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }

    IEnumerator FadeOutMusic()
    {
        if (musicAudioSource == null) yield break;

        float duration = 1f;
        float startVolume = musicAudioSource.volume;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / duration);
            yield return null;
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop();

        SaveInventoryState();

        yield return new WaitForSeconds(1f);
        shouldLoadInventory = true;
        SceneManager.LoadScene(targetLevel);
    }

    void SaveInventoryState()
    {
        var gabrielInv = gabrielObject?.GetComponent<GabrielInventoryManager>();
        if (gabrielInv != null)
        {
            for (int i = 0; i < savedGabrielInventory.Length; i++)
            {
                savedGabrielInventory[i] = gabrielInv.slots[i] != null ? new Item(gabrielInv.slots[i]) : null;
            }
        }

        var peraltaInv = peraltaObject?.GetComponent<PeraltaInventoryManager>();
        if (peraltaInv != null)
        {
            for (int i = 0; i < savedPeraltaInventory.Length; i++)
            {
                savedPeraltaInventory[i] = peraltaInv.slots[i] != null ? new MagicItem(peraltaInv.slots[i]) : null;
            }
        }
    }
}
