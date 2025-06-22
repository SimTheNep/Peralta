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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        StartCoroutine(FallAndDieSequence());
    }

    private IEnumerator FallAndDieSequence()
    {
        // 1. Prepara a animação de Damage (fica no último frame durante a queda)
        if (gabrielAnimator != null)
        {
            gabrielAnimator.SetTrigger("Damage");
            yield return null; // Garante que o Animator atualiza
            gabrielAnimator.speed = 0f; // Pausa no primeiro frame de Damage
        }

        // 2. Move Gabriel do topo para a posição inicial (queda)
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

        // 3. Agora toca a animação Die (e fica parado no último frame)
        if (gabrielAnimator != null)
        {
            gabrielAnimator.speed = 1f; // Volta a ativar o Animator
            gabrielAnimator.SetTrigger("Die");
            yield return null; // Espera um frame para garantir que a animação começa
            yield return new WaitForSeconds(0.1f); // Pequeno delay para garantir transição

            // Pausa no último frame da animação Die
            gabrielAnimator.speed = 0f;
        }

    }



    // 2. Animação de levantar (morte invertida)
    public void PlayGetUpAnimation()
    {
        gabrielAnimator.speed = 1f;
=======
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
        if (gabrielAnimator != null) gabrielAnimator.SetTrigger("Damage");
    }

    // 2. Animação de levantar (morte invertida)
    public void PlayGetUpAnimation()
    {
<<<<<<< Updated upstream
<<<<<<< Updated upstream
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
=======
>>>>>>> Stashed changes
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
<<<<<<< Updated upstream
<<<<<<< Updated upstream
<<<<<<< Updated upstream
        yield return new WaitForSeconds(0.35f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.35f);
=======
        yield return new WaitForSeconds(0.15f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.15f);
>>>>>>> Stashed changes
=======
        yield return new WaitForSeconds(0.15f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.15f);
>>>>>>> Stashed changes
=======
        yield return new WaitForSeconds(0.15f);
        gabrielController.GetComponent<SpriteRenderer>().flipX = true;
        yield return new WaitForSeconds(0.15f);
>>>>>>> Stashed changes
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

