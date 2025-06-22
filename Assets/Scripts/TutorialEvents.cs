using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TutorialEvents : MonoBehaviour
{
    public GabrielController gabrielController;
    public Animator gabrielAnimator;
    public GameObject exclamationIndicator; // indicador de exclama��o gabriel
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
    public Transform cabriolaTransform; // s� preenchido dps do spawn


    // 1. Anima��o de queda/dano
    public void PlayFallAnimation()
    {
        StartCoroutine(FallAndDieSequence());
    }

    private IEnumerator FallAndDieSequence()
    {
        // 1. Prepara a anima��o de Damage (fica no �ltimo frame durante a queda)
        if (gabrielAnimator != null)
        {
            gabrielAnimator.SetTrigger("Damage");
            yield return null; // Garante que o Animator atualiza
            gabrielAnimator.speed = 0f; // Pausa no primeiro frame de Damage
        }

        // 2. Move Gabriel do topo para a posi��o inicial (queda)
        Vector3 start = gabrielTransform.position + new Vector3(0, 5f, 0); // ajusta o offset conforme precisares
        Vector3 end = gabrielTransform.position;
        gabrielTransform.position = start;

        float duration = 2.0f; // tempo da queda (ajusta para mais lento)
        float elapsed = 0f;
        while (elapsed < duration)
        {
            gabrielTransform.position = Vector3.Lerp(start, end, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        gabrielTransform.position = end;

        // 3. Agora toca a anima��o Die (e fica parado no �ltimo frame)
        if (gabrielAnimator != null)
        {
            gabrielAnimator.speed = 1f; // Volta a ativar o Animator
            gabrielAnimator.SetTrigger("Die");
            yield return null; // Espera um frame para garantir que a anima��o come�a
            yield return new WaitForSeconds(0.1f); // Pequeno delay para garantir transi��o

            // Pausa no �ltimo frame da anima��o Die
            gabrielAnimator.speed = 0f;
        }

    }



    // 2. Anima��o de levantar (morte invertida)
    public void PlayGetUpAnimation()
    {
        gabrielAnimator.speed = 1f;
        if (gabrielAnimator != null) gabrielAnimator.SetTrigger("GetUp");
    }

    // 3. Flip r�pido do sprite idle + som c�mico
    public void FlipConfusedLook()
    {
        if (gabrielController != null) StartCoroutine(FlipRoutine());
        if (audioSource != null && comicClip != null) audioSource.PlayOneShot(comicClip);
    }
    private IEnumerator FlipRoutine()
    {
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
        yield return new WaitForSeconds(0.35f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.35f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = false;
    }

    // 4. Exclama��o + som de perigo
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

    // 5. Mostrar invent�rio
    public void ShowGabrielInventory()
    {
        if (gabrielInventoryUI != null) gabrielInventoryUI.gameObject.SetActive(true);
        SetDialogueBoxAlpha(0.5f); // Meio transparente
    }
    // 6. Esconder invent�rio
    public void HideGabrielInventory()
    {
        if (gabrielInventoryUI != null) gabrielInventoryUI.gameObject.SetActive(false);
        SetDialogueBoxAlpha(1f); // Opaco
    }

    // 7. Alterar transpar�ncia da DialogueBox (Image)
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

    // 10. Mudar c�mara para Cabriola
    public void FocusCameraOnCabriola()
    {
        if (cameraFollow != null && cabriolaTransform != null)
            cameraFollow.SetTarget(cabriolaTransform);
    }

    // 11. Voltar c�mara para Gabriel
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

