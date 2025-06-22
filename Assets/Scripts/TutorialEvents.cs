using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialEvents : MonoBehaviour
{
    public GabrielController gabrielController;
    public Animator gabrielAnimator;
    public GameObject exclamationIndicator; // indicador de exclamação gabriel
    public AudioSource audioSource; // audiosource do Gabriel
    public AudioClip dangerClip;
    public AudioClip comicClip;
    public AudioClip roarClip;
    public InventoryUI gabrielInventoryUI;
    public Image dialogueBox;
    public GameObject cabriolaPrefab;
    public Transform cabriolaSpawnPoint;
    public CameraFollow cameraFollow;
    public Transform gabrielTransform;
    public Transform cabriolaTransform; // só preenchido dps do spawn


    // 1. Animação de queda/dano
    public void PlayFallAnimation()
    {
        if (gabrielAnimator != null) gabrielAnimator.SetTrigger("Damage");
    }

    // 2. Animação de levantar (morte invertida)
    public void PlayGetUpAnimation()
    {
        if (gabrielAnimator != null) gabrielAnimator.SetTrigger("GetUp");
    }

    // 3. Flip rápido do sprite idle + som cómico
    public void FlipConfusedLook()
    {
        if (gabrielController != null) StartCoroutine(FlipRoutine());
        if (audioSource != null && comicClip != null) audioSource.PlayOneShot(comicClip);
    }
    private IEnumerator FlipRoutine()
    {
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.15f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.15f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
    }

    // 4. Exclamação + som de perigo
    public void ShowExclamation() => ShowExclamationCustom(1f);
    public void ShowExclamationCustom(float duration)
    {
        StartCoroutine(ShowExclamationRoutine(duration));
    }
    private IEnumerator ShowExclamationRoutine(float duration)
    {
        if (exclamationIndicator == null) yield break;
        exclamationIndicator.SetActive(true);
        if (audioSource != null && dangerClip != null) audioSource.PlayOneShot(dangerClip);
        yield return new WaitForSeconds(duration);
        exclamationIndicator.SetActive(false);
    }

    // 5. Mostrar inventário
    public void ShowGabrielInventory()
    {
        if (gabrielInventoryUI != null) gabrielInventoryUI.gameObject.SetActive(true);
        SetDialogueBoxAlpha(0.5f); // Meio transparente
    }
    // 6. Esconder inventário
    public void HideGabrielInventory()
    {
        if (gabrielInventoryUI != null) gabrielInventoryUI.gameObject.SetActive(false);
        SetDialogueBoxAlpha(1f); // Opaco
    }

    // 7. Alterar transparência da DialogueBox (Image)
    public void SetDialogueBoxAlpha(float alpha)
    {
        if (dialogueBox != null)
        {
            var color = dialogueBox.color;
            color.a = Mathf.Clamp01(alpha);
            dialogueBox.color = color;
        }
    }


    // 7. olhar para direita/esquerda (sprite flip)
    public void LookAround()
    {
        StartCoroutine(LookAroundRoutine());
    }
    private IEnumerator LookAroundRoutine()
    {
        if (gabrielController == null) yield break;
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.5f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.5f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
    }

    // 8. tocar som de rugido
    public void PlayRoarSound()
    {
        if (audioSource != null && roarClip != null)
            audioSource.PlayOneShot(roarClip);
    }

    // 9. Spawnar Cabriola
    public void SpawnCabriola()
    {
        if (cabriolaPrefab != null && cabriolaSpawnPoint != null)
        {
            GameObject cabriola = Instantiate(cabriolaPrefab, cabriolaSpawnPoint.position, Quaternion.identity);
            cabriolaTransform = cabriola.transform;
        }
    }

    // 10. Mudar câmara para Cabriola
    public void FocusCameraOnCabriola()
    {
        if (cameraFollow != null && cabriolaTransform != null)
            cameraFollow.SetTarget(cabriolaTransform);
    }

    // 11. Voltar câmara para Gabriel
    public void FocusCameraOnGabriel()
    {
        if (cameraFollow != null && gabrielTransform != null)
            cameraFollow.SetTarget(gabrielTransform);
    }

    // 12. Tocar som de perigo (caso precises de um evento separado)
    public void PlayDangerSound()
    {
        if (audioSource != null && dangerClip != null) audioSource.PlayOneShot(dangerClip);
    }
}

