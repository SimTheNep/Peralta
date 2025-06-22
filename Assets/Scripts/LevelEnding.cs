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

    private bool gabrielInside = false;
    private bool peraltaInside = false;

    private void Start()
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0f;
            canvasGroup.interactable = false;
            canvasGroup.blocksRaycasts = false;
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
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("TestRoom");
    }
}
